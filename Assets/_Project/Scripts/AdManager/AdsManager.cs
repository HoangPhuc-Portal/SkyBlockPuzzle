using System;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using BlockBlast.UI;

// The existing manager remains the only owner of SDK initialization and native ads.
public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance { get; private set; }
    [SerializeField] private AdsConfiguration configuration;
    // Retained only to preserve serialized values in the old TestQC scene; never used for requests.
    [SerializeField, HideInInspector] private string bannerAdUnitId, rewardedAdUnitId, interstitialAdUnitId;
    [SerializeField, HideInInspector] private bool loadBannerOnStart, bannerAtTop;
    private readonly ConcurrentQueue<Action> mainThread = new ConcurrentQueue<Action>();
    private BannerView bannerView;
    private RewardedAd rewardedAd;
    private RewardedRequest activeRequest;
    private bool alive, consentPending, consentReady, initializeStarted, isInitialized;
    private bool bannerLoaded, bannerLoading, rewardLoading, showing, menuLeaving, privacyShowing;
    private int bannerGeneration, rewardGeneration, bannerFailures, rewardFailures, consentGeneration;
    private float bannerRetryAt, rewardRetryAt, consentRetryAt, bannerDeadline, rewardDeadline, rewardLoadedAt;
    private int bannerWidth, bannerScreenHeight;
    private bool networkWasAvailable;
    private float consentDeadline, initializationDeadline, initializationRetryAt, displayDeadline;
    private int initializationGeneration;
#if UNITY_EDITOR
    // Used only by deterministic Editor regression checks; never included in a player build.
    internal static bool? NetworkOverrideForTests;
#endif
    public static bool NetworkAvailable
    {
        get
        {
#if UNITY_EDITOR
            if (NetworkOverrideForTests.HasValue) return NetworkOverrideForTests.Value;
#endif
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }
    public string RewardedStatus => !NetworkAvailable ? "Bạn cần kết nối Internet để xem quảng cáo."
        : !isInitialized ? "Đang khởi tạo quảng cáo. Vui lòng thử lại sau."
        : rewardLoading ? "Quảng cáo đang được tải, vui lòng thử lại sau."
        : "Quảng cáo chưa sẵn sàng. Vui lòng thử lại sau.";
    public bool IsInitialized => isInitialized;
    public bool IsShowing => showing;
    public bool IsRewardedReady => NetworkAvailable && isInitialized && consentReady && !privacyShowing && !showing && rewardedAd != null && rewardedAd.CanShowAd();
    public bool BannerVisible { get; private set; }
    public bool PrivacyOptionsRequired => ConsentInformation.PrivacyOptionsRequirementStatus == PrivacyOptionsRequirementStatus.Required;
    public int InitializationCalls { get; private set; }
    public int BannerCreations { get; private set; }
    public string Status { get; private set; } = "Chưa khởi tạo";
    private bool MenuAllowed => configuration != null && !menuLeaving && !privacyShowing && SceneManager.GetActiveScene().name == configuration.mainMenuScene;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap() => EnsureInstance();
    public static AdsManager EnsureInstance()
    {
        if (Instance != null) return Instance;
        var existing = FindFirstObjectByType<AdsManager>();
        return existing != null ? existing : new GameObject("AdsManager").AddComponent<AdsManager>();
    }
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        alive = true;
        networkWasAvailable = NetworkAvailable;
        DontDestroyOnLoad(gameObject);
        if (configuration == null) configuration = Resources.Load<AdsConfiguration>("AdsConfiguration");
        SceneManager.activeSceneChanged += SceneChanged;
    }
    private void Start() { if (Instance == this) InitializeAds(); }
    private void Post(Action action) { mainThread.Enqueue(action); }
    private void Update()
    {
        if (!alive) return;
        // Drain a snapshot so synchronous SDK clients cannot cause a recursive load loop.
        int count = mainThread.Count;
        for (int i = 0; i < count && mainThread.TryDequeue(out var action); i++)
            try { action(); } catch (Exception e) { Debug.LogException(e, this); }
        if (configuration == null) return;
        if (showing && Time.unscaledTime > displayDeadline && Application.isFocused)
            FinishDisplay(rewardedAd, activeRequest, true);
        bool online = NetworkAvailable;
        if (!online)
        {
            if (networkWasAvailable)
            {
                DestroyBanner();
                if (!showing) DestroyRewarded();
                if (consentPending) { consentGeneration++; consentPending = false; }
            }
            networkWasAvailable = false;
            return;
        }
        if (!networkWasAvailable)
        {
            networkWasAvailable = true;
            consentRetryAt = bannerRetryAt = rewardRetryAt = initializationRetryAt = 0;
        }
        if (consentPending && Time.unscaledTime > consentDeadline)
        { consentGeneration++; consentPending = false; consentRetryAt = Time.unscaledTime + 30; }
        if (initializeStarted && !isInitialized && Time.unscaledTime > initializationDeadline)
        { initializationGeneration++; initializeStarted = false; initializationRetryAt = Time.unscaledTime + 30; }
        if (!consentReady && !consentPending && !privacyShowing && Time.unscaledTime >= consentRetryAt) InitializeAds();
        if (consentReady && !isInitialized && !initializeStarted && !privacyShowing && Time.unscaledTime >= initializationRetryAt) InitializeSdk();
        if (!isInitialized || !consentReady || privacyShowing) return;
        if (!MenuAllowed) { if (bannerView != null && (bannerLoading || Application.isEditor)) DestroyBanner(); else if (BannerVisible) HideBanner(); }
        else
        {
            if (bannerView != null && (bannerWidth != MobileAds.Utils.GetDeviceSafeWidth() || bannerScreenHeight != Screen.height)) DestroyBanner();
            if (bannerLoading && Time.unscaledTime > bannerDeadline) { DestroyBanner(); bannerRetryAt = Time.unscaledTime + RetryDelay(++bannerFailures); }
            if (bannerView == null && Time.unscaledTime >= bannerRetryAt) LoadBanner();
            else if (bannerLoaded && !BannerVisible) ShowBanner();
        }
        if (rewardLoading && Time.unscaledTime > rewardDeadline)
        {
            rewardGeneration++; rewardLoading = false;
            rewardRetryAt = Time.unscaledTime + RetryDelay(++rewardFailures);
        }
        if (!showing && rewardedAd != null && Time.unscaledTime - rewardLoadedAt > 3300) DestroyRewarded();
        if (!showing && !rewardLoading && rewardedAd == null && Time.unscaledTime >= rewardRetryAt) LoadRewarded();
    }
    private static float RetryDelay(int attempts) => Mathf.Min(60, 2 * Mathf.Pow(2, Mathf.Min(attempts, 5)));
    public void InitializeAds()
    {
        if (!NetworkAvailable || configuration == null || consentPending || consentReady || privacyShowing || Time.unscaledTime < consentRetryAt) return;
        consentPending = true;
        consentDeadline = Time.unscaledTime + 60;
        int generation = ++consentGeneration;
        Status = "Đang kiểm tra quyền riêng tư";
        // Audience confirmed by the owner: ages 6+, worldwide. Apply child protections to all users.
        try
        {
            MobileAds.SetRequestConfiguration(new RequestConfiguration {
                AgeRestrictedTreatment = AgeRestrictedTreatment.Child,
                MaxAdContentRating = MaxAdContentRating.G
            });
            ConsentInformation.Update(new ConsentRequestParameters { TagForUnderAgeOfConsent = true }, error => Post(() =>
            {
                if (!alive || generation != consentGeneration) return;
                if (error != null) { FinishConsent(error); return; }
                ConsentForm.LoadAndShowConsentFormIfRequired(formError => Post(() =>
                {
                    if (alive && generation == consentGeneration) FinishConsent(formError);
                }));
            }));
        }
        catch (Exception e) { consentPending = false; consentRetryAt = Time.unscaledTime + 30; Status = "Không thể kiểm tra quyền riêng tư"; Debug.LogWarning(e.Message, this); }
    }
    private void FinishConsent(FormError error)
    {
        consentPending = false;
        consentReady = ConsentInformation.CanRequestAds();
        if (error != null) Debug.LogWarning("[AdsManager] Consent: " + error.Message, this);
        if (!consentReady) { consentRetryAt = Time.unscaledTime + 30; Status = "Chưa đủ điều kiện yêu cầu quảng cáo"; return; }
        InitializeSdk();
    }
    private void InitializeSdk()
    {
        if (!NetworkAvailable || !consentReady || initializeStarted || isInitialized || Time.unscaledTime < initializationRetryAt) return;
        initializeStarted = true;
        initializationDeadline = Time.unscaledTime + 60;
        int generation = ++initializationGeneration;
        InitializationCalls++;
        Status = "Đang khởi tạo quảng cáo";
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        try
        {
            MobileAds.Initialize(status => Post(() =>
            {
                if (!alive || generation != initializationGeneration) return;
                isInitialized = status != null;
                if (!isInitialized) { initializeStarted = false; initializationRetryAt = Time.unscaledTime + 30; }
                Status = isInitialized ? "SDK sẵn sàng" : "SDK không khả dụng trong phiên này";
                if (isInitialized) { LoadRewarded(); if (MenuAllowed) LoadBanner(); }
            }));
        }
        catch (Exception e) { initializeStarted = false; initializationRetryAt = Time.unscaledTime + 30; Status = "SDK không khả dụng trong phiên này"; Debug.LogWarning(e.Message, this); }
    }
    public void ShowPrivacyOptions()
    {
        if (!PrivacyOptionsRequired || showing || privacyShowing || consentPending) return;
        privacyShowing = true;
        consentReady = false;
        DestroyBanner(); DestroyRewarded();
        try { ConsentForm.ShowPrivacyOptionsForm(error => Post(() =>
        {
            if (!alive) return;
            privacyShowing = false;
            FinishConsent(error);
        })); }
        catch (Exception e)
        {
            privacyShowing = false;
            consentRetryAt = Time.unscaledTime + 30;
            Debug.LogWarning(e.Message, this);
        }
    }
    private void SceneChanged(Scene previous, Scene next)
    {
        menuLeaving = false;
        activeRequest?.CancelRecipient();
        if (!MenuAllowed) { if (bannerView != null && (bannerLoading || Application.isEditor)) DestroyBanner(); else if (BannerVisible) HideBanner(); }
        else if (isInitialized && consentReady) ShowBanner();
    }
    public void LeaveMainMenu() { menuLeaving = true; if (bannerView != null && (bannerLoading || Application.isEditor)) DestroyBanner(); else HideBanner(); }
    public void ResumeMainMenu() { menuLeaving = false; ShowBanner(); }
    public void LoadBanner()
    {
        if (!NetworkAvailable || !isInitialized || !consentReady || !MenuAllowed || bannerView != null || bannerLoading || Time.unscaledTime < bannerRetryAt) return;
        if (string.IsNullOrWhiteSpace(configuration.BannerId)) return;
        bannerWidth = MobileAds.Utils.GetDeviceSafeWidth();
        bannerScreenHeight = Screen.height;
        var size = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(bannerWidth);
        var banner = new BannerView(configuration.BannerId, size, AdPosition.Bottom);
        bannerView = banner; bannerLoading = true; bannerLoaded = false;
        bannerDeadline = Time.unscaledTime + 60;
        int generation = ++bannerGeneration;
        BannerCreations++;
        banner.Hide();
        banner.OnBannerAdLoaded += () => Post(() =>
        {
            if (!alive || generation != bannerGeneration) return;
            bannerLoading = false; bannerLoaded = true; bannerFailures = 0;
            if (MenuAllowed) ShowBanner(); else HideBanner();
        });
        banner.OnBannerAdLoadFailed += error => Post(() =>
        {
            if (!alive || generation != bannerGeneration) return;
            DestroyBanner(); bannerRetryAt = Time.unscaledTime + RetryDelay(++bannerFailures);
            Debug.LogWarning("[AdsManager] Banner unavailable: " + error, this);
        });
        try { banner.LoadAd(new AdRequest()); }
        catch (Exception e) { DestroyBanner(); bannerRetryAt = Time.unscaledTime + RetryDelay(++bannerFailures); Debug.LogWarning(e.Message, this); }
    }
    public void ShowBanner()
    {
        if (!NetworkAvailable || !MenuAllowed || !consentReady) { HideBanner(); return; }
        if (bannerView == null) { LoadBanner(); return; }
        if (!bannerLoaded || BannerVisible) return;
        bannerView.Show(); BannerVisible = true;
    }
    public void HideBanner() { if (bannerView != null) bannerView.Hide(); BannerVisible = false; }
    public void DestroyBanner()
    {
        bannerGeneration++; bannerLoaded = bannerLoading = BannerVisible = false;
        var old = bannerView; bannerView = null; old?.Destroy();
    }
    public void LoadRewarded()
    {
        if (!NetworkAvailable || !isInitialized || !consentReady || privacyShowing || showing || rewardLoading || rewardedAd != null || Time.unscaledTime < rewardRetryAt) return;
        if (string.IsNullOrWhiteSpace(configuration.RewardedId)) return;
        rewardLoading = true; rewardDeadline = Time.unscaledTime + 60;
        int generation = ++rewardGeneration;
        try
        {
            RewardedAd.Load(configuration.RewardedId, new AdRequest(), (ad, error) => Post(() =>
            {
                if (!alive || generation != rewardGeneration || !NetworkAvailable) { ad?.Destroy(); if (generation == rewardGeneration) rewardLoading = false; return; }
                rewardLoading = false;
                if (error != null || ad == null)
                {
                    ad?.Destroy(); rewardRetryAt = Time.unscaledTime + RetryDelay(++rewardFailures);
                    Status = "Quảng cáo chưa sẵn sàng. Vui lòng thử lại sau."; return;
                }
                rewardedAd = ad; rewardFailures = 0; rewardLoadedAt = Time.unscaledTime; Status = "Rewarded sẵn sàng";
            }));
        }
        catch (Exception e) { rewardLoading = false; rewardRetryAt = Time.unscaledTime + RetryDelay(++rewardFailures); Debug.LogWarning(e.Message, this); }
    }
    public RewardedRequest ShowRewarded(Action<RewardResult> completed)
    {
        if (!IsRewardedReady) { completed?.Invoke(RewardResult.Unavailable); PrepareRewarded(); return null; }
        showing = true;
        displayDeadline = Time.unscaledTime + 180;
        var ad = rewardedAd;
        var ticket = new RewardedRequest(completed);
        activeRequest = ticket;
        ad.OnAdFullScreenContentClosed += () => Post(() => FinishDisplay(ad, ticket, false));
        ad.OnAdFullScreenContentFailed += error => Post(() => FinishDisplay(ad, ticket, true));
        try { ad.Show(reward => Post(() => { if (alive && activeRequest == ticket) ticket.Earn(); })); }
        catch (Exception e) { FinishDisplay(ad, ticket, true); Debug.LogWarning(e.Message, this); }
        return ticket;
    }
    public void PrepareRewarded()
    {
        if (!NetworkAvailable) return;
        if (!consentReady) InitializeAds();
        else if (!isInitialized) InitializeSdk();
        else LoadRewarded();
    }
    // Preserve the old API for external callers, with the same once-only reward protection.
    public void ShowRewarded(Action onRewardEarned) => ShowRewarded(result => { if (result == RewardResult.Earned) onRewardEarned?.Invoke(); });
    private void FinishDisplay(RewardedAd ad, RewardedRequest ticket, bool failed)
    {
        if (!alive || activeRequest != ticket) return;
        activeRequest = null; showing = false; rewardedAd = null;
        ad?.Destroy();
        try { if (failed) ticket.Fail(); else ticket.Close(); }
        finally { LoadRewarded(); }
    }
    private void DestroyRewarded()
    {
        rewardGeneration++; rewardLoading = false;
        var old = rewardedAd; rewardedAd = null; old?.Destroy();
    }
    // Kept for legacy TestQC UnityEvents; this task deliberately does not serve interstitials.
    public void LoadInterstitial() { }
    public void ShowInterstitial() { }
    private void OnDestroy()
    {
        if (Instance != this) return;
        alive = false; consentGeneration++;
        SceneManager.activeSceneChanged -= SceneChanged;
        activeRequest?.CancelRecipient(); activeRequest = null;
        DestroyBanner(); DestroyRewarded();
        Instance = null;
        while (mainThread.TryDequeue(out _)) { }
    }
}






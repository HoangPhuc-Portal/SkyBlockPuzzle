using UnityEngine;

[CreateAssetMenu(menuName = "Block Blast/Ads Configuration")]
public sealed class AdsConfiguration : ScriptableObject
{
    public string mainMenuScene = "MainMenu";
    [Tooltip("Keep enabled until production IDs and audience/privacy setup are confirmed.")]
    public bool useTestAds = true;
    public bool productionApproved;
    public string androidBanner = "ca-app-pub-8282327217241136/5746621358";
    public string androidRewarded = "ca-app-pub-8282327217241136/4050396302";
    public string iosBanner = "";
    public string iosRewarded = "";
    public bool TestMode => Application.isEditor || Debug.isDebugBuild || useTestAds || !productionApproved;
    public string BannerId => GetId(false);
    public string RewardedId => GetId(true);
    private string GetId(bool rewarded)
    {
#if UNITY_IOS && !UNITY_EDITOR
        if (TestMode) return rewarded ? "ca-app-pub-3940256099942544/1712485313" : "ca-app-pub-3940256099942544/2435281174";
        return rewarded ? iosRewarded : iosBanner;
#else
        if (TestMode) return rewarded ? "ca-app-pub-3940256099942544/5224354917" : "ca-app-pub-3940256099942544/9214589741";
        return rewarded ? androidRewarded : androidBanner;
#endif
    }
}

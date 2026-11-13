using System;
using BlockBlast.UI;

public sealed class AdMobRewardedProvider : RewardedProvider
{
    private RewardedRequest ownedRequest;
    public override bool IsReady => AdsManager.Instance != null && AdsManager.Instance.IsRewardedReady;
    public override bool HasNetwork => AdsManager.NetworkAvailable;
    public override string UnavailableMessage => AdsManager.Instance != null
        ? AdsManager.Instance.RewardedStatus : "Đang khởi tạo quảng cáo. Vui lòng thử lại sau.";
    public override void Prepare() => AdsManager.Instance?.PrepareRewarded();
    private void Awake() => AdsManager.EnsureInstance();
    public override void Show(Action<RewardResult> completed)
    {
        if (AdsManager.Instance == null) { completed(RewardResult.Unavailable); return; }
        ownedRequest = AdsManager.Instance.ShowRewarded(completed);
    }
    public override void Cancel()
    {
        ownedRequest?.CancelRecipient();
        ownedRequest = null;
    }
    private void OnDisable() => Cancel();
}

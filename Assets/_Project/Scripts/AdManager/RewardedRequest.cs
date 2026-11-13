using System;
using BlockBlast.UI;

// One SDK display owns one ticket. Closing alone can never manufacture a reward.
public sealed class RewardedRequest
{
    private Action<RewardResult> callback;
    private bool earned, finished;
    public RewardedRequest(Action<RewardResult> completed) { callback = completed; }
    public void Earn() { if (!finished) earned = true; }
    public void CancelRecipient() { callback = null; }
    public void Close() => Finish(earned ? RewardResult.Earned : RewardResult.Cancelled);
    public void Fail() => Finish(RewardResult.Failed);
    private void Finish(RewardResult result)
    {
        if (finished) return;
        finished = true;
        var recipient = callback;
        callback = null;
        recipient?.Invoke(result);
    }
}

using System;
using UnityEngine;
namespace BlockBlast.UI
{
    public enum RewardResult { Earned, Cancelled, Failed, Unavailable }
    public abstract class RewardedProvider : MonoBehaviour
    {
        public abstract bool IsReady { get; }
        // Reachability only detects definite offline; readiness still comes from the SDK.
        public virtual bool HasNetwork => Application.internetReachability != NetworkReachability.NotReachable;
        public virtual string UnavailableMessage => "Quảng cáo chưa sẵn sàng. Vui lòng thử lại sau.";
        public virtual void Prepare() { }
        // Invoke once on Unity's main thread, after the ad closes; Earned requires SDK reward confirmation.
        public abstract void Show(Action<RewardResult> completed);
        public abstract void Cancel();
    }
}

using System;
using UnityEngine;
namespace BlockBlast.UI
{
    public sealed class MockRewardedProvider : RewardedProvider
    {
        [SerializeField] bool ready = true;
        Action<RewardResult> pending;
        public override bool IsReady {
            get {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                return ready && pending == null && isActiveAndEnabled;
#else
                return false;
#endif
            }
        }
        public override void Show(Action<RewardResult> completed) {
            if(!IsReady){completed(RewardResult.Unavailable);return;}
            pending=completed;
        }
        public void Complete(RewardResult result) {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            var callback=pending;pending=null;callback?.Invoke(result);
#endif
        }
        public override void Cancel(){pending=null;}
        void OnDisable()=>Cancel();
        void OnGUI(){
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if(pending==null)return;
            GUILayout.BeginArea(new Rect(20,20,Mathf.Min(Screen.width-40,500),220),GUI.skin.box);
            GUILayout.Label("MOCK REWARDED — DEVELOPMENT ONLY");
            if(GUILayout.Button("Complete and earn reward",GUILayout.Height(45)))Complete(RewardResult.Earned);
            if(GUILayout.Button("Close without reward",GUILayout.Height(45)))Complete(RewardResult.Cancelled);
            if(GUILayout.Button("Simulate failure",GUILayout.Height(45)))Complete(RewardResult.Failed);
            GUILayout.EndArea();
#endif
        }
    }
}

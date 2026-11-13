using UnityEngine;
namespace BlockBlast.Menu
{
    [RequireComponent(typeof(Animator))]
    public sealed class MenuDragonAnimation : MonoBehaviour
    {
        Animator animator;
        float nextBlink;
        bool departing;
        readonly System.Random random = new System.Random();
        static readonly int Idle = Animator.StringToHash("Idle");
        static readonly int Blink = Animator.StringToHash("Blink");
        static readonly int Reaction = Animator.StringToHash("PlayReaction");
        void Awake() { animator=GetComponent<Animator>(); }
        void OnEnable() { ResetIdle(); }
        void Schedule() { nextBlink=Time.unscaledTime+3+(float)random.NextDouble()*4; }
        void Update()
        {
            if(!departing&&Time.unscaledTime>=nextBlink&&animator.GetCurrentAnimatorStateInfo(0).shortNameHash==Idle)
            { animator.Play(Blink,0,0);Schedule(); }
        }
        public void PlayReaction() { if(departing)return;departing=true;animator.Play(Reaction,0,0); }
        public void ResetIdle() { if(animator==null)animator=GetComponent<Animator>();departing=false;animator.Play(Idle,0,0);Schedule(); }
    }
}

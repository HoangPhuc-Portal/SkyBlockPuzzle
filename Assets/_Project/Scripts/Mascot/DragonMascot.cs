using UnityEngine;
using UnityEngine.UI;
using BlockBlast.Core;
using BlockBlast.Grid;
using BlockBlast.Managers;

namespace BlockBlast.Mascot
{
    /// <summary>Animates the existing illustrated mascot as a whole; no invented rig or facial deformation.</summary>
    [DisallowMultipleComponent, RequireComponent(typeof(RectTransform))]
    public sealed class DragonMascot : MonoBehaviour
    {
        public enum Reaction { Idle, Happy, Excited, Encourage }
        [SerializeField] bool gameplay;
        [SerializeField] Image artwork;
        [SerializeField] RectTransform safeArea, topBar;
        [SerializeField] float idlePixels=3, idleAngle=1.2f, happyPixels=9, excitedPixels=17;
        [SerializeField] float happyDuration=.65f, excitedDuration=1.1f, encourageDuration=1.2f;
        [SerializeField] int[] excitedThresholds={6,8};
        [SerializeField] Vector2 gameplaySize=new Vector2(130,172);
        RectTransform rect;
        Vector3 originalScale;
        Quaternion originalRotation;
        Vector2 originalPosition;
        Button tap;
        float clock, reactionTime, duration, amplitude;
        int lastCombo;
        bool initialized, visible=true, over;
        readonly Vector3[] corners=new Vector3[4];
        public Reaction State {get;private set;}
        public int ReactionCount {get;private set;}
        public bool IsVisible=>visible;
        public bool IsGameplay=>gameplay;
        void Awake()
        {
            rect=(RectTransform)transform;originalScale=rect.localScale;originalRotation=rect.localRotation;originalPosition=rect.anchoredPosition;
            if(artwork==null)artwork=GetComponent<Image>();tap=GetComponent<Button>();initialized=true;
            if(gameplay&&artwork!=null)artwork.raycastTarget=false;
        }
        void OnEnable()
        {
            if(!initialized)return;
            if(gameplay){GameEvents.OnLinesCleared+=Clear;GameEvents.OnComboChanged+=Combo;GameEvents.OnGameOver+=GameOver;GameEvents.OnBoardReset+=BoardReset;GameEvents.OnGameStateChanged+=StateChanged;}
            else if(tap!=null)tap.onClick.AddListener(Tap);
            Canvas.willRenderCanvases+=Layout;
            ResetPose();lastCombo=GameManager.Instance!=null?GameManager.Instance.CurrentCombo:0;
            over=gameplay&&GameManager.Instance!=null&&GameManager.Instance.IsGameOver;
        }
        void OnDisable()
        {
            GameEvents.OnLinesCleared-=Clear;GameEvents.OnComboChanged-=Combo;GameEvents.OnGameOver-=GameOver;GameEvents.OnBoardReset-=BoardReset;GameEvents.OnGameStateChanged-=StateChanged;
            Canvas.willRenderCanvases-=Layout;if(tap!=null)tap.onClick.RemoveListener(Tap);if(initialized)ResetPose();
        }
        void Tap(){Begin(Reaction.Happy,happyDuration,happyPixels);}
        void Clear(int lines)
        {
            if(lines<=0||over||State==Reaction.Excited||State==Reaction.Encourage)return;
            Begin(Reaction.Happy,happyDuration,lines>1?happyPixels*1.4f:happyPixels);
        }
        void Combo(int combo)
        {
            bool crossed=false;if(excitedThresholds!=null)foreach(int threshold in excitedThresholds)if(lastCombo<threshold&&combo>=threshold)crossed=true;
            lastCombo=combo;if(crossed&&!over)Begin(Reaction.Excited,excitedDuration,excitedPixels);
        }
        void GameOver(){if(over)return;over=true;Begin(Reaction.Encourage,encourageDuration,5);}
        void BoardReset(GridBoard source){over=false;lastCombo=0;ResetPose();}
        void StateChanged(GameState state){if(state==GameState.Gameplay){over=false;ResetPose();lastCombo=GameManager.Instance!=null?GameManager.Instance.CurrentCombo:0;}}
        void Begin(Reaction reaction,float seconds,float pixels)
        {
            if(!visible)return;State=reaction;duration=Mathf.Max(.1f,seconds);reactionTime=0;amplitude=pixels;ReactionCount++;
        }
        void ResetPose(){State=Reaction.Idle;reactionTime=0;clock=0;rect.anchoredPosition=originalPosition;rect.localScale=originalScale;rect.localRotation=originalRotation;}
        void Update()
        {
            if(!initialized||!visible||artwork==null||!artwork.enabled)return;
            if(gameplay&&GameManager.Instance!=null&&GameManager.Instance.RewardModalOpen)return;
            float dt=Time.unscaledDeltaTime;clock+=dt;
            float y=Mathf.Sin(clock*1.65f)*idlePixels,angle=Mathf.Sin(clock*.9f)*idleAngle,scale=1;
            if(State!=Reaction.Idle)
            {
                reactionTime+=dt;float t=Mathf.Clamp01(reactionTime/duration),envelope=Mathf.Sin(t*Mathf.PI);
                if(State==Reaction.Encourage){y-=amplitude*envelope;angle-=5*envelope;}
                else{y+=amplitude*Mathf.Abs(Mathf.Sin(t*Mathf.PI*(State==Reaction.Excited?2:1)));angle+=3*Mathf.Sin(t*Mathf.PI*2)*envelope;scale+=.035f*envelope;}
                if(t>=1)State=Reaction.Idle;
            }
            rect.anchoredPosition=originalPosition+Vector2.up*y;rect.localRotation=originalRotation*Quaternion.Euler(0,0,angle);rect.localScale=originalScale*scale;
        }
        void Layout()
        {
            if(!gameplay||safeArea==null||topBar==null||artwork==null)return;
            topBar.GetWorldCorners(corners);float highest=float.MinValue;foreach(var c in corners)highest=Mathf.Max(highest,safeArea.InverseTransformPoint(c).y);
            float available=safeArea.rect.yMax-highest-42;
            float height=Mathf.Min(gameplaySize.y,available);
            visible=height>=70;artwork.enabled=visible;
            if(!visible){State=Reaction.Idle;return;}
            float width=height*gameplaySize.x/gameplaySize.y;
            Vector2 offset=rect.anchoredPosition-originalPosition;
            rect.sizeDelta=new Vector2(width,height);originalPosition=new Vector2(-28-width*.5f,-22-height*.5f);
            rect.anchoredPosition=originalPosition+offset;
            // Center pivot leaves room for both idle and reaction without entering the HUD.
        }
    }
}

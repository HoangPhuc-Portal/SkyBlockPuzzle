using UnityEngine;
using TMPro;
using BlockBlast.Core;
using BlockBlast.Managers;
namespace BlockBlast.UI
{
    public sealed class GameHUD : MonoBehaviour
    {
        [SerializeField] UITheme theme;
        // Preserve existing layout/references: left = record, central BEST = round score.
        [SerializeField] TextMeshProUGUI score, highScore;
        [SerializeField] bool separateScorePlates;
        int displayed, displayedRecord, savedRecord;
        Coroutine tween;
        Vector3 originalScale;
        void Awake() { originalScale = highScore.transform.localScale; }
        void OnEnable() { GameEvents.OnScoreChanged += Score; GameEvents.OnHighScoreChanged += High; GameEvents.OnGameOver += RefreshImmediately; if(GameManager.Instance!=null)RefreshImmediately(); }
        void OnDisable() { GameEvents.OnScoreChanged -= Score; GameEvents.OnHighScoreChanged -= High; GameEvents.OnGameOver -= RefreshImmediately; StopAllCoroutines(); tween=null; highScore.transform.localScale=originalScale; }
        void Start() => RefreshImmediately();
        public void RefreshImmediately()
        {
            var model=GameManager.Instance;if(model==null)return;
            StopAllCoroutines();tween=null;displayed=model.CurrentScore;
            savedRecord=displayedRecord=model.HighScore;Render();highScore.transform.localScale=originalScale;
        }
        void High(int value) { savedRecord=value; }
        void Render() { score.text=displayedRecord.ToString();highScore.text=separateScorePlates?displayed.ToString():"<color=#FFD54A>BEST</color> <color=#FFFFFF>"+displayed+"</color>"; }
        void Score(int value)
        {
            if(value==0){RefreshImmediately();return;}
            if(tween!=null)StopCoroutine(tween);
            int from=displayed, recordFrom=displayedRecord;
            int targetRecord=GameManager.Instance!=null?GameManager.Instance.HighScore:savedRecord;
            targetRecord=Mathf.Max(recordFrom,targetRecord);
            tween=StartCoroutine(UITween.Run(theme.scoreTime,t=>{
                float progress=UITween.Quad(t);
                displayed=Mathf.RoundToInt(Mathf.Lerp(from,value,progress));
                displayedRecord=Mathf.RoundToInt(Mathf.Lerp(recordFrom,targetRecord,progress));
                Render();highScore.transform.localScale=originalScale*(1+.08f*Mathf.Sin(t*Mathf.PI));
            },()=>{displayed=value;displayedRecord=targetRecord;Render();highScore.transform.localScale=originalScale;tween=null;}));
        }
    }
}

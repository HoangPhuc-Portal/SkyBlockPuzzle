
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using BlockBlast.Core;
using BlockBlast.Managers;
using BlockBlast.Grid;
using BlockBlast.Shape;

namespace BlockBlast.UI
{
    /// <summary>Scene-owned presentation and ordered reset of the existing gameplay systems.</summary>
    public sealed class GameOverController : MonoBehaviour
    {
        [SerializeField] private UITheme theme;
        [SerializeField] private GridBoard board;
        [SerializeField] private TrayManager tray;
        [SerializeField] private GameManager scoreManager;
        [SerializeField] private GameObject popup;
        [SerializeField] private RectTransform panel;
        [SerializeField] private CanvasGroup group;
        [SerializeField] private TextMeshProUGUI finalScore, highScore;
        [SerializeField] private GameObject crown;
        [SerializeField] private Image recordGlow;
        [SerializeField] private Button replayButton, viewBoardButton;
        [SerializeField] private GameHUD hud;
        [SerializeField] private HintController hint;
        [SerializeField] private SettingsPanel settings;
        [SerializeField] private ClearPresentation clearPresentation;
        [SerializeField] private BlockBlast.Effects.BubblePopEffectPlayer bubbles;

        [SerializeField] private RewardedGameplay rewards;
        public bool CanRequestRevive => requested && !transitioning && popup.activeSelf && group.interactable;
        private bool requested, transitioning;
        public bool IsPopupVisible => popup.activeSelf;
        public int FinalScore { get; private set; }
        public bool IsNewRecord { get; private set; }

        private void Awake()
        {
            popup.SetActive(false);
            if (viewBoardButton != null) viewBoardButton.gameObject.SetActive(false);
            crown.SetActive(false);
            recordGlow.color = Color.clear;
            var replayLabel = replayButton.GetComponentInChildren<TMP_Text>(true);
            if (replayLabel != null) replayLabel.text = "Chơi lại";
        }
        private void OnEnable()
        {
            GameEvents.OnGameOver += Show;
            replayButton.onClick.AddListener(Replay);
        }
        private void OnDisable()
        {
            GameEvents.OnGameOver -= Show;
            replayButton.onClick.RemoveListener(Replay);
            StopAllCoroutines();
        }
        private void Show()
        {
            if (requested) return;
            requested = true;
            FinalScore = scoreManager.CurrentScore;
            IsNewRecord = FinalScore > scoreManager.RoundStartHighScore;
            finalScore.text = FinalScore.ToString();
            highScore.text = "KỶ LỤC  " + scoreManager.HighScore;
            crown.SetActive(IsNewRecord);
            settings.Close();
            rewards.ResetRequests();
            StartCoroutine(Open());
        }
        private IEnumerator Open()
        {
            yield return new WaitForSecondsRealtime(theme.gameOverDelay);
            popup.SetActive(true);
            popup.transform.SetAsLastSibling();
            group.interactable = false;
            group.blocksRaycasts = true;
            yield return UITween.RunRealtime(theme.gameOverOpenTime, t =>
            {
                group.alpha = Mathf.Clamp01(t * 2);
                panel.localScale = Vector3.one * Mathf.LerpUnclamped(.75f, 1, UITween.Back(t));
            });
            group.alpha = 1;
            panel.localScale = Vector3.one;
            group.interactable = true;
            if (IsNewRecord) StartCoroutine(FlashRecord());
        }
        private IEnumerator FlashRecord()
        {
            yield return UITween.RunRealtime(theme.recordFlashTime, t =>
            {
                Color color = theme.recordGold;
                color.a = .22f * (1 - t) * (.6f + .4f * Mathf.Cos(t * Mathf.PI * 4));
                recordGlow.color = color;
            });
            recordGlow.color = Color.clear;
        }
        public void ViewBoard()
        {
            // Retained for old serialized callbacks; viewing the board is no longer supported.
        }
        public void Replay()
        {
            if (!CanRequestRevive) return;
            StartCoroutine(Close());
        }
        public void SetReplayInteractable(bool value) => replayButton.interactable = value;
        public void RestartFromSettings()
        {
            if (!settings.IsOpen || scoreManager.IsGameOver || transitioning) return;
            transitioning = true;
            ResetRound();
            transitioning = false;
        }
        private IEnumerator Close()
        {
            transitioning = true;
            group.interactable = false;
            float fromAlpha = group.alpha;
            yield return UITween.RunRealtime(theme.gameOverCloseTime, t =>
            {
                group.alpha = fromAlpha * (1 - t);
                panel.localScale = Vector3.one * Mathf.Lerp(1, .85f, UITween.Quad(t));
            });
            popup.SetActive(false);
            group.blocksRaycasts = false;
            ResetRound();
            transitioning = false;
        }
        public void Revive()
        {
            if (!CanRequestRevive) return;
            StopAllCoroutines(); transitioning=true; group.interactable=false;
            settings.Close(); hint.Cancel(); clearPresentation.ResetEffects(); bubbles.StopEffects();
            if(EventSystem.current!=null)EventSystem.current.SetSelectedGameObject(null);
            tray.ClearHand(); board.ResetBoard(); board.SyncModel(); scoreManager.PrepareRevive();
            hint.ResumeAfterRevive(); hud.RefreshImmediately(); tray.SpawnNewHand();
            popup.SetActive(false); group.blocksRaycasts=false; group.alpha=0;
            requested=transitioning=false; crown.SetActive(false); recordGlow.color=Color.clear;
            scoreManager.UnlockNewRound(); tray.CheckGameOverCondition();
        }
        private void ResetRound()
        {
            StopAllCoroutines();
            if (rewards != null) rewards.ResetRequests();
            settings.Close();
            popup.SetActive(false);
            group.interactable = group.blocksRaycasts = false;
            group.alpha = 0;
            hint.ResetRound();
            clearPresentation.ResetEffects();
            bubbles.StopEffects();
            if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);
            tray.ClearHand();
            board.ResetBoard();
            scoreManager.ResetRound();
            hud.RefreshImmediately();
            tray.SpawnNewHand();
            requested = false;
            crown.SetActive(false);
            recordGlow.color = Color.clear;
            scoreManager.UnlockNewRound();
            tray.CheckGameOverCondition();
        }
    }
}

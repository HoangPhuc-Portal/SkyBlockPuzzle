using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using BlockBlast.Grid;
using BlockBlast.Shape;
using BlockBlast.Core;

namespace BlockBlast.UI
{
    public sealed class HintController : MonoBehaviour
    {
        [SerializeField] private UITheme theme;
        [SerializeField] private GridBoard board;
        [SerializeField] private TrayManager tray;
        [SerializeField] private Button button;
        [SerializeField, Min(0)] private int initialHintCount = 3;
        [SerializeField] private TMPro.TMP_Text counter;
        [SerializeField] private RewardedGameplay rewards;
        public int Remaining { get; private set; }
        private float nextAllowedTime;
        private Coroutine pulse;
        private bool ended;
        private ButtonFeedback feedback;

        private void Awake()
        {
            feedback = button.GetComponent<ButtonFeedback>();
            if (feedback == null) feedback = button.gameObject.AddComponent<ButtonFeedback>();
            feedback.UseControlledFeedback();
            Remaining = initialHintCount; UpdateCounter();
        }

        private void OnEnable()
        {
            GameEvents.OnShapePlaced += ResetTurn;
            GameEvents.OnGameOver += EndGame;
            button.onClick.AddListener(ShowHint);
        }
        private void OnDisable()
        {
            GameEvents.OnShapePlaced -= ResetTurn;
            GameEvents.OnGameOver -= EndGame;
            button.onClick.RemoveListener(ShowHint);
            Cancel();
            feedback.ResetScale();
        }
        public void ResetRound()
        {
            ended = false;
            Remaining = Mathf.Max(0, initialHintCount); UpdateCounter();
            nextAllowedTime = 0;
            ResetTurn();
        }

        private void ResetTurn()
        {

            Cancel();
            button.interactable = !ended;
        }
        private void EndGame()
        {
            ended = true;
            Cancel();
            button.interactable = false;
        }
        public void Cancel()
        {
            if (pulse != null) StopCoroutine(pulse);
            pulse = null;
            board.ClearPreviewAndHighlights();
        }

        // Deterministic ties: first tray piece, then first row/column.
        // A higher number of completed lines wins over an earlier non-clear target.
        public bool FindHint(out DraggableShape piece, out Vector2Int position)
        {
            piece = null;
            position = default;
            board.SyncModel();
            int best = -1;
            foreach (var candidate in tray.Pieces)
            {
                if (candidate == null) continue;
                var matrix = candidate.ShapeData.GetMatrix();
                for (int row = 0; row < GridBoard.GRID_SIZE; row++)
                for (int col = 0; col < GridBoard.GRID_SIZE; col++)
                {
                    int lines = board.Model.CompletedBy(matrix, row, col);
                    if (lines <= best) continue;
                    best = lines;
                    piece = candidate;
                    position = new Vector2Int(row, col);
                }
            }
            return best >= 0;
        }
        public void ShowHint()
        {
            if (button.interactable && !ended) feedback.Pulse();
            if (ended || BlockBlast.Managers.GameManager.Instance.IsInputLocked) return;
            if (Remaining == 0) { rewards.OpenHint(); return; }
            if (pulse != null || Time.unscaledTime < nextAllowedTime) return;
            if (!FindHint(out var piece, out var position)) return;
            Remaining--; UpdateCounter();
            nextAllowedTime = Time.unscaledTime + theme.hintCooldown;

            pulse = StartCoroutine(Pulse(piece, position));
        }
        void UpdateCounter(){if(counter!=null)counter.text=Remaining.ToString();}
        public void AddRewardHints(){Remaining+=2;UpdateCounter();}
        public void ResumeAfterRevive(){ended=false;Cancel();nextAllowedTime=0;button.interactable=true;UpdateCounter();}
        private IEnumerator Pulse(DraggableShape piece, Vector2Int position)
        {
            for (int i = 0; i < 3; i++)
            {
                if (piece == null) break;
                board.UpdateDragPreview(piece.ShapeData,
                    board.CellPosition(position.x, position.y), piece.BlockSprite);
                yield return new WaitForSecondsRealtime(theme.hintPulseTime);
                board.ClearPreviewAndHighlights();
                yield return new WaitForSecondsRealtime(theme.hintPulseTime);
            }
            pulse = null;
        }
    }
}

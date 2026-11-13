using UnityEngine;
using BlockBlast.Core;

namespace BlockBlast.Managers
{
    /// <summary>
    /// Quản lý điểm số, xu và kỷ lục chơi game
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public int CurrentScore { get; private set; }
        public int HighScore { get; private set; }
        public int CurrentCombo { get; private set; }
        public int ConsecutiveNoClearTurns { get; private set; }
        public int LastScoreGain { get; private set; }
        public bool LastPerfectClear { get; private set; }
        public int Coins { get; private set; }
        public int RoundStartHighScore { get; private set; }
        public int RunSequence { get; private set; }
        public int LastClearedLines { get; private set; }
        private bool inputLocked;
        public bool RewardModalOpen { get; set; }
        public bool IsInputLocked { get => inputLocked || RewardModalOpen; private set => inputLocked=value; }
        public bool IsGameOver { get; private set; }

        private const string HIGH_SCORE_KEY = "BlockBlast_HighScore";

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            HighScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
            RoundStartHighScore = HighScore;
            Coins = 6527; // Điểm xu cố định ban đầu
        }

        private void OnEnable()
        {

            GameEvents.OnGameOver += HandleGameOver;
        }

        private void OnDisable()
        {

            GameEvents.OnGameOver -= HandleGameOver;
        }

        private void Start()
        {
            GameEvents.TriggerScoreChanged(CurrentScore);
            GameEvents.TriggerHighScoreChanged(HighScore);
            GameEvents.TriggerCoinsChanged(Coins);
        }

        internal void ScorePlacement(int cellCount, int lineCount, bool boardEmpty)
        {
            if (IsInputLocked || cellCount <= 0 || lineCount < 0) return;
            int previousCombo = CurrentCombo;
            LastClearedLines = lineCount;
            if (lineCount > 0)
            {
                CurrentCombo = Mathf.Min(8, CurrentCombo + 1);
                ConsecutiveNoClearTurns = 0;
            }
            else
            {
                ConsecutiveNoClearTurns = Mathf.Min(3, ConsecutiveNoClearTurns + 1);
                if (ConsecutiveNoClearTurns == 3) CurrentCombo = 0;
            }
            LastPerfectClear = lineCount > 0 && boardEmpty;
            LastScoreGain = cellCount + 10 * lineCount * lineCount * CurrentCombo + (LastPerfectClear ? 300 : 0);
            CurrentScore += LastScoreGain;
            if (CurrentCombo != previousCombo) GameEvents.OnComboChanged?.Invoke(CurrentCombo);
            Coins += lineCount * 15;
            UpdateRecord();
            GameEvents.TriggerScoreChanged(CurrentScore);
            GameEvents.TriggerCoinsChanged(Coins);
        }
        private void UpdateRecord()
        {
            int previous = HighScore;
            HighScore = Mathf.Max(HighScore, PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0), CurrentScore);
            if (HighScore > PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0))
            {
                PlayerPrefs.SetInt(HIGH_SCORE_KEY, HighScore);
                PlayerPrefs.Save();
            }
            if (HighScore > previous) GameEvents.TriggerHighScoreChanged(HighScore);
        }

        private void HandleGameOver()
        {
            UpdateRecord();
            IsGameOver = IsInputLocked = true;
            PlayerPrefs.Save();
            var state = BlockBlast.Core.StateMachine.GameStateManager.Instance;
            if (state != null) state.ChangeState(GameState.GameOver);
            else GameEvents.TriggerGameStateChanged(GameState.GameOver);
        }

        public bool ConfirmGameOver()
        {
            if (IsGameOver || IsInputLocked) return false;
            UpdateRecord();
            IsGameOver = IsInputLocked = true;
            GameEvents.TriggerGameOver();
            return true;
        }

        public void PrepareRevive()
        {
            IsInputLocked=true; IsGameOver=false; RewardModalOpen=false;
            CurrentCombo=ConsecutiveNoClearTurns=LastScoreGain=0; LastPerfectClear=false;
            GameEvents.OnComboChanged?.Invoke(CurrentCombo);
        }
        public void ResetRound()
        {
            RunSequence++;
            LastClearedLines = 0;
            IsInputLocked = true;
            IsGameOver = false;
            CurrentScore = 0;
            CurrentCombo = ConsecutiveNoClearTurns = LastScoreGain = 0;
            GameEvents.OnComboChanged?.Invoke(CurrentCombo);
            LastPerfectClear = false;
            RoundStartHighScore = HighScore;
            GameEvents.TriggerScoreChanged(0);
            GameEvents.TriggerHighScoreChanged(HighScore);
            GameEvents.TriggerCoinsChanged(Coins);
        }

        public void UnlockNewRound()
        {
            var state = BlockBlast.Core.StateMachine.GameStateManager.Instance;
            if (state != null) state.ChangeState(GameState.Gameplay);
            else GameEvents.TriggerGameStateChanged(GameState.Gameplay);
            IsInputLocked = false;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}

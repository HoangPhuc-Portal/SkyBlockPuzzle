using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using BlockBlast.Core;

namespace BlockBlast.UI
{
    /// <summary>
    /// Điều khiển toàn bộ giao diện người dùng UI (Thanh điểm, Xu, Bảng Game Over)
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("Top Bar UI Elements")]
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI highScoreText;

        [Header("Game Over Modal")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TextMeshProUGUI finalScoreText;

        private void OnEnable()
        {
            GameEvents.OnScoreChanged += UpdateScore;
            GameEvents.OnHighScoreChanged += UpdateHighScore;
            GameEvents.OnCoinsChanged += UpdateCoins;
            GameEvents.OnGameOver += ShowGameOverModal;
            RefreshFromModel();
        }

        private void OnDisable()
        {
            GameEvents.OnScoreChanged -= UpdateScore;
            GameEvents.OnHighScoreChanged -= UpdateHighScore;
            GameEvents.OnCoinsChanged -= UpdateCoins;
            GameEvents.OnGameOver -= ShowGameOverModal;
        }

        private void Start() => RefreshFromModel();
        private void RefreshFromModel()
        {
            var model = BlockBlast.Managers.GameManager.Instance;
            if (model == null) return;
            UpdateScore(model.CurrentScore);
            UpdateHighScore(model.HighScore);
            UpdateCoins(model.Coins);
        }

        private void UpdateScore(int score)
        {
            if (scoreText != null) scoreText.text = score.ToString();
        }

        private void UpdateHighScore(int highScore)
        {
            if (highScoreText != null) highScoreText.text = highScore.ToString();
        }

        private void UpdateCoins(int coins)
        {
            if (coinText != null) coinText.text = coins.ToString("N0");
        }

        private void ShowGameOverModal()
        {
            if (FindFirstObjectByType<GameOverController>() != null) return;
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
                if (finalScoreText != null && BlockBlast.Managers.GameManager.Instance != null)
                {
                    finalScoreText.text = BlockBlast.Managers.GameManager.Instance.CurrentScore.ToString();
                }
            }
        }

        public void OnClickRestartButton()
        {
            var flow = FindFirstObjectByType<GameOverController>();
            if (flow != null) flow.Replay();
            else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BlockBlast.Managers;

namespace BlockBlast.UI
{
    // Owns one reward request at a time. The provider reports the final result after closing.
    public sealed class RewardedGameplay : MonoBehaviour
    {
        [SerializeField] private RewardedProvider provider;
        [SerializeField] private GameOverController gameOver;
        [SerializeField] private SettingsPanel settings;
        [SerializeField] private Button continueButton;
        [SerializeField] private TMP_Text reviveStatus;
        private bool busy;
        private int request;
        public bool IsBusy => busy;

        private void OnEnable()
        {
            continueButton.onClick.AddListener(WatchRevive);
        }
        private void OnDisable()
        {
            continueButton.onClick.RemoveListener(WatchRevive);
            ResetRequests();
        }


        private void WatchRevive() => Watch();
        // Compatibility for old UnityEvents: replay never requests an advertisement.
        public void WatchReplay() => gameOver.Replay();
        public void WatchSettingsReplay() => gameOver.RestartFromSettings();
        private void SetBusy(bool value)
        {
            busy = value;
            settings.SetNavigationBusy(value);
            if (continueButton != null) continueButton.interactable = !value;
            if (gameOver != null) gameOver.SetReplayInteractable(!value);
        }
        private void Watch()
        {
            if (busy) return;
            if (!gameOver.CanRequestRevive) return;
            var status = reviveStatus;
            if (provider != null && !provider.HasNetwork)
            {
                status.text = "Bạn cần kết nối Internet để xem quảng cáo và tiếp tục ván chơi.";
                return;
            }
            if (provider == null || !provider.IsReady)
            {
                provider?.Prepare();
                status.text = provider != null ? provider.UnavailableMessage : "Quảng cáo chưa sẵn sàng. Vui lòng thử lại sau.";
                return;
            }
            SetBusy(true);
            int token = ++request;
            status.text = "Đang chờ kết quả quảng cáo...";
            try
            {
                provider.Show(result => Complete(token, status, result));
            }
            catch (System.Exception exception)
            {
                Complete(token, status, RewardResult.Failed);
                provider.Cancel();
                Debug.LogWarning("Rewarded provider could not show: " + exception.Message, this);
            }
        }
        private void Complete(int token, TMP_Text status, RewardResult result)
        {
            // A replay, unload or completed request invalidates duplicate/late callbacks.
            if (this == null || !isActiveAndEnabled || token != request || !busy) return;
            request++;
            SetBusy(false);
            if (result == RewardResult.Earned)
            {
                status.text = "";
                gameOver.Revive();
            }
            else status.text = result == RewardResult.Cancelled
                ? "Chưa nhận thưởng. Bạn có thể thử lại."
                : "Không thể hoàn tất quảng cáo. Vui lòng thử lại.";
        }
        public void ResetRequests()
        {
            request++;
            SetBusy(false);
            if (provider != null) provider.Cancel();
            if (reviveStatus != null) reviveStatus.text = "";
            if (GameManager.Instance != null) GameManager.Instance.RewardModalOpen = false;
        }
    }
}

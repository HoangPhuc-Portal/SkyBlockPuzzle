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
        [SerializeField] private HintController hint;
        [SerializeField] private GameOverController gameOver;
        [SerializeField] private SettingsPanel settings;
        [SerializeField] private GameObject hintPopup;
        [SerializeField] private Button watchHint, closeHint, continueButton;
        [SerializeField] private TMP_Text hintStatus, reviveStatus;
        private bool busy;
        private int request;
        private enum RewardAction { Hint, Revive }
        public bool IsBusy => busy;

        private void Awake() => hintPopup.SetActive(false);
        private void OnEnable()
        {
            watchHint.onClick.AddListener(WatchHint);
            closeHint.onClick.AddListener(CloseHint);
            continueButton.onClick.AddListener(WatchRevive);
        }
        private void OnDisable()
        {
            watchHint.onClick.RemoveListener(WatchHint);
            closeHint.onClick.RemoveListener(CloseHint);
            continueButton.onClick.RemoveListener(WatchRevive);
            ResetRequests();
        }
        public void OpenHint()
        {
            if (busy || GameManager.Instance.IsGameOver) return;
            settings.Close();
            hint.Cancel();
            hintStatus.text = provider != null && !provider.HasNetwork
                ? "Bạn cần kết nối Internet để xem quảng cáo và nhận thêm lượt Hint."
                : "Xem quảng cáo để nhận thêm 2 lượt Hint.";
            hintPopup.SetActive(true);
            hintPopup.transform.SetAsLastSibling();
            GameManager.Instance.RewardModalOpen = true;
            foreach (var piece in BlockBlast.Shape.TrayManager.Instance.Pieces)
                if (piece != null) piece.OnCancel(null);
        }
        public void CloseHint()
        {
            if (busy) return;
            hintPopup.SetActive(false);
            if (GameManager.Instance != null) GameManager.Instance.RewardModalOpen = false;
        }
        private void WatchHint() => Watch(RewardAction.Hint);
        private void WatchRevive() => Watch(RewardAction.Revive);
        // Compatibility for old UnityEvents: replay never requests an advertisement.
        public void WatchReplay() => gameOver.Replay();
        public void WatchSettingsReplay() => gameOver.RestartFromSettings();
        private void SetBusy(bool value)
        {
            busy = value;
            settings.SetNavigationBusy(value);
            if (watchHint != null) watchHint.interactable = !value;
            if (closeHint != null) closeHint.interactable = !value;
            if (continueButton != null) continueButton.interactable = !value;
            if (gameOver != null) gameOver.SetReplayInteractable(!value);
        }
        private void Watch(RewardAction action)
        {
            if (busy) return;
            if (action == RewardAction.Revive ? !gameOver.CanRequestRevive : !hintPopup.activeSelf) return;
            var status = action == RewardAction.Revive ? reviveStatus : hintStatus;
            if (provider != null && !provider.HasNetwork)
            {
                status.text = action == RewardAction.Hint
                    ? "Bạn cần kết nối Internet để xem quảng cáo và nhận thêm lượt Hint."
                    : "Bạn cần kết nối Internet để xem quảng cáo và tiếp tục ván chơi.";
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
                provider.Show(result => Complete(token, action, status, result));
            }
            catch (System.Exception exception)
            {
                Complete(token, action, status, RewardResult.Failed);
                provider.Cancel();
                Debug.LogWarning("Rewarded provider could not show: " + exception.Message, this);
            }
        }
        private void Complete(int token, RewardAction action, TMP_Text status, RewardResult result)
        {
            // A replay, unload or completed request invalidates duplicate/late callbacks.
            if (this == null || !isActiveAndEnabled || token != request || !busy) return;
            request++;
            SetBusy(false);
            if (result == RewardResult.Earned)
            {
                status.text = "";
                if (action == RewardAction.Revive) gameOver.Revive();
                else { hint.AddRewardHints(); CloseHint(); }
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
            if (hintPopup != null) hintPopup.SetActive(false);
            if (reviveStatus != null) reviveStatus.text = "";
            if (GameManager.Instance != null) GameManager.Instance.RewardModalOpen = false;
        }
    }
}

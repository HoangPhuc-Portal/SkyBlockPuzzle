using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using BlockBlast.Managers;

namespace BlockBlast.UI
{
    public sealed class SettingsPanel : MonoBehaviour
    {
        [SerializeField] private GameObject popup;
        [SerializeField] private Slider music, sfx;
        [SerializeField] private Toggle haptic;
        [SerializeField] private Button open, close;
        private ButtonFeedback feedback;
        private bool wasOpen, leaving;
        [SerializeField] private Button home, replay;
        [SerializeField] private TMP_Text replayStatus;
        [SerializeField] private RewardedGameplay rewards;
        private GameOverController roundController;
        public bool IsOpen => popup.activeInHierarchy;
        public TMP_Text ReplayStatus => replayStatus;
        private bool Busy => leaving || (rewards != null && rewards.IsBusy);

        // Build from the existing Settings button style for both scene and prefab instances.
        private void BuildNavigation()
        {
            if (home != null && replay != null && replayStatus != null) return;
            var parent = close.transform.parent;
            foreach (RectTransform child in parent)
            {
                if (child.name == "Background") continue;
                var min = child.anchorMin; var max = child.anchorMax;
                min.y = .28f + .72f * min.y; max.y = .28f + .72f * max.y;
                child.anchorMin = min; child.anchorMax = max;
            }
            home = NavigationButton("Home", "HOME", .07f, .43f);
            replay = NavigationButton("Replay", "CHƠI LẠI", .47f, .93f);
            var label = close.GetComponentInChildren<TMP_Text>(true);
            replayStatus = Instantiate(label, parent);
            replayStatus.name = "ReplayStatus";
            replayStatus.text = "";
            replayStatus.enableAutoSizing = true;
            replayStatus.fontSizeMin = 18; replayStatus.fontSizeMax = 28;
            var rect = replayStatus.rectTransform;
            rect.anchorMin = new Vector2(.05f, .19f); rect.anchorMax = new Vector2(.95f, .28f);
            rect.offsetMin = rect.offsetMax = Vector2.zero;
        }
        private Button NavigationButton(string name, string label, float left, float right)
        {
            var button = Instantiate(close, close.transform.parent);
            button.name = name;
            button.onClick = new Button.ButtonClickedEvent();
            var rect = (RectTransform)button.transform;
            rect.anchorMin = new Vector2(left, .04f); rect.anchorMax = new Vector2(right, .17f);
            rect.offsetMin = rect.offsetMax = Vector2.zero;
            var text = button.GetComponentInChildren<TMP_Text>(true);
            text.text = label; text.enableAutoSizing = true; text.fontSizeMin = 18; text.fontSizeMax = 36;
            return button;
        }
        public void SetNavigationBusy(bool value)
        {
            if (home != null) home.interactable = !value && !leaving;
            if (replay != null) replay.interactable = !value && !leaving;
            close.interactable = !value && !leaving;
        }
        public void Home()
        {
            if (!IsOpen || Busy) return;
            if (!Application.CanStreamedLevelBeLoaded("MainMenu"))
            { replayStatus.text = "Không tìm thấy MainMenu trong Build Settings."; return; }
            leaving = true; SetNavigationBusy(true);
            PlayerPrefs.Save();
            // Navigation never requests a fullscreen ad.
            try { SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single); }
            catch (System.Exception error)
            {
                leaving = false; SetNavigationBusy(false);
                replayStatus.text = "Không thể trở về menu. Vui lòng thử lại.";
                Debug.LogException(error, this);
            }
        }
        public void Replay()
        {
            if (!IsOpen || Busy) return;
            if (roundController == null)
            {
                replayStatus.text = "Không tìm thấy bộ điều khiển ván chơi.";
                Debug.LogError("SettingsPanel requires GameOverController in this scene.", this);
                return;
            }
            roundController.RestartFromSettings();
        }
        public static bool HapticEnabled => PlayerPrefs.GetInt("UI.Haptic", 1) == 1;

        private void Awake()
        {
            BuildNavigation();
            // GameplayScreen owns Rewards above the nested UI Canvas, not inside it.
            if (rewards == null) rewards = GetComponentInParent<RewardedGameplay>(true);
            roundController = FindFirstObjectByType<GameOverController>();
            feedback = open.GetComponent<ButtonFeedback>();
            if (feedback == null) feedback = open.gameObject.AddComponent<ButtonFeedback>();
            feedback.UseControlledFeedback();
            music.SetValueWithoutNotify(PlayerPrefs.GetFloat("UI.Music", .7f));
            sfx.SetValueWithoutNotify(PlayerPrefs.GetFloat("UI.SFX", 1));
            haptic.SetIsOnWithoutNotify(HapticEnabled);
            popup.SetActive(false);
        }
        private void OnEnable()
        {
            home.onClick.AddListener(Home);
            replay.onClick.AddListener(Replay);
            open.onClick.AddListener(Open);
            close.onClick.AddListener(Close);
            music.onValueChanged.AddListener(Music);
            sfx.onValueChanged.AddListener(Sfx);
            haptic.onValueChanged.AddListener(Haptic);
        }
        private void Start() => AudioManager.Instance?.ApplyVolumes(music.value, sfx.value);
        private void OnDisable()
        {
            feedback.ResetScale();
            wasOpen = false;
            home.onClick.RemoveListener(Home);
            replay.onClick.RemoveListener(Replay);
            open.onClick.RemoveListener(Open);
            close.onClick.RemoveListener(Close);
            music.onValueChanged.RemoveListener(Music);
            sfx.onValueChanged.RemoveListener(Sfx);
            haptic.onValueChanged.RemoveListener(Haptic);
            PlayerPrefs.Save();
        }
        private void Open()
        {
            if (Busy || GameManager.Instance.IsGameOver || GameManager.Instance.RewardModalOpen) return;
            GetComponent<HintController>()?.Cancel();
            GameManager.Instance.RewardModalOpen = true;
            foreach (var piece in BlockBlast.Shape.TrayManager.Instance.Pieces)
                if (piece != null) piece.OnCancel(null);
            replayStatus.text = "";
            // Keep the modal and its backdrop above the authored gameplay artwork.
            popup.transform.SetAsLastSibling();
            popup.SetActive(true);
            wasOpen = true;
            feedback.SetExpanded(true);
        }
        public void Close()
        {
            if (Busy) return;
            if (IsOpen && GameManager.Instance != null) GameManager.Instance.RewardModalOpen = false;
            popup.SetActive(false);
            wasOpen = false;
            feedback.SetExpanded(false);
            PlayerPrefs.Save();
        }
        private void LateUpdate()
        {
            bool visible = popup.activeInHierarchy;
            if (visible == wasOpen) return;
            if (!visible && GameManager.Instance != null && !Busy) GameManager.Instance.RewardModalOpen = false;
            wasOpen = visible;
            feedback.SetExpanded(visible);
        }
        private void Music(float value)
        {
            PlayerPrefs.SetFloat("UI.Music", value);
            AudioManager.Instance?.ApplyVolumes(value, sfx.value);
        }
        private void Sfx(float value)
        {
            PlayerPrefs.SetFloat("UI.SFX", value);
            AudioManager.Instance?.ApplyVolumes(music.value, value);
        }
        private void Haptic(bool value) => PlayerPrefs.SetInt("UI.Haptic", value ? 1 : 0);
        private void OnApplicationPause(bool paused) { if (paused) PlayerPrefs.Save(); }
    }
}

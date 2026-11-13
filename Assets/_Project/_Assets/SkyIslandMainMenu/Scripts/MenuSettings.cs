using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using BlockBlast.Managers;
using BlockBlast.UI;

namespace BlockBlast.Menu
{
    // Uses the same preference keys, audio service and feedback as Gameplay, without its round dependencies.
    public sealed class MenuSettings : MonoBehaviour
    {
        public GameObject popup;
        public Button open, close, play;
        public Slider music, sfx;
        public Toggle haptic;
        ButtonFeedback feedback;
        public bool IsOpen => popup != null && popup.activeSelf;
        void Awake()
        {
            feedback = open.GetComponent<ButtonFeedback>();
            feedback.UseControlledFeedback();
            music.SetValueWithoutNotify(PlayerPrefs.GetFloat("UI.Music", .7f));
            sfx.SetValueWithoutNotify(PlayerPrefs.GetFloat("UI.SFX", 1));
            haptic.SetIsOnWithoutNotify(PlayerPrefs.GetInt("UI.Haptic", 1) != 0);
            popup.SetActive(false);
        }
        void OnEnable()
        {
            open.onClick.AddListener(Open); close.onClick.AddListener(Close);
            music.onValueChanged.AddListener(Music); sfx.onValueChanged.AddListener(Sfx);
            haptic.onValueChanged.AddListener(Haptic);
        }
        public void Open()
        {
            if (!play.interactable) return;
            popup.SetActive(true); popup.transform.SetAsLastSibling();
            play.interactable = false; feedback.SetExpanded(true);
        }
        public void Close()
        {
            if (!IsOpen) return;
            popup.SetActive(false); play.interactable = true;
            feedback.SetExpanded(false); PlayerPrefs.Save();
        }
        void Music(float value) { PlayerPrefs.SetFloat("UI.Music", value); Apply(); }
        void Sfx(float value) { PlayerPrefs.SetFloat("UI.SFX", value); Apply(); }
        void Haptic(bool value) => PlayerPrefs.SetInt("UI.Haptic", value ? 1 : 0);
        void Apply() => AudioManager.Instance?.ApplyVolumes(music.value, sfx.value);
        void Update()
        {
            if (IsOpen && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) Close();
        }
        void OnDisable()
        {
            open.onClick.RemoveListener(Open); close.onClick.RemoveListener(Close);
            music.onValueChanged.RemoveListener(Music); sfx.onValueChanged.RemoveListener(Sfx);
            haptic.onValueChanged.RemoveListener(Haptic);
            if (feedback != null) feedback.ResetScale();
            PlayerPrefs.Save();
        }
    }
}

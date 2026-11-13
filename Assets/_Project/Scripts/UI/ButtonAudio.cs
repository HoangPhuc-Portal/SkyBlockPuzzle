using UnityEngine;
using UnityEngine.UI;
using BlockBlast.Managers;
namespace BlockBlast.UI
{
    // Button.onClick covers completed pointer clicks and keyboard/controller Submit.
    // Pointer down/drag-away and disabled Buttons do not invoke it.
    [DisallowMultipleComponent, RequireComponent(typeof(Button))]
    public sealed class ButtonAudio : MonoBehaviour
    {
        private Button button;
        private void Awake() => button = GetComponent<Button>();
        private void OnEnable() => button.onClick.AddListener(Play);
        private void OnDisable() => button.onClick.RemoveListener(Play);
        private void Play() => AudioManager.Instance?.PlayButtonSound();
    }
}

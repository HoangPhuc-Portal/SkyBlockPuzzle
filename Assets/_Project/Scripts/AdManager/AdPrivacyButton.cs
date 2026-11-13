using UnityEngine;
using UnityEngine.UI;

// Keep the privacy entry accessible wherever UMP requires it, without moving existing controls.
public sealed class AdPrivacyButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private CanvasGroup visibility;
    private void OnEnable() => button.onClick.AddListener(Open);
    private void OnDisable() => button.onClick.RemoveListener(Open);
    private void Update()
    {
        bool visible = AdsManager.Instance != null && AdsManager.Instance.PrivacyOptionsRequired;
        visibility.alpha = visible ? 1 : 0;
        visibility.blocksRaycasts = visibility.interactable = visible;
    }
    private void Open() => AdsManager.Instance?.ShowPrivacyOptions();
}

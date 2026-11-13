using UnityEngine;
using BlockBlast.Shape;
using BlockBlast.Grid;

namespace BlockBlast.UI
{
    public sealed class TrayController : MonoBehaviour
    {
        [SerializeField] private UITheme theme;
        [SerializeField] private TrayManager tray;
        [SerializeField] private CanvasGroup[] slots;
        [SerializeField] private float slotFadeSeconds = .2f;
        [SerializeField] private bool anchorToSafeBottom = true;

        private void OnEnable() => tray.HandChanged += Refresh;
        private void OnDisable()
        {
            tray.HandChanged -= Refresh;
            StopAllCoroutines();
        }
        private void Start()
        {
            var rect = (RectTransform)transform;
            if (anchorToSafeBottom)
            {
                rect.anchorMin = new Vector2(rect.anchorMin.x, 0);
                rect.offsetMin = new Vector2(rect.offsetMin.x, theme.bottomInset);
            }
            Refresh();
        }
        private void Refresh()
        {
            StopAllCoroutines();
            for (int i = 0; i < slots.Length; i++)
            {
                bool filled = i < tray.Pieces.Count && tray.Pieces[i] != null;
                var group = slots[i];
                float from = group.alpha;
                StartCoroutine(UITween.Run(slotFadeSeconds,
                    t => group.alpha = Mathf.Lerp(from, filled ? 1 : 0, t)));
                if (!filled) continue;
                var piece = tray.Pieces[i];
                var view = piece.GetComponent<PieceView>();
                view.Configure(theme);
                piece.RefreshPlacementAvailability();
            }
        }
    }
}

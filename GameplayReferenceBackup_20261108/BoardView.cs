using UnityEngine;
using BlockBlast.Grid;

namespace BlockBlast.UI
{
    [DefaultExecutionOrder(100)]
    public sealed class BoardView : MonoBehaviour
    {
        [SerializeField] private UITheme theme;
        [SerializeField] private GridBoard board;
        [SerializeField] private RectTransform viewport;
        [SerializeField] private SpriteRenderer panel;
        [SerializeField] private SpriteRenderer frame;
        private readonly Vector3[] corners = new Vector3[4];
        private Rect lastBounds;
        private Vector2Int lastScreen;
        public UITheme Theme => theme;

        private void OnEnable() => Canvas.willRenderCanvases += Layout;
        private void OnDisable() => Canvas.willRenderCanvases -= Layout;

        private void Start()
        {
            if (board != null)
            {
                var existingCrest = board.transform.Find("ReferenceBoardCrest");
                if (existingCrest != null) Destroy(existingCrest.gameObject);
            }
            foreach (var cell in board.GetComponentsInChildren<GridCell>())
                cell.GetComponent<CellView>().Configure(theme);
            Layout();
        }

        // Only a resolution/safe-area change triggers reflow. No per-frame board rebuilding.
        private void Layout()
        {
            if (board == null || viewport == null || theme == null || Camera.main == null) return;
            var canvas = viewport.GetComponentInParent<Canvas>();
            var uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            viewport.GetWorldCorners(corners);
            Vector2 low = RectTransformUtility.WorldToScreenPoint(uiCamera, corners[0]);
            Vector2 high = RectTransformUtility.WorldToScreenPoint(uiCamera, corners[2]);
            var bounds = new Rect(low, high - low);
            var screen = new Vector2Int(Screen.width, Screen.height);
            if (lastBounds == bounds && lastScreen == screen) return;
            lastBounds = bounds;
            lastScreen = screen;

            float referenceWidth = GridBoard.GRID_SIZE * theme.cellPixels
                + (GridBoard.GRID_SIZE - 1) * theme.gap + 2 * (theme.padding + theme.frame);
            float fitWidth = bounds.width, fitHeight = bounds.height;
            if (theme.scaleFrameArtwork)
            {
                var inset = theme.frameArtworkInsets;
                float innerFraction = 1 - 2 * theme.frame / referenceWidth;
                fitWidth *= Mathf.Max(.1f, 1 - inset.x - inset.z) / innerFraction;
                fitHeight *= Mathf.Max(.1f, 1 - inset.y - inset.w) / innerFraction;
            }
            float pixels = Mathf.Min(fitWidth, fitHeight, Mathf.Min(referenceWidth, theme.boardMaxWidth) * canvas.scaleFactor);
            var camera = Camera.main;
            var plane = new Plane(Vector3.forward, new Vector3(0, 0, board.BoardZ));
            var center = (low + high) * .5f;
            var ray = camera.ScreenPointToRay(center);
            if (!plane.Raycast(ray, out float distance)) return;
            Vector3 world = ray.GetPoint(distance);
            var edgeRay = camera.ScreenPointToRay(center + Vector2.right * pixels * .5f);
            if (!plane.Raycast(edgeRay, out float edgeDistance)) return;
            float width = Vector3.Distance(world, edgeRay.GetPoint(edgeDistance)) * 2;
            if (theme.scaleFrameArtwork)
            {
                var inset = theme.frameArtworkInsets;
                float inner = width - 2 * width / referenceWidth * theme.frame;
                float frameWidth = inner / Mathf.Max(.1f, 1 - inset.x - inset.z);
                float frameHeight = inner / Mathf.Max(.1f, 1 - inset.y - inset.w);
                world -= new Vector3((inset.z - inset.x) * frameWidth * .5f,
                    (inset.w - inset.y) * frameHeight * .5f, 0);
            }
            board.Reflow(world, width / referenceWidth * (theme.cellPixels + theme.gap),
                width / referenceWidth * theme.cellPixels);
            if (frame != null)
            {
                frame.size = Vector2.one * width;
                if (theme.scaleFrameArtwork && frame.sprite != null)
                {
                    frame.drawMode = SpriteDrawMode.Simple;
                    var artwork = frame.sprite.bounds.size;
                    var inset = theme.frameArtworkInsets;
                    float inner = width - 2 * width / referenceWidth * theme.frame;
                    float frameWidth = inner / Mathf.Max(.1f, 1 - inset.x - inset.z);
                    float frameHeight = inner / Mathf.Max(.1f, 1 - inset.y - inset.w);
                    frame.transform.localScale = new Vector3(frameWidth / artwork.x, frameHeight / artwork.y, 1);
                    frame.transform.rotation = Quaternion.identity;
                    frame.transform.position = board.transform.position + new Vector3(
                        (inset.z - inset.x) * frameWidth * .5f,
                        (inset.w - inset.y) * frameHeight * .5f, 0);

                }
            }
            if (panel != null) panel.size = Vector2.one * (width - 2 * width / referenceWidth * theme.frame);
        }
    }
}

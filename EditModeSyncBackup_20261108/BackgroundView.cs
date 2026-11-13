using UnityEngine;
namespace BlockBlast.UI
{
    public sealed class BackgroundView : MonoBehaviour
    {
        [SerializeField] private UITheme theme;
        [SerializeField] private SpriteRenderer dim;
        [SerializeField] private SpriteRenderer soft;
        [SerializeField] private Transform plane;
        private Vector2Int lastSize;
        [SerializeField] private SpriteRenderer background;
        [SerializeField] private RectTransform artworkViewport;
        [SerializeField] private SpriteRenderer extendedSky;
        private readonly Vector3[] corners = new Vector3[4];
        private Rect lastArtworkBounds;
        private MaterialPropertyBlock skyProperties;

        private void OnEnable() => Canvas.willRenderCanvases += Layout;
        private void OnDisable() => Canvas.willRenderCanvases -= Layout;
        private void Layout()
        {
            var size = new Vector2Int(Screen.width, Screen.height);
            Rect artworkBounds = default;
            if (artworkViewport != null)
            {
                artworkViewport.GetWorldCorners(corners);
                artworkBounds = new Rect(corners[0], corners[2] - corners[0]);
            }
            if ((size == lastSize && artworkBounds == lastArtworkBounds) || Camera.main == null) return;
            lastArtworkBounds = artworkBounds;
            lastSize = size;
            var camera = Camera.main;
            var worldPlane = new Plane(Vector3.forward, new Vector3(0, 0, plane.position.z));
            for (int i = 0; i < 4; i++)
            {
                Vector2 pixel = new Vector2((i == 1 || i == 2) ? size.x : 0, i >= 2 ? size.y : 0);
                var ray = camera.ScreenPointToRay(pixel);
                if (!worldPlane.Raycast(ray, out float distance)) return;
                corners[i] = ray.GetPoint(distance);
            }
            float minX = Mathf.Min(corners[0].x, corners[3].x);
            float maxX = Mathf.Max(corners[1].x, corners[2].x);
            Vector3 center = new Vector3((minX + maxX) * .5f, (corners[0].y + corners[2].y) * .5f, plane.position.z);
            Vector2 extent = new Vector2(maxX - minX, corners[2].y - corners[0].y) * 1.02f;
            if (extendedSky != null && extendedSky.sprite != null)
            {
                extendedSky.transform.position = new Vector3(center.x, center.y, extendedSky.transform.position.z);
                var skySize = extendedSky.sprite.bounds.size;
                float skyCover = Mathf.Max(extent.x / skySize.x, extent.y / skySize.y);
                extendedSky.transform.localScale = new Vector3(skyCover, skyCover, 1);
            }
            if (background != null && background.sprite != null)
            {
                var native = background.sprite.bounds.size;
                Vector3 artCenter = center;
                Vector2 artExtent = extent;
                if (artworkViewport != null)
                {
                    artworkViewport.GetWorldCorners(corners);
                    var canvas = artworkViewport.GetComponentInParent<Canvas>();
                    var uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
                    var low = RectTransformUtility.WorldToScreenPoint(uiCamera, corners[0]);
                    var high = RectTransformUtility.WorldToScreenPoint(uiCamera, corners[2]);
                    if (extendedSky != null)
                    {
                        if (skyProperties == null) skyProperties = new MaterialPropertyBlock();
                        extendedSky.GetPropertyBlock(skyProperties);
                        skyProperties.SetVector("_ArtworkRect", new Vector4(low.x / size.x, low.y / size.y,
                            (high.x - low.x) / size.x, (high.y - low.y) / size.y));
                        extendedSky.SetPropertyBlock(skyProperties);
                    }
                    var a = camera.ScreenPointToRay(low);
                    var b = camera.ScreenPointToRay(high);
                    if (worldPlane.Raycast(a, out float da) && worldPlane.Raycast(b, out float db))
                    {
                        var first = a.GetPoint(da); var last = b.GetPoint(db);
                        artCenter = (first + last) * .5f;
                        artExtent = new Vector2(last.x - first.x, last.y - first.y);
                    }
                }
                float cover = Mathf.Max(artExtent.x / native.x, artExtent.y / native.y);
                background.transform.position = new Vector3(artCenter.x, artCenter.y, background.transform.position.z);
                background.transform.localScale = new Vector3(cover, cover, 1);
            }
            foreach (var renderer in new[] { dim, soft })
            {
                renderer.transform.SetPositionAndRotation(center, Quaternion.identity);
                renderer.transform.localScale = new Vector3(extent.x / renderer.sprite.bounds.size.x,
                    extent.y / renderer.sprite.bounds.size.y, 1);
            }
            dim.color = theme.overlay;
        }
    }
}

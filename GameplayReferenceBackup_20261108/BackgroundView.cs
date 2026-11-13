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
        private readonly Vector3[] corners = new Vector3[4];

        private void OnEnable() => Canvas.willRenderCanvases += Layout;
        private void OnDisable() => Canvas.willRenderCanvases -= Layout;
        private void Layout()
        {
            var size = new Vector2Int(Screen.width, Screen.height);
            if (size == lastSize || Camera.main == null) return;
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
            if (background != null && background.sprite != null)
            {
                var native = background.sprite.bounds.size;
                float cover = Mathf.Max(extent.x / native.x, extent.y / native.y);
                background.transform.position = new Vector3(center.x, center.y, background.transform.position.z);
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

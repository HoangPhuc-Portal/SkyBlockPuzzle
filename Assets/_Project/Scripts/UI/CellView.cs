using UnityEngine;
namespace BlockBlast.UI
{
    [RequireComponent(typeof(BlockBlast.Grid.GridCell))]
    public sealed class CellView : MonoBehaviour
    {
        [SerializeField] private UITheme theme;
        [SerializeField] private SpriteRenderer background;
        private Coroutine placement;
        private Transform animatedFill;
        private Vector3 restScale;
        private SpriteRenderer placementGlow;
        public UITheme Theme => theme;
        public bool IsAnimating => placement != null;

        public void Configure(UITheme value)
        {
            theme = value;
            if (background == null) background = GetComponent<SpriteRenderer>();
            background.sprite = theme.cellSprite;
            background.drawMode = SpriteDrawMode.Sliced;
            background.size = Vector2.one * .96f;
            background.color = Color.white;
        }

        public void AnimatePlacement(Transform fill, SpriteRenderer glowSource = null)
        {
            if (theme == null || !isActiveAndEnabled) return;
            ResetAnimation();
            animatedFill = fill;
            restScale = fill.localScale;
            // Reuse the existing outline artwork/material, independently of line-preview glow.
            if (placementGlow == null && glowSource != null)
            {
                var go = new GameObject("PlacementGlow");
                go.transform.SetParent(transform, false);
                go.transform.localPosition = glowSource.transform.localPosition;
                go.transform.localScale = glowSource.transform.localScale;
                placementGlow = go.AddComponent<SpriteRenderer>();
                placementGlow.sprite = glowSource.sprite;
                placementGlow.drawMode = glowSource.drawMode;
                placementGlow.size = glowSource.size;
                placementGlow.sharedMaterial = glowSource.sharedMaterial;
                placementGlow.sortingLayerID = glowSource.sortingLayerID;
                placementGlow.sortingOrder = fill.GetComponent<SpriteRenderer>().sortingOrder + 1;
            }
            float bounce = Mathf.Max(.01f, theme.placementTime);
            float flash = Mathf.Max(.01f, theme.placementHighlightDuration);
            float duration = Mathf.Max(bounce, flash);
            placement = StartCoroutine(UITween.RunRealtime(duration, t =>
            {
                float b = Mathf.Clamp01(t * duration / bounce);
                // One small overshoot, exact rest scale at completion. Only visual child moves.
                fill.localScale = restScale * (1 + theme.placeBounceStrength * Mathf.Sin(b * Mathf.PI));
                if (placementGlow != null)
                {
                    float a = Mathf.Clamp01(1 - t * duration / flash);
                    placementGlow.enabled = a > 0;
                    placementGlow.color = new Color(1, 1, 1, theme.placementHighlightIntensity * a);
                }
            }, () => { RestoreVisual(); placement = null; }));
        }

        private void RestoreVisual()
        {
            if (animatedFill != null) animatedFill.localScale = restScale;
            animatedFill = null;
            if (placementGlow != null) placementGlow.enabled = false;
        }

        public void ResetAnimation()
        {
            if (placement != null) StopCoroutine(placement);
            placement = null;
            RestoreVisual();
        }
        private void OnEnable() => BlockBlast.Core.GameEvents.OnGameOver += ResetAnimation;
        private void OnDisable()
        {
            BlockBlast.Core.GameEvents.OnGameOver -= ResetAnimation;
            ResetAnimation();
        }
    }
}

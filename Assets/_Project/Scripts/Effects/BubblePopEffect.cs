using BlockBlast.Core;
using UnityEngine;

namespace BlockBlast.Effects
{
    public sealed class BubblePopEffect : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer blockVisual;
        [SerializeField] private SpriteRenderer[] bubbleVisuals;
        [SerializeField] private SpriteRenderer ringVisual;

        private BubblePopConfig config;
        private ClearedCellInfo snapshot;
        private float elapsed;
        private float delay;
        private float extent;
        private Color burstColor;
        private float intensity = 1f;
        private LineRenderer outline;

        public bool IsConfigured => blockVisual != null && ringVisual != null &&
            bubbleVisuals != null && bubbleVisuals.Length > 0;
        public int BubbleCapacity => bubbleVisuals != null ? bubbleVisuals.Length : 0;

        public void Initialize(BubblePopConfig settings)
        {
            config = settings;
            if (outline == null)
            {
                var child = new GameObject("GoldenOutline");
                child.transform.SetParent(transform, false);
                outline = child.AddComponent<LineRenderer>();
                outline.sharedMaterial = ringVisual.sharedMaterial;
                outline.useWorldSpace = false;
                outline.loop = true;
                outline.positionCount = 16;
                outline.numCornerVertices = 3;
            }
            for (int i = 0; i < bubbleVisuals.Length; i++)
            {
                bubbleVisuals[i].sprite = config.bubbleSprite;
            }
            ringVisual.sprite = config.ringSprite;
            ResetVisuals();
        }

        public void Begin(ClearedCellInfo cell, float startDelay, float strength = 1f)
        {
            ResetVisuals();
            snapshot = cell;
            intensity = Mathf.Clamp(strength, 1f, 2f);
            delay = startDelay;
            elapsed = 0f;
            transform.SetPositionAndRotation(cell.Position, cell.Rotation);
            transform.localScale = Vector3.one;
            extent = cell.Sprite != null
                ? Mathf.Max(Mathf.Abs(cell.Sprite.bounds.size.x * cell.Scale.x),
                    Mathf.Abs(cell.Sprite.bounds.size.y * cell.Scale.y)) : 1f;
            burstColor=cell.Color;
            if(config.theme!=null){int index=System.Array.IndexOf(config.theme.blockSprites,cell.Sprite);if(index>=0&&index<6)burstColor=config.theme.blockColors[index];}
            blockVisual.sprite = cell.Sprite;
            blockVisual.color = cell.Color;
            blockVisual.transform.localScale = cell.Scale;
            blockVisual.enabled = cell.Sprite != null;
            SetSorting(blockVisual, cell);
            for (int i = 0; i < bubbleVisuals.Length; i++)
            {
                SetSorting(bubbleVisuals[i], cell);
                bubbleVisuals[i].enabled = false;
            }
            SetSorting(ringVisual, cell);
            outline.sortingLayerID = cell.SortingLayerID;
            outline.sortingOrder = cell.SortingOrder + config.sortingOrderOffset + 1;
            outline.widthMultiplier = extent * config.outlineWidth;
            // Rounded square matching the block silhouette, allocated only once per pooled effect.
            float half = extent * .47f, radius = extent * .09f;
            for (int corner = 0; corner < 4; corner++)
            {
                float angle = corner * Mathf.PI * .5f;
                Vector3 center = new Vector3(corner == 0 || corner == 3 ? 1 : -1,
                    corner < 2 ? 1 : -1, 0) * (half - radius);
                for (int step = 0; step < 4; step++)
                {
                    float a = angle + step * Mathf.PI / 6f;
                    outline.SetPosition(corner * 4 + step, center + new Vector3(Mathf.Cos(a), Mathf.Sin(a), 0) * radius);
                }
            }
            ringVisual.enabled = false;
            gameObject.SetActive(true);
        }

        private void SetSorting(SpriteRenderer renderer, ClearedCellInfo cell)
        {
            renderer.sortingLayerID = cell.SortingLayerID;
            renderer.sortingOrder = cell.SortingOrder + config.sortingOrderOffset;
        }

        private static void SetSize(SpriteRenderer renderer, float size)
        {
            if (renderer.sprite == null) return;
            Vector3 bounds = renderer.sprite.bounds.size;
            renderer.transform.localScale = new Vector3(size / bounds.x, size / bounds.y, 1f);
        }

        // Called only by the event-started runner; there is no gameplay polling in Update.
        public bool Tick(float deltaTime)
        {
            elapsed += deltaTime;
            float time = elapsed - config.clearPreviewDuration - delay;
            float borderAlpha = time < 0 ? Mathf.Clamp01(elapsed / .025f) : 1f - Mathf.Clamp01(time / config.duration);
            Color gold = config.energyColor;
            gold.a *= borderAlpha * Mathf.Min(1f, .8f * intensity);
            outline.startColor = outline.endColor = gold;
            outline.enabled = borderAlpha > 0;
            if (time < 0f)
            {
                // This is a detached snapshot: the real cell is already empty and reusable.
                float pulse = Mathf.Sin(Mathf.Clamp01(elapsed / Mathf.Max(.001f, config.clearPreviewDuration)) * Mathf.PI);
                Color bright = snapshot.Color * (1 + config.clearHighlightIntensity * intensity * pulse);
                bright.a = snapshot.Color.a;
                blockVisual.color = bright;
                blockVisual.transform.localScale = snapshot.Scale;
                ringVisual.enabled = false;
                ringVisual.color = new Color(1, 1, 1, config.clearHighlightIntensity * intensity * pulse);
                SetSize(ringVisual, extent * config.clearEffectScale * 1.05f);
                return false;
            }
            if (time >= config.duration) return true;
            float inflateTime = config.duration * config.inflateFraction;
            if (time < inflateTime)
            {
                ringVisual.enabled = false;
                float amount = config.inflateCurve.Evaluate(time / Mathf.Max(.001f, inflateTime));
                blockVisual.color = snapshot.Color;
                blockVisual.transform.localScale = snapshot.Scale * Mathf.Lerp(1, 1 + (config.inflateScale - 1) * intensity, amount);
                return false;
            }

            float fraction = Mathf.Clamp01((time - inflateTime) / Mathf.Max(.001f, config.duration - inflateTime));
            blockVisual.enabled = fraction < 1f;
            blockVisual.sprite = snapshot.Sprite;
            blockVisual.transform.localScale = snapshot.Scale * (1 + (config.inflateScale - 1) * intensity) * (1 - fraction);
            blockVisual.color = new Color(snapshot.Color.r, snapshot.Color.g, snapshot.Color.b, snapshot.Color.a * (1 - fraction));
            float movement = config.burstCurve.Evaluate(fraction);
            Color color = config.useBlockColor ? burstColor : config.bubbleColor;
            color.a *= Mathf.Clamp01(config.fadeCurve.Evaluate(fraction));
            for (int i = 0; i < bubbleVisuals.Length; i++)
            {
                var bubble = bubbleVisuals[i];
                bubble.enabled = i < Mathf.Min(bubbleVisuals.Length, Mathf.CeilToInt(config.bubbleCount * intensity));
                if (!bubble.enabled) continue;
                Color shard = i % 3 == 0 ? config.energyColor : i % 3 == 1 ? Color.white : color;
                shard.a = color.a;
                bubble.color = shard;
                float angle = i * Mathf.PI * 2f / Mathf.Min(bubbleVisuals.Length, Mathf.CeilToInt(config.bubbleCount * intensity));
                Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
                bubble.transform.localPosition = direction * (extent * config.travelDistance * movement * config.clearEffectScale * intensity)+Vector3.down*(config.gravity*extent*fraction*fraction);
                SetSize(bubble, extent * config.bubbleSize * config.clearEffectScale * (1f - fraction));
                bubble.transform.localRotation = Quaternion.Euler(0, 0, i * 67f + fraction * 160f);
                bubble.transform.localScale = Vector3.Scale(bubble.transform.localScale, new Vector3(.65f, 1f, 1f));
            }

            float ringFraction = Mathf.Clamp01((time - inflateTime) / config.ringDuration);
            ringVisual.enabled = ringVisual.sprite != null && ringFraction < 1f;
            if (ringVisual.enabled)
            {
                color = config.energyColor;
                color.a *= 1f - ringFraction;
                ringVisual.color = color;
                SetSize(ringVisual, extent * config.clearEffectScale * Mathf.Lerp(config.ringStartSize, config.ringEndSize * intensity, ringFraction));
            }
            return false;
        }

        public void ResetVisuals()
        {
            blockVisual.enabled = false;
            blockVisual.sprite = null;
            ringVisual.enabled = false;
            if (outline != null) outline.enabled = false;
            foreach (var bubble in bubbleVisuals) bubble.enabled = false;
            gameObject.SetActive(false);
        }
    }
}

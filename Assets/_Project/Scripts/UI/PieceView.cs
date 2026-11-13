using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlockBlast.UI
{
    public sealed class PieceView : MonoBehaviour
    {
        [SerializeField] UITheme theme;
        public UITheme Theme => theme;
        readonly List<Outline> dragOutlines = new List<Outline>();
        Coroutine glowTween;
        public bool IsDragGlowActive { get; private set; }
        public void Configure(UITheme value) { theme = value; }

        public void Appearance(int index, bool highlighted, bool disabled)
        {
            if (theme == null) return;
            var owner = GetComponent<BlockBlast.Shape.DraggableShape>();
            disabled |= owner != null && !owner.CanPlaceOnBoard;
            var sprite = disabled ? theme.disabledSprites[index] : highlighted ? theme.highlightedSprites[index] : theme.blockSprites[index];
            foreach (var image in GetComponentsInChildren<Image>())
            {
                if (image.transform == transform) continue;
                image.sprite = sprite;
                image.color = owner != null ? owner.BlockTint : theme.TintFor(index);
            }
        }

        public void SetDragging(bool dragging)
        {
            if (glowTween != null) StopCoroutine(glowTween);
            glowTween = null;
            IsDragGlowActive = dragging && theme != null && isActiveAndEnabled;
            if (!IsDragGlowActive)
            {
                foreach (var outline in dragOutlines) if (outline != null) outline.enabled = false;
                return;
            }
            dragOutlines.Clear();
            foreach (var image in GetComponentsInChildren<Image>())
            {
                if (image.transform == transform) continue;
                var outline = image.GetComponent<Outline>();
                if (outline == null) outline = image.gameObject.AddComponent<Outline>();
                outline.effectDistance = Vector2.one * theme.dragGlowPixels;
                outline.useGraphicAlpha = true;
                outline.enabled = true;
                dragOutlines.Add(outline);
            }
            var owner = GetComponent<BlockBlast.Shape.DraggableShape>();
            Color tint = Color.Lerp(owner != null ? theme.blockColors[owner.PaletteIndex] : Color.white, Color.white, .7f);
            glowTween = StartCoroutine(UITween.RunRealtime(theme.dragLiftTime, t =>
            {
                tint.a = theme.dragGlowIntensity * UITween.Quad(t);
                foreach (var outline in dragOutlines) if (outline != null) outline.effectColor = tint;
            }, () => glowTween = null));
        }
        void OnDisable() => SetDragging(false);
    }
}

using UnityEngine;

namespace BlockBlast.Grid
{
    public class GridCell : MonoBehaviour
    {
        [Header("Renderers")]
        [SerializeField] private Renderer cellBackgroundRenderer;
        [SerializeField] private GameObject fillVisual;
        [SerializeField] private Renderer fillRenderer;
        [SerializeField] private GameObject highlightGlowVisual;

        public int Row { get; private set; }
        public int Col { get; private set; }
        private BoardModel model;
        private bool localOccupied;
        public bool IsOccupied => model!=null ? model.Occupied(Row,Col) : localOccupied;
        public void BindModel(BoardModel value){model=value;if(model!=null)model.Set(Row,Col,localOccupied);}
        public Color CurrentColor { get; private set; }
        public Sprite CurrentSprite { get; private set; }
        private SpriteRenderer ghost;
        private Sprite defaultFillSprite;
        private Vector3 defaultFillScale;
        private Vector3 defaultFillSize;
        private bool initializedVisual;

        private void InitializeVisual()
        {
            if (initializedVisual || !(fillRenderer is SpriteRenderer renderer)) return;
            defaultFillSprite = renderer.sprite;
            defaultFillScale = renderer.transform.localScale;
            if (defaultFillSprite != null)
                defaultFillSize = Vector3.Scale(defaultFillSprite.bounds.size, defaultFillScale);
            else
            {
                // Reimports can invalidate the prefab's sprite sub-asset reference.
                // Use the cell surface as the sizing reference, not the replacement artwork.
                var background = cellBackgroundRenderer as SpriteRenderer;
                if (background != null)
                {
                    Vector3 size = background.drawMode == SpriteDrawMode.Simple
                        ? (background.sprite != null ? background.sprite.bounds.size : Vector3.zero)
                        : (Vector3)background.size;
                    Vector3 parentScale = renderer.transform.parent != null
                        ? renderer.transform.parent.lossyScale : Vector3.one;
                    Vector3 worldSize = Vector3.Scale(size, background.transform.lossyScale);
                    defaultFillSize = new Vector3(
                        Mathf.Abs(parentScale.x) > Mathf.Epsilon ? worldSize.x / parentScale.x : 0,
                        Mathf.Abs(parentScale.y) > Mathf.Epsilon ? worldSize.y / parentScale.y : 0, 0);
                }
                var theme = GetComponent<BlockBlast.UI.CellView>()?.Theme;
                defaultFillSprite = theme != null ? theme.whiteBlock : null;
            }
            initializedVisual = true;
        }

        private void ShowSprite(Sprite sprite)
        {
            InitializeVisual();
            if (!(fillRenderer is SpriteRenderer renderer)) return;
            renderer.sprite = sprite != null ? sprite : defaultFillSprite;
            if (renderer.sprite == null) return;
            Vector3 bounds = renderer.sprite.bounds.size;
            Vector3 size = defaultFillSize;
            if (size.x <= 0 || size.y <= 0) return;
            renderer.transform.localScale = new Vector3(
                bounds.x > 0f ? size.x / bounds.x : defaultFillScale.x,
                bounds.y > 0f ? size.y / bounds.y : defaultFillScale.y, defaultFillScale.z);
        }

        public Core.ClearedCellInfo CaptureVisual()
        {
            GetComponent<BlockBlast.UI.CellView>()?.ResetAnimation();
            var renderer = fillRenderer as SpriteRenderer;
            return new Core.ClearedCellInfo(Row, Col, renderer != null ? renderer.transform.position : transform.position,
                CurrentSprite, CurrentColor, renderer != null ? renderer.transform.lossyScale : transform.lossyScale,
                renderer != null ? renderer.transform.rotation : transform.rotation,
                fillRenderer != null ? fillRenderer.sortingLayerID : 0, fillRenderer != null ? fillRenderer.sortingOrder : 0);
        }

        private void Awake()
        {
            if (cellBackgroundRenderer == null || fillVisual == null || fillRenderer == null || highlightGlowVisual == null)
                Debug.LogError("GridCell requires background, fill and highlight references.", this);
        }

        private static void SetColor(Renderer renderer, Color color)
        {
            if (renderer is SpriteRenderer sprite) sprite.color = color;
            else if (renderer != null)
            {
                var properties = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(properties);
                properties.SetColor("_BaseColor", color);
                properties.SetColor("_Color", color);
                renderer.SetPropertyBlock(properties);
            }
        }

        public void Init(int row, int col)
        {
            Row = row;
            Col = col;
            SetOccupied(false);
        }

        public void SetOccupied(bool occupied, Color color = default, Sprite sprite = null)
        {
            GetComponent<BlockBlast.UI.CellView>()?.ResetAnimation();
            InitializeVisual();
            CurrentSprite = occupied ? (sprite != null ? sprite : defaultFillSprite) : null;
            localOccupied = occupied;
            if(model!=null)model.Set(Row,Col,occupied);
            CurrentColor = color;
            SetStateNormal();
            if(occupied){var view=GetComponent<BlockBlast.UI.CellView>();if(view!=null)view.AnimatePlacement(fillRenderer.transform, highlightGlowVisual != null ? highlightGlowVisual.GetComponent<SpriteRenderer>() : null);}
        }

        public void SetStateNormal()
        {
            if(ghost!=null)ghost.enabled=false;
            if (highlightGlowVisual != null) highlightGlowVisual.SetActive(false);
            if (fillVisual != null) fillVisual.SetActive(IsOccupied);
            if (GetComponent<BlockBlast.UI.CellView>()?.IsAnimating != true)
                ShowSprite(IsOccupied ? CurrentSprite : defaultFillSprite);
            if (IsOccupied) SetColor(fillRenderer, CurrentColor);
        }

        public void SetStatePreview(Color color, Sprite sprite = null)
        {
            var theme=GetComponent<BlockBlast.UI.CellView>()?.Theme;
            ShowGhost(theme!=null?theme.ghostValid:color,theme!=null?theme.whiteBlock:sprite);
        }
        private void ShowGhost(Color color, Sprite sprite)
        {
            InitializeVisual();
            var source = fillRenderer as SpriteRenderer;
            if (source == null) return;
            Sprite previewSprite = sprite != null ? sprite : defaultFillSprite;
            if (previewSprite == null || previewSprite.bounds.size.x <= Mathf.Epsilon ||
                previewSprite.bounds.size.y <= Mathf.Epsilon ||
                defaultFillSize.x <= 0 || defaultFillSize.y <= 0)
            {
                if (ghost != null) ghost.enabled = false;
                return;
            }
            if (ghost == null)
            {
                var go = new GameObject("Ghost");
                go.transform.SetParent(transform, false);
                ghost = go.AddComponent<SpriteRenderer>();
                ghost.sharedMaterial = source.sharedMaterial;
                ghost.sortingLayerID = source.sortingLayerID;
                ghost.sortingOrder = source.sortingOrder + 2;
            }
            ghost.sprite = previewSprite;
            ghost.color = color;
            ghost.transform.localScale = new Vector3(
                defaultFillSize.x / previewSprite.bounds.size.x,
                defaultFillSize.y / previewSprite.bounds.size.y, 1);
            ghost.enabled = true;
        }
        public void SetStateInvalidPreview(){var theme=GetComponent<BlockBlast.UI.CellView>()?.Theme;ShowGhost(theme!=null?theme.ghostInvalid:new Color(.937f,.267f,.267f,.6f),theme!=null?theme.whiteBlock:null);}
        public void SetStateHighlightLine()
        {
            if (highlightGlowVisual == null) return;
            var glow = highlightGlowVisual.GetComponent<SpriteRenderer>();
            if (glow != null && fillRenderer != null)
            {
                // Preview must cover both occupied blocks and the ghost (fill + 2).
                // Also handles authored cells with an older prefab sorting override.
                glow.sortingLayerID = fillRenderer.sortingLayerID;
                glow.sortingOrder = fillRenderer.sortingOrder + 3;
                glow.enabled = true;
            }
            highlightGlowVisual.SetActive(true);
        }
    }
}

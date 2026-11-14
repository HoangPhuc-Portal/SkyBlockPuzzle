using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using BlockBlast.Data;
using BlockBlast.Grid;

namespace BlockBlast.Shape
{
    /// <summary>UI shape and input adapter. GridBoard owns validation and placement.</summary>
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public class DraggableShape : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, ICancelHandler
    {
        [Header("Drag Settings")]
        [SerializeField] private float dragOffsetYScreen = 100f;

        [Header("Tray Display")]
        [SerializeField] private float trayTileSize = 60f;
        [SerializeField, Range(0.8f, 1f)] private float tileFillRatio = 0.92f;
        [SerializeField] private float hitAreaPadding = 40f;

        [Header("Prefab for UI Tiles")]
        [SerializeField] private GameObject blockTilePrefab;

        public ShapeRuntimeData ShapeData { get; private set; }
        public Sprite BlockSprite { get; private set; }
        public bool CanPlaceOnBoard { get; private set; } = true;

        public void RefreshPlacementAvailability()
        {
            CanPlaceOnBoard = ShapeData != null && GridBoard.Instance != null &&
                GridBoard.Instance.CanPlaceShapeAnywhere(ShapeData);
            if (!CanPlaceOnBoard) CancelDrag();
            if (canvasGroup != null)
            {
                canvasGroup.interactable = CanPlaceOnBoard;
                canvasGroup.blocksRaycasts = CanPlaceOnBoard && !isDragging && !returning;
            }
            var view = GetComponent<BlockBlast.UI.PieceView>();
            if (view != null) view.Appearance(PaletteIndex, false, !CanPlaceOnBoard);
        }

        private RectTransform rectTransform;
        private Canvas parentCanvas;
        private CanvasGroup canvasGroup;
        private Transform originalParent;
        private int originalSiblingIndex;
        private Vector2 originalAnchoredPosition;
        private Vector3 originalScale;
        private float currentTileSize;
        private bool isDragging;
        private bool returning;
        private Coroutine motion;
        private bool restoreOnEnable;
        public Color BlockTint {get;private set;}=Color.white;
        public int PaletteIndex {get;private set;}
        private BlockBlast.UI.UITheme Theme=>GetComponent<BlockBlast.UI.PieceView>()?.Theme;

        private void Awake()
        {
            rectTransform = (RectTransform)transform;
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        public void Setup(ShapeData data, Sprite blockSprite = null)
        {
            SetupRuntime(data != null ? data.CreateRuntime() : null, blockSprite);
        }

        // The selector has already validated this orientation; never reroll it here.
        public void SetupRuntime(ShapeRuntimeData data, Sprite blockSprite = null)
        {
            ShapeData = data;
            parentCanvas = GetComponentInParent<Canvas>();
            if (ShapeData == null || parentCanvas == null || blockTilePrefab == null)
            {
                Debug.LogError($"DraggableShape '{name}' is missing ShapeData, Canvas, or tile prefab.");
                enabled = false;
                return;
            }

            var fallbackImage = blockTilePrefab.GetComponent<Image>();
            BlockSprite = blockSprite != null ? blockSprite : fallbackImage != null ? fallbackImage.sprite : null;
            var view=GetComponent<BlockBlast.UI.PieceView>();if(view!=null&&view.Theme!=null){PaletteIndex=Random.Range(0,8);BlockSprite=view.Theme.blockSprites[PaletteIndex];BlockTint=view.Theme.TintFor(PaletteIndex);trayTileSize=view.Theme.trayCellPixels;dragOffsetYScreen=view.Theme.dragOffset;}
            BlockTint = ShapeData.blockColor;
            BuildVisualShape();
            RememberTrayTransform();
        }

        private void BuildVisualShape()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
                Destroy(transform.GetChild(i).gameObject);

            int rows = ShapeData.rows;
            int cols = ShapeData.columns;
            currentTileSize = trayTileSize;
            RectTransform slotRect = transform.parent as RectTransform;
            if (slotRect != null)
            {
                float slotExtent = Mathf.Min(slotRect.rect.width, slotRect.rect.height);
                if (slotExtent > 0f)
                {
                    // Fit each axis separately. Hit-area padding belongs to the invisible input
                    // rect, not to the artwork: a horizontal line may use the full slot width.
                    currentTileSize = Mathf.Min(trayTileSize,
                        Mathf.Max(1, slotRect.rect.width - 12) / cols,
                        Mathf.Max(1, slotRect.rect.height - 12) / rows);
                }
            }

            Image rootImage = GetComponent<Image>();
            if (rootImage != null) rootImage.color = new Color(1f, 1f, 1f, 0f);
            rectTransform.sizeDelta = new Vector2(cols * currentTileSize + hitAreaPadding, rows * currentTileSize + hitAreaPadding);

            bool[,] matrix = ShapeData.GetMatrix();
            for (int row = 0; row < rows; row++)
            for (int col = 0; col < cols; col++)
            {
                if (!matrix[row, col]) continue;
                GameObject tile = Instantiate(blockTilePrefab, transform);
                RectTransform tileRect = tile.GetComponent<RectTransform>();
                tileRect.sizeDelta = Vector2.one * currentTileSize * tileFillRatio;
                tileRect.anchoredPosition = new Vector2((col - (cols - 1) * .5f) * currentTileSize, (row - (rows - 1) * .5f) * currentTileSize);

                Image image = tile.GetComponent<Image>();
                if (image != null)
                {
                    image.sprite = BlockSprite;
                    image.color = BlockSprite != null ? BlockTint : ShapeData.blockColor;
                    image.raycastTarget = false;
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (isDragging || returning) return;
            RefreshPlacementAvailability();
            if (isDragging || returning || !CanDrag()) return;
            var hint=parentCanvas.GetComponentInChildren<BlockBlast.UI.HintController>();if(hint!=null)hint.Cancel();
            GridBoard.Instance.ClearPreviewAndHighlights();
            RememberTrayTransform();
            isDragging = true;
            GetComponent<BlockBlast.UI.PieceView>()?.SetDragging(true);
            BlockBlast.Managers.AudioManager.Instance?.PlayDragSound();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha=Theme!=null?Theme.dragAlpha:.9f;
            transform.SetParent(parentCanvas.transform, true);
            transform.SetAsLastSibling();
            StopMotion();
            Vector3 liftFrom = transform.localScale;
            Vector3 liftTo = originalScale * GetBoardScaleFactor() * (Theme != null ? Theme.dragScale : 1.1f);
            motion = StartCoroutine(BlockBlast.UI.UITween.RunRealtime(Theme != null ? Theme.dragLiftTime : .1f,
                t => transform.localScale = Vector3.Lerp(liftFrom, liftTo, BlockBlast.UI.UITween.Quad(t)),
                () => { transform.localScale = liftTo; motion = null; }));
            UpdateDrag(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!CanDrag()) { CancelDrag(); return; }
            if (isDragging) UpdateDrag(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!CanDrag()) { CancelDrag(); return; }
            if (!isDragging) return;
            isDragging = false;
            StopMotion();
            GetComponent<BlockBlast.UI.PieceView>()?.SetDragging(false);
            Vector2 screenPosition = eventData.position + Vector2.up * dragOffsetYScreen;
            bool placed = GridBoard.Instance != null && GridBoard.Instance.TryPlaceShape(ShapeData, GetShapeAnchorWorldPosition(screenPosition), BlockSprite, BlockTint);
            if (placed)
            {
                // Board already owns the committed visuals and its bounce. Do not draw a second piece.
                canvasGroup.alpha = 0;
                canvasGroup.blocksRaycasts = false;
                TrayManager.Instance?.OnShapePlaced(this);
                Discard();
                return;
            }

            GridBoard.Instance?.ClearPreviewAndHighlights();
            ReturnToTray();
        }

        private void OnEnable()
        {
            BlockBlast.Core.GameEvents.OnGameOver += CancelDrag;
        }
        private void LateUpdate()
        {
            if (restoreOnEnable) { RestoreTray(); restoreOnEnable = false; }
        }
        private void OnDisable()
        {
            BlockBlast.Core.GameEvents.OnGameOver -= CancelDrag;
            StopMotion();
            bool needsRestore = isDragging || returning;
            if (isDragging) GridBoard.Instance?.ClearPreviewAndHighlights();
            isDragging = returning = false;
            // Unity forbids reparenting/sibling changes during hierarchy deactivation/unload.
            if (needsRestore && originalParent != null)
            {
                if (gameObject.activeInHierarchy) RestoreTray();
                else restoreOnEnable = true;
            }
        }
        private void OnApplicationFocus(bool focused)
        {
            if (!focused) CancelDrag();
        }
        public void OnCancel(BaseEventData eventData) => CancelDrag();
        private void CancelDrag()
        {
            if (!isDragging && !returning) return;
            isDragging = false;
            StopMotion();
            GridBoard.Instance?.ClearPreviewAndHighlights();
            if (originalParent != null) ReturnToTray();
        }

        public void Discard()
        {
            StopMotion();
            isDragging = returning = false;
            if (canvasGroup != null) canvasGroup.blocksRaycasts = false;
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        private bool CanDrag() => !restoreOnEnable && CanPlaceOnBoard && ShapeData != null && parentCanvas != null &&
            GridBoard.Instance != null && !GridBoard.Instance.IsInputLocked && TrayManager.Instance != null;

        private void UpdateDrag(Vector2 pointerPosition)
        {
            Vector2 screenPosition = pointerPosition + Vector2.up * dragOffsetYScreen;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(parentCanvas.transform as RectTransform, screenPosition, GetUICamera(), out Vector3 worldPosition))
                transform.position = worldPosition;
            var anchor=GetShapeAnchorWorldPosition(screenPosition);GridBoard.Instance.UpdateDragPreview(ShapeData, anchor, BlockSprite);
            var view=GetComponent<BlockBlast.UI.PieceView>();if(view!=null){bool valid=GridBoard.Instance.IsValid(ShapeData,anchor);view.Appearance(PaletteIndex,valid,false);}
        }

        private void RememberTrayTransform()
        {
            originalParent = transform.parent;
            originalSiblingIndex = transform.GetSiblingIndex();
            originalAnchoredPosition = rectTransform.anchoredPosition;
            originalScale = transform.localScale;
        }

        private void ReturnToTray()
        {
            GetComponent<BlockBlast.UI.PieceView>()?.SetDragging(false);
            StopMotion();
            GetComponent<BlockBlast.UI.PieceView>()?.Appearance(PaletteIndex,false,false);
            canvasGroup.alpha=1;returning=true;
            Vector3 from=transform.position;Vector3 scale=transform.localScale;
            transform.SetParent(originalParent,true);transform.SetSiblingIndex(originalSiblingIndex);
            rectTransform.anchoredPosition=originalAnchoredPosition;Vector3 target=transform.position;transform.position=from;
            if (!isActiveAndEnabled || (GridBoard.Instance != null && GridBoard.Instance.IsInputLocked))
            { RestoreTray(); return; }
            motion = StartCoroutine(BlockBlast.UI.UITween.RunRealtime(Theme != null ? Theme.returnTime : .25f, t =>
            {
                float eased = Theme != null && Theme.returnWithOvershoot
                    ? BlockBlast.UI.UITween.Back(t) : BlockBlast.UI.UITween.Quad(t);
                transform.position = Vector3.LerpUnclamped(from, target, eased);
                transform.localScale = Vector3.LerpUnclamped(scale, originalScale, eased);
            }, () => { motion = null; RestoreTray(); }));
        }

        private void StopMotion()
        {
            if (motion != null) StopCoroutine(motion);
            motion = null;
        }

        private void RestoreTray()
        {
            if (originalParent != null)
            {
                transform.SetParent(originalParent, false);
                transform.SetSiblingIndex(originalSiblingIndex);
                rectTransform.anchoredPosition = originalAnchoredPosition;
                transform.localScale = originalScale;
            }
            returning = false;
            if (canvasGroup != null) { canvasGroup.alpha = 1; canvasGroup.blocksRaycasts = CanPlaceOnBoard; }
        }

        private float GetBoardScaleFactor()
        {
            Camera camera = Camera.main;
            GridBoard board = GridBoard.Instance;
            if (camera == null || board == null || currentTileSize <= 0f) return 1f;
            float halfCell = board.CellSize * .5f;
            float boardCellPixels = Vector2.Distance(camera.WorldToScreenPoint(board.transform.position - Vector3.right * halfCell), camera.WorldToScreenPoint(board.transform.position + Vector3.right * halfCell));
            return boardCellPixels > 0f ? boardCellPixels / (currentTileSize * parentCanvas.scaleFactor * originalScale.x) : 1f;
        }

        private Camera GetUICamera() => parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : parentCanvas.worldCamera;

        private Vector3 GetShapeAnchorWorldPosition(Vector2 screenPosition)
        {
            Camera camera = Camera.main;
            GridBoard board = GridBoard.Instance;
            if (camera == null || board == null) return new Vector3(float.MaxValue, float.MaxValue, 0f);
            Plane boardPlane = new Plane(Vector3.forward, new Vector3(0f, 0f, board.BoardZ));
            Ray ray = camera.ScreenPointToRay(screenPosition);
            if (!boardPlane.Raycast(ray, out float distance)) return new Vector3(float.MaxValue, float.MaxValue, 0f);

            Vector3 center = ray.GetPoint(distance);
            center.x -= (ShapeData.columns - 1) * .5f * board.CellSize;
            center.y -= (ShapeData.rows - 1) * .5f * board.CellSize;
            return center;
        }
    }
}

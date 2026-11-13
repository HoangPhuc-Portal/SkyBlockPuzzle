using System.Collections.Generic;
using UnityEngine;
using BlockBlast.Core;
using BlockBlast.Data;

namespace BlockBlast.Grid
{
    /// <summary>
    /// Quản lý logic bàn chơi 8x8, kiểm tra vị trí đặt khối và nhận diện xóa hàng/cột real-time
    /// </summary>
    public class GridBoard : MonoBehaviour
    {
        public static GridBoard Instance { get; private set; }

        public const int GRID_SIZE = 8;

        // Mặt phẳng Z nơi toàn bộ các ô lưới được sinh ra (xem GenerateBoard).
        // Dùng để raycast từ camera 3D xuống đúng mặt phẳng bàn cờ khi kéo-thả.
        public float BoardZ => transform.position.z;

        // Khoảng cách giữa tâm 2 ô liền kề (đơn vị world). DraggableShape dùng để đổi
        // kích thước khối khi kéo cho khớp đúng kích thước ô trên board.
        public float CellSize => cellSize;

        [Header("Board Settings")]
        [SerializeField] private GridCell cellPrefab;
        [SerializeField] private Transform boardParent;
        [SerializeField] private float cellSize = 1.1f;
        [SerializeField] private GridCell[] authoredCells = new GridCell[0];
        public bool HasAuthoredCells => authoredCells != null && authoredCells.Length == GRID_SIZE * GRID_SIZE;

        public BoardModel Model { get; private set; }
        public bool IsInputLocked => BlockBlast.Managers.GameManager.Instance != null &&
            BlockBlast.Managers.GameManager.Instance.IsInputLocked;

        public void ResetBoard()
        {
            GameEvents.OnBoardReset?.Invoke(this);
            ClearPreviewAndHighlights();
            for (int r = 0; r < GRID_SIZE; r++)
            for (int c = 0; c < GRID_SIZE; c++)
            {
                var cell = grid[r,c];
                if (cell == null) continue;
                cell.GetComponent<BlockBlast.UI.CellView>()?.ResetAnimation();
                cell.SetOccupied(false);
            }
        }
        private GridCell[,] grid = new GridCell[GRID_SIZE, GRID_SIZE];
        private List<GridCell> currentPreviewCells = new List<GridCell>();
        private List<GridCell> currentHighlightedLineCells = new List<GridCell>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else { Destroy(gameObject); return; }

            Model = new BoardModel(GRID_SIZE);
            GenerateBoard();
        }

        private ShapeRuntimeData previewShape;
        private Sprite previewSprite;
        private int previewRow = int.MinValue;
        private int previewCol = int.MinValue;

        private void OnEnable() => GameEvents.OnGameOver += ClearPreviewAndHighlights;
        private void OnDisable()
        {
            GameEvents.OnGameOver -= ClearPreviewAndHighlights;
            ClearPreviewAndHighlights();
        }
        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void GenerateBoard()
        {
            if (authoredCells != null && authoredCells.Length > 0)
            {
                var unique = new HashSet<GridCell>(authoredCells);
                if (!HasAuthoredCells || unique.Count != GRID_SIZE * GRID_SIZE || unique.Contains(null))
                {
                    Debug.LogError("Authored board requires 64 distinct assigned cells. No duplicate board will be generated.", this);
                    enabled = false;
                    return;
                }
                for (int r = 0; r < GRID_SIZE; r++) for (int c = 0; c < GRID_SIZE; c++)
                {
                    var cell = authoredCells[r * GRID_SIZE + c];
                    cell.Init(r, c); cell.BindModel(Model); grid[r, c] = cell;
                }
                return;
            }
            Vector3 origin = transform.position - new Vector3((GRID_SIZE - 1) * cellSize / 2f, (GRID_SIZE - 1) * cellSize / 2f, 0);

            for (int r = 0; r < GRID_SIZE; r++)
            {
                for (int c = 0; c < GRID_SIZE; c++)
                {
                    Vector3 cellPos = origin + new Vector3(c * cellSize, r * cellSize, 0);
                    GridCell cell = Instantiate(cellPrefab, cellPos, Quaternion.identity, boardParent);
                    cell.Init(r, c);
                    cell.BindModel(Model);
                    grid[r, c] = cell;
                }
            }
        }

        // ----------------------------------------------------------------------
        // CƠ CHẾ XEM TRƯỚC VÀ PHÁT SÁNG HÀNG/CỘT THỜI GIAN THỰC (REAL-TIME PREVIEW)
        // ----------------------------------------------------------------------
        public void UpdateDragPreview(ShapeRuntimeData shapeData, Vector3 worldPosition, Sprite sprite = null)
        {
            if (shapeData == null || !GetGridCoordinatesFromWorld(worldPosition, out int startRow, out int startCol))
            {
                ClearPreviewAndHighlights();
                return;
            }
            if (previewShape == shapeData && previewSprite == sprite && previewRow == startRow && previewCol == startCol) return;
            ClearPreviewAndHighlights();
            previewShape = shapeData;
            previewSprite = sprite;
            previewRow = startRow;
            previewCol = startCol;

            bool[,] matrix = shapeData.GetMatrix();
            bool canPlace = CanPlaceShape(matrix, startRow, startCol);

            // Show a green preview when valid; otherwise mark the in-bounds empty cells red.
            List<Vector2Int> shapeFootprint = new List<Vector2Int>();
            for (int r = 0; r < shapeData.rows; r++)
            {
                for (int c = 0; c < shapeData.columns; c++)
                {
                    if (matrix[r, c])
                    {
                        int targetR = startRow + r;
                        int targetC = startCol + c;
                        if (targetR >= 0 && targetR < GRID_SIZE && targetC >= 0 && targetC < GRID_SIZE)
                        {
                            GridCell cell = grid[targetR, targetC];
                            if (canPlace)
                            {
                                cell.SetStatePreview(new Color(.29f,.871f,.502f,.6f), sprite);
                                shapeFootprint.Add(new Vector2Int(targetR, targetC));
                            }
                            else
                            {
                                cell.SetStateInvalidPreview();
                            }
                            currentPreviewCells.Add(cell);
                        }
                    }
                }
            }

            if (canPlace)
                SimulateAndHighlightLines(shapeFootprint);
        }

        private void SimulateAndHighlightLines(List<Vector2Int> footprint)
        {
            bool[,] virtualBoard = new bool[GRID_SIZE, GRID_SIZE];
            for (int r = 0; r < GRID_SIZE; r++)
            {
                for (int c = 0; c < GRID_SIZE; c++)
                {
                    virtualBoard[r, c] = grid[r, c].IsOccupied;
                }
            }

            foreach (var pos in footprint)
            {
                virtualBoard[pos.x, pos.y] = true;
            }

            List<int> fullRows = new List<int>();
            List<int> fullCols = new List<int>();

            for (int r = 0; r < GRID_SIZE; r++)
            {
                bool isFull = true;
                for (int c = 0; c < GRID_SIZE; c++)
                {
                    if (!virtualBoard[r, c]) { isFull = false; break; }
                }
                if (isFull && footprint.Exists(pos => pos.x == r)) fullRows.Add(r);
            }

            for (int c = 0; c < GRID_SIZE; c++)
            {
                bool isFull = true;
                for (int r = 0; r < GRID_SIZE; r++)
                {
                    if (!virtualBoard[r, c]) { isFull = false; break; }
                }
                if (isFull && footprint.Exists(pos => pos.y == c)) fullCols.Add(c);
            }

            HashSet<GridCell> cellsToHighlight = new HashSet<GridCell>();
            foreach (int r in fullRows)
            {
                for (int c = 0; c < GRID_SIZE; c++) cellsToHighlight.Add(grid[r, c]);
            }
            foreach (int c in fullCols)
            {
                for (int r = 0; r < GRID_SIZE; r++) cellsToHighlight.Add(grid[r, c]);
            }

            foreach (var cell in cellsToHighlight)
            {
                cell.SetStateHighlightLine();
                currentHighlightedLineCells.Add(cell);
            }
        }

        public void ClearPreviewAndHighlights()
        {
            previewShape = null;
            previewSprite = null;
            previewRow = previewCol = int.MinValue;
            foreach (var cell in currentPreviewCells) if (cell != null) cell.SetStateNormal();
            foreach (var cell in currentHighlightedLineCells) if (cell != null) cell.SetStateNormal();
            currentPreviewCells.Clear();
            currentHighlightedLineCells.Clear();
        }

        // ----------------------------------------------------------------------
        // ĐẶT KHỐI CHÍNH THỨC VÀ TÍNH ĐIỂM
        // ----------------------------------------------------------------------
        public bool TryPlaceShape(ShapeRuntimeData shapeData, Vector3 worldPosition, Sprite sprite = null, Color? tint = null)
        {
            if (IsInputLocked) return false;
            ClearPreviewAndHighlights();

            if (shapeData == null) return false;

            if (!GetGridCoordinatesFromWorld(worldPosition, out int startRow, out int startCol))
                return false;

            bool[,] matrix = shapeData.GetMatrix();
            if (!CanPlaceShape(matrix, startRow, startCol))
                return false;

            for (int r = 0; r < shapeData.rows; r++)
            {
                for (int c = 0; c < shapeData.columns; c++)
                {
                    if (matrix[r, c])
                    {
                        Model.Set(startRow+r,startCol+c,true);
                        grid[startRow + r, startCol + c].SetOccupied(true, sprite != null ? tint ?? Color.white : shapeData.blockColor, sprite);
                    }
                }
            }

            GameEvents.TriggerShapePlaced();
            ProcessCompletedLines(shapeData.Cells.Count);
            return true;
        }

        private void ProcessCompletedLines(int placedCells)
        {
            List<int> rowsToClear = new List<int>();
            List<int> colsToClear = new List<int>();

            for (int r = 0; r < GRID_SIZE; r++)
            {
                bool full = true;
                for (int c = 0; c < GRID_SIZE; c++) if (!grid[r, c].IsOccupied) full = false;
                if (full) rowsToClear.Add(r);
            }

            for (int c = 0; c < GRID_SIZE; c++)
            {
                bool full = true;
                for (int r = 0; r < GRID_SIZE; r++) if (!grid[r, c].IsOccupied) full = false;
                if (full) colsToClear.Add(c);
            }

            int totalCleared = rowsToClear.Count + colsToClear.Count;
            if (totalCleared > 0)
            {
                HashSet<GridCell> targetCells = new HashSet<GridCell>();
                foreach (int r in rowsToClear) for (int c = 0; c < GRID_SIZE; c++) targetCells.Add(grid[r, c]);
                foreach (int c in colsToClear) for (int r = 0; r < GRID_SIZE; r++) targetCells.Add(grid[r, c]);

                var snapshots = new List<ClearedCellInfo>(targetCells.Count);
                foreach (var cell in targetCells) snapshots.Add(cell.CaptureVisual());
                snapshots.Sort((a, b) => a.Row != b.Row ? a.Row.CompareTo(b.Row) : a.Col.CompareTo(b.Col));
                foreach (var cell in targetCells) { cell.SetOccupied(false); Model.Set(cell.Row, cell.Col, false); }

                bool empty = true;
                foreach (var cell in grid) if (cell.IsOccupied) { empty = false; break; }
                BlockBlast.Managers.GameManager.Instance?.ScorePlacement(placedCells, totalCleared, empty);
                // Audio and VFX do not award score.
                GameEvents.TriggerLinesCleared(totalCleared);
                GameEvents.TriggerCellsCleared(this, snapshots.AsReadOnly());
            }
            else BlockBlast.Managers.GameManager.Instance?.ScorePlacement(placedCells, 0, false);
        }

        public bool IsValid(ShapeRuntimeData shape,Vector3 world){int r,c;return shape!=null&&GetGridCoordinatesFromWorld(world,out r,out c)&&CanPlaceShape(shape.GetMatrix(),r,c);}
        public Vector3 SnapCenter(ShapeRuntimeData shape,Vector3 world){int r,c;if(!GetGridCoordinatesFromWorld(world,out r,out c))return world;return CellPosition(r,c)+new Vector3((shape.columns-1)*CellSize*.5f,(shape.rows-1)*CellSize*.5f,0);}
        public void Reflow(Vector3 center,float spacing,float cellWidth) {
            float old=cellSize; transform.position=center;cellSize=spacing;
            Vector3 origin=center-new Vector3((GRID_SIZE-1)*spacing/2,(GRID_SIZE-1)*spacing/2,0);
            for(int r=0;r<GRID_SIZE;r++)for(int col=0;col<GRID_SIZE;col++){
                var cell=grid[r,col]!=null?grid[r,col]:(HasAuthoredCells?authoredCells[r*GRID_SIZE+col]:null);
                if(cell==null)continue;
                cell.transform.position=origin+new Vector3(col*spacing,r*spacing,0);
                cell.transform.localScale=Vector3.one*(cellWidth/.96f);
            }
        }
        public Vector3 CellPosition(int r,int c)=>transform.position+new Vector3((c-(GRID_SIZE-1)*.5f)*cellSize,(r-(GRID_SIZE-1)*.5f)*cellSize,0);
        public void SyncModel(){if(Model==null)Model=new BoardModel(GRID_SIZE);for(int r=0;r<GRID_SIZE;r++)for(int c=0;c<GRID_SIZE;c++)Model.Set(r,c,grid[r,c].IsOccupied);}
        public bool CanPlaceShapeAnywhere(ShapeRuntimeData shapeData)
        {
            // Phòng ngừa trường hợp shapeData chưa được gán vào TrayManager
            if (shapeData == null)
            {
                Debug.Log("ShapeRuntimeData is null. Cannot check placement.");
                return false;
            }
            bool[,] matrix = shapeData.GetMatrix();
            for (int r = 0; r < GRID_SIZE; r++)
            {
                for (int c = 0; c < GRID_SIZE; c++)
                {
                    if (CanPlaceShape(matrix, r, c)) return true;
                }
            }
            return false;
        }

        private bool CanPlaceShape(bool[,] matrix, int startRow, int startCol)
        {
            SyncModel(); return Model.CanPlace(matrix,startRow,startCol);
        }

        private bool GetGridCoordinatesFromWorld(Vector3 worldPos, out int row, out int col)
        {
            Vector3 origin = transform.position - new Vector3((GRID_SIZE - 1) * cellSize / 2f, (GRID_SIZE - 1) * cellSize / 2f, 0);
            Vector3 relPos = worldPos - origin;
            if (float.IsNaN(relPos.x) || float.IsNaN(relPos.y) ||
                relPos.x < -cellSize * .5f || relPos.y < -cellSize * .5f ||
                relPos.x >= (GRID_SIZE - .5f) * cellSize || relPos.y >= (GRID_SIZE - .5f) * cellSize)
            {
                row = col = -1;
                return false;
            }

            col = Mathf.RoundToInt(relPos.x / cellSize);
            row = Mathf.RoundToInt(relPos.y / cellSize);

            if (row >= 0 && row < GRID_SIZE && col >= 0 && col < GRID_SIZE) return true;
            return false;
        }
    }
}



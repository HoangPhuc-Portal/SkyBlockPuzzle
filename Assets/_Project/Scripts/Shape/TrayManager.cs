using System.Collections.Generic;
using UnityEngine;
using BlockBlast.Core;
using BlockBlast.Data;
using BlockBlast.Grid;
using BlockBlast.Difficulty;

namespace BlockBlast.Shape
{
    /// <summary>
    /// Quản lý khay chứa 3 khối gạch bên dưới màn hình và kiểm tra điều kiện thua game
    /// </summary>
    public class TrayManager : MonoBehaviour
    {
        public static TrayManager Instance { get; private set; }

        [Header("References")]
        [SerializeField] private Transform[] slots = new Transform[3];
        [SerializeField] private DraggableShape shapePrefab;
        [SerializeField] private List<ShapeData> availableShapes;
        [SerializeField] private BlockSpriteSet spriteSet;
        [SerializeField] private DifficultyConfig difficultyConfig;
        private HandSelector selector;
        private readonly DifficultyState difficultyState = new DifficultyState();
        private int difficultyRun = -1;
        public DifficultyState DifficultyState => difficultyState;
        public HandSelection LastSelection { get; private set; }

        public System.Collections.Generic.IReadOnlyList<DraggableShape> Pieces=>currentShapes;
        public event System.Action HandChanged;
        private DraggableShape[] currentShapes = new DraggableShape[3];
        private readonly HashSet<DraggableShape> spawnedShapes = new HashSet<DraggableShape>();
        private bool gameOverConfirmed;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            if (difficultyConfig == null) difficultyConfig = Resources.Load<DifficultyConfig>("DifficultyConfig");
            if (slots == null || slots.Length == 0 || shapePrefab == null || availableShapes == null || availableShapes.Count == 0)
            {
                Debug.LogError("TrayManager is missing slots, shape prefab, or available shapes.");
                enabled = false;
                return;
            }

            if (spriteSet == null || spriteSet.ValidCount == 0)
                Debug.LogWarning("TrayManager has no valid sprite set; using the tile prefab fallback.", this);
            currentShapes = new DraggableShape[slots.Length];
            SpawnNewHand();
        }

        public void SpawnNewHand()
        {
            if (BlockBlast.Managers.GameManager.Instance != null &&
                BlockBlast.Managers.GameManager.Instance.IsGameOver) return;
            var validShapes = availableShapes.FindAll(shape => shape != null && shape.IsValid);
            if (validShapes.Count == 0) { Debug.LogError("No valid connected ShapeData available.", this); return; }
            ShapeRuntimeData[] selected = null;
            bool emptyHand = System.Array.TrueForAll(currentShapes, p => p == null);
            var manager = BlockBlast.Managers.GameManager.Instance;
            if (emptyHand && difficultyConfig != null && difficultyConfig.enabledDda && GridBoard.Instance != null && manager != null)
            {
                if (difficultyRun != manager.RunSequence)
                {
                    difficultyRun = manager.RunSequence;
                    difficultyState.Reset(manager.RoundStartHighScore);
                }
                if (selector == null) selector = new HandSelector(validShapes, Random.Range(0, int.MaxValue));
                GridBoard.Instance.SyncModel();
                LastSelection = selector.Select(GridBoard.Instance.Model, manager.CurrentScore, difficultyState, difficultyConfig);
                selected = LastSelection.hand;
                difficultyState.StartHand();
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                if (difficultyConfig.logDecisions) Debug.Log("[DDA] " + LastSelection.Summary(), this);
#endif
            }
            spawnedShapes.RemoveWhere(piece => piece == null);
            for (int i = 0; i < currentShapes.Length; i++)
            {
                // Không dùng slots[i].childCount: khối vừa đặt xong vẫn còn là con của slot
                // cho tới cuối frame (Destroy bị trì hoãn), nên slot đó bị coi nhầm là "còn khối".
                // currentShapes[i] đã được gán null ngay trong OnShapePlaced nên đáng tin cậy hơn.
                if (currentShapes[i] != null) continue;

                if (slots[i] == null)
                {
                    Debug.LogError($"TrayManager slot {i} is not assigned.");
                    continue;
                }

                ShapeRuntimeData runtime = selected != null && i < selected.Length
                    ? selected[i] : validShapes[Random.Range(0, validShapes.Count)].CreateRuntime();
                DraggableShape newShape = Instantiate(shapePrefab, slots[i]);
                Sprite sprite = spriteSet != null && spriteSet.ValidCount > 0
                    ? spriteSet.SelectByIndex(Random.Range(0, spriteSet.ValidCount)) : null;
                newShape.SetupRuntime(runtime, sprite);
                currentShapes[i] = newShape;
                spawnedShapes.Add(newShape);
            }

            HandChanged?.Invoke();
            CheckGameOverCondition();
        }

        public void OnShapePlaced(DraggableShape shape)
        {
            if (BlockBlast.Managers.GameManager.Instance != null &&
                BlockBlast.Managers.GameManager.Instance.IsInputLocked) return;
            bool tracked = false;
            for (int i = 0; i < currentShapes.Length; i++)
            {
                if (currentShapes[i] == shape)
                {
                    currentShapes[i] = null;
                    tracked = true;
                    break;
                }
            }
            if (!tracked) return;
            var manager = BlockBlast.Managers.GameManager.Instance;
            if (difficultyConfig != null && manager != null && GridBoard.Instance != null)
                difficultyState.Record(manager.LastClearedLines, manager.CurrentCombo, BoardOpportunity.Occupancy(GridBoard.Instance.Model), difficultyConfig);

            HandChanged?.Invoke();
            bool isEmpty = true;
            foreach (var s in currentShapes)
            {
                if (s != null) { isEmpty = false; break; }
            }

            if (isEmpty)
            {
                difficultyState.CompleteHand();
                SpawnNewHand();
            }
            else
            {
                HandChanged?.Invoke();
            CheckGameOverCondition();
            }
        }

        public void ClearHand()
        {
            // Revive preserves this run's record snapshot, but discards stale pressure/timing.
            // Replay is recognized separately via GameManager.RunSequence at the next spawn.
            difficultyState.Continue();
            foreach (var piece in spawnedShapes) if (piece != null) piece.Discard();
            spawnedShapes.Clear();
            currentShapes = new DraggableShape[slots.Length];
            gameOverConfirmed = false;
            HandChanged?.Invoke();
        }

        private void OnDestroy() { if (Instance == this) Instance = null; }
        private void Update()
        {
            // Only a clock runs per frame. Board analysis/search runs solely at spawn time.
            var manager = BlockBlast.Managers.GameManager.Instance;
            if (manager != null && !manager.IsInputLocked && Application.isFocused && Time.timeScale > 0)
                difficultyState.Tick(Time.unscaledDeltaTime);
        }

        public void CheckGameOverCondition()
        {
            if (gameOverConfirmed || GridBoard.Instance == null) return;
            var manager = BlockBlast.Managers.GameManager.Instance;
            if (manager != null && manager.IsInputLocked) return;
            bool hasPiece = false;
            bool canPlaceAny = false;

            for (int i = 0; i < currentShapes.Length; i++)
            {
                if (currentShapes[i] != null)
                {
                    hasPiece = true;
                    if (GridBoard.Instance.CanPlaceShapeAnywhere(currentShapes[i].ShapeData))
                    {
                        canPlaceAny = true;
                        break;
                    }
                }
            }

            if (hasPiece && !canPlaceAny)
            {
                gameOverConfirmed = true;
                difficultyState.NoMoves();
                if (manager != null) manager.ConfirmGameOver();
                else GameEvents.TriggerGameOver();
            }
        }
    }
}


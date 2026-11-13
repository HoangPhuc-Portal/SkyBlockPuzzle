using System.Collections;
using System.Collections.Generic;
using BlockBlast.Core;
using BlockBlast.Core.Pool;
using BlockBlast.Grid;
using UnityEngine;

namespace BlockBlast.Effects
{
    public sealed class BubblePopEffectPlayer : MonoBehaviour
    {
        [SerializeField] private GridBoard board;
        [SerializeField] private BubblePopConfig config;
        [SerializeField] private BlockBlast.UI.UITheme theme;
        private BubblePopConfig runtimeConfig;
        private BubblePopEffectPool pool;
        private List<BubblePopEffect> active;
        private Coroutine runner;
        public int ActiveCount => pool != null ? pool.ActiveCount : 0;
        public int CreatedCount => pool != null ? pool.CreatedCount : 0;

        private void OnEnable()
        {
            if (board == null || config == null || !config.HasValidReferences ||
                !config.effectPrefab.IsConfigured || config.effectPrefab.BubbleCapacity < config.bubbleCount)
            {
                Debug.LogError("BubblePopEffectPlayer needs a board and a valid config/effect prefab.", this);
                return;
            }
            if (pool == null)
            {
                runtimeConfig=Instantiate(config);
                if (theme != null) runtimeConfig.theme = theme;
                pool = new BubblePopEffectPool(runtimeConfig, transform);
                active = new List<BubblePopEffect>(Mathf.Max(1, config.maximumCount));
            }
            GameEvents.OnCellsCleared += PlayCells;
            GameEvents.OnShapePlaced += StopEffects;
            GameEvents.OnBoardReset += BoardReset;
            GameEvents.OnGameOver += StopEffects;
            GameEvents.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            GameEvents.OnCellsCleared -= PlayCells;
            GameEvents.OnShapePlaced -= StopEffects;
            GameEvents.OnBoardReset -= BoardReset;
            GameEvents.OnGameOver -= StopEffects;
            GameEvents.OnGameStateChanged -= OnGameStateChanged;
            StopEffects();
        }

        private void OnDestroy(){if(runtimeConfig!=null)Destroy(runtimeConfig);}

        private void OnGameStateChanged(GameState state)
        {
            if (state == GameState.GameOver || state == GameState.MainMenu) StopEffects();
        }

        private void PlayCells(GridBoard source, IReadOnlyList<ClearedCellInfo> cells)
        {
            if (source != board || cells == null) return;
            StopEffects();
            int size = board.Model != null ? board.Model.Size : GridBoard.GRID_SIZE;
            var rows = new int[size];
            var cols = new int[size];
            var unique = new HashSet<Vector2Int>();
            foreach (var cell in cells)
                if (unique.Add(new Vector2Int(cell.Row, cell.Col))) { rows[cell.Row]++; cols[cell.Col]++; }
            int lineCount = 0;
            for (int i = 0; i < size; i++) { if (rows[i] == size) lineCount++; if (cols[i] == size) lineCount++; }
            float intensity = 1 + Mathf.Min(2, Mathf.Max(0, lineCount - 1)) * runtimeConfig.multiClearIntensity;
            unique.Clear();
            bool capped = false;
            for (int i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                if (!unique.Add(new Vector2Int(cell.Row, cell.Col))) continue;
                // Horizontal left-to-right, vertical bottom-to-top. At intersections the first wave wins.
                int rank = size - 1;
                if (rows[cell.Row] == size) rank = cell.Col;
                if (cols[cell.Col] == size) rank = Mathf.Min(rank, cell.Row);
                var effect = pool.Rent();
                if (effect == null) { capped = true; continue; }
                effect.Begin(cell, Mathf.Min(rank * runtimeConfig.staggerDelay, runtimeConfig.maximumStagger), intensity);
                active.Add(effect);
            }
            if (capped) Debug.LogWarning("Bubble pop pool reached its configured limit; excess visuals skipped.", this);
            if (runner == null && active.Count > 0) runner = StartCoroutine(Animate());
        }

        private void BoardReset(GridBoard source) { if (source == board) StopEffects(); }

        private IEnumerator Animate()
        {
            // Yield before the first tick so every cell in the event begins on the same frame.
            while (active.Count > 0)
            {
                yield return null;
                for (int i = active.Count - 1; i >= 0; i--)
                {
                    if (!active[i].Tick(Time.deltaTime)) continue;
                    pool.Return(active[i]);
                    int last = active.Count - 1;
                    active[i] = active[last];
                    active.RemoveAt(last);
                }
            }
            runner = null;
        }

        public void StopEffects()
        {
            if (runner != null) StopCoroutine(runner);
            runner = null;
            pool?.ReleaseAll();
            active?.Clear();
        }
    }
}

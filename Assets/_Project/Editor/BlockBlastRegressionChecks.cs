using System;
using System.Collections.Generic;
using System.Reflection;
using BlockBlast.Core;
using BlockBlast.Data;
using BlockBlast.Grid;
using BlockBlast.Managers;
using UnityEditor;
using UnityEngine;

// Runs against an isolated inactive board, so neither the player's board nor PlayerPrefs are changed.
public static class BlockBlastRegressionChecks
{
    private const BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
    private static void Check(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
    private static void Set(object target, string name, object value) =>
        target.GetType().GetField(name, Private).SetValue(target, value);
    private static object Get(object target, string name) =>
        target.GetType().GetField(name, Private).GetValue(target);
    private static ShapeData Shape(int rows, int cols, params int[] indices)
    {
        var shape = ScriptableObject.CreateInstance<ShapeData>();
        shape.rows = rows;
        shape.columns = cols;
        var matrix = new bool[rows * cols];
        foreach (int index in indices) matrix[index] = true;
        Set(shape, "matrixData", matrix);
        shape.blockColor = Color.white;
        return shape;
    }

    [MenuItem("Block Blast/Run Regression Checks")]
    public static void RunMenu() => Debug.Log(Run());

    public static string Run()
    {
        if (Application.isPlaying) throw new InvalidOperationException("Run regression checks in Edit Mode.");
        var root = new GameObject("BlockBlast isolated regression board");
        root.SetActive(false);
        var board = root.AddComponent<GridBoard>();
        var prefab = AssetDatabase.LoadAssetAtPath<GridCell>("Assets/_Project/Prefabs/Shapes/GridCell.prefab");
        var cells = new GridCell[GridBoard.GRID_SIZE, GridBoard.GRID_SIZE];
        int n = GridBoard.GRID_SIZE;
        var one = Shape(1, 1, 0);
        var asymmetric = Shape(2, 2, 0, 1, 2);
        var empty = Shape(1, 1);
        var model = root.AddComponent<GameManager>();
        // Avoid any high-score persistence during this check.
        Set(model, "<HighScore>k__BackingField", int.MaxValue);

        int placements = 0, lines = 0, scoreEvents = 0;
        Action placed = () => placements++;
        Action<int> cleared = count => { lines += count; };
        Action<int> scored = score => scoreEvents++;
        GameEvents.OnShapePlaced += placed;
        GameEvents.OnLinesCleared += cleared;
        GameEvents.OnScoreChanged += scored;
        var passed = new List<string>();
        try
        {
            for (int r = 0; r < n; r++)
            for (int c = 0; c < n; c++)
            {
                cells[r, c] = UnityEngine.Object.Instantiate(prefab, root.transform);
                cells[r, c].Init(r, c);
            }
            Set(board, "grid", cells);
            Func<int, int, Vector3> position = (r, c) =>
                board.transform.position + new Vector3((c - (n - 1) * .5f) * board.CellSize,
                    (r - (n - 1) * .5f) * board.CellSize, 0);
            Func<int> highlights = () =>
            {
                int count = 0;
                foreach (var cell in cells)
                    if (((GameObject)Get(cell, "highlightGlowVisual")).activeSelf) count++;
                return count;
            };
            Action reset = () =>
            {
                board.ClearPreviewAndHighlights();
                foreach (var cell in cells) cell.SetOccupied(false);
            };
            board.UpdateDragPreview(one, position(0, 0));
            Check(highlights() == 0 && !cells[0, 0].IsOccupied, "Preview mutated occupancy");
            Check(board.TryPlaceShape(one, position(0, 0)), "Valid placement rejected");
            board.ClearPreviewAndHighlights();
            Check(cells[0, 0].IsOccupied && ((GameObject)Get(cells[0, 0], "fillVisual")).activeSelf,
                "Committed visual disappeared");
            var fill = (SpriteRenderer)Get(cells[0, 0], "fillRenderer");
            Check(fill.sprite != null && fill.color.a == 1 && fill.enabled, "Committed sprite invalid");
            passed.Add("valid placement and persistent opaque sprite");
            Check(!board.TryPlaceShape(one, position(0, 0)) && placements == 1 && model.CurrentScore == 0,
                "Occupied drop changed events or score");
            Check(!board.TryPlaceShape(asymmetric, position(n - 1, n - 1)) && !cells[n - 1, n - 1].IsOccupied,
                "Out-of-bounds drop partially committed");
            Check(!board.TryPlaceShape(empty, position(1, 1)), "Empty shape accepted");
            passed.Add("invalid occupied/out-of-bounds/empty placements remain atomic");
            reset();
            for (int c = 0; c < n - 1; c++) cells[0, c].SetOccupied(true, Color.white);
            board.UpdateDragPreview(one, position(0, n - 1));
            Check(highlights() == n && !cells[0, n - 1].IsOccupied && model.CurrentScore == 0, "Row preview invalid");
            // A cached preview must not recreate state.
            board.UpdateDragPreview(one, position(0, n - 1));
            Check(highlights() == n, "Repeated target changed highlight");
            board.UpdateDragPreview(one, position(1, 0));
            Check(highlights() == 0 && cells[0, 0].IsOccupied, "Highlight on occupied cells leaked");
            passed.Add("row preview, cached target and cleanup on target change");
            board.UpdateDragPreview(one, position(0, n - 1));
            board.UpdateDragPreview(one, position(0, 0));
            Check(highlights() == 0, "Invalid target retained highlight");
            board.UpdateDragPreview(one, position(0, n - 1));
            board.UpdateDragPreview(one, position(-2, -2));
            Check(highlights() == 0, "Outside target retained highlight");
            passed.Add("invalid and outside target cleanup");
            reset();
            for (int r = 0; r < n - 1; r++) cells[r, 0].SetOccupied(true, Color.white);
            board.UpdateDragPreview(one, position(n - 1, 0));
            Check(highlights() == n, "Column preview invalid");
            board.ClearPreviewAndHighlights();
            Check(highlights() == 0, "Explicit cancellation retained highlight");
            passed.Add("column preview and explicit cancellation cleanup");
            reset();
            for (int c = 1; c < n; c++) cells[0, c].SetOccupied(true, Color.white);
            for (int r = 1; r < n; r++) cells[r, 0].SetOccupied(true, Color.white);
            board.UpdateDragPreview(one, position(0, 0));
            Check(highlights() == 2 * n - 1, "Row-column intersection highlighted twice");
            Check(board.TryPlaceShape(one, position(0, 0)), "Cross placement rejected");
            Check(lines == 2,
                "Cross clear scoring differs from existing rule");
            foreach (var cell in cells)
                Check(!cell.IsOccupied && !((GameObject)Get(cell, "fillVisual")).activeSelf, "Cross clear left occupancy/visual");
            Check(highlights() == 0, "Commit retained preview");
            passed.Add("cross-line union, unique cells and synchronized clear (scoring covered by ScoringChecks)");
            reset();
            for (int r = 0; r < 2; r++)
            for (int c = 1; c < n; c++) cells[r, c].SetOccupied(true, Color.white);
            var vertical = Shape(2, 1, 0, 1);
            try
            {
                board.UpdateDragPreview(vertical, position(0, 0));
                Check(highlights() == 2 * n, "Multiple row preview invalid");
                typeof(GridBoard).GetMethod("OnDisable", Private).Invoke(board, null);
                Check(highlights() == 0, "Disable cleanup retained preview");
            }
            finally { UnityEngine.Object.DestroyImmediate(vertical); }
            passed.Add("multiple-row preview and board disable cleanup");
            reset();
            Check(board.TryPlaceShape(asymmetric, position(n - 2, n - 2)), "Valid asymmetric edge placement rejected");
            Check(cells[n - 2, n - 2].IsOccupied && cells[n - 2, n - 1].IsOccupied &&
                  cells[n - 1, n - 2].IsOccupied && !cells[n - 1, n - 1].IsOccupied, "Shape orientation incorrect");
            Check(board.TryPlaceShape(one, position(0, 0)) && model.CurrentScore == 0, "Consecutive placement changed score rule");
            passed.Add("asymmetric edge orientation and consecutive placements");
            return "PASS (" + passed.Count + " groups):\n" + string.Join("\n", passed);
        }
        finally
        {
            GameEvents.OnShapePlaced -= placed;
            GameEvents.OnLinesCleared -= cleared;
            GameEvents.OnScoreChanged -= scored;
            UnityEngine.Object.DestroyImmediate(root);
            UnityEngine.Object.DestroyImmediate(one);
            UnityEngine.Object.DestroyImmediate(asymmetric);
            UnityEngine.Object.DestroyImmediate(empty);
        }
    }
}


using System;
using System.Collections.Generic;
using System.Reflection;
using BlockBlast.Core;
using BlockBlast.Core.Pool;
using BlockBlast.Data;
using BlockBlast.Effects;
using BlockBlast.Grid;
using UnityEditor;
using UnityEngine;

public static class BubblePopRegressionChecks
{
    private const BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
    private static void Check(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    public static string SpriteSelection()
    {
        var source = AssetDatabase.LoadAssetAtPath<BlockSpriteSet>("Assets/_Project/ScriptableObjects/BlockSpriteSet.asset");
        Check(source != null && source.ValidCount == 6, "Configured palette does not contain the six prefab sprites");
        var copy = UnityEngine.Object.Instantiate(source);
        try
        {
            var field = typeof(BlockSpriteSet).GetField("sprites", Private);
            Sprite first = source.SelectByIndex(0), second = source.SelectByIndex(1);
            field.SetValue(copy, new Sprite[] { null, first, null, second });
            Check(copy.ValidCount == 2 && copy.SelectByIndex(0) == first && copy.SelectByIndex(1) == second,
                "Null entries affected valid selection");
            Check(copy.SelectByIndex(2) == first && copy.SelectByIndex(-1) == second, "Index wrapping failed");
            field.SetValue(copy, new Sprite[0]);
            Check(copy.ValidCount == 0 && copy.SelectByIndex(0) == null, "Empty palette crashed or returned a sprite");
            field.SetValue(copy, null);
            Check(copy.ValidCount == 0 && copy.SelectByIndex(0) == null, "Null palette crashed");
            return "PASS: six configured sprites; null skipping; deterministic index selection; empty/null fallback";
        }
        finally { UnityEngine.Object.DestroyImmediate(copy); }
    }

    public static string EffectPool()
    {
        var config = UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<BubblePopConfig>(
            "Assets/_Project/ScriptableObjects/BubblePopConfig.asset"));
        var root = new GameObject("Isolated bubble pool regression");
        root.SetActive(false);
        config.prewarmCount = 2;
        config.maximumCount = 3;
        try
        {
            var pool = new BubblePopEffectPool(config, root.transform);
            Check(pool.CreatedCount == 2 && pool.ActiveCount == 0, "Prewarm count incorrect");
            var first = pool.Rent();
            var second = pool.Rent();
            var third = pool.Rent();
            Check(pool.CreatedCount == 3 && pool.ActiveCount == 3 && pool.Rent() == null, "Pool cap exceeded");
            var sprites = AssetDatabase.LoadAssetAtPath<BlockSpriteSet>("Assets/_Project/ScriptableObjects/BlockSpriteSet.asset");
            Sprite sprite = sprites.SelectByIndex(0);
            var snapshot = new ClearedCellInfo(0, 0, Vector3.one, sprite, Color.white,
                Vector3.one, Quaternion.identity, 0, 2);
            first.Begin(snapshot, .1f);
            Check(first.transform.position == Vector3.one, "Effect position differs from snapshot");
            Check(!first.Tick(.05f), "Stagger completed too soon");
            Check(!first.Tick(.1f), "Inflation completed too soon");
            Check(!first.Tick(.15f), "Burst completed too soon");
            Check(first.Tick(1f), "Effect did not complete");
            pool.Return(first);
            pool.Return(first);
            Check(pool.ActiveCount == 2, "Double return duplicated an instance");
            var reused = pool.Rent();
            Check(reused == first && pool.CreatedCount == 3, "Pool instantiated instead of reusing");
            Sprite nextSprite = sprites.SelectByIndex(1);
            reused.Begin(new ClearedCellInfo(1, 1, Vector3.zero, nextSprite, Color.white,
                Vector3.one, Quaternion.identity, 0, 2), 0f);
            var block = (SpriteRenderer)typeof(BubblePopEffect).GetField("blockVisual", Private).GetValue(reused);
            Check(block.sprite == nextSprite && block.color == Color.white, "Reused effect retained old sprite/color");
            pool.ReleaseAll();
            Check(pool.ActiveCount == 0 && !first.gameObject.activeSelf && !second.gameObject.activeSelf &&
                !third.gameObject.activeSelf, "Cleanup left active effects");
            Check(pool.Rent() != null && pool.Rent() != null && pool.Rent() != null && pool.Rent() == null,
                "Cleanup duplicated or lost pool entries");
            pool.ReleaseAll();
            return "PASS: prewarm, bounded expansion, cap, reuse, double return, snapshot position/sprite, inflate/burst completion, cleanup";
        }
        finally
        {
            UnityEngine.Object.DestroyImmediate(root);
            UnityEngine.Object.DestroyImmediate(config);
        }
    }

    public static string ClearSnapshots()
    {
        var root = new GameObject("Isolated clear snapshot regression");
        root.SetActive(false);
        var board = root.AddComponent<GridBoard>();
        var prefab = AssetDatabase.LoadAssetAtPath<GridCell>("Assets/_Project/Prefabs/Shapes/GridCell.prefab");
        int size = GridBoard.GRID_SIZE;
        var cells = new GridCell[size, size];
        var shape = ScriptableObject.CreateInstance<ShapeData>();
        shape.rows = shape.columns = 1;
        typeof(ShapeData).GetField("matrixData", Private).SetValue(shape, new[] { true });
        var sprite = AssetDatabase.LoadAssetAtPath<BlockSpriteSet>("Assets/_Project/ScriptableObjects/BlockSpriteSet.asset").SelectByIndex(2);
        bool scoreEventFinished = false;
        IReadOnlyList<ClearedCellInfo> captured = null;
        Action<int> lines = count => { Check(count == 2, "Cross line count changed"); scoreEventFinished = true; };
        Action<GridBoard, IReadOnlyList<ClearedCellInfo>> snapshots = (source, batch) =>
        {
            Check(source == board && scoreEventFinished, "VFX event ran before scoring event");
            foreach (var cell in cells) Check(!cell.IsOccupied, "VFX event ran before occupancy clear");
            captured = batch;
        };
        GameEvents.OnLinesCleared += lines;
        GameEvents.OnCellsCleared += snapshots;
        try
        {
            for (int r = 0; r < size; r++)
            for (int c = 0; c < size; c++)
            {
                cells[r, c] = UnityEngine.Object.Instantiate(prefab, root.transform);
                cells[r, c].Init(r, c);
                cells[r, c].transform.position = new Vector3(c, r, 0);
                if ((r == 0 || c == 0) && (r != 0 || c != 0)) cells[r, c].SetOccupied(true, Color.white, sprite);
            }
            typeof(GridBoard).GetField("grid", Private).SetValue(board, cells);
            Vector3 anchor = new Vector3(-(size - 1) * .5f * board.CellSize, -(size - 1) * .5f * board.CellSize, 0);
            Check(board.TryPlaceShape(shape, anchor, sprite), "Cross placement rejected");
            Check(captured != null && captured.Count == size * 2 - 1, "Intersecting cell duplicated or omitted");
            var coordinates = new HashSet<Vector2Int>();
            foreach (var cell in captured)
            {
                Check(coordinates.Add(new Vector2Int(cell.Row, cell.Col)), "Duplicate snapshot");
                Check(cell.Sprite == sprite && cell.Color == Color.white, "Snapshot lost old sprite/color");
            }
            var old = captured[0];
            var nextSprite = AssetDatabase.LoadAssetAtPath<BlockSpriteSet>("Assets/_Project/ScriptableObjects/BlockSpriteSet.asset").SelectByIndex(3);
            Check(board.TryPlaceShape(shape, anchor, nextSprite), "Cell not reusable immediately after clear");
            cells[0, 0].SetStateHighlightLine();
            cells[0, 0].SetStateNormal();
            Check(cells[0, 0].CurrentSprite == nextSprite && old.Sprite == sprite,
                "New placement or preview cleanup mutated committed sprite/snapshot");
            return "PASS: unique cross snapshots, score-before-VFX, occupancy cleared before event, immediate re-placement, immutable old sprite";
        }
        finally
        {
            GameEvents.OnLinesCleared -= lines;
            GameEvents.OnCellsCleared -= snapshots;
            UnityEngine.Object.DestroyImmediate(root);
            UnityEngine.Object.DestroyImmediate(shape);
        }
    }
}

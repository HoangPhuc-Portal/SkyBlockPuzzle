using System;
using UnityEngine;
using BlockBlast.Grid;

public static class UIRegressionChecks
{
    public static string GhostFallback()
    {
        var root = new GameObject("Isolated missing-sprite preview test");
        root.SetActive(false);
        var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GridCell>(
            "Assets/_Project/Prefabs/Shapes/GridCell.prefab");
        var theme = UnityEngine.Object.Instantiate(UnityEditor.AssetDatabase.LoadAssetAtPath<BlockBlast.UI.UITheme>(
            "Assets/_Project/ScriptableObjects/UITheme.asset"));
        try
        {
            var cell = UnityEngine.Object.Instantiate(prefab, root.transform);
            var serialized = new UnityEditor.SerializedObject(cell);
            var fill = (SpriteRenderer)serialized.FindProperty("fillRenderer").objectReferenceValue;
            var background = (SpriteRenderer)serialized.FindProperty("cellBackgroundRenderer").objectReferenceValue;
            var expectedSize = Vector2.Scale(background.size,
                new Vector2(background.transform.lossyScale.x, background.transform.lossyScale.y));
            var suppliedSprite = fill.sprite;
            Require(suppliedSprite != null, "Prefab fill sprite still missing");
            fill.sprite = null; // Reproduce a broken sub-asset reference before initialization.
            cell.Init(0, 0);
            cell.SetStatePreview(Color.green, suppliedSprite);
            var ghost = cell.transform.Find("Ghost").GetComponent<SpriteRenderer>();
            Require(ghost.enabled && !cell.IsOccupied, "Fallback preview failed or changed occupancy");
            Require(Mathf.Abs(ghost.bounds.size.x - expectedSize.x) < .001f &&
                Mathf.Abs(ghost.bounds.size.y - expectedSize.y) < .001f, "Fallback sizing ignored cell surface");
            for (int i = 0; i < 25; i++) cell.SetStatePreview(Color.green, suppliedSprite);
            Require(cell.GetComponentsInChildren<SpriteRenderer>(true).Length ==
                prefab.GetComponentsInChildren<SpriteRenderer>(true).Length + 1, "Ghost duplicated");
            cell.SetStateInvalidPreview();
            Require(ghost.enabled && ghost.color.r > ghost.color.g, "Invalid ghost lost red color");
            cell.SetOccupied(true, Color.white, suppliedSprite);
            Require(cell.IsOccupied && !ghost.enabled &&
                Mathf.Abs(fill.bounds.size.x - expectedSize.x) < .001f, "Fallback placement size/occupancy wrong");
            cell.SetOccupied(false);
            Require(!ghost.enabled && !cell.IsOccupied, "Reset left preview or occupancy");

            var noFallback = UnityEngine.Object.Instantiate(prefab, root.transform);
            var missing = new UnityEditor.SerializedObject(noFallback);
            ((SpriteRenderer)missing.FindProperty("fillRenderer").objectReferenceValue).sprite = null;
            theme.whiteBlock = null;
            var view = new UnityEditor.SerializedObject(noFallback.GetComponent<BlockBlast.UI.CellView>());
            view.FindProperty("theme").objectReferenceValue = theme;
            view.ApplyModifiedPropertiesWithoutUndo();
            noFallback.Init(1, 1);
            noFallback.SetStatePreview(Color.green);
            noFallback.SetStateInvalidPreview();
            Require(noFallback.transform.Find("Ghost") == null, "Empty sprite created invalid ghost");
            noFallback.SetStateNormal();
            Require(!noFallback.IsOccupied, "Missing-all-sprites path mutated occupancy");
            return "PASS: null fill sprite, supplied/theme/no sprite fallback, valid/invalid preview, sizing, reuse, placement and reset";
        }
        finally
        {
            UnityEngine.Object.DestroyImmediate(root);
            UnityEngine.Object.DestroyImmediate(theme);
        }
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
    public static string Model()
    {
        var board = new BoardModel(8);
        var single = new bool[,] { { true } };
        for (int c = 0; c < 7; c++) board.Set(0, c, true);
        Require(board.CompletedBy(single, 0, 7) == 1, "Row completion");
        Require(board.CompletedBy(single, 1, 7) == 0, "Valid non-clear target");
        Require(!board.CanPlace(single, 0, 0), "Occupied rejection");
        Require(!board.CanPlace(single, -1, 7), "Out of bounds rejection");
        Require(!board.CanPlace(new bool[,] { { false } }, 0, 7), "Empty shape rejection");
        Require(!board.Occupied(0, 7), "Simulation must not mutate occupancy");
        for (int r = 1; r < 8; r++) board.Set(r, 7, true);
        Require(board.CompletedBy(single, 0, 7) == 2, "Cross completion");
        var asymmetric = new bool[,] { { true, false }, { true, true } };
        Require(board.CanPlace(asymmetric, 2, 2), "Asymmetric valid target");
        Require(!board.CanPlace(asymmetric, 7, 6), "Asymmetric edge rejection");
        return "PASS: model valid/invalid, row/column/cross completion scoring, simulation immutability";
    }
}

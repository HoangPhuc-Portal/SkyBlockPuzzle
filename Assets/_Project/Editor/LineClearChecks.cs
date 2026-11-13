using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using BlockBlast.Core;
using BlockBlast.Data;
using BlockBlast.Effects;
using BlockBlast.Grid;
using BlockBlast.Managers;
using BlockBlast.Shape;
using BlockBlast.UI;

public static class LineClearChecks
{
    const BindingFlags F = BindingFlags.NonPublic | BindingFlags.Instance;
    const string Key = "BlockBlast_HighScore";
    static IEnumerator routine;
    static double next, deadline;
    static bool hadRecord;
    static int record;
    static readonly List<ShapeData> temporary = new List<ShapeData>();
    static IReadOnlyList<ClearedCellInfo> captured;
    static GridCell[] cells;
    static T Get<T>(object o, string name) => (T)o.GetType().GetField(name, F).GetValue(o);
    static void Check(bool ok, string reason) { if (!ok) throw new Exception(reason); }
    static ShapeRuntimeData Shape(int rows, int cols)
    {
        var shape = ScriptableObject.CreateInstance<ShapeData>(); temporary.Add(shape);
        shape.rows = rows; shape.columns = cols; shape.rotationPolicy = RotationPolicy.NoRotation;
        typeof(ShapeData).GetField("matrixData", F).SetValue(shape, Enumerable.Repeat(true, rows * cols).ToArray());
        return shape.CreateRuntime();
    }
    static void Capture(GridBoard source, IReadOnlyList<ClearedCellInfo> batch)
    {
        captured = batch;
        Check(batch.Select(c => new Vector2Int(c.Row, c.Col)).Distinct().Count() == batch.Count, "Duplicate intersection snapshot");
        foreach (var old in batch)
        {
            var cell = cells.First(c => c.Row == old.Row && c.Col == old.Col);
            Check(!cell.IsOccupied && !Get<GameObject>(cell, "fillVisual").activeSelf, "Live cell not cleared before VFX");
        }
    }
    [MenuItem("Block Blast/Tests/Run Line Clear Play Checks")]
    static void Run()
    {
        if (!EditorApplication.isPlaying) throw new Exception("Enter Play Mode first; test loads a fresh GamePlay scene.");
        if (routine != null) return;
        hadRecord = PlayerPrefs.HasKey(Key); record = PlayerPrefs.GetInt(Key);
        routine = Exercise(); next = 0; deadline = EditorApplication.timeSinceStartup + 70;
        Directory.CreateDirectory("Captures/Highlight"); EditorApplication.isPaused = false; EditorApplication.update += Tick;
    }
    static void Tick()
    {
        try
        {
            Check(EditorApplication.isPlaying && EditorApplication.timeSinceStartup < deadline, "Play stopped / timeout");
            if (EditorApplication.timeSinceStartup < next) return;
            if (!routine.MoveNext()) { Finish("PASS: preview visibility above fill/ghost; row/column/cross/multi-line preview; out-of-bounds/overlap/failed-drop/commit cleanup; row, column, multiple rows/columns, cross intersection once, 3 lines, 16-line perfect clear; unchanged score formula/combo; immediate occupancy clear; exact snapshot/wave ordering; bounded 64 pool reuse; next placement, reset, Game Over and Continue cleanup; preview/inflate/burst lifecycle. High score restored."); return; }
            next = EditorApplication.timeSinceStartup + (routine.Current is float delay ? delay : 0);
        }
        catch (Exception e) { Finish("FAIL: " + e); }
    }
    static void Finish(string message)
    {
        EditorApplication.update -= Tick; GameEvents.OnCellsCleared -= Capture;
        (routine as IDisposable)?.Dispose(); routine = null;
        foreach (var shape in temporary) if (shape != null) UnityEngine.Object.DestroyImmediate(shape);
        temporary.Clear();
        if (hadRecord) PlayerPrefs.SetInt(Key, record); else PlayerPrefs.DeleteKey(Key);
        PlayerPrefs.Save(); File.WriteAllText("Captures/line-clear-checks.txt", message);
        if (message.StartsWith("PASS")) Debug.Log(message); else Debug.LogError(message);
        if (EditorApplication.isPlaying) SceneManager.LoadScene("MainMenu");
    }
    static IEnumerator Exercise()
    {
        SceneManager.LoadScene("GamePlay"); yield return 1f;
        var board = GridBoard.Instance; var manager = GameManager.Instance;
        var player = UnityEngine.Object.FindFirstObjectByType<BubblePopEffectPlayer>();
        cells = UnityEngine.Object.FindObjectsByType<GridCell>(FindObjectsSortMode.None);
        var sprite = TrayManager.Instance.Pieces.First(p => p != null).BlockSprite;
        GameEvents.OnCellsCleared += Capture;
        int n = board.Model.Size, created = player.CreatedCount;
        var presentation = UnityEngine.Object.FindFirstObjectByType<ClearPresentation>();
        var single = Shape(1, 1);
        // Rows, columns, hole height, hole width. Last case: all cells except (0,0).
        int[,] cases = { {1,0,1,1}, {0,1,1,1}, {2,0,2,1}, {0,2,1,2}, {1,1,1,1}, {3,0,3,1}, {8,8,1,1} };
        for (int k = 0; k < cases.GetLength(0); k++)
        {
            board.ResetBoard(); Check(player.ActiveCount == 0, "Reset left old VFX");
            int rows = cases[k,0], cols = cases[k,1], h = cases[k,2], w = cases[k,3];
            foreach (var c in cells)
            {
                bool target = c.Row < rows || c.Col < cols;
                bool hole = c.Row < h && c.Col < w;
                bool sentinel = k < 6 && c.Row == n-1 && c.Col == n-1;
                c.SetOccupied((target && !hole) || sentinel, Color.white, sprite);
                c.GetComponent<CellView>()?.ResetAnimation();
            }
            captured = null;
            var candidate = Shape(h,w);
            var position = board.CellPosition(0,0);
            int occupiedBefore = cells.Count(c=>c.IsOccupied);
            board.UpdateDragPreview(candidate, position, sprite);
            var highlighted = cells.Where(c=>Get<GameObject>(c,"highlightGlowVisual").activeSelf).ToArray();
            // Artificial perfect-clear setup already contains full lines; preview only
            // announces lines completed by this footprint, as in normal gameplay.
            int previewRows = Mathf.Min(rows,h), previewCols = Mathf.Min(cols,w);
            Check(highlighted.Length == previewRows*n+previewCols*n-previewRows*previewCols, "Preview missed completed lines " + k);
            foreach (var c in highlighted)
            {
                var glow = Get<GameObject>(c,"highlightGlowVisual").GetComponent<SpriteRenderer>();
                var fill = Get<Renderer>(c,"fillRenderer");
                Check(glow.enabled && glow.sprite != null && glow.color.a > 0 &&
                    glow.sortingLayerID == fill.sortingLayerID && glow.sortingOrder > fill.sortingOrder+2,
                    "Preview hidden behind block/ghost");
            }
            Check(cells.Count(c=>c.IsOccupied)==occupiedBefore, "Preview mutated occupancy");
            if(k==4) { ScreenCapture.CaptureScreenshot("Captures/Highlight/preview-cross.png"); yield return .15f; }
            board.UpdateDragPreview(candidate, board.CellPosition(n+2,n+2), sprite);
            Check(cells.All(c=>!Get<GameObject>(c,"highlightGlowVisual").activeSelf), "Out of bounds left highlight");
            board.UpdateDragPreview(candidate, position, sprite);
            board.UpdateDragPreview(candidate, board.CellPosition(n-1,n-1), sprite);
            Check(cells.All(c=>!Get<GameObject>(c,"highlightGlowVisual").activeSelf), "Invalid overlap left highlight");
            board.UpdateDragPreview(candidate, position, sprite);
            Check(!board.TryPlaceShape(candidate,board.CellPosition(n+2,n+2),sprite), "Invalid drop accepted");
            Check(cells.All(c=>!Get<GameObject>(c,"highlightGlowVisual").activeSelf), "Failed drop left highlight");
            board.UpdateDragPreview(candidate, position, sprite);
            Check(board.TryPlaceShape(Shape(h,w), board.CellPosition(0,0), sprite, Color.white), "Placement rejected in scenario " + k);
            Check(cells.All(c=>!Get<GameObject>(c,"highlightGlowVisual").activeSelf), "Commit left preview highlight");
            int expected = rows * n + cols * n - rows * cols;
            Check(captured != null && captured.Count == expected && player.ActiveCount == expected, "Snapshot / pool count scenario " + k);
            Check(cells.Count(c => c.IsOccupied) == (k == 6 ? 0 : 1), "Wrong surviving cells " + k);
            int lineCount = rows + cols;
            Check(Get<int>(presentation,"activeLines")==lineCount,"Energy streak count incorrect");
            var starts=Get<Vector3[]>(presentation,"lineStarts");
            var ends=Get<Vector3[]>(presentation,"lineEnds");
            for(int j=0;j<lineCount;j++) Check(ends[j].x>starts[j].x || ends[j].y>starts[j].y,"Energy sweep direction incorrect");
            int gain = h*w + 10*lineCount*lineCount*manager.CurrentCombo + (k==6?300:0);
            Check(manager.LastScoreGain == gain && manager.CurrentCombo == k+1 && manager.LastPerfectClear == (k==6), "Score/combo changed " + k);
            var effects = Get<List<BubblePopEffect>>(player, "active");
            var config = Get<BubblePopConfig>(player, "runtimeConfig");
            foreach (var effect in effects)
            {
                var old = Get<ClearedCellInfo>(effect, "snapshot");
                int rank = n-1;
                if (old.Row < rows) rank = old.Col;
                if (old.Col < cols) rank = Mathf.Min(rank, old.Row);
                Check(Mathf.Approximately(Get<float>(effect,"delay"), Mathf.Min(rank*config.staggerDelay,config.maximumStagger)), "Wave order incorrect");
                var visual = Get<SpriteRenderer>(effect,"blockVisual");
                Check(visual.sprite == old.Sprite && visual.color == old.Color, "Snapshot appearance not preserved");
            }
            Check(player.CreatedCount == created && created <= config.maximumCount, "Pool grew during clear");
            if (k == 0)
            {
                Check(board.TryPlaceShape(single, board.CellPosition(0,0), sprite, Color.white), "Immediate replacement rejected");
                Check(player.ActiveCount == 0 && board.Model.Occupied(0,0), "Old wave affected new placement");
            }
            if (k == 4) ScreenCapture.CaptureScreenshot("Captures/line-clear-cross.png");
            yield return .6f;
            double end = EditorApplication.timeSinceStartup + 2;
            while (player.ActiveCount > 0 && EditorApplication.timeSinceStartup < end) yield return 0f;
            var hud = UnityEngine.Object.FindFirstObjectByType<GameHUD>();
            Check(hud != null, "Missing GameHUD scene controller");
            Check(Get<TMPro.TextMeshProUGUI>(hud, "highScore").text == manager.CurrentScore.ToString(), "Score UI did not finish count-up " + k);
            Check(Get<TMPro.TextMeshProUGUI>(hud, "score").text == manager.HighScore.ToString(), "Best UI did not update " + k);
            Check(PlayerPrefs.GetInt(Key, 0) == manager.HighScore, "High score not persisted " + k);
            Check(player.ActiveCount == 0, "Wave did not release pool");
            Check(Get<int>(presentation,"activeLines")==0 && Get<SpriteRenderer[]>(presentation,"lines").All(r=>!r.enabled),"Energy streaks not released");
        }
        // Deterministic stage sampling independent of Editor screenshot stalls.
        var prefab = AssetDatabase.LoadAssetAtPath<BubblePopConfig>("Assets/_Project/ScriptableObjects/BubblePopConfig.asset");
        var probe = UnityEngine.Object.Instantiate(prefab.effectPrefab);
        try
        {
            probe.Initialize(prefab); probe.Begin(captured[0], .05f, 1.36f);
            var visual = Get<SpriteRenderer>(probe,"blockVisual");
            probe.Tick(.04f);
            var outline=Get<LineRenderer>(probe,"outline");
            Check(outline.enabled && outline.startColor.r > outline.startColor.b, "Gold outline missing");
            Check(visual.color.r > captured[0].Color.r, "Preview did not brighten");
            probe.Tick(.1f); Check(visual.transform.localScale != captured[0].Scale, "Inflation missing");
            probe.Tick(.07f); Check(Get<SpriteRenderer[]>(probe,"bubbleVisuals").Count(r=>r.enabled)==Mathf.CeilToInt(prefab.bubbleCount*1.36f), "Multi-clear particle amount wrong");
            Check(probe.Tick(1), "Pop not complete"); probe.ResetVisuals();
            Check(!outline.enabled, "Pool reset left gold outline");
            Check(probe.GetComponentsInChildren<SpriteRenderer>(true).All(r=>!r.enabled), "Pool reset left visuals");
        }
        finally { UnityEngine.Object.Destroy(probe.gameObject); }
        // Replay the immutable batch solely to test interruptions without mutating score.
        GameEvents.OnCellsCleared -= Capture;
        GameEvents.TriggerCellsCleared(board,captured); manager.ConfirmGameOver();
        Check(player.ActiveCount == 0, "Game Over left clear VFX");
        yield return 1f;
        var controller = UnityEngine.Object.FindFirstObjectByType<GameOverController>();
        Check(controller.CanRequestRevive, "Continue not ready");
        GameEvents.TriggerCellsCleared(board,captured); controller.Revive();
        Check(player.ActiveCount == 0 && !cells.Any(c=>c.IsOccupied), "Continue left VFX or occupancy");
        Check(TrayManager.Instance.Pieces.Count(p=>p!=null)==3, "Continue did not spawn three pieces");
        yield return .2f;
    }
}

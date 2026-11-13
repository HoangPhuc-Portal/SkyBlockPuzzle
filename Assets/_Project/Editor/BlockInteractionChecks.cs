using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using BlockBlast.Grid;
using BlockBlast.Shape;
using BlockBlast.UI;
using BlockBlast.Managers;

// Explicit menu test: loads a fresh gameplay scene, drives the real input handlers,
// and restores the saved high score even when an assertion fails.
public static class BlockInteractionChecks
{
    static IEnumerator routine;
    static double next, deadline;
    static bool hadRecord;
    static int record;
    const string Key = "BlockBlast_HighScore";
    const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic;
    static T Field<T>(object o, string name) => (T)o.GetType().GetField(name, Flags).GetValue(o);
    static void Check(bool ok, string reason) { if (!ok) throw new Exception(reason); }
    static GridCell[] Cells => UnityEngine.Object.FindObjectsByType<GridCell>(FindObjectsSortMode.None);
    static int Occupied => Cells.Count(c => c.IsOccupied);
    static DraggableShape Piece => TrayManager.Instance.Pieces.First(p => p != null);
    static PointerEventData Pointer(Vector2 point) => new PointerEventData(EventSystem.current) { position = point };
    static Vector2 DropPoint(DraggableShape p)
    {
        var board = GridBoard.Instance;
        var center = board.CellPosition(0, 0) + new Vector3(p.ShapeData.columns - 1, p.ShapeData.rows - 1) * board.CellSize * .5f;
        return (Vector2)Camera.main.WorldToScreenPoint(center) - Vector2.up * Field<float>(p, "dragOffsetYScreen");
    }
    static void Begin(DraggableShape p) => p.OnBeginDrag(Pointer(RectTransformUtility.WorldToScreenPoint(null, p.transform.position)));
    static void Settled(DraggableShape p, Transform parent, Vector2 pos, Vector3 scale)
    {
        Check(p != null && p.transform.parent == parent, "Piece lost / wrong parent");
        Check(Vector2.Distance(((RectTransform)p.transform).anchoredPosition, pos) < .001f, "Tray position drift");
        Check(Vector3.Distance(p.transform.localScale, scale) < .0001f, "Tray scale drift");
        Check(!Field<bool>(p, "isDragging") && !Field<bool>(p, "returning") && Field<Coroutine>(p, "motion") == null, "Motion not cancelled");
    }
    [MenuItem("Block Blast/Tests/Run Block Interaction Play Checks")]
    static void Run()
    {
        if (!EditorApplication.isPlaying) throw new Exception("Enter Play Mode first. This test loads a fresh GamePlay scene.");
        if (routine != null) return;
        hadRecord = PlayerPrefs.HasKey(Key); record = PlayerPrefs.GetInt(Key);
        Directory.CreateDirectory("Captures");
        routine = Exercise(); next = 0; deadline = EditorApplication.timeSinceStartup + 60;
        EditorApplication.update += Tick;
    }
    static void Tick()
    {
        try
        {
            Check(EditorApplication.isPlaying && EditorApplication.timeSinceStartup < deadline, "Play stopped / timeout");
            if (EditorApplication.timeSinceStartup < next) return;
            if (!routine.MoveNext()) { Finish("PASS: drag lift; invalid/outside return; cancel; repeated drag; valid placement exactly once; bounce/flash restore; preview during bounce; clear/reset cancellation; Game Over cancellation; Continue clears animations and spawns 3 pieces. Saved high score restored."); return; }
            next = EditorApplication.timeSinceStartup + (routine.Current is float delay ? delay : 0);
        }
        catch (Exception e) { Finish("FAIL: " + e); }
    }
    static void Finish(string result)
    {
        EditorApplication.update -= Tick; (routine as IDisposable)?.Dispose(); routine = null;
        if (hadRecord) PlayerPrefs.SetInt(Key, record); else PlayerPrefs.DeleteKey(Key);
        PlayerPrefs.Save();
        File.WriteAllText("Captures/block-interaction-checks.txt", result);
        if (result.StartsWith("PASS")) Debug.Log(result); else Debug.LogError(result);
        if (EditorApplication.isPlaying) SceneManager.LoadScene("MainMenu");
    }
    static IEnumerator Exercise()
    {
        SceneManager.LoadScene("GamePlay");
        yield return 1f;
        var p = Piece; var parent = p.transform.parent; var pos = ((RectTransform)p.transform).anchoredPosition; var scale = p.transform.localScale;
        Begin(p); Check(Field<bool>(p, "isDragging"), "Drag did not start");
        var firstScale = p.transform.localScale;
        yield return .15f;
        Check(p.transform.localScale != firstScale, "No smooth drag lift");
        p.OnEndDrag(Pointer(new Vector2(-1000, -1000)));
        Check(Field<bool>(p, "returning"), "Outside drop did not tween back");
        yield return .4f;
        Settled(p, parent, pos, scale); Check(Occupied == 0, "Outside drop changed board");
        for (int i = 0; i < 3; i++)
        {
            Begin(p); yield return .02f; p.OnCancel(new BaseEventData(EventSystem.current));
            yield return .4f; Settled(p, parent, pos, scale);
        }
        Begin(p);
        foreach (var c in Cells) c.SetOccupied(true, Color.white, p.BlockSprite);
        int count = Occupied;
        p.OnEndDrag(Pointer(DropPoint(p)));
        yield return .4f;
        Check(Occupied == count, "Invalid drop mutated occupancy"); Settled(p, parent, pos, scale);
        GridBoard.Instance.ResetBoard();
        Begin(p); var drop = Pointer(DropPoint(p)); int expected = p.ShapeData.Cells.Count;
        p.OnDrag(drop); p.OnEndDrag(drop); p.OnEndDrag(drop);
        Check(Occupied == expected, "Placement duplicated/lost cells");
        Check(!p.gameObject.activeSelf, "Committed tray visual still visible");
        var cell = Cells.First(c => c.IsOccupied); var view = cell.GetComponent<CellView>();
        Check(view.IsAnimating, "No placement animation");
        var fill = Field<Renderer>(cell, "fillRenderer"); var rest = Field<Vector3>(view, "restScale"); var color = ((SpriteRenderer)fill).color;
        yield return .04f;
        Check(fill.transform.localScale != rest, "Bounce not visible");
        cell.SetStateNormal(); Check(view.IsAnimating, "Preview cleanup stopped bounce");
        var glow = cell.transform.Find("PlacementGlow").GetComponent<SpriteRenderer>();
        Check(glow.enabled && glow.color.a > 0, "Placement flash missing");
        ScreenCapture.CaptureScreenshot("Captures/block-placement-flash.png");
        yield return .3f;
        // Capturing a full-resolution Simulator frame can stall the Editor; allow the
        // player loop to finish its coroutine before asserting final state.
        double settleDeadline = EditorApplication.timeSinceStartup + 2;
        while (view.IsAnimating && EditorApplication.timeSinceStartup < settleDeadline) yield return 0f;
        Check(!view.IsAnimating && !glow.enabled && fill.transform.localScale == rest && ((SpriteRenderer)fill).color == color,
            $"Placement did not restore visuals: running={view.IsAnimating}, glow={glow.enabled}, scale={fill.transform.localScale}, rest={rest}, color={((SpriteRenderer)fill).color}, original={color}");
        cell.SetOccupied(true, color, cell.CurrentSprite); cell.SetOccupied(false);
        yield return .3f;
        Check(!view.IsAnimating && !glow.enabled && !fill.gameObject.activeSelf, "Clear left stale animation");
        GridBoard.Instance.ResetBoard();
        p = Piece; parent = p.transform.parent; pos = ((RectTransform)p.transform).anchoredPosition; scale = p.transform.localScale;
        Begin(p); p.OnEndDrag(Pointer(new Vector2(-1000, -1000)));
        cell.SetOccupied(true, Color.white, p.BlockSprite);
        GameManager.Instance.ConfirmGameOver();
        Check(!view.IsAnimating && !glow.enabled, "Game Over did not cancel board animation");
        Settled(p, parent, pos, scale);
        yield return 1f;
        var controller = UnityEngine.Object.FindFirstObjectByType<GameOverController>();
        Check(controller.CanRequestRevive, "Continue not available");
        // Exercise the authorized reward-completion method, without requesting a real ad.
        cell.SetOccupied(true, Color.white, p.BlockSprite);
        controller.Revive();
        yield return .4f;
        Check(Occupied == 0 && !view.IsAnimating && !glow.enabled, "Continue left board effects");
        Check(TrayManager.Instance.Pieces.Count(x => x != null) == 3 && !GameManager.Instance.IsInputLocked, "Continue tray/input incorrect");
        ScreenCapture.CaptureScreenshot("Captures/block-interaction-after-continue.png");
        yield return .1f;
    }
}

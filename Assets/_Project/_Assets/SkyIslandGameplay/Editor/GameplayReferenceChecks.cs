using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using BlockBlast.Grid;
using BlockBlast.Shape;
using BlockBlast.UI;
using BlockBlast.Managers;

public static class GameplayReferenceChecks
{
    static IEnumerator routine;
    static double next, deadline;
    static bool background;
    static readonly StringBuilder log = new StringBuilder();
    const BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
    static void Check(bool pass, string message) { if (!pass) throw new Exception(message); log.AppendLine("PASS " + message); }
    public static void Run()
    {
        if (!EditorApplication.isPlaying || routine != null) throw new Exception("Enter GamePlay Play Mode");
        background = Application.runInBackground; Application.runInBackground = true;
        System.IO.Directory.CreateDirectory("Captures/GameplayReference");
        log.Clear(); log.AppendLine("Reference layout QA UTC " + DateTime.UtcNow.ToString("o"));
        routine = Exercise(); next = 0; deadline = EditorApplication.timeSinceStartup + 120;
        EditorApplication.update += Tick;
    }
    static void Tick()
    {
        try
        {
            if (!EditorApplication.isPlaying || EditorApplication.timeSinceStartup > deadline) throw new Exception("Stopped or timeout");
            if (EditorApplication.timeSinceStartup < next) return;
            if (!routine.MoveNext()) { Finish(null); return; }
            next = EditorApplication.timeSinceStartup + (routine.Current is float ? (float)routine.Current : 0);
        }
        catch (Exception e) { Finish(e.ToString()); }
    }
    static void Finish(string error)
    {
        EditorApplication.update -= Tick; routine = null; Application.runInBackground = background;
        log.AppendLine(error == null ? "ALL CHECKS PASSED" : "FAIL " + error);
        System.IO.Directory.CreateDirectory("Captures/GameplayReference");
        System.IO.File.WriteAllText("Captures/GameplayReference/layout-qa.txt", log.ToString());
        if (error != null) Debug.LogError(error);
    }
    static IEnumerator Exercise()
    {
        var board = GridBoard.Instance; var tray = TrayManager.Instance;
        foreach (int height in new[] { 960, 1170, 1200 })
        {
            GameplayEditModeChecks.SetView(540, height); yield return 1f;
            Check(GameManager.Instance.CurrentScore == 0 && board.GetComponentsInChildren<GridCell>().All(c => !c.IsOccupied), height + ": empty board / score zero");
            Check(tray.Pieces.Count(p => p != null) == 3, height + ": three runtime pieces");
            var a = Camera.main.WorldToScreenPoint(board.CellPosition(0, 0));
            var b = Camera.main.WorldToScreenPoint(board.CellPosition(7, 7));
            Check(a.x > 0 && a.y > 0 && b.x < Screen.width && b.y < Screen.height, height + ": board fully visible");
            Check(Mathf.Abs((b.x - a.x) - (b.y - a.y)) < 1, height + ": square cells / no aspect stretch");
            var frame = GameObject.Find("ReferenceStoneFrame");
            Check(frame.GetComponentsInChildren<Image>().All(i => i.sprite != null && !i.raycastTarget), height + ": frame slices assigned / no input blocking");
            ScreenCapture.CaptureScreenshot("Captures/GameplayReference/" + height + ".png"); yield return .3f;
        }
        var piece = tray.Pieces[0]; var original = piece.ShapeData;
        int count = 0;
        foreach (var shape in ShapeLibraryRebuild.Load()) foreach (var variant in shape.Variants())
        {
            typeof(DraggableShape).GetProperty("ShapeData").SetValue(piece, variant);
            typeof(DraggableShape).GetMethod("BuildVisualShape", Private).Invoke(piece, null);
            yield return .02f;
            var slot = (RectTransform)piece.transform.parent; var corners = new Vector3[4];
            foreach (RectTransform tile in piece.transform)
            {
                tile.GetWorldCorners(corners);
                foreach (var corner in corners) if (!slot.rect.Contains((Vector2)slot.InverseTransformPoint(corner))) throw new Exception("Tray overflow " + variant.Key);
            }
            count++;
        }
        typeof(DraggableShape).GetProperty("ShapeData").SetValue(piece, original);
        typeof(DraggableShape).GetMethod("BuildVisualShape", Private).Invoke(piece, null);
        Check(count == 41, "All 41 orientations fit inside tray artwork slots");
        var settings = UnityEngine.Object.FindFirstObjectByType<SettingsPanel>();
        var button = (Button)typeof(SettingsPanel).GetField("open", Private).GetValue(settings);
        var originalScale = button.transform.localScale;
        for (int i = 0; i < 3; i++)
        {
            button.onClick.Invoke(); yield return .25f;
            Check(settings.IsOpen && GameManager.Instance.IsInputLocked, "Settings open locks input " + i);
            settings.Close(); yield return .25f;
            Check(!settings.IsOpen && !GameManager.Instance.IsInputLocked && Vector3.Distance(button.transform.localScale, originalScale) < .001f, "Settings close restores input and scale " + i);
        }
        GameplayEditModeChecks.SetView(540, 960); yield return 1f;
        ScreenCapture.CaptureScreenshot("Captures/GameplayReference/final.png"); yield return .3f;
        int missing = 0;
        foreach (var root in board.gameObject.scene.GetRootGameObjects()) foreach (var t in root.GetComponentsInChildren<Transform>(true)) missing += GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject);
        Check(missing == 0, "No missing MonoBehaviour references");
    }
}


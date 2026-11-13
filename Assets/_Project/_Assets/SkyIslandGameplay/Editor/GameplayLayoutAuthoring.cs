using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using BlockBlast.Grid;
using BlockBlast.UI;

// Only loaded by the Editor. No ExecuteAlways, gameplay events, spawning or automatic scene saves.
[InitializeOnLoad]
public static class GameplayLayoutAuthoring
{
    static double next;
    static string previous;
    static GameplayLayoutAuthoring() { EditorApplication.update += Preview; }
    static bool CanPreview => !EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isCompiling
        && !EditorApplication.isUpdating && PrefabStageUtility.GetCurrentPrefabStage() == null;
    static BoardView View() => UnityEngine.Object.FindObjectsByType<BoardView>(FindObjectsSortMode.None)
        .FirstOrDefault(v => v.isActiveAndEnabled && v.gameObject.scene == UnityEngine.SceneManagement.SceneManager.GetActiveScene() && v.PreviewLayoutInEditor);
    static void Preview()
    {
        if (!CanPreview || EditorApplication.timeSinceStartup < next) return;
        next = EditorApplication.timeSinceStartup + .15;
        var view = View(); if (view == null) { previous = null; return; }
        Canvas.ForceUpdateCanvases();
        var canvas = view.GetComponentInParent<Canvas>();
        var camera = Camera.main; if (camera == null || canvas == null) return;
        var signature = view.GetInstanceID() + ":" + Screen.width + ":" + Screen.height + ":" + camera.projectionMatrix
            + camera.worldToCameraMatrix + EditorJsonUtility.ToJson(view.Theme);
        foreach (var rect in canvas.GetComponentsInChildren<RectTransform>(true))
            signature += rect.GetInstanceID() + rect.rect.ToString("F3") + rect.localToWorldMatrix;
        if (signature == previous) return;
        previous = signature;
        Refresh(view);
        SceneView.RepaintAll();
    }
    public static void Refresh(BoardView view)
    {
        Canvas.ForceUpdateCanvases();
        view.RefreshAuthoredLayout();
        foreach (var background in UnityEngine.Object.FindObjectsByType<BackgroundView>(FindObjectsSortMode.None))
            if (background.gameObject.scene == view.gameObject.scene) background.RefreshAuthoredLayout();
    }
    [MenuItem("Block Blast/Gameplay/Prepare Editable Scene")]
    public static void Prepare()
    {
        if (!CanPreview) throw new InvalidOperationException("Open the scene in Edit Mode, outside Prefab Mode.");
        var view = UnityEngine.Object.FindFirstObjectByType<BoardView>();
        var vs = new SerializedObject(view);
        var board = (GridBoard)vs.FindProperty("board").objectReferenceValue;
        var bs = new SerializedObject(board);
        var cells = bs.FindProperty("authoredCells");
        Undo.RegisterFullObjectHierarchyUndo(board.gameObject, "Prepare editable gameplay");
        if (cells.arraySize == 0)
        {
            var prefab = (GridCell)bs.FindProperty("cellPrefab").objectReferenceValue;
            var container = new GameObject("AuthoredCells").transform;
            Undo.RegisterCreatedObjectUndo(container.gameObject, "Create authored cells");
            container.SetParent(board.transform, false);
            cells.arraySize = GridBoard.GRID_SIZE * GridBoard.GRID_SIZE;
            for (int r = 0; r < GridBoard.GRID_SIZE; r++) for (int c = 0; c < GridBoard.GRID_SIZE; c++)
            {
                var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab.gameObject, container);
                Undo.RegisterCreatedObjectUndo(go, "Create authored cell");
                go.name = "Cell_" + r + "_" + c;
                go.transform.rotation = Quaternion.identity;
                var cell = go.GetComponent<GridCell>(); cell.Init(r, c);
                cell.GetComponent<CellView>().Configure(view.Theme);
                cells.GetArrayElementAtIndex(r * GridBoard.GRID_SIZE + c).objectReferenceValue = cell;
                PrefabUtility.RecordPrefabInstancePropertyModifications(cell.GetComponent<SpriteRenderer>());
                foreach (var t in go.GetComponentsInChildren<Transform>(true))
                {
                    PrefabUtility.RecordPrefabInstancePropertyModifications(t);
                    PrefabUtility.RecordPrefabInstancePropertyModifications(t.gameObject);
                }
            }
            bs.FindProperty("boardParent").objectReferenceValue = container;
            bs.ApplyModifiedProperties();
        }
        vs.FindProperty("previewLayoutInEditor").boolValue = true; vs.ApplyModifiedProperties();
        var settings = UnityEngine.Object.FindFirstObjectByType<SettingsPanel>();
        Undo.RegisterFullObjectHierarchyUndo(settings.gameObject, "Author Settings navigation");
        typeof(SettingsPanel).GetMethod("BuildNavigation", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(settings, null);
        EditorUtility.SetDirty(settings);
        var environment = UnityEngine.Object.FindFirstObjectByType<BlockBlast.Environment.IslandEnvironment>();
        if (environment != null)
        {
            var es = new SerializedObject(environment);
            var config = (BlockBlast.Environment.IslandEnvironmentSettings)es.FindProperty("settings").objectReferenceValue;
            var renderer = (SpriteRenderer)es.FindProperty("background").objectReferenceValue;
            Undo.RecordObject(renderer, "Match authored background material");
            renderer.sharedMaterial = config.waterMaterial;
        }
        Refresh(view);
        EditorSceneManager.MarkSceneDirty(board.gameObject.scene);
    }
}

[CustomEditor(typeof(BoardView))]
public class AuthoredBoardViewInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("Move/resize BoardRegion to lay out the grid. Edit individual Cell SpriteRenderers or their prefab for artwork. Preview never creates cells or starts gameplay.", MessageType.Info);
        using (new EditorGUI.DisabledScope(EditorApplication.isPlayingOrWillChangePlaymode))
            if (GUILayout.Button("Refresh authored layout")) GameplayLayoutAuthoring.Refresh((BoardView)target);
    }
}

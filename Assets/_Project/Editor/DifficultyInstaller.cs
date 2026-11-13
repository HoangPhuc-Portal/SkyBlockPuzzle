using System;
using System.Linq;
using BlockBlast.Data;
using BlockBlast.Difficulty;
using BlockBlast.Shape;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
public static class DifficultyInstaller
{
    public const string ConfigPath="Assets/_Project/Resources/DifficultyConfig.asset";
    public static string Install()
    {
        if(EditorApplication.isPlaying)throw new InvalidOperationException("Stop Play Mode first");
        var config=AssetDatabase.LoadAssetAtPath<DifficultyConfig>(ConfigPath);
        if(config==null){config=ScriptableObject.CreateInstance<DifficultyConfig>();AssetDatabase.CreateAsset(config,ConfigPath);}
        var shapes=ShapeLibraryRebuild.Load();
        foreach(var shape in shapes)
        {
            if(shape==null)throw new Exception("Catalog missing");
            var difficulty=ShapeDifficulty.Medium;
            switch(shape.name)
            {
                case "Single":case "Line_2":case "Line_3":case "SmallCorner_3":case "Square_2x2":difficulty=ShapeDifficulty.Easy;break;
                case "Line_5":case "Square_3x3":case "LargeCorner_5":case "Diagonal_3":difficulty=ShapeDifficulty.Hard;break;
            }
            shape.difficulty=difficulty;EditorUtility.SetDirty(shape);
        }
        string prefabPath="Assets/_Project/Prefabs/UI/GameplayScreen.prefab";
        var prefab=PrefabUtility.LoadPrefabContents(prefabPath);
        try{foreach(var tray in prefab.GetComponentsInChildren<TrayManager>(true))Assign(tray,config);PrefabUtility.SaveAsPrefabAsset(prefab,prefabPath);}finally{PrefabUtility.UnloadPrefabContents(prefab);}
        foreach(string path in new[]{"Assets/Scenes/GamePlay.unity","Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity"})
        {
            var scene=UnityEngine.SceneManagement.SceneManager.GetSceneByPath(path);bool opened=!scene.isLoaded;
            if(opened)scene=EditorSceneManager.OpenScene(path,OpenSceneMode.Additive);
            foreach(var root in scene.GetRootGameObjects())foreach(var tray in root.GetComponentsInChildren<TrayManager>(true))Assign(tray,config);
            EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);if(opened)EditorSceneManager.CloseScene(scene,true);
        }
        AssetDatabase.SaveAssets();return "DDA installed; Easy="+shapes.Count(s=>s.difficulty==ShapeDifficulty.Easy)+", Medium="+shapes.Count(s=>s.difficulty==ShapeDifficulty.Medium)+", Hard="+shapes.Count(s=>s.difficulty==ShapeDifficulty.Hard);
    }
    static void Assign(TrayManager tray,DifficultyConfig config){var so=new SerializedObject(tray);so.FindProperty("difficultyConfig").objectReferenceValue=config;so.ApplyModifiedPropertiesWithoutUndo();EditorUtility.SetDirty(tray);}
}

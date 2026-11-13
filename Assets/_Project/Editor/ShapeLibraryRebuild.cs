using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BlockBlast.Data;
using BlockBlast.Shape;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class ShapeLibraryRebuild
{
    public const string Folder = "Assets/_Project/ScriptableObjects/Shapes";
    public static readonly string[] Names = {
        "Single", "Line_2", "Line_3", "Line_4", "Line_5", "Square_2x2", "Square_3x3", "Rectangle_2x3",
        "SmallCorner_3", "L_4", "J_4", "LargeCorner_5", "T_4", "S_4", "Z_4", "Diagonal_2", "Diagonal_3"
    };
    public static readonly string[] Designs = {
        "1", "11", "111", "1111", "11111", "11/11", "111/111/111", "111/111",
        "10/11", "10/10/11", "01/01/11", "100/100/111", "111/010", "011/110", "110/011", "10/01", "100/010/001"
    };
    public static readonly int[] RotationCounts = {1,2,2,2,2,1,1,2,4,4,4,4,4,2,2,2,2};
    public static ShapeData[] Load() => Names.Select((name,i) => AssetDatabase.LoadAssetAtPath<ShapeData>(PathFor(i))).ToArray();
    public static string PathFor(int i) => Folder + (i<15?"/Core/":"/Diagonal/") + Names[i]+".asset";
    public static bool[,] Matrix(string design)
    {
        var rows=design.Split('/');int size=Math.Max(3,Math.Max(rows.Length,rows[0].Length));
        var matrix=new bool[size,size];
        for(int r=0;r<rows.Length;r++)for(int c=0;c<rows[r].Length;c++)matrix[r,c]=rows[r][c]=='1';
        return matrix;
    }
    public static void Configure(ShapeData shape, string design, bool diagonal)
    {
        var matrix=Matrix(design);int size=matrix.GetLength(0);
        var so=new SerializedObject(shape);
        so.FindProperty("designSize").intValue=size;
        so.FindProperty("migrated").boolValue=true;
        so.FindProperty("rows").intValue=size;so.FindProperty("columns").intValue=size;
        so.FindProperty("matrixData").arraySize=0;
        var cells=so.FindProperty("cells");cells.arraySize=size*size;
        for(int i=0;i<size*size;i++)cells.GetArrayElementAtIndex(i).boolValue=matrix[i/size,i%size];
        so.FindProperty("rotationPolicy").enumValueIndex=(int)RotationPolicy.UniqueRotations;
        so.FindProperty("allowCornerConnections").boolValue=diagonal;
        so.FindProperty("blockColor").colorValue=Color.white;
        so.ApplyModifiedPropertiesWithoutUndo();EditorUtility.SetDirty(shape);
    }
    static void Assign(TrayManager tray, ShapeData[] shapes)
    {
        var so=new SerializedObject(tray);var list=so.FindProperty("availableShapes");list.arraySize=shapes.Length;
        for(int i=0;i<shapes.Length;i++)list.GetArrayElementAtIndex(i).objectReferenceValue=shapes[i];
        so.ApplyModifiedPropertiesWithoutUndo();EditorUtility.SetDirty(tray);
    }
    // Intentionally not a menu command: replacement is an explicit one-time operation with external backup.
    public static string Rebuild()
    {
        if(EditorApplication.isPlaying)throw new InvalidOperationException("Stop Play Mode first.");
        if(!Directory.Exists("ShapeLibraryBackup_20261104/Shapes"))throw new InvalidOperationException("Original shape backup missing.");
        var old=Directory.GetFiles(Folder,"*.asset",SearchOption.TopDirectoryOnly).Select(p=>p.Replace('\\','/')).ToArray();
        var oldGuids=old.Select(AssetDatabase.AssetPathToGUID).ToArray();
        if(!AssetDatabase.IsValidFolder(Folder+"/Core"))AssetDatabase.CreateFolder(Folder,"Core");
        if(!AssetDatabase.IsValidFolder(Folder+"/Diagonal"))AssetDatabase.CreateFolder(Folder,"Diagonal");
        for(int i=0;i<Names.Length;i++)
        {
            if(AssetDatabase.LoadAssetAtPath<ShapeData>(PathFor(i))!=null)throw new InvalidOperationException("New catalog already exists; refusing overwrite: "+PathFor(i));
            var shape=ScriptableObject.CreateInstance<ShapeData>();shape.name=Names[i];Configure(shape,Designs[i],i>=15);
            AssetDatabase.CreateAsset(shape,PathFor(i));
        }
        AssetDatabase.SaveAssets();var shapes=Load();Validate();
        string prefabPath="Assets/_Project/Prefabs/UI/GameplayScreen.prefab";
        var prefab=PrefabUtility.LoadPrefabContents(prefabPath);
        try { foreach(var tray in prefab.GetComponentsInChildren<TrayManager>(true))Assign(tray,shapes);PrefabUtility.SaveAsPrefabAsset(prefab,prefabPath); }
        finally { PrefabUtility.UnloadPrefabContents(prefab); }
        foreach(string path in new[]{"Assets/Scenes/GamePlay.unity","Assets/_Project/Environment/Backups/GamePlay_BeforeTask01.unity"})
        {
            var scene=UnityEngine.SceneManagement.SceneManager.GetSceneByPath(path);
            bool opened=!scene.isLoaded;
            if(opened)scene=EditorSceneManager.OpenScene(path,OpenSceneMode.Additive);
            foreach(var root in scene.GetRootGameObjects())foreach(var tray in root.GetComponentsInChildren<TrayManager>(true))Assign(tray,shapes);
            EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
            if(opened)EditorSceneManager.CloseScene(scene,true);
        }
        // Verify every serialized consumer was migrated before removing old assets.
        foreach(string p in AssetDatabase.GetAllAssetPaths().Where(p=>p.StartsWith("Assets/")&&(p.EndsWith(".unity")||p.EndsWith(".prefab")||p.EndsWith(".asset"))))
        {
            if(old.Contains(p))continue;
            var text=File.ReadAllText(p);
            if(oldGuids.Any(g=>text.Contains(g)))throw new InvalidOperationException("Old shape reference remains in "+p+"; originals were not removed.");
        }
        foreach(string path in old)if(!AssetDatabase.DeleteAsset(path))throw new InvalidOperationException("Could not remove old shape "+path);
        AssetDatabase.SaveAssets();
        return "Replaced "+old.Length+" original assets; installed 17 roots / 41 unique orientations in prefab, GamePlay and archived scene. "+Validate();
    }
    [MenuItem("Block Blast/Tests/Validate Shape Catalog")]
    public static void ValidateMenu() => Debug.Log(Validate());
    public static string Validate()
    {
        var shapes=Load();var keys=new HashSet<string>();int count=0;
        for(int i=0;i<shapes.Length;i++)
        {
            var s=shapes[i];if(s==null||!s.IsValid)throw new Exception("Invalid/missing "+Names[i]);
            if(s.rotationPolicy!=RotationPolicy.UniqueRotations)throw new Exception("Incorrect rotation policy "+s.name);
            var variants=s.Variants();if(variants.Count!=RotationCounts[i])throw new Exception("Wrong orientations "+s.name);
            int cells=Designs[i].Count(c=>c=='1');
            foreach(var v in variants)if(v.Cells.Count!=cells||!keys.Add(v.Key))throw new Exception("Wrong cell count/duplicate "+s.name);
            count+=variants.Count;
            if(i==14&&count!=37)throw new Exception("Core does not have 37 orientations");
        }
        if(count!=41)throw new Exception("Expected 41 with diagonals");
        // Independent fixed-orientation checklist supplied by the user.
        string[] reference={"1","11","1/1","111","1/1/1","1111","1/1/1/1","11111","1/1/1/1/1","11/11","111/111/111","111/111","11/11/11","10/11","01/11","11/10","11/01","10/10/11","01/01/11","111/100","111/001","11/01/01","11/10/10","001/111","100/111","100/100/111","001/001/111","111/100/100","111/001/001","111/010","010/111","10/11/10","01/11/01","110/011","011/110","10/11/01","01/11/10","10/01","01/10","100/010/001","001/010/100"};
        foreach(var design in reference)if(!keys.Contains(new ShapeRuntimeData(shapes[0],Matrix(design),0).Key))throw new Exception("Missing requested orientation: "+design);
        return "PASS: 15 core roots cover exactly 37 specified orientations; 2 diagonal roots add 4; all cell counts/rotation policies valid, no duplicates.";
    }
}

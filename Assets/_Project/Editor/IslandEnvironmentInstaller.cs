using System;
using System.Linq;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using BlockBlast.Environment;

public static class IslandEnvironmentInstaller
{
    const string Root="Assets/_Project/Environment";
    static void Set(UnityEngine.Object o,string field,UnityEngine.Object value)
    {var s=new SerializedObject(o);s.FindProperty(field).objectReferenceValue=value;s.ApplyModifiedPropertiesWithoutUndo();}
    [MenuItem("Block Blast/Environment/Install Task 01")]
    public static void Install()
    {
        if(EditorApplication.isPlaying)throw new InvalidOperationException("Stop Play Mode before installing");
        var importer=(TextureImporter)AssetImporter.GetAtPath(Root+"/Art/CloudLeafAtlas.png");
        importer.GetSourceTextureWidthAndHeight(out int width,out int height);
        importer.textureType=TextureImporterType.Sprite;importer.spriteImportMode=SpriteImportMode.Multiple;
        importer.alphaIsTransparency=true;importer.mipmapEnabled=false;importer.maxTextureSize=1024;
        importer.filterMode=FilterMode.Bilinear;importer.textureCompression=TextureImporterCompression.Compressed;
        importer.spritePixelsPerUnit=100;
#pragma warning disable 618
        importer.spritesheet=new[]{
            new SpriteMetaData{name="Cloud",rect=new Rect(0,height*.5f,width,height*.5f),alignment=9,pivot=Vector2.one*.5f},
            new SpriteMetaData{name="Leaf",rect=new Rect(width*.32f,height*.055f,width*.38f,height*.41f),alignment=9,pivot=Vector2.one*.5f}};
#pragma warning restore 618
        importer.SaveAndReimport();
        var sprites=AssetDatabase.LoadAllAssetsAtPath(importer.assetPath).OfType<Sprite>().ToArray();
        var cloud=sprites.Single(s=>s.name=="Cloud");var leaf=sprites.Single(s=>s.name=="Leaf");
        string materialPath=Root+"/Materials/IslandWater.mat";
        var material=AssetDatabase.LoadAssetAtPath<Material>(materialPath);
        if(material==null){material=new Material(Shader.Find("SkyBlock/IslandWaterUnlit"));AssetDatabase.CreateAsset(material,materialPath);}
        if(!AssetDatabase.IsValidFolder(Root+"/Backups"))AssetDatabase.CreateFolder(Root,"Backups");
        var active=SceneManager.GetActiveScene();
        foreach(string name in new[]{"MainMenu","GamePlay"})
        {
            var scene=SceneManager.GetSceneByPath("Assets/Scenes/"+name+".unity");bool opened=!scene.isLoaded;
            if(opened)scene=EditorSceneManager.OpenScene("Assets/Scenes/"+name+".unity",OpenSceneMode.Additive);
            string backup=Root+"/Backups/"+name+"_BeforeTask01.unity";
            if(!File.Exists(backup)&&!EditorSceneManager.SaveScene(scene,backup,true))throw new Exception("Could not back up "+name);
            var backgrounds=scene.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<SpriteRenderer>(true)).Where(r=>r.name=="Background").ToArray();
            if(backgrounds.Length!=1||backgrounds[0].sprite==null)throw new Exception("Expected one valid background in "+name);
            var camera=scene.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Camera>(true)).Single(c=>c.CompareTag("MainCamera"));
            bool game=name=="GamePlay";
            string configPath=Root+"/Configs/"+name+"Environment.asset";
            var config=AssetDatabase.LoadAssetAtPath<IslandEnvironmentSettings>(configPath);
            if(config==null)
            {
                config=ScriptableObject.CreateInstance<IslandEnvironmentSettings>();config.cloudSprite=cloud;config.leafSprite=leaf;config.waterMaterial=material;
                config.cloudOpacity=game?.13f:.23f;config.waterStrength=game?.18f:.16f;config.leafSpawnRate=game?.2f:.27f;
                config.waterAreas=game?new[]{new Vector4(.32f,.08f,.32f,.40f),Vector4.zero,Vector4.zero,Vector4.zero}
                    :new[]{new Vector4(.477f,.14f,.078f,.265f),new Vector4(.137f,.665f,.047f,.15f),new Vector4(.718f,.60f,.04f,.19f),new Vector4(.12f,.01f,.12f,.12f)};
                AssetDatabase.CreateAsset(config,configPath);
            }
            string prefabPath=Root+"/Prefabs/"+name+"Environment.prefab";
            var prefab=AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if(prefab==null)
            {
                var temp=new GameObject(name+"Environment");var component=temp.AddComponent<IslandEnvironment>();Set(component,"settings",config);
                var so=new SerializedObject(component);so.FindProperty("gameplay").boolValue=game;so.ApplyModifiedPropertiesWithoutUndo();
                prefab=PrefabUtility.SaveAsPrefabAsset(temp,prefabPath);UnityEngine.Object.DestroyImmediate(temp);
            }
            var existing=scene.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<IslandEnvironment>(true)).ToArray();
            if(existing.Length>1)throw new Exception("Duplicate environment in "+name);
            var env=existing.Length==1?existing[0]:((GameObject)PrefabUtility.InstantiatePrefab(prefab,scene)).GetComponent<IslandEnvironment>();
            Set(env,"settings",config);Set(env,"background",backgrounds[0]);Set(env,"viewCamera",camera);
            PrefabUtility.RecordPrefabInstancePropertyModifications(env);
            EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
            if(opened)EditorSceneManager.CloseScene(scene,true);
        }
        SceneManager.SetActiveScene(active);AssetDatabase.SaveAssets();
        Debug.Log("ENVIRONMENT INSTALL PASS: scene backups, 2 configs/prefabs, cloud+leaf atlas, masked URP waterfall; camera/board unchanged.");
    }
}

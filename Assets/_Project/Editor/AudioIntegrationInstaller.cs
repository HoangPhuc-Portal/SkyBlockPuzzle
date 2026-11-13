using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using BlockBlast.Managers;
using BlockBlast.UI;

public static class AudioIntegrationInstaller
{
    const string Prefab = "Assets/_Project/Prefabs/Audio/AudioManager.prefab";
    static void Set(UnityEngine.Object target,string name,UnityEngine.Object value)
    { var s=new SerializedObject(target);s.FindProperty(name).objectReferenceValue=value;s.ApplyModifiedPropertiesWithoutUndo(); }
    static AudioSource Source(Transform parent,string name,bool loop)
    {
        var go=new GameObject(name);go.transform.SetParent(parent,false);
        var s=go.AddComponent<AudioSource>();s.playOnAwake=false;s.loop=loop;s.spatialBlend=0;return s;
    }
    static void Buttons(GameObject root)
    { foreach(var b in root.GetComponentsInChildren<Button>(true)) if(b.GetComponent<ButtonAudio>()==null)b.gameObject.AddComponent<ButtonAudio>(); }
    [MenuItem("Block Blast/Audio/Install and validate")]
    public static void Install()
    {
        if(EditorApplication.isPlaying)throw new InvalidOperationException("Stop Play Mode first");
        var inventory=new StringBuilder("File | Seconds | Channels | Hz | LoadType | Compression | Quality | Preload\n");
        foreach(string guid in AssetDatabase.FindAssets("t:AudioClip",new[]{"Assets/_Project/Audio"}))
        {
            string path=AssetDatabase.GUIDToAssetPath(guid);var clip=AssetDatabase.LoadAssetAtPath<AudioClip>(path);
            var importer=(AudioImporter)AssetImporter.GetAtPath(path);var sample=importer.defaultSampleSettings;
            // Short interaction SFX must be ready on the first button/drag action.
            if(!sample.preloadAudioData){sample.preloadAudioData=true;importer.defaultSampleSettings=sample;importer.SaveAndReimport();}
            inventory.AppendLine($"{path} | {clip.length:F3} | {clip.channels} | {clip.frequency} | {sample.loadType} | {sample.compressionFormat} | {sample.quality} | {sample.preloadAudioData}");
        }
        Directory.CreateDirectory("Captures");File.WriteAllText("Captures/audio-inventory.txt",inventory.ToString());
        if(!AssetDatabase.IsValidFolder("Assets/_Project/Prefabs/Audio"))AssetDatabase.CreateFolder("Assets/_Project/Prefabs","Audio");
        GameObject prefab=AssetDatabase.LoadAssetAtPath<GameObject>(Prefab);
        if(prefab==null)
        {
            var root=new GameObject("AudioManager");var manager=root.AddComponent<AudioManager>();
            Set(manager,"sfxSource",Source(root.transform,"SFX",false));Set(manager,"bgmSource",Source(root.transform,"BGM",true));
            string[] fields={"buttonClip","dragClip","blockPlaceClip","lineClearClip","gameOverClip"};
            string[] files={"select_button","drag_block","drop_block","xoa_hang_cot","gameOver"};
            for(int i=0;i<fields.Length;i++)
            {
                var clip=AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/_Project/Audio/"+files[i]+".mp3");
                if(clip==null)throw new InvalidOperationException("Missing clip "+files[i]);Set(manager,fields[i],clip);
            }
            prefab=PrefabUtility.SaveAsPrefabAsset(root,Prefab);UnityEngine.Object.DestroyImmediate(root);
        }
        var active=SceneManager.GetActiveScene();
        foreach(string path in new[]{"Assets/Scenes/MainMenu.unity","Assets/Scenes/GamePlay.unity"})
        {
            var scene=SceneManager.GetSceneByPath(path);bool opened=!scene.isLoaded;
            if(opened)scene=EditorSceneManager.OpenScene(path,OpenSceneMode.Additive);
            var managers=scene.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<AudioManager>(true)).ToArray();
            bool installed=false;
            foreach(var old in managers)
            {
                if(PrefabUtility.GetCorrespondingObjectFromSource(old.gameObject)==prefab){installed=true;continue;}
                // The legacy component shared _Managers with GameStateManager. Leave that manager intact.
                var data=new SerializedObject(old);
                foreach(string field in new[]{"sfxSource","bgmSource"})
                {
                    var source=data.FindProperty(field).objectReferenceValue as AudioSource;
                    if(source!=null)UnityEngine.Object.DestroyImmediate(source);
                }
                UnityEngine.Object.DestroyImmediate(old);
            }
            if(!installed)PrefabUtility.InstantiatePrefab(prefab,scene);
            foreach(var root in scene.GetRootGameObjects())Buttons(root);
            EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
            if(opened)EditorSceneManager.CloseScene(scene,true);
        }
        SceneManager.SetActiveScene(active);
        const string gameplay="Assets/_Project/Prefabs/UI/GameplayScreen.prefab";
        var ui=PrefabUtility.LoadPrefabContents(gameplay);
        try { Buttons(ui);PrefabUtility.SaveAsPrefabAsset(ui,gameplay); }
        finally { PrefabUtility.UnloadPrefabContents(ui); }
        AssetDatabase.SaveAssets();
        Debug.Log("AUDIO INSTALL PASS: 5 clips, persistent AudioManager prefab in MainMenu/GamePlay, UI ButtonAudio, inventory exported.");
    }
}

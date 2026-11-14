using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class FeatureRemovalVerification
{
    const string Key = "FeatureRemovalVerification.Stage";
    static double started;
    static FeatureRemovalVerification() { EditorApplication.update += Tick; started = EditorApplication.timeSinceStartup; }
    public static void Run()
    {
        try
        {
            Debug.Log(BlockBlastRegressionChecks.Run());
            Debug.Log(UIRegressionChecks.Model());
            Debug.Log(UIRegressionChecks.GhostFallback());
            foreach (var path in new[]{"Assets/Scenes/MainMenu.unity", "Assets/Scenes/GamePlay.unity"})
            {
                var scene=EditorSceneManager.OpenScene(path);
                foreach(var root in scene.GetRootGameObjects())
                foreach(var t in root.GetComponentsInChildren<Transform>(true))
                {
                    if(t.name=="Hint" || t.name=="HintCounter" || t.name=="HintRewardPopup") throw new Exception("Remaining UI: "+t.name);
                    if(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject)>0) throw new Exception("Missing script: "+t.name);
                }
            }
            SessionState.SetInt(Key,1);
            EditorApplication.isPlaying=true;
        }
        catch(Exception e) { Debug.LogException(e); EditorApplication.Exit(1); }
    }
    static void Tick()
    {
        int stage=SessionState.GetInt(Key,0);
        if(stage==0 || EditorApplication.isCompiling) return;
        if(EditorApplication.timeSinceStartup-started>240) { Debug.LogError("Verification timeout"); EditorApplication.Exit(2); }
        if(stage==1 && EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode.Equals(false))
        {
            if(EditorApplication.timeSinceStartup-started<5) return;
            SessionState.SetInt(Key,2);
            if(File.Exists("Captures/offline-online-ads-checks.txt")) File.Copy("Captures/offline-online-ads-checks.txt","Captures/HintRemoval/previous-ads-checks.txt",true);
            SessionState.SetString("FeatureRemovalVerification.Time",DateTime.UtcNow.ToString("o"));
            OfflineAdsChecks.Run();
        }
        if(stage==2 && File.Exists("Captures/offline-online-ads-checks.txt") && File.GetLastWriteTimeUtc("Captures/offline-online-ads-checks.txt")>DateTime.Parse(SessionState.GetString("FeatureRemovalVerification.Time",""),null,System.Globalization.DateTimeStyles.RoundtripKind))
        {
            bool ok=File.ReadAllText("Captures/offline-online-ads-checks.txt").Contains("ALL PASS");
            SessionState.SetInt(Key,0); EditorApplication.Exit(ok?0:1);
        }
    }
}

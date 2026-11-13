#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AdMobLifecycleChecks
{
    private static int stage, managerId;
    private static double deadline, nextCheck;
    [MenuItem("Block Blast/Tests/AdMob scene lifecycle (Play Mode)")]
    private static void Start()
    {
        if (!EditorApplication.isPlaying || SceneManager.GetActiveScene().name != "MainMenu")
        { Debug.LogWarning("Start Play Mode from MainMenu before running AdMob lifecycle checks."); return; }
        EditorApplication.update -= Tick;
        stage = 0; deadline = EditorApplication.timeSinceStartup + 45; nextCheck = 0;
        EditorApplication.update += Tick;
    }
    private static int VisibleBanners() => UnityEngine.Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None)
        .Count(c => c.gameObject.name == "ADAPTIVE(Clone)" && c.gameObject.activeInHierarchy);
    private static void Tick()
    {
        if (!EditorApplication.isPlaying) { EditorApplication.update -= Tick; return; }
        if (EditorApplication.timeSinceStartup < nextCheck) return;
        try
        {
            if (EditorApplication.timeSinceStartup > deadline) throw new Exception("Timed out waiting for SDK simulator banner");
            var ads = AdsManager.Instance;
            if (ads == null) return;
            if (stage == 0)
            {
                if (!ads.BannerVisible || VisibleBanners() != 1) return;
                managerId = ads.GetInstanceID();
                ads.LeaveMainMenu();
                if (ads.BannerVisible) throw new Exception("Banner not hidden before load");
                SceneManager.LoadScene("GamePlay"); stage = 1;
                nextCheck = EditorApplication.timeSinceStartup + 1;
            }
            else if (stage == 1)
            {
                if (ads.BannerVisible || VisibleBanners() != 0) throw new Exception("Banner leaked into Gameplay");
                SceneManager.LoadScene("MainMenu"); stage = 2;
                nextCheck = EditorApplication.timeSinceStartup + 1;
            }
            else
            {
                if (!ads.BannerVisible || VisibleBanners() != 1) return;
                if (ads.GetInstanceID() != managerId || ads.InitializationCalls != 1 ||
                    UnityEngine.Object.FindObjectsByType<AdsManager>(FindObjectsSortMode.None).Length != 1)
                    throw new Exception("Duplicate manager or SDK initialization");
                Finish("PASS: MainMenu visible banner=1; hidden before load; Gameplay visible banner=0; returned MainMenu visible banner=1; same singleton; initialization=1. Banner creations=" + ads.BannerCreations);
            }
        }
        catch (Exception e) { Finish("FAIL: " + e.Message); }
    }
    private static void Finish(string result)
    {
        EditorApplication.update -= Tick;
        Directory.CreateDirectory("Captures");
        File.WriteAllText("Captures/admob-lifecycle-check.txt", result);
        if (result.StartsWith("PASS")) Debug.Log(result); else Debug.LogError(result);
    }
}
#endif

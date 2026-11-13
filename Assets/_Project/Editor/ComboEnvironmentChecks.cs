using System;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using BlockBlast.Environment;
using BlockBlast.Managers;
using BlockBlast.Grid;
using BlockBlast.UI;

public static class ComboEnvironmentChecks
{
    static IEnumerator run;static double next,deadline;static bool had,backgroundSimulation;static int record;
    const string Key="BlockBlast_HighScore";
    static void Check(bool value,string message){if(!value)throw new Exception(message);}
    [MenuItem("Block Blast/Environment/Run Combo Play checks")]
    public static void Run()
    {
        if(!EditorApplication.isPlaying||run!=null)return;
        backgroundSimulation=Application.runInBackground;Application.runInBackground=true;EditorApplication.isPaused=false;
        had=PlayerPrefs.HasKey(Key);record=PlayerPrefs.GetInt(Key);deadline=EditorApplication.timeSinceStartup+80;
        run=Exercise();next=0;EditorApplication.update+=Tick;
    }
    static void Tick()
    {
        try {Check(EditorApplication.isPlaying&&EditorApplication.timeSinceStartup<deadline,"Timeout / Play stopped");if(Time.time<next)return;
            if(!run.MoveNext()){Finish("PASS: authoritative combo x1..8 mapping, score formula, hold on 2 no-clear turns, reset on third, smooth fade, particle cap, Game Over, actual Continue, Replay reset, disable/re-enable, low quality and scene unload. Record restored.");return;}
            next=Time.time+(run.Current is float?(float)run.Current:0);
        }catch(Exception e){Finish("FAIL: "+e);}
    }
    static void Finish(string result)
    {
        EditorApplication.update-=Tick;(run as IDisposable)?.Dispose();run=null;
        Application.runInBackground=backgroundSimulation;
        if(had)PlayerPrefs.SetInt(Key,record);else PlayerPrefs.DeleteKey(Key);PlayerPrefs.Save();
        Directory.CreateDirectory("Captures");File.WriteAllText("Captures/combo-environment-checks.txt",result);
        if(result.StartsWith("PASS"))Debug.Log(result);else Debug.LogError(result);
        if(EditorApplication.isPlaying)SceneManager.LoadScene("MainMenu");
    }
    static void Score(GameManager gm,int lines){typeof(GameManager).GetMethod("ScorePlacement",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(gm,new object[]{1,lines,false});}
    static IEnumerator Exercise()
    {
        SceneManager.LoadScene("GamePlay");yield return 1f;
        var gm=GameManager.Instance;var fx=UnityEngine.Object.FindFirstObjectByType<ComboEnvironment>();var board=GridBoard.Instance;
        Check(fx!=null&&fx.settings!=null&&fx.boardFrame!=null,"Scene references");
        Check(fx.Level==0&&fx.Intensity==0&&fx.ParticleCount==0,"Initial state");
        foreach(var r in fx.GetComponentsInChildren<SpriteRenderer>())Check(!r.enabled,"Normal state has visible glow");
        int children=fx.transform.childCount;int score=gm.CurrentScore;
        float quality=QualitySettings.GetQualityLevel()==0?fx.settings.lowQualityMultiplier:1;
        for(int c=1;c<=9;c++)
        {
            Score(gm,1);int combo=Mathf.Min(c,8);score+=1+10*combo;
            Check(gm.CurrentCombo==combo&&gm.CurrentScore==score,"Score/combo changed");
            Check(fx.Level==(combo<3?1:combo<6?2:3),"Level mapping");yield return .8f;
            Check(Mathf.Abs(fx.Intensity-fx.settings.At(fx.settings.crystalEmissionIntensity,fx.Level)*quality)<.01f,"Intensity target");
        }
        Check(fx.ParticleCount>0&&fx.ParticleCount<=fx.settings.maximumParticleCount,"Particle cap");
        Check(fx.transform.childCount==children,"Objects growing");
        ScreenCapture.CaptureScreenshot("Captures/combo-environment-high.png");
        Score(gm,0);Score(gm,0);Check(gm.CurrentCombo==8&&fx.Level==3,"Combo lost before 3 turns");
        float before=fx.Intensity;Score(gm,0);Check(gm.CurrentCombo==0&&fx.Level==0&&Mathf.Approximately(before,fx.Intensity),"Reset not smooth");
        yield return .3f;Check(fx.Intensity>0&&fx.Intensity<before,"No fade transition");
        yield return 2f;Check(fx.Intensity<.001f&&fx.ParticleCount==0,"Reset particles linger");
        foreach(var r in fx.GetComponentsInChildren<SpriteRenderer>())Check(!r.enabled,"Glow remains after reset");
        ScreenCapture.CaptureScreenshot("Captures/combo-environment-normal.png");
        Score(gm,1);yield return .8f;gm.ConfirmGameOver();yield return 1.1f;Check(fx.Level==0&&fx.Intensity<.001f,"Game Over fade");
        var over=UnityEngine.Object.FindFirstObjectByType<GameOverController>();Check(over.CanRequestRevive,"Continue unavailable");over.Revive();
        Check(!gm.IsGameOver&&gm.CurrentCombo==0&&fx.Level==0&&fx.ParticleCount==0,"Continue reset");
        Score(gm,1);yield return .8f;fx.enabled=false;Check(fx.ParticleCount==0&&fx.Intensity==0,"Disable cleanup");fx.enabled=true;yield return .8f;Check(fx.Level==1,"Re-enable sync");
        int oldQuality=QualitySettings.GetQualityLevel();
        try {QualitySettings.SetQualityLevel(0,false);Score(gm,1);yield return .8f;Check(Mathf.Abs(fx.Intensity-fx.settings.crystalEmissionIntensity[1]*fx.settings.lowQualityMultiplier)<.01f,"Low quality");}
        finally {QualitySettings.SetQualityLevel(oldQuality,false);}
        board.ResetBoard();gm.ResetRound();gm.UnlockNewRound();Check(fx.Level==0&&fx.Intensity==0&&fx.ParticleCount==0&&gm.CurrentScore==0,"Replay reset");
        SceneManager.LoadScene("MainMenu");yield return .7f;Check(UnityEngine.Object.FindObjectsByType<ComboEnvironment>(FindObjectsSortMode.None).Length==0,"Scene leaked effect");
    }
}

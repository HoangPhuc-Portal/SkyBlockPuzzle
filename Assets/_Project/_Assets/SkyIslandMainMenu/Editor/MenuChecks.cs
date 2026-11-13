using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using BlockBlast.Menu;
using BlockBlast.UI;

public static class MenuChecks
{
    static IEnumerator routine; static double next, deadline;
    static readonly StringBuilder log=new StringBuilder();
    static bool background;static float music,sfx;static int haptic;static bool hm,hs,hh;
    const BindingFlags Flags=BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic;
    static void Check(bool pass,string message){if(!pass)throw new Exception(message);log.AppendLine("PASS "+message);}
    public static string Validate()
    {
        var p=UnityEngine.Object.FindFirstObjectByType<MenuPresentation>();
        if(p==null)throw new Exception("Menu presentation missing");
        int missing=0;
        foreach(var root in p.gameObject.scene.GetRootGameObjects())foreach(var t in root.GetComponentsInChildren<Transform>(true))missing+=GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject);
        Check(missing==0,"No missing scripts");
        foreach(var i in p.GetComponentsInChildren<Image>(true))if(i.material!=null)Check(!ShaderUtil.ShaderHasError(i.material.shader),"Shader "+i.material.shader.name);
        Check(p.clouds.Length==4&&p.motes.Length==18&&p.leaves.Length==3&&p.mist.Length==4,"Fixed VFX pool 32 objects");
        Check(p.explorerAnimator.runtimeAnimatorController!=null&&p.dragonIdle!=null,"Character animation references");
        var settings=UnityEngine.Object.FindFirstObjectByType<MenuSettings>();
        Check(settings!=null&&settings.popup&&settings.close&&settings.music&&settings.sfx&&settings.haptic,"Settings references assigned");
        Check(p.playArt.GetComponent<Button>().onClick.GetPersistentEventCount()==1,"Single persistent Play handler");
        return "MainMenu reference validation PASS";
    }
    public static void SetView(int width,int height)
    {
        foreach(var w in Resources.FindObjectsOfTypeAll<EditorWindow>())if(w.GetType().FullName.Contains("SimulatorWindow"))w.Close();
        var assembly=typeof(Editor).Assembly;var sizesType=assembly.GetType("UnityEditor.GameViewSizes");
        var singleton=typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
        var sizes=singleton.GetProperty("instance",BindingFlags.Public|BindingFlags.Static|BindingFlags.FlattenHierarchy).GetValue(null);
        var groupType=assembly.GetType("UnityEditor.GameViewSizeGroupType");
        var group=sizesType.GetMethod("GetGroup").Invoke(sizes,new[]{Enum.Parse(groupType,"Android")});
        var sizeType=assembly.GetType("UnityEditor.GameViewSize");var kind=assembly.GetType("UnityEditor.GameViewSizeType");
        var size=Activator.CreateInstance(sizeType,BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic,null,new object[]{Enum.Parse(kind,"FixedResolution"),width,height,"Menu QA "+width+"x"+height},null);
        group.GetType().GetMethod("AddCustomSize").Invoke(group,new[]{size});
        int count=(int)group.GetType().GetMethod("GetTotalCount").Invoke(group,null);
        var view=EditorWindow.GetWindow(assembly.GetType("UnityEditor.GameView"));view.GetType().GetProperty("selectedSizeIndex",Flags).SetValue(view,count-1);
        view.Show();view.Focus();
    }
    [MenuItem("Block Blast/Main Menu/Run Play QA (5 minutes)")]
    public static void Run()
    {
        if(!EditorApplication.isPlaying||routine!=null)throw new Exception("Enter Play Mode on MainMenu first");
        log.Clear();log.AppendLine("MainMenu QA "+DateTime.UtcNow.ToString("o"));
        background=Application.runInBackground;Application.runInBackground=true;
        hm=PlayerPrefs.HasKey("UI.Music");hs=PlayerPrefs.HasKey("UI.SFX");hh=PlayerPrefs.HasKey("UI.Haptic");
        music=PlayerPrefs.GetFloat("UI.Music",.7f);sfx=PlayerPrefs.GetFloat("UI.SFX",1);haptic=PlayerPrefs.GetInt("UI.Haptic",1);
        routine=Exercise();next=0;deadline=EditorApplication.timeSinceStartup+480;EditorApplication.update+=Tick;
    }
    static void Tick()
    {
        try {if(!EditorApplication.isPlaying||EditorApplication.timeSinceStartup>deadline)throw new Exception("Play stopped or timeout");
            if(EditorApplication.timeSinceStartup<next)return;
            if(!routine.MoveNext()){Finish(null);return;}next=EditorApplication.timeSinceStartup+(routine.Current is float?(float)routine.Current:0);
        }catch(Exception e){Finish(e.ToString());}
    }
    static void Finish(string error)
    {
        EditorApplication.update-=Tick;routine=null;Application.runInBackground=background;
        if(hm)PlayerPrefs.SetFloat("UI.Music",music);else PlayerPrefs.DeleteKey("UI.Music");
        if(hs)PlayerPrefs.SetFloat("UI.SFX",sfx);else PlayerPrefs.DeleteKey("UI.SFX");
        if(hh)PlayerPrefs.SetInt("UI.Haptic",haptic);else PlayerPrefs.DeleteKey("UI.Haptic");
        BlockBlast.Managers.AudioManager.Instance?.ReloadPreferences();PlayerPrefs.Save();
        log.AppendLine(error==null?"ALL CHECKS PASSED":error);Directory.CreateDirectory("Captures");File.WriteAllText("Captures/mainmenu-qa.txt",log.ToString());
        if(error!=null)Debug.LogError(error);else Debug.Log("MainMenu QA PASS");
    }
    static IEnumerator Exercise()
    {
        Validate();
        foreach(int height in new[]{960,1170,1200})
        {
            SetView(540,height);yield return 1f;
            var p=UnityEngine.Object.FindFirstObjectByType<MenuPresentation>();
            Check(Screen.width==540&&Screen.height==height,"Game View resolution "+height);
            foreach(var r in new[]{p.playArt,p.dragon,p.explorer,p.settings.GetComponent<RectTransform>(),GameObject.Find("Logo_img").GetComponent<RectTransform>()})
            {
                var corners=new Vector3[4];r.GetWorldCorners(corners);
                Check(corners.All(c=>c.x>=-1&&c.x<=Screen.width+1&&c.y>=-1&&c.y<=Screen.height+1),"Visible "+r.name+" at "+height);
            }
            ScreenCapture.CaptureScreenshot("Captures/mainmenu-540x"+height+".png");yield return .3f;
        }
        var settings=UnityEngine.Object.FindFirstObjectByType<MenuSettings>();var original=settings.open.transform.localScale;
        for(int i=0;i<4;i++){settings.open.onClick.Invoke();yield return .2f;Check(settings.IsOpen&&!settings.play.interactable,"Settings opens and blocks Play");settings.close.onClick.Invoke();yield return .2f;Check(!settings.IsOpen&&Vector3.Distance(settings.open.transform.localScale,original)<.001f,"Settings closes and restores scale");}
        settings.Open();settings.music.value=.31f;settings.sfx.value=.42f;settings.haptic.isOn=false;
        Check(Mathf.Abs(PlayerPrefs.GetFloat("UI.Music")-.31f)<.001f&&Mathf.Abs(BlockBlast.Managers.AudioManager.Instance.MusicVolume-.31f)<.001f,"Music saved/applied immediately");
        Check(Mathf.Abs(PlayerPrefs.GetFloat("UI.SFX")-.42f)<.001f&&PlayerPrefs.GetInt("UI.Haptic")==0,"SFX and haptic saved");
        ScreenCapture.CaptureScreenshot("Captures/mainmenu-settings.png");yield return .3f;settings.Close();
        for(int cycle=0;cycle<3;cycle++)
        {
            var p=UnityEngine.Object.FindFirstObjectByType<MenuPresentation>();var loader=p.playArt.GetComponent<PlaySceneLoader>();
            loader.Play();loader.Play();loader.Play();yield return .45f;
            Check(SceneManager.GetActiveScene().name=="MainMenu"&&p.IsReacting,"Play reaction precedes scene load");
            yield return .5f;ScreenCapture.CaptureScreenshot("Captures/mainmenu-departure.png");
            double limit=EditorApplication.timeSinceStartup+30;while(SceneManager.GetActiveScene().name!="GamePlay"&&EditorApplication.timeSinceStartup<limit)yield return .2f;
            Check(SceneManager.GetActiveScene().name=="GamePlay","Play loads Gameplay exactly once cycle "+cycle);
            Check(BlockBlast.Shape.TrayManager.Instance.Pieces.Count(x=>x!=null)==3,"Gameplay initializes 3 pieces");
            SceneManager.LoadScene("MainMenu");yield return 1f;
            Check(UnityEngine.Object.FindObjectsByType<MenuPresentation>(FindObjectsSortMode.None).Length==1,"Return has exactly one menu controller");
        }
        var menu=UnityEngine.Object.FindFirstObjectByType<MenuPresentation>();int count=menu.GetComponentsInChildren<Transform>(true).Length;
        long memory=UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong();float start=menu.AnimationTime;
        for(int i=0;i<10;i++)
        {
            yield return 30f;
            Check(menu.GetComponentsInChildren<Transform>(true).Length==count,"Pool stable at "+((i+1)*30)+" seconds");
            log.AppendLine("sample: allocated="+UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong()+" batches="+UnityStats.batches+" drawCalls="+UnityStats.drawCalls);
            File.WriteAllText("Captures/mainmenu-qa-progress.txt",log.ToString());
        }
        Check(menu.AnimationTime>start+100,"Ambient animation advances during soak");
        log.AppendLine("5 minute soak memory delta="+(UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong()-memory)+" bytes (whole Editor/player, not device performance)");
        ScreenCapture.CaptureScreenshot("Captures/mainmenu-final.png");yield return .3f;
    }
}

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using BlockBlast.Managers;
using BlockBlast.UI;
using BlockBlast.Grid;
using BlockBlast.Shape;
using BlockBlast.Data;

public static class AudioIntegrationChecks
{
    static int stage, identity, record; static double deadline; static bool hadRecord;
    static readonly string[] keys={"UI.Music","UI.SFX","UI.Music.Enabled","UI.SFX.Enabled"};
    static bool[] exists;static float[] values; static AudioManager audio;
    static ShapeData shape;
    static readonly BindingFlags F=BindingFlags.NonPublic|BindingFlags.Instance;
    static T Get<T>(object o,string n)=>(T)o.GetType().GetField(n,F).GetValue(o);
    static void Check(bool ok,string reason){if(!ok)throw new Exception(reason);}
    static void Count(int before,int delta,string why)=>Check(audio.PlayedSfxCount==before+delta,why+" count="+(audio.PlayedSfxCount-before));
    [MenuItem("Block Blast/Audio/Run Play checks")]
    static void Run()
    {
        if(!EditorApplication.isPlaying)throw new Exception("Start Play Mode first");
        EditorApplication.update-=Tick;
        exists=keys.Select(PlayerPrefs.HasKey).ToArray();values=keys.Select((k,i)=>i<2?PlayerPrefs.GetFloat(k):PlayerPrefs.GetInt(k)).ToArray();
        hadRecord=PlayerPrefs.HasKey("BlockBlast_HighScore");record=PlayerPrefs.GetInt("BlockBlast_HighScore");
        stage=0;deadline=EditorApplication.timeSinceStartup+60;
        if(SceneManager.GetActiveScene().name!="MainMenu")SceneManager.LoadScene("MainMenu");
        EditorApplication.update+=Tick;
    }
    static void Tick()
    {
        try
        {
            Check(EditorApplication.isPlaying&&EditorApplication.timeSinceStartup<deadline,"Timeout or stopped");
            if(stage==0)
            {
                audio=AudioManager.Instance;if(audio==null)return;identity=audio.GetInstanceID();
                audio.SetMusicEnabled(true);audio.SetSfxEnabled(true);audio.ApplyVolumes(.4f,1);
                foreach(var field in new[]{"buttonClip","dragClip","blockPlaceClip","lineClearClip","gameOverClip"})
                    Check(Get<AudioClip>(audio,field)!=null,"Unassigned "+field);
                int n=audio.PlayedSfxCount;
                var loader=UnityEngine.Object.FindFirstObjectByType<PlaySceneLoader>();if(loader==null)return;
                Get<Button>(loader,"playButton").onClick.Invoke();Count(n,1,"MainMenu Play click");Check(Get<AudioSource>(audio,"sfxSource").isPlaying,"AudioSource did not start playback");stage=1;return;
            }
            if(stage==1)
            {
                if(SceneManager.GetActiveScene().name!="GamePlay"||GameManager.Instance==null||TrayManager.Instance==null||TrayManager.Instance.Pieces.Count(p=>p!=null)!=3)return;
                Check(AudioManager.Instance.GetInstanceID()==identity,"Music service replaced on scene load");
                Check(UnityEngine.Object.FindObjectsByType<AudioManager>(FindObjectsSortMode.None).Length==1,"Duplicate manager");
                Check(audio.GetComponentsInChildren<AudioSource>().Length==2,"Expected two fixed sources");
                var settings=UnityEngine.Object.FindFirstObjectByType<SettingsPanel>();
                int n=audio.PlayedSfxCount;Get<Button>(settings,"open").onClick.Invoke();Count(n,1,"Settings click");
                Get<Slider>(settings,"sfx").value=0;
                Check(Get<AudioSource>(audio,"sfxSource").mute&&!Get<AudioSource>(audio,"sfxSource").isPlaying,"SFX zero did not stop tails");
                n=audio.PlayedSfxCount;audio.PlayButtonSound();Count(n,0,"Muted click");
                Get<Slider>(settings,"sfx").value=.6f;Get<Slider>(settings,"music").value=.3f;
                audio.ReloadPreferences();Check(Mathf.Approximately(audio.SfxVolume,.6f)&&Mathf.Approximately(audio.MusicVolume,.3f),"Volume persistence");
                audio.SetSfxEnabled(false);audio.ReloadPreferences();n=audio.PlayedSfxCount;audio.PlayDragSound();Count(n,0,"Disabled SFX");audio.SetSfxEnabled(true);
                audio.SetMusicEnabled(false);audio.ReloadPreferences();Check(!audio.MusicEnabled&&Get<AudioSource>(audio,"bgmSource").mute,"Music disabled preference");
                n=audio.PlayedSfxCount;Get<Button>(settings,"close").onClick.Invoke();Count(n,1,"Close click");
                var button=Get<Button>(settings,"open");button.interactable=false;n=audio.PlayedSfxCount;
                ExecuteEvents.Execute(button.gameObject,new PointerEventData(EventSystem.current){button=PointerEventData.InputButton.Left},ExecuteEvents.pointerClickHandler);
                Count(n,0,"Disabled button");button.interactable=true;
                var piece=TrayManager.Instance.Pieces[0];var pointer=new PointerEventData(EventSystem.current){position=new Vector2(Screen.width/2,Screen.height/2)};
                n=audio.PlayedSfxCount;piece.OnBeginDrag(pointer);Count(n,1,"Drag start");piece.OnDrag(pointer);piece.OnDrag(pointer);Count(n,1,"Drag movement repeated sound");piece.OnCancel(null);Count(n,1,"Cancel played place sound");
                var board=GridBoard.Instance;board.ResetBoard();shape=ScriptableObject.CreateInstance<ShapeData>();shape.rows=shape.columns=1;typeof(ShapeData).GetField("matrixData",F).SetValue(shape,new[]{true});var runtime=shape.CreateRuntime();
                n=audio.PlayedSfxCount;Check(board.TryPlaceShape(runtime,board.CellPosition(0,0)),"Placement failed");Count(n,1,"Valid place");
                n=audio.PlayedSfxCount;Check(!board.TryPlaceShape(runtime,board.CellPosition(0,0)),"Invalid placement accepted");Count(n,0,"Invalid played sound");
                board.ResetBoard();var cells=Get<GridCell[,]>(board,"grid");int size=cells.GetLength(0);
                for(int i=1;i<size;i++){cells[0,i].SetOccupied(true);cells[i,0].SetOccupied(true);}board.SyncModel();
                n=audio.PlayedSfxCount;Check(board.TryPlaceShape(runtime,board.CellPosition(0,0)),"Multi clear placement failed");Count(n,2,"Place plus ONE clear for row+column");
                Check(GameManager.Instance.LastPerfectClear,"Perfect clear setup");
                n=audio.PlayedSfxCount;GameManager.Instance.ConfirmGameOver();GameManager.Instance.ConfirmGameOver();Count(n,1,"Duplicate Game Over");
                PlayerPrefs.Save();SceneManager.LoadScene("MainMenu");stage=2;return;
            }
            if(SceneManager.GetActiveScene().name!="MainMenu")return;
            Check(AudioManager.Instance.GetInstanceID()==identity,"Manager replaced returning Home");
            Check(UnityEngine.Object.FindObjectsByType<AudioManager>(FindObjectsSortMode.None).Length==1,"Manager duplicated Home");
            Check(!audio.MusicEnabled&&Mathf.Approximately(audio.SfxVolume,.6f),"Preferences lost on transition");
            // Recreate the service to exercise Awake, rather than only reloading preferences in-place.
            audio.SetSfxEnabled(false);PlayerPrefs.SetFloat("UI.Music",0);PlayerPrefs.SetFloat("UI.SFX",0);PlayerPrefs.Save();
            UnityEngine.Object.DestroyImmediate(audio.gameObject);
            var prefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Project/Prefabs/Audio/AudioManager.prefab");
            var fresh=UnityEngine.Object.Instantiate(prefab);audio=fresh.GetComponent<AudioManager>();
            Check(AudioManager.Instance==audio&&!audio.SfxEnabled&&!audio.MusicEnabled,"Startup enabled preferences");
            Check(Get<AudioSource>(audio,"sfxSource").mute&&Get<AudioSource>(audio,"bgmSource").mute&&audio.PlayedSfxCount==0,"Startup audible before preferences");
            Finish("PASS: actual AudioSource playback; muted persisted settings applied in fresh Awake; assigned 5 clips; Play/Settings/Close single click; disabled/muted silent; drag once, no move/cancel/place overlap; valid placement once, invalid silent; two-line perfect clear emits one clear; GameOver once; singleton and 2 sources across MainMenu/GamePlay/MainMenu; sliders and enabled preferences reload correctly.");
        }
        catch(Exception e){Finish("FAIL: "+e);}
    }
    static void Finish(string result)
    {
        EditorApplication.update-=Tick;
        for(int i=0;i<keys.Length;i++){if(!exists[i])PlayerPrefs.DeleteKey(keys[i]);else if(i<2)PlayerPrefs.SetFloat(keys[i],values[i]);else PlayerPrefs.SetInt(keys[i],(int)values[i]);}
        if(hadRecord)PlayerPrefs.SetInt("BlockBlast_HighScore",record);else PlayerPrefs.DeleteKey("BlockBlast_HighScore");PlayerPrefs.Save();
        if(audio!=null)audio.ReloadPreferences();if(shape!=null)UnityEngine.Object.DestroyImmediate(shape);
        Directory.CreateDirectory("Captures");File.WriteAllText("Captures/audio-play-checks.txt",result);
        if(result.StartsWith("PASS"))Debug.Log(result);else Debug.LogError(result);
    }
}

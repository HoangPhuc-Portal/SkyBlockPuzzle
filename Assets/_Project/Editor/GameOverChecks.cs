
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using BlockBlast.Core;
using BlockBlast.Data;
using BlockBlast.Grid;
using BlockBlast.Managers;
using BlockBlast.Shape;
using BlockBlast.UI;

public static class GameOverChecks
{
    private const BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
    private const string Key = "BlockBlast_HighScore";
    private static object Get(object target,string name) => target.GetType().GetField(name,Private).GetValue(target);
    private static void Set(object target,string name,object value) => target.GetType().GetField(name,Private).SetValue(target,value);
    private static void Check(bool condition,string message) { if(!condition)throw new Exception(message); }
    private static IEnumerator runner;
    private static bool hadPref;
    private static int savedPref;
    private static float savedScale;
    private static ShapeData small,large;
    private static int events;
    private static Action ended;
    public static string LastResult { get; private set; }
    private static bool waitingForPlay;
    private static float stamp;
    [InitializeOnLoadMethod]
    private static void ResumeAfterReload()
    {
        if (!SessionState.GetBool("GameOverChecks.Pending", false)) return;
        hadPref=PlayerPrefs.HasKey(Key);savedPref=PlayerPrefs.GetInt(Key);savedScale=Time.timeScale;
        waitingForPlay=true;stamp=(float)EditorApplication.timeSinceStartup;
        EditorApplication.update+=Tick;
    }
    public static string RunScoreAndLock()
    {
        Check(!Application.isPlaying,"Edit Mode required");
        bool exists=PlayerPrefs.HasKey(Key);int previous=PlayerPrefs.GetInt(Key);
        var go=new GameObject("Isolated Game Over score check");go.SetActive(false);
        var model=go.AddComponent<GameManager>();
        int count=0;Action listener=()=>count++;
        GameEvents.OnGameOver+=listener;
        try
        {
            Set(model,"<HighScore>k__BackingField",100);
            Set(model,"<RoundStartHighScore>k__BackingField",100);
            var score=typeof(GameManager).GetMethod("ScorePlacement",Private);
            score.Invoke(model,new object[]{100,0,false});
            Check(model.CurrentScore==100 && !(model.CurrentScore>model.RoundStartHighScore),"Tie counted as a record");
            score.Invoke(model,new object[]{100,0,false});
            Check(model.CurrentScore==200 && model.HighScore==200 && PlayerPrefs.GetInt(Key)==200,"High score not persisted");
            Check(model.ConfirmGameOver()&&!model.ConfirmGameOver()&&count==1,"Game Over not idempotent");
            score.Invoke(model,new object[]{100,0,false});
            Check(model.CurrentScore==200,"Score changed while locked");
            model.ResetRound();
            Check(model.CurrentScore==0&&model.HighScore==200&&model.RoundStartHighScore==200&&model.IsInputLocked&&!model.IsGameOver,"Reset score/baseline/lock failed");
            model.UnlockNewRound();
            Check(!model.IsInputLocked,"New round did not unlock");
            return "PASS: strict record, persistence, one event, score lock, reset baseline";
        }
        finally
        {
            GameEvents.OnGameOver-=listener;
            UnityEngine.Object.DestroyImmediate(go);
            if(exists)PlayerPrefs.SetInt(Key,previous);else PlayerPrefs.DeleteKey(Key);
            PlayerPrefs.Save();
        }
    }

    [MenuItem("Block Blast/Run Game Over Play Checks")]
    public static void StartPlayChecks()
    {
        if(runner!=null||waitingForPlay)throw new Exception("Checks already running");
        hadPref=PlayerPrefs.HasKey(Key);savedPref=PlayerPrefs.GetInt(Key);savedScale=Time.timeScale;
        LastResult="RUNNING";stamp=(float)EditorApplication.timeSinceStartup;
        waitingForPlay=!EditorApplication.isPlaying;
        EditorApplication.update+=Tick;
        if(waitingForPlay){SessionState.SetBool("GameOverChecks.Pending",true);EditorApplication.isPlaying=true;}
        else Begin();
    }
    private static void Begin()
    {
        SessionState.SetBool("GameOverChecks.Pending",false);
        waitingForPlay=false;
        events=0;ended=()=>events++;GameEvents.OnGameOver+=ended;
        runner=Play();
        EditorApplication.update-=Tick;
        GameManager.Instance.StartCoroutine(RuntimeRunner());
    }
    private static IEnumerator RuntimeRunner()
    {
        while(true)
        {
            bool moved;
            try { moved=runner.MoveNext(); }
            catch(Exception error) { Finish(error);yield break; }
            if(!moved){Finish(null);yield break;}
            yield return null;
        }
    }
    private static void Tick()
    {
        try
        {
            if(waitingForPlay)
            {
                if(EditorApplication.isPlaying&&!EditorApplication.isCompiling&&TrayManager.Instance!=null&&TrayManager.Instance.Pieces.All(p=>p!=null))Begin();
                else if(EditorApplication.timeSinceStartup-stamp>60)throw new Exception("Play Mode startup timeout");
                return;
            }
            if(!EditorApplication.isPlaying)throw new Exception("Play Mode interrupted");
            if(runner!=null&&!runner.MoveNext())Finish(null);
        }
        catch(Exception error){Finish(error);}
    }
    private static ShapeData Shape(int n)
    {
        var data=ScriptableObject.CreateInstance<ShapeData>();data.rows=data.columns=n;
        Set(data,"matrixData",Enumerable.Repeat(true,n*n).ToArray());data.blockColor=Color.white;
        return data;
    }
    private static IEnumerator Wait(float seconds)
    {
        float end=Time.realtimeSinceStartup+seconds;
        while(Time.realtimeSinceStartup<end)yield return null;
    }
    private static IEnumerator Play()
    {
        // Let Editor startup, tool execution and the first Game View render settle.
        var warmup=Wait(5);while(warmup.MoveNext())yield return null;
        var board=GridBoard.Instance;var tray=TrayManager.Instance;var score=GameManager.Instance;
        var flow=UnityEngine.Object.FindFirstObjectByType<GameOverController>();
        Check(flow!=null,"Missing scene flow");
        var cells=(GridCell[,])Get(board,"grid");
        var group=(CanvasGroup)Get(flow,"group");
        var crown=(GameObject)Get(flow,"crown");
        var glow=(Image)Get(flow,"recordGlow");
        var replay=(Button)Get(flow,"replayButton");
        var view=(Button)Get(flow,"viewBoardButton");
        var hud=(GameHUD)Get(flow,"hud");
        small=Shape(1);large=Shape(2);
        Set(score,"<HighScore>k__BackingField",100);
        Set(score,"<RoundStartHighScore>k__BackingField",100);
        typeof(GameManager).GetMethod("ScorePlacement",Private).Invoke(score,new object[]{200,0,false});
        Check(score.CurrentScore==200,"Score setup failed");
        foreach(var cell in cells)cell.SetOccupied(true,Color.white);
        cells[0,0].SetOccupied(false);
        foreach(var piece in tray.Pieces)piece.Setup(large,piece.BlockSprite);
        tray.Pieces[2].Setup(small,tray.Pieces[2].BlockSprite);
        tray.CheckGameOverCondition();
        Check(!score.IsGameOver&&events==0,"Only the first pieces were examined");
        tray.Pieces[2].Setup(large,tray.Pieces[2].BlockSprite);
        var originalIds=tray.Pieces.Select(p=>p.GetInstanceID()).ToArray();
        float started=Time.realtimeSinceStartup;
        tray.CheckGameOverCondition();tray.CheckGameOverCondition();
        Check(events==1&&score.IsInputLocked&&score.IsGameOver&&!flow.IsPopupVisible,"Confirmation/duplicate/input lock failed");
        Check(!board.TryPlaceShape(small,board.CellPosition(0,0))&&!cells[0,0].IsOccupied,"Placement accepted after Game Over");
        // Attempt actual input handler while locked, not just a model query.
        var pieceLocked=tray.Pieces[0];
        pieceLocked.OnBeginDrag(new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current){position=Vector2.zero});
        Check(!(bool)Get(pieceLocked,"isDragging"),"BeginDrag accepted after Game Over");
        Time.timeScale=0;
        while(!flow.IsPopupVisible&&Time.realtimeSinceStartup-started<2)yield return null;
        float delay=Time.realtimeSinceStartup-started;
        Check(delay>=.49f&&delay<1.5f,"Realtime popup delay out of bounds: "+delay);
        Check(group.blocksRaycasts&&!group.interactable,"Backdrop/open interaction wrong");
        float opening=Time.realtimeSinceStartup;
        while(!group.interactable&&Time.realtimeSinceStartup-opening<3)yield return null;
        Check(group.interactable&&flow.FinalScore==200&&flow.IsNewRecord&&crown.activeSelf,"Record popup incorrect");
        Check(glow.color.a>0,"Gold flash absent");
        ScreenCapture.CaptureScreenshot("Captures/gameover-record.png");
        var wait=Wait(.5f);while(wait.MoveNext())yield return null;
        view.onClick.Invoke();view.onClick.Invoke();
        float viewClosed=Time.realtimeSinceStartup;
        while(flow.IsPopupVisible&&Time.realtimeSinceStartup-viewClosed<2)yield return null;
        Check(Time.realtimeSinceStartup-viewClosed<.5f,"View close duration wrong");
        Check(!flow.IsPopupVisible&&!group.blocksRaycasts&&score.IsInputLocked&&score.CurrentScore==200,"View-board changed session state");
        Check(cells.Cast<GridCell>().Count(c=>c.IsOccupied)==GridBoard.GRID_SIZE*GridBoard.GRID_SIZE-1,"View-board changed occupancy");
        Check(tray.Pieces.Select(p=>p.GetInstanceID()).SequenceEqual(originalIds),"View-board regenerated tray");
        wait=Wait(.7f);while(wait.MoveNext())yield return null;
        Check(!flow.IsPopupVisible&&events==1,"Popup reopened");
        // Arrange another round through the exact production reset, without reloading the scene.
        typeof(GameOverController).GetMethod("ResetRound",Private).Invoke(flow,null);
        yield return null;
        Check(score.CurrentScore==0&&score.HighScore==200&&tray.Pieces.Count(p=>p!=null)==3,"First reset failed");
        Check(cells.Cast<GridCell>().All(c=>!c.IsOccupied),"Reset left occupied cells");
        Check(((TMPro.TextMeshProUGUI)Get(hud,"highScore")).text.Contains("<color=#FFFFFF>0</color>"),"BEST did not reset immediately");
        Check(((TMPro.TextMeshProUGUI)Get(hud,"score")).text==score.HighScore.ToString(),"HUD lost the saved record after reset");
        // Positive input regression after reset.
        var fresh=tray.Pieces[0];
        fresh.OnBeginDrag(new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current){position=new Vector2(Screen.width*.5f,Screen.height*.5f)});
        Check((bool)Get(fresh,"isDragging"),"Input remained locked");
        fresh.OnCancel(new UnityEngine.EventSystems.BaseEventData(UnityEngine.EventSystems.EventSystem.current));
        Check(board.TryPlaceShape(small,board.CellPosition(0,0)),"Placement after reset failed");
        board.ResetBoard();
        GameEvents.TriggerLinesCleared(1);
        foreach(var cell in cells)cell.SetOccupied(true,Color.white);
        cells[0,0].SetOccupied(false);
        foreach(var piece in tray.Pieces)piece.Setup(large,piece.BlockSprite);
        tray.CheckGameOverCondition();
        opening=Time.realtimeSinceStartup;
        while(!group.interactable&&Time.realtimeSinceStartup-opening<3)yield return null;
        Check(events==2&&!flow.IsNewRecord&&!crown.activeSelf&&glow.color.a==0,"Lower score showed record effect");
        ScreenCapture.CaptureScreenshot("Captures/gameover-no-record.png");
        wait=Wait(.5f);while(wait.MoveNext())yield return null;
        var secondIds=tray.Pieces.Select(p=>p.GetInstanceID()).ToArray();
        float closed=Time.realtimeSinceStartup;
        replay.onClick.Invoke();replay.onClick.Invoke();
        Check(score.IsInputLocked,"Unlocked before reset");
        while(flow.IsPopupVisible&&Time.realtimeSinceStartup-closed<2)yield return null;
        float closeTime=Time.realtimeSinceStartup-closed;
        Check(closeTime>=.14f&&closeTime<.5f,"Close duration wrong: "+closeTime);
        yield return null;
        Check(!score.IsGameOver&&!score.IsInputLocked&&score.CurrentScore==0&&score.HighScore==200,"Replay central state wrong");
        Check(tray.Pieces.Count(p=>p!=null)==3&&tray.Pieces.All(p=>!secondIds.Contains(p.GetInstanceID())),"Replay tray not exactly three fresh pieces");
        Check(UnityEngine.Object.FindObjectsByType<DraggableShape>(FindObjectsSortMode.None).Length==3,"Orphan or duplicate piece");
        Check(cells.Cast<GridCell>().All(c=>!c.IsOccupied&& !((GameObject)Get(c,"fillVisual")).activeSelf && !((GameObject)Get(c,"highlightGlowVisual")).activeSelf),"Replay left visuals/highlights");
        Check(board.GetComponentsInChildren<SpriteRenderer>().Where(r=>r.name=="Ghost").All(r=>!r.enabled),"Replay left ghost");
        Check(tray.Pieces.All(p=>!(bool)Get(p,"isDragging")&&!(bool)Get(p,"returning")),"Replay left input state");
        Check(PlayerPrefs.GetInt(Key)==200,"High score was reset");
        wait=Wait(.8f);while(wait.MoveNext())yield return null;
        Check(cells.Cast<GridCell>().All(c=>!c.IsOccupied)&&!flow.IsPopupVisible&&score.CurrentScore==0,"Old callbacks affected new round");
        LastResult="PASS Play Mode: all remaining pieces; single event; realtime delay "+delay.ToString("F3")+"s at timeScale=0; placement/drag locked; record crown/gold; view-only unchanged; popup close "+closeTime.ToString("F3")+"s; two in-place resets; three fresh pieces; no stale preview/drag/callback; high score retained.";
    }
    private static void Finish(Exception error)
    {
        EditorApplication.update-=Tick;
        SessionState.SetBool("GameOverChecks.Pending",false);
        if(ended!=null)GameEvents.OnGameOver-=ended;
        ended=null;runner=null;waitingForPlay=false;
        Time.timeScale=savedScale;
        if(small!=null)UnityEngine.Object.DestroyImmediate(small);
        if(large!=null)UnityEngine.Object.DestroyImmediate(large);
        if(hadPref)PlayerPrefs.SetInt(Key,savedPref);else PlayerPrefs.DeleteKey(Key);
        PlayerPrefs.Save();
        if(error!=null)LastResult="FAIL: "+error;
        System.IO.File.WriteAllText("Captures/gameover-play-checks.txt",LastResult);
        if(error!=null)Debug.LogError(LastResult);else Debug.Log(LastResult);
        EditorApplication.isPlaying=false;
    }
}


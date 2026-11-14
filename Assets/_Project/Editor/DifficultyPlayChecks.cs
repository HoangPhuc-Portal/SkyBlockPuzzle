using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using BlockBlast.Difficulty;
using BlockBlast.Grid;
using BlockBlast.Managers;
using BlockBlast.Shape;
using BlockBlast.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public static class DifficultyPlayChecks
{
    static IEnumerator routine;static float next;static double deadline;
    static bool hadRecord,background;static int record,placed;
    static UnityEngine.Random.State random;
    const string Key="BlockBlast_HighScore";
    const BindingFlags Flags=BindingFlags.Instance|BindingFlags.NonPublic;
    static void Check(bool value,string why){if(!value)throw new Exception(why);}
    static float Offset(DraggableShape p)=>(float)typeof(DraggableShape).GetField("dragOffsetYScreen",Flags).GetValue(p);
    [MenuItem("Block Blast/Tests/DDA Play Checks")]
    public static void Run()
    {
        if(!EditorApplication.isPlaying||routine!=null)return;
        hadRecord=PlayerPrefs.HasKey(Key);record=PlayerPrefs.GetInt(Key);PlayerPrefs.SetInt(Key,100);
        background=Application.runInBackground;Application.runInBackground=true;random=UnityEngine.Random.state;
        EditorApplication.isPaused=false;placed=0;routine=Exercise();deadline=EditorApplication.timeSinceStartup+120;next=0;EditorApplication.update+=Tick;
    }
    static void Tick()
    {
        try{Check(EditorApplication.isPlaying&&EditorApplication.timeSinceStartup<deadline,"Timeout/Play stopped");if(Time.unscaledTime<next)return;
            if(!routine.MoveNext()){Finish(null);return;}next=Time.unscaledTime+(routine.Current is float?(float)routine.Current:0);
        }catch(Exception error){Finish(error.ToString());}
    }
    static IEnumerator Exercise()
    {
        SceneManager.LoadScene("GamePlay");yield return 1f;
        var tray=TrayManager.Instance;var gm=GameManager.Instance;var board=GridBoard.Instance;
        Check(tray.LastSelection!=null&&tray.DifficultyState.BestScoreAtRunStart==100,"DDA not initialized from snapshot");
        for(int h=0;h<12;h++)
        {
            var selection=tray.LastSelection;Check(selection.complete,"Default catalog failed to find complete hand");
            DifficultyChecks.VerifyProof(board.Model,selection);
            for(int i=0;i<3;i++)Check(object.ReferenceEquals(tray.Pieces[i].ShapeData,selection.hand[i]),"Spawn rerolled chosen rotation");
            var expected=board.Model.Clone();
            foreach(var step in selection.path)
            {
                var piece=tray.Pieces[step.piece];var shape=piece.ShapeData;
                var matrix=new bool[shape.rows,shape.columns];foreach(var c in shape.Cells)matrix[c.x,c.y]=true;
                Check(expected.PlaceAndClear(matrix,step.row,step.column)>=0,"Invalid proof");
                var center=board.CellPosition(step.row,step.column)+new Vector3(shape.columns-1,shape.rows-1)*board.CellSize*.5f;
                var pointer=new PointerEventData(EventSystem.current){position=(Vector2)Camera.main.WorldToScreenPoint(center)-Vector2.up*Offset(piece)};
                piece.OnBeginDrag(new PointerEventData(EventSystem.current){position=RectTransformUtility.WorldToScreenPoint(null,piece.transform.position)});
                piece.OnDrag(pointer);piece.OnEndDrag(pointer);placed++;
                Check(tray.DifficultyState.Placements==placed,"Placement statistics lost/duplicated");
                for(int r=0;r<board.Model.Size;r++)for(int c=0;c<board.Model.Size;c++)Check(board.Model.Occupied(r,c)==expected.Occupied(r,c),"Simulation differs from real clear");
                Check(tray.DifficultyState.BestScoreAtRunStart==100,"Live high score changed snapshot");yield return .03f;
            }
            Check(tray.Pieces.Count(p=>p!=null)==3&&!gm.IsGameOver,"New hand missing/instant Game Over");
        }
        Check(gm.HighScore>100,"Test never crossed record");
        Check(tray.DifficultyState.RecentCount<=6&&tray.DifficultyState.RecentCount>0,"History not bounded");
        var hand=tray.Pieces.Select(p=>p.ShapeData).ToArray();var selected=tray.LastSelection;
        gm.RewardModalOpen=true;float elapsed=tray.DifficultyState.ActiveSeconds;yield return .3f;
        Check(tray.DifficultyState.ActiveSeconds==elapsed&&tray.LastSelection==selected&&tray.Pieces.Select(p=>p.ShapeData).SequenceEqual(hand),"Modal time or displayed hand changed");gm.RewardModalOpen=false;
        gm.ConfirmGameOver();yield return 1f;var over=UnityEngine.Object.FindFirstObjectByType<GameOverController>();int before=gm.CurrentScore,run=gm.RunSequence;
        over.Revive();Check(gm.CurrentScore==before&&gm.RunSequence==run&&tray.DifficultyState.BestScoreAtRunStart==100&&tray.DifficultyState.RecentCount==0,"Continue snapshot/history wrong");
        gm.ConfirmGameOver();yield return 1f;int high=gm.HighScore;over.Replay();yield return .3f;
        Check(gm.CurrentScore==0&&!gm.IsInputLocked&&gm.RunSequence==run+1&&tray.DifficultyState.BestScoreAtRunStart==high&&tray.DifficultyState.Placements==0&&tray.DifficultyState.RecentCount==0,"Replay retained old DDA");
        Check(tray.DifficultyState.Index==0&&tray.LastSelection.decision.weights.z==0,"Replay not Starter");
        ScreenCapture.CaptureScreenshot("Captures/dda-gameplay.png");yield return .3f;
    }
    static void Finish(string error)
    {
        EditorApplication.update-=Tick;(routine as IDisposable)?.Dispose();routine=null;
        if(hadRecord)PlayerPrefs.SetInt(Key,record);else PlayerPrefs.DeleteKey(Key);PlayerPrefs.Save();
        Application.runInBackground=background;UnityEngine.Random.state=random;
        string result=error==null?"PASS: 12 actual DDA hands / 36 drag placements; selected rotations retained; simulated occupancy and line clears match real board; score exceeds initial record while snapshot stays 100; history capped at 6; modal time excluded and existing hand unchanged; Continue preserves run and record snapshot, Replay resets all run statistics and Starter weights; PlayerPrefs restored.":"FAIL after "+placed+" placements: "+error;
        Directory.CreateDirectory("Captures");File.WriteAllText("Captures/dda-play-checks.txt",result);if(error==null)Debug.Log(result);else Debug.LogError(result);
        if(EditorApplication.isPlaying)SceneManager.LoadScene("MainMenu");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using BlockBlast.Data;
using BlockBlast.Grid;
using BlockBlast.Shape;
using BlockBlast.UI;
using BlockBlast.Managers;

public static class ShapeLibraryPlayChecks
{
    const BindingFlags Flags=BindingFlags.Instance|BindingFlags.NonPublic;
    static IEnumerator routine;
    static double deadline;
    static float next;
    static bool hadRecord,background;
    static int record;
    static UnityEngine.Random.State random;
    static int tested;
    static T Field<T>(object o,string n)=>(T)o.GetType().GetField(n,Flags).GetValue(o);
    static void Check(bool value,string reason){if(!value)throw new Exception(reason);}
    static GridCell[] Cells=>GridBoard.Instance.GetComponentsInChildren<GridCell>();
    static void ForceVariant(DraggableShape piece,ShapeRuntimeData variant)
    {
        typeof(DraggableShape).GetProperty("ShapeData").SetValue(piece,variant);
        typeof(DraggableShape).GetMethod("BuildVisualShape",Flags).Invoke(piece,null);
        piece.RefreshPlacementAvailability();
    }
    static void Fresh()
    {
        TrayManager.Instance.ClearHand();GridBoard.Instance.ResetBoard();GameManager.Instance.ResetRound();GameManager.Instance.UnlockNewRound();TrayManager.Instance.SpawnNewHand();
    }
    static Vector2 DropPoint(DraggableShape p)
    {
        var board=GridBoard.Instance;
        var center=board.CellPosition(0,0)+new Vector3(p.ShapeData.columns-1,p.ShapeData.rows-1)*board.CellSize*.5f;
        return (Vector2)Camera.main.WorldToScreenPoint(center)-Vector2.up*Field<float>(p,"dragOffsetYScreen");
    }
    [MenuItem("Block Blast/Tests/Shape Library Play Checks")]
    public static void Run()
    {
        if(!EditorApplication.isPlaying||routine!=null)return;
        hadRecord=PlayerPrefs.HasKey("BlockBlast_HighScore");record=PlayerPrefs.GetInt("BlockBlast_HighScore");
        random=UnityEngine.Random.state;background=Application.runInBackground;Application.runInBackground=true;EditorApplication.isPaused=false;
        tested=0;routine=Exercise();deadline=EditorApplication.timeSinceStartup+180;next=0;EditorApplication.update+=Tick;
    }
    static void Tick()
    {
        try{
            Check(EditorApplication.isPlaying&&EditorApplication.timeSinceStartup<deadline,"Play stopped/timeout");
            if(Time.unscaledTime<next)return;
            if(!routine.MoveNext()){Finish(null);return;}
            next=Time.unscaledTime+(routine.Current is float?(float)routine.Current:0);
        }catch(Exception error){Finish(error.ToString());}
    }
    static IEnumerator Exercise()
    {
        Check(ShapeLibraryRebuild.Validate().StartsWith("PASS"),"Catalog validation");
        SceneManager.LoadScene("GamePlay");yield return 1f;
        var shapes=ShapeLibraryRebuild.Load();
        Check(Field<List<ShapeData>>(TrayManager.Instance,"availableShapes").SequenceEqual(shapes),"Available Shapes differs from catalog");
        var randomKeys=new HashSet<string>();UnityEngine.Random.InitState(41037);
        foreach(var s in shapes)for(int i=0;i<100;i++){var v=s.CreateRuntime();Check(v.Source==s,"Wrong source");randomKeys.Add(v.Key);}
        Check(randomKeys.Count==41,"Random rotation cannot reach all 41 variants");
        foreach(var shape in shapes)foreach(var variant in shape.Variants())
        {
            Fresh();var p=TrayManager.Instance.Pieces[0];ForceVariant(p,variant);yield return .04f;
            Check(p.transform.childCount==variant.Cells.Count,"Rendered cell count: "+shape.name);
            Check(p.ShapeData==variant&&p.CanPlaceOnBoard,"Runtime shape invalid: "+shape.name);
            if(Math.Max(variant.rows,variant.columns)>3)
            {
                var slot=(RectTransform)p.transform.parent;var corners=new Vector3[4];
                foreach(RectTransform tile in p.transform){tile.GetWorldCorners(corners);foreach(var c in corners){var local=(Vector2)slot.InverseTransformPoint(c);Check(slot.rect.Contains(local),"Long line overflows tray slot: "+variant.Key);}}
            }
            var board=GridBoard.Instance;
            Check(!board.IsValid(variant,board.CellPosition(GridBoard.GRID_SIZE-1,GridBoard.GRID_SIZE-1))||variant.Cells.Count==1,"Out-of-bounds shape accepted");
            p.OnBeginDrag(new PointerEventData(EventSystem.current){position=RectTransformUtility.WorldToScreenPoint(null,p.transform.position)});
            var pointer=new PointerEventData(EventSystem.current){position=DropPoint(p)};
            p.OnDrag(pointer);
            Check(p.ShapeData==variant&&Field<List<GridCell>>(board,"currentPreviewCells").Count==variant.Cells.Count,"Preview lost/rotated cells: "+variant.Key);
            p.OnEndDrag(pointer);p.OnEndDrag(pointer);
            Check(Cells.Count(c=>c.IsOccupied)==variant.Cells.Count&&GameManager.Instance.CurrentScore==variant.Cells.Count,"Drag commit/score: "+variant.Key);
            foreach(var cell in Cells)Check(cell.IsOccupied==variant.Cells.Contains(new Vector2Int(cell.Row,cell.Col)),"Wrong occupancy orientation: "+variant.Key);
            Check(!board.TryPlaceShape(variant,board.CellPosition(0,0)),"Overlap accepted");
            Check(GameManager.Instance.CurrentScore==variant.Cells.Count,"Invalid placement changed score");
            tested++;yield return .02f;
        }
        // New long shape completes real horizontal and vertical lines with unchanged scoring.
        foreach(var variant in shapes[4].Variants())
        {
            Fresh();var board=GridBoard.Instance;bool horizontal=variant.rows==1;
            foreach(var c in Cells)if(horizontal?c.Row==0&&c.Col>=5:c.Col==0&&c.Row>=5)c.SetOccupied(true);
            board.SyncModel();Check(board.TryPlaceShape(variant,board.CellPosition(0,0)),"Long line clear placement failed");
            Check(Cells.All(c=>!c.IsOccupied)&&GameManager.Instance.CurrentCombo==1&&GameManager.Instance.CurrentScore==315,"Long line clear/score changed");
        }
        Fresh();foreach(var c in Cells)c.SetOccupied(true);GridBoard.Instance.SyncModel();TrayManager.Instance.CheckGameOverCondition();
        Check(GameManager.Instance.IsGameOver,"Full board not Game Over with new library");yield return 1f;
        UnityEngine.Object.FindFirstObjectByType<GameOverController>().Replay();yield return .3f;
        Check(!GameManager.Instance.IsInputLocked&&Cells.All(c=>!c.IsOccupied)&&TrayManager.Instance.Pieces.Count(p=>p!=null)==3,"Replay regression");
        // Review image: horizontal/vertical 5-cell line plus the optional 3-cell diagonal.
        Fresh();var lines=shapes[4].Variants();ForceVariant(TrayManager.Instance.Pieces[0],lines[0]);ForceVariant(TrayManager.Instance.Pieces[1],lines[1]);ForceVariant(TrayManager.Instance.Pieces[2],shapes[16].Variants()[0]);
        yield return .3f;ScreenCapture.CaptureScreenshot("Captures/shape-library-long-lines.png");yield return .3f;
    }
    static void Finish(string error)
    {
        EditorApplication.update-=Tick;(routine as IDisposable)?.Dispose();routine=null;
        if(hadRecord)PlayerPrefs.SetInt("BlockBlast_HighScore",record);else PlayerPrefs.DeleteKey("BlockBlast_HighScore");PlayerPrefs.Save();
        UnityEngine.Random.state=random;Application.runInBackground=background;
        string result=error==null?"PASS: 17 catalog references; 41 unique random/runtime orientations; all 41 rendered with correct cell count, real OnBeginDrag/OnDrag/OnEndDrag, preview, exact occupancy, score and invalid overlap. Long lines fit tray, clear horizontal/vertical lines with score 315. Full-board Game Over and free Replay pass. Record restored.":"FAIL after "+tested+" orientations: "+error;
        Directory.CreateDirectory("Captures");File.WriteAllText("Captures/shape-library-play-checks.txt",result);
        if(error==null)Debug.Log(result);else Debug.LogError(result);
        if(EditorApplication.isPlaying)SceneManager.LoadScene("MainMenu");
    }
}

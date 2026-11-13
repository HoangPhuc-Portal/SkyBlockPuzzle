using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BlockBlast.Grid;
using BlockBlast.Shape;
using BlockBlast.UI;

public static class TrayFeedbackChecks
{
    static IEnumerator routine;static double next,deadline;
    static readonly System.Text.StringBuilder log=new System.Text.StringBuilder();
    static void Check(bool value,string message){if(!value)throw new Exception(message);log.AppendLine("PASS "+message);}
    public static void Run(){log.Clear();routine=Exercise();next=0;deadline=EditorApplication.timeSinceStartup+45;EditorApplication.update+=Tick;}
    static void Tick(){try{if(!EditorApplication.isPlaying||EditorApplication.timeSinceStartup>deadline)throw new Exception("Stopped or timeout");if(EditorApplication.timeSinceStartup<next)return;if(!routine.MoveNext()){Finish(null);return;}next=EditorApplication.timeSinceStartup+(routine.Current is float?(float)routine.Current:0);}catch(Exception e){Finish(e.ToString());}}
    static void Finish(string error){EditorApplication.update-=Tick;routine=null;log.AppendLine(error??"ALL PASSED");System.IO.Directory.CreateDirectory("Captures/TrayFeedback");System.IO.File.WriteAllText("Captures/TrayFeedback/checks.txt",log.ToString());if(error!=null)Debug.LogError(error);}
    static IEnumerator Exercise()
    {
        var board=GridBoard.Instance;var piece=TrayManager.Instance.Pieces.First(p=>p!=null);var view=piece.GetComponent<PieceView>();
        var pointer=new PointerEventData(EventSystem.current){position=new Vector2(Screen.width*.5f,Screen.height*.5f)};
        int tiles=piece.transform.childCount;
        Check(!view.IsDragGlowActive,"Tray starts without glow");
        for(int i=0;i<3;i++){
            piece.OnBeginDrag(pointer);yield return .15f;
            Check(view.IsDragGlowActive&&piece.GetComponentsInChildren<Outline>().All(o=>o.enabled&&o.effectColor.a>.4f),"Drag glow enabled "+i);
            Check(piece.transform.childCount==tiles&&piece.GetComponentsInChildren<Outline>().Length==tiles,"No duplicate tiles/glows "+i);
            if(i==0){ScreenCapture.CaptureScreenshot("Captures/TrayFeedback/drag.png");yield return .1f;}
            if(i==1)piece.OnEndDrag(new PointerEventData(EventSystem.current){position=new Vector2(-1000,-1000)});else piece.OnCancel(null);
            yield return .35f;
            Check(!view.IsDragGlowActive&&piece.GetComponentsInChildren<Outline>().All(o=>!o.enabled),"Cancel/invalid return clears glow "+i);
        }
        var cell=board.GetComponentsInChildren<GridCell>().First();var cv=cell.GetComponent<CellView>();
        var serialized=new SerializedObject(cell);var fill=(GameObject)serialized.FindProperty("fillVisual").objectReferenceValue;
        cell.SetOccupied(true,Color.white,piece.BlockSprite);
        var baseline=(Vector3)typeof(CellView).GetField("restScale",BindingFlags.NonPublic|BindingFlags.Instance).GetValue(cv);
        yield return .04f;
        Check(cv.IsAnimating&&fill.transform.localScale.x>baseline.x,"Successful occupancy starts visual bounce");
        var glow=(SpriteRenderer)typeof(CellView).GetField("placementGlow",BindingFlags.NonPublic|BindingFlags.Instance).GetValue(cv);
        Check(glow!=null&&glow.enabled&&glow.color.a>0,"Placement flash active");
        yield return .3f;
        Check(!cv.IsAnimating&&!glow.enabled&&Vector3.Distance(fill.transform.localScale,baseline)<.0001f,"Exact scale restored and flash stopped");
        cell.SetOccupied(false);
        piece.OnBeginDrag(pointer);yield return .12f;piece.gameObject.SetActive(false);
        Check(!view.IsDragGlowActive,"Disable clears drag glow");piece.gameObject.SetActive(true);yield return .3f;
        Check(!view.IsDragGlowActive,"Reenable does not revive glow");
    }
}

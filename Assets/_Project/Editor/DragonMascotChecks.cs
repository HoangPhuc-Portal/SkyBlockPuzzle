using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using BlockBlast.Core;
using BlockBlast.Data;
using BlockBlast.Grid;
using BlockBlast.Managers;
using BlockBlast.Mascot;
using BlockBlast.Shape;
using BlockBlast.UI;
public static class DragonMascotChecks
{
    static IEnumerator routine;static double deadline;static float next;static bool had,background;static int record;static ShapeData shape;
    const string Key="BlockBlast_HighScore";
    static void Check(bool b,string s){if(!b)throw new Exception(s);}
    [MenuItem("Block Blast/Tests/Run Dragon Mascot Play Checks")]
    public static void Run(){if(!EditorApplication.isPlaying||routine!=null)return;had=PlayerPrefs.HasKey(Key);record=PlayerPrefs.GetInt(Key);background=Application.runInBackground;Application.runInBackground=true;EditorApplication.isPaused=false;routine=Exercise();deadline=EditorApplication.timeSinceStartup+100;next=0;EditorApplication.update+=Tick;}
    static void Tick(){try{Check(EditorApplication.isPlaying&&EditorApplication.timeSinceStartup<deadline,"Timeout/Play stopped");if(Time.unscaledTime<next)return;if(!routine.MoveNext()){Finish("PASS: Menu idle/tap, Gameplay placement and non-raycast, real single/multiple line clears, combo 6/8 crossing and no retrigger, priority, Game Over/Continue/Replay, disable/re-enable, scene roundtrip and record restoration. Blink/tail clips unavailable for the original blue sprite.");return;}next=Time.unscaledTime+(routine.Current is float?(float)routine.Current:0);}catch(Exception e){Finish("FAIL: "+e);}}
    static void Finish(string s){EditorApplication.update-=Tick;(routine as IDisposable)?.Dispose();routine=null;Application.runInBackground=background;if(shape!=null)UnityEngine.Object.DestroyImmediate(shape);if(had)PlayerPrefs.SetInt(Key,record);else PlayerPrefs.DeleteKey(Key);PlayerPrefs.Save();Directory.CreateDirectory("Captures");File.WriteAllText("Captures/dragon-mascot-checks.txt",s);if(s.StartsWith("PASS"))Debug.Log(s);else Debug.LogError(s);if(EditorApplication.isPlaying)SceneManager.LoadScene("MainMenu");}
    static DragonMascot Find()=>UnityEngine.Object.FindFirstObjectByType<DragonMascot>();
    static Rect ScreenRect(RectTransform r){var c=new Vector3[4];r.GetWorldCorners(c);var canvas=r.GetComponentInParent<Canvas>();var cam=canvas.renderMode==RenderMode.ScreenSpaceOverlay?null:canvas.worldCamera;var lo=RectTransformUtility.WorldToScreenPoint(cam,c[0]);var hi=RectTransformUtility.WorldToScreenPoint(cam,c[2]);return Rect.MinMaxRect(lo.x,lo.y,hi.x,hi.y);}
    static void Score(GameManager gm,int lines){typeof(GameManager).GetMethod("ScorePlacement",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(gm,new object[]{1,lines,false});}
    static void PlaceClear(int rows)
    {
        var board=GridBoard.Instance;var cells=board.GetComponentsInChildren<GridCell>();
        foreach(var c in cells)c.SetOccupied(c.Row<rows&&!(c.Row==0&&c.Col==0),Color.cyan,TrayManager.Instance.Pieces.First(p=>p!=null).BlockSprite);
        board.SyncModel();Check(board.TryPlaceShape(shape.CreateRuntime(),board.CellPosition(0,0),TrayManager.Instance.Pieces.First(p=>p!=null).BlockSprite),"Fixture placement failed");
    }
    static IEnumerator Exercise()
    {
        SceneManager.LoadScene("MainMenu");yield return .8f;var menu=Find();Check(menu!=null&&!menu.IsGameplay,"Menu mascot missing");var rect=(RectTransform)menu.transform;var p=rect.anchoredPosition;yield return .3f;Check(rect.anchoredPosition!=p,"No idle movement");menu.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();Check(menu.State==DragonMascot.Reaction.Happy,"Menu tap");yield return .8f;Check(menu.State==DragonMascot.Reaction.Idle,"Menu return idle");ScreenCapture.CaptureScreenshot("Captures/dragon-mainmenu.png");yield return .25f;
        SceneManager.LoadScene("GamePlay");yield return 1f;var dragon=Find();var gm=GameManager.Instance;Check(dragon!=null&&dragon.IsGameplay&&dragon.IsVisible,"Gameplay mascot missing");Check(!dragon.GetComponent<UnityEngine.UI.Image>().raycastTarget,"Mascot captures input");var hud=GameObject.Find("Canvas/SafeArea/HUD/ReferenceWoodHUD").GetComponent<RectTransform>();var tray=GameObject.Find("Canvas/SafeArea/TrayPanel").GetComponent<RectTransform>();var bounds=ScreenRect((RectTransform)dragon.transform);Check(!bounds.Overlaps(ScreenRect(hud))&&!bounds.Overlaps(ScreenRect(tray)),"Mascot overlaps HUD/tray");Check(Screen.safeArea.Contains(bounds.min)&&Screen.safeArea.Contains(bounds.max),"Outside safe area");ScreenCapture.CaptureScreenshot("Captures/dragon-gameplay.png");yield return .25f;
        shape=ScriptableObject.CreateInstance<ShapeData>();shape.rows=shape.columns=1;shape.rotationPolicy=RotationPolicy.NoRotation;typeof(ShapeData).GetField("matrixData",BindingFlags.Instance|BindingFlags.NonPublic).SetValue(shape,new[]{true});
        PlaceClear(1);Check(dragon.State==DragonMascot.Reaction.Happy,"Single clear reaction");yield return .8f;Check(dragon.State==DragonMascot.Reaction.Idle,"Happy stuck");PlaceClear(2);Check(dragon.State==DragonMascot.Reaction.Happy,"Multi clear reaction");yield return .8f;
        for(int i=gm.CurrentCombo;i<6;i++)Score(gm,1);Check(dragon.State==DragonMascot.Reaction.Excited,"Combo 6 reaction");GameEvents.TriggerLinesCleared(1);Check(dragon.State==DragonMascot.Reaction.Excited,"Clear overrides combo");int count=dragon.ReactionCount;GameEvents.OnComboChanged?.Invoke(6);Check(dragon.ReactionCount==count,"Repeated same combo");yield return 1.3f;Score(gm,1);Check(dragon.ReactionCount==count,"Combo 7 incorrectly excited");Score(gm,1);Check(dragon.State==DragonMascot.Reaction.Excited&&dragon.ReactionCount==count+1,"Combo 8");Score(gm,1);Check(dragon.ReactionCount==count+1,"Max combo repeated");
        gm.ConfirmGameOver();Check(dragon.State==DragonMascot.Reaction.Encourage,"Game Over reaction");yield return 1.4f;Check(dragon.State==DragonMascot.Reaction.Idle,"Encourage stuck");var over=UnityEngine.Object.FindFirstObjectByType<GameOverController>();Check(over.CanRequestRevive,"Continue unavailable");over.Revive();Check(dragon.State==DragonMascot.Reaction.Idle&&!gm.IsGameOver,"Continue mascot stale");
        PlaceClear(1);GridBoard.Instance.ResetBoard();gm.ResetRound();gm.UnlockNewRound();Check(dragon.State==DragonMascot.Reaction.Idle,"Replay stale");dragon.enabled=false;dragon.enabled=true;count=dragon.ReactionCount;PlaceClear(1);Check(dragon.ReactionCount==count+1,"Duplicate subscriptions");
        SceneManager.LoadScene("MainMenu");yield return .8f;Check(UnityEngine.Object.FindObjectsByType<DragonMascot>(FindObjectsSortMode.None).Length==1&&!Find().IsGameplay,"Duplicate after scene roundtrip");
    }
}

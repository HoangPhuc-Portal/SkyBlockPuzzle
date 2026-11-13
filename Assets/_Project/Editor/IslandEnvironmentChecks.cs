using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using BlockBlast.Environment;
using BlockBlast.Grid;
using BlockBlast.Managers;

public static class IslandEnvironmentChecks
{
    static int stage;static double stamp,deadline;static float oldScale,frozen;static Vector3 cloudPosition,boardPosition,cameraPosition;static Quaternion cameraRotation;static Matrix4x4 projection;
    static IslandEnvironment env;static Camera camera;static int children;
    static int previousEnvironmentId;
    static readonly BindingFlags F=BindingFlags.NonPublic|BindingFlags.Instance;
    static T Get<T>(object o,string field)=>(T)o.GetType().GetField(field,F).GetValue(o);
    static void Check(bool ok,string message){if(!ok)throw new Exception(message);}
    static void CheckEntry()
    {
        Check(env.ActiveLeaves>=2,"Leaves missing on scene entry");
        var leaves=Get<SpriteRenderer[]>(env,"leaves");
        Check(leaves.Count(r=>r.enabled&&r.color.a>.5f)>=2,"Entry leaves too faint");
        var clouds=Get<SpriteRenderer[]>(env,"clouds");
        Check(clouds.Count(r=>r.enabled&&r.color.a>=.4f)>=2,"Entry clouds too faint");
        var cam=Get<Camera>(env,"viewCamera");
        Check(clouds.Any(r=>r.enabled&&cam.WorldToViewportPoint(r.transform.position).x>.1f&&cam.WorldToViewportPoint(r.transform.position).x<.9f),"Clouds outside view on entry");
    }
    [MenuItem("Block Blast/Environment/Run Play checks")]
    static void Run()
    {
        if(!EditorApplication.isPlaying)throw new Exception("Start Play Mode first");
        EditorApplication.update-=Tick;oldScale=Time.timeScale;Time.timeScale=1;stage=0;deadline=EditorApplication.timeSinceStartup+65;
        var previous=UnityEngine.Object.FindFirstObjectByType<IslandEnvironment>();
        previousEnvironmentId=previous!=null?previous.GetInstanceID():0;
        SceneManager.LoadScene("MainMenu");
        Directory.CreateDirectory("Captures");EditorApplication.update+=Tick;
    }
    static void Tick()
    {
        try
        {
            Check(EditorApplication.isPlaying&&EditorApplication.timeSinceStartup<deadline,"Timeout / Play stopped");
            if(stage==0)
            {
                env=UnityEngine.Object.FindFirstObjectByType<IslandEnvironment>();if(env==null||env.GetInstanceID()==previousEnvironmentId)return;
                CheckEntry();
                Check(Get<SpriteRenderer>(env,"background").sharedMaterial.shader.isSupported,"Water shader unsupported");
                cloudPosition=env.transform.GetChild(0).position;children=env.transform.childCount;stamp=EditorApplication.timeSinceStartup;
                ScreenCapture.CaptureScreenshot("Captures/environment-menu-before.png");stage=1;return;
            }
            if(stage==1)
            {
                if(EditorApplication.timeSinceStartup-stamp<8)return;
                Check(env.transform.GetChild(0).position!=cloudPosition,"Cloud did not move");Check(env.ActiveLeaves>0&&env.ActiveLeaves<=env.PoolSize,"Leaf spawn/pool");
                Check(env.transform.childCount==children,"Object count grew");
                ScreenCapture.CaptureScreenshot("Captures/environment-menu-after.png");Time.timeScale=0;frozen=env.AnimationTime;stamp=EditorApplication.timeSinceStartup;stage=2;return;
            }
            if(stage==2)
            {
                if(EditorApplication.timeSinceStartup-stamp<.4)return;
                Check(Mathf.Approximately(env.AnimationTime,frozen),"TimeScale pause failed");
                env.enabled=false;Check(env.GetComponentsInChildren<SpriteRenderer>().All(r=>!r.enabled),"Disable left sprites visible");
                Check(Get<SpriteRenderer>(env,"background").sharedMaterial.shader.name!="SkyBlock/IslandWaterUnlit","Disable did not restore background");
                env.enabled=true;Time.timeScale=1;SceneManager.LoadScene("GamePlay");stage=3;return;
            }
            if(stage==3)
            {
                env=UnityEngine.Object.FindFirstObjectByType<IslandEnvironment>();if(env==null||env.gameObject.scene.name!="GamePlay"||GridBoard.Instance==null)return;
                CheckEntry();ScreenCapture.CaptureScreenshot("Captures/environment-gameplay-entry.png");
                camera=Camera.main;cameraPosition=camera.transform.position;cameraRotation=camera.transform.rotation;projection=camera.projectionMatrix;boardPosition=GridBoard.Instance.transform.position;
                GameManager.Instance.RewardModalOpen=true;frozen=env.AnimationTime;stamp=EditorApplication.timeSinceStartup;stage=4;return;
            }
            if(stage==4)
            {
                if(EditorApplication.timeSinceStartup-stamp<.4)return;
                Check(Mathf.Approximately(env.AnimationTime,frozen),"Gameplay modal pause failed");GameManager.Instance.RewardModalOpen=false;stamp=EditorApplication.timeSinceStartup;stage=5;return;
            }
            if(stage==5)
            {
                if(EditorApplication.timeSinceStartup-stamp<8)return;
                Check(camera.transform.position==cameraPosition&&camera.transform.rotation==cameraRotation&&camera.projectionMatrix==projection,"Camera changed");
                Check(GridBoard.Instance.transform.position==boardPosition,"Board moved");
                Check(env.ActiveLeaves>0&&env.ActiveLeaves<=env.PoolSize,"Gameplay leaves missing");
                var board=GridBoard.Instance;var a=camera.WorldToViewportPoint(board.CellPosition(0,0));var b=camera.WorldToViewportPoint(board.CellPosition(GridBoard.GRID_SIZE-1,GridBoard.GRID_SIZE-1));
                var cell=camera.WorldToViewportPoint(board.CellPosition(0,0)+Vector3.right*board.CellSize)-a;
                var rect=Rect.MinMaxRect(Mathf.Min(a.x,b.x)-Mathf.Abs(cell.x)*.5f,Mathf.Min(a.y,b.y)-Mathf.Abs(cell.x)*.5f,Mathf.Max(a.x,b.x)+Mathf.Abs(cell.x)*.5f,Mathf.Max(a.y,b.y)+Mathf.Abs(cell.x)*.5f);
                foreach(var r in env.GetComponentsInChildren<SpriteRenderer>())if(r.enabled)
                {
                    var lo=camera.WorldToViewportPoint(r.bounds.min);var hi=camera.WorldToViewportPoint(r.bounds.max);
                    var fx=Rect.MinMaxRect(Mathf.Min(lo.x,hi.x),Mathf.Min(lo.y,hi.y),Mathf.Max(lo.x,hi.x),Mathf.Max(lo.y,hi.y));
                    Check(!rect.Overlaps(fx),"Effect overlaps board: "+r.name);
                }
                Check(env.GetComponentsInChildren<Collider>().Length==0&&env.GetComponentsInChildren<Collider2D>().Length==0&&env.GetComponentsInChildren<UnityEngine.UI.Graphic>().Length==0,"Effect can intercept input");
                ScreenCapture.CaptureScreenshot("Captures/environment-gameplay.png");stage=6;stamp=EditorApplication.timeSinceStartup;return;
            }
            if(stage==6)
            {
                if(EditorApplication.timeSinceStartup-stamp<.3)return;
                SceneManager.LoadScene("MainMenu");stage=7;return;
            }
            env=UnityEngine.Object.FindFirstObjectByType<IslandEnvironment>();if(env==null||env.gameObject.scene.name!="MainMenu"||env.AnimationTime<.3f)return;
            Check(UnityEngine.Object.FindObjectsByType<IslandEnvironment>(FindObjectsSortMode.None).Length==1,"Duplicate environment after scene roundtrip");
            Finish("PASS: cloud movement; bounded leaf pool; supported water shader; TimeScale/modal pause; disable restores background; board/camera unchanged; no sprite bounds over board or input components; one environment after MainMenu/GamePlay/MainMenu. Device FPS and native GPU still require phone testing.");
        }
        catch(Exception e){Finish("FAIL: "+e);}
    }
    static void Finish(string result)
    {
        EditorApplication.update-=Tick;Time.timeScale=oldScale;if(GameManager.Instance!=null)GameManager.Instance.RewardModalOpen=false;
        File.WriteAllText("Captures/environment-play-checks.txt",result);if(result.StartsWith("PASS"))Debug.Log(result);else Debug.LogError(result);
    }
}

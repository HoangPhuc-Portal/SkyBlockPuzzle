using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using BlockBlast.Grid;
using BlockBlast.UI;

public static class GameplayEditModeChecks
{
    public static void SetView(int width, int height)
    {
        var assembly=typeof(Editor).Assembly;
        var sizesType=assembly.GetType("UnityEditor.GameViewSizes");
        var singleton=typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
        var sizes=singleton.GetProperty("instance",System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.FlattenHierarchy).GetValue(null);
        var flags=System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance;
        var group=sizesType.GetProperty("currentGroup",flags).GetValue(sizes);
        var sizeType=assembly.GetType("UnityEditor.GameViewSize");
        var kind=assembly.GetType("UnityEditor.GameViewSizeType");
        var size=Activator.CreateInstance(sizeType,flags,null,new object[]{Enum.Parse(kind,"FixedResolution"),width,height,"Gameplay QA "+width+"x"+height},null);
        group.GetType().GetMethod("AddCustomSize").Invoke(group,new[]{size});
        int count=(int)group.GetType().GetMethod("GetTotalCount").Invoke(group,null);
        var view=EditorWindow.GetWindow(assembly.GetType("UnityEditor.GameView"));
        view.GetType().GetProperty("selectedSizeIndex",flags).SetValue(view,count-1);view.Show();view.Focus();
    }
    [Serializable] public class Entry { public string path; public Vector3 position, scale; public Color color; public string sprite; }
    [Serializable] public class Snapshot { public Entry[] entries; public int cells, home, replay; }
    static string Path(Transform t) => t.parent == null ? t.name : Path(t.parent) + "/" + t.name;
    static Snapshot Read()
    {
        var board = UnityEngine.Object.FindFirstObjectByType<GridBoard>();
        var scene = board.gameObject.scene;
        var cells = board.GetComponentsInChildren<GridCell>(true);
        var targets = cells.Select(c => c.transform).ToList();
        foreach (string name in new[] { "Background", "BoardPanelView", "BoardRegion", "TrayPanel", "ReferenceStoneFrame", "BestPlate", "ScorePlate", "Settings" })
        {
            var go = GameObject.Find(name); if (go != null) targets.Add(go.transform);
        }
        var all = scene.GetRootGameObjects().SelectMany(r => r.GetComponentsInChildren<Transform>(true)).ToArray();
        return new Snapshot {
            cells = cells.Length,
            home = all.Count(t => t.name == "Home"), replay = all.Count(t => t.name == "Replay"),
            entries = targets.Select(t => { var r=t.GetComponent<SpriteRenderer>(); return new Entry { path=Path(t),position=t.position,scale=t.lossyScale,
                color=r!=null?r.color:Color.white,sprite=r!=null&&r.sprite!=null?AssetDatabase.GetAssetPath(r.sprite):"" }; }).OrderBy(e=>e.path).ToArray()
        };
    }
    public static string Capture()
    {
        GameplayLayoutAuthoring.Refresh(UnityEngine.Object.FindFirstObjectByType<BoardView>());
        var snapshot=Read();
        SessionState.SetString("Gameplay.EditSnapshot",JsonUtility.ToJson(snapshot));
        return "Captured "+snapshot.entries.Length+" static transforms, cells="+snapshot.cells+", Home="+snapshot.home+", Replay="+snapshot.replay;
    }
    public static string Compare()
    {
        var a=JsonUtility.FromJson<Snapshot>(SessionState.GetString("Gameplay.EditSnapshot","")); var b=Read();
        if(a.cells!=64||b.cells!=64||a.home!=b.home||a.replay!=b.replay||a.entries.Length!=b.entries.Length)throw new Exception("Static object count changed");
        for(int i=0;i<a.entries.Length;i++)
        {
            var x=a.entries[i]; var y=b.entries[i];
            if(x.path!=y.path||Vector3.Distance(x.position,y.position)>.001f||Vector3.Distance(x.scale,y.scale)>.001f||x.color!=y.color||x.sprite!=y.sprite)
                throw new Exception("Edit/Play mismatch: "+x.path+" "+x.position+" -> "+y.position+" scale "+x.scale+" -> "+y.scale);
        }
        string result="PASS: "+b.entries.Length+" static transforms and cell sprites/colors match Edit Mode; exactly 64 cells; no extra Home/Replay.";
        System.IO.Directory.CreateDirectory("Captures/EditModeSync");System.IO.File.AppendAllText("Captures/EditModeSync/checks.txt",DateTime.UtcNow.ToString("o")+" "+result+"\n");
        return result;
    }
    public static void CaptureGameView(string name)
    {
        var type=typeof(Editor).Assembly.GetType("UnityEditor.GameView"); var view=EditorWindow.GetWindow(type);
        var flags=System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic;
        var size=(Vector2)type.GetProperty("targetRenderSize",flags).GetValue(view,null);
        var rt=(RenderTexture)type.GetMethod("RenderView",flags).Invoke(view,new object[]{size,false});
        var previous=RenderTexture.active;
        try {RenderTexture.active=rt;var texture=new Texture2D(rt.width,rt.height,TextureFormat.RGB24,false);
            texture.ReadPixels(new Rect(0,0,rt.width,rt.height),0,0);
            // GameView's D3D render target is top-origin; normalize the readback for PNG.
            if (SystemInfo.graphicsUVStartsAtTop) {
                var pixels=texture.GetPixels32();var row=new Color32[rt.width];
                for(int y=0;y<rt.height/2;y++) {int a=y*rt.width,b=(rt.height-1-y)*rt.width;
                    Array.Copy(pixels,a,row,0,rt.width);Array.Copy(pixels,b,pixels,a,rt.width);Array.Copy(row,0,pixels,b,rt.width);}
                texture.SetPixels32(pixels);
            }
            texture.Apply();System.IO.Directory.CreateDirectory("Captures/EditModeSync");
            System.IO.File.WriteAllBytes("Captures/EditModeSync/"+name+".png",texture.EncodeToPNG());UnityEngine.Object.DestroyImmediate(texture);
        }finally{RenderTexture.active=previous;}
    }
}

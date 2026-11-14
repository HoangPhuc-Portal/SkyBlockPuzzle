using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using BlockBlast.UI;
using TMPro;
public static class GameplayArtInstaller
{
 const string R="Assets/_Project/_Assets/SkyIslandGameplay/";
 static Sprite S(string n)=>AssetDatabase.LoadAllAssetsAtPath(R+n+".png").OfType<Sprite>().First();
 static void Set(Object o,string field,Object v){var so=new SerializedObject(o);so.FindProperty(field).objectReferenceValue=v;so.ApplyModifiedPropertiesWithoutUndo();}
 static void Area(RectTransform t,Transform parent,float x,float y,float w,float h){t.SetParent(parent,false);t.anchorMin=new Vector2(x,y);t.anchorMax=new Vector2(x+w,y+h);t.offsetMin=t.offsetMax=Vector2.zero;t.localScale=Vector3.one;}
 static Image Art(string name,Transform parent,string sprite,float x,float y,float w,float h){var go=new GameObject(name,typeof(RectTransform),typeof(Image));var im=go.GetComponent<Image>();Area((RectTransform)go.transform,parent,x,y,w,h);im.sprite=S(sprite);im.raycastTarget=false;return im;}
 static void Label(Transform parent,string value,float y){var g=new GameObject(value,typeof(RectTransform),typeof(TextMeshProUGUI));Area((RectTransform)g.transform,parent,.15f,y,.7f,.28f);var t=g.GetComponent<TextMeshProUGUI>();t.text=value;t.fontSize=38;t.alignment=TextAlignmentOptions.Center;t.raycastTarget=false;}
 public static string Install(){
 if(EditorApplication.isPlaying)throw new System.Exception("Edit Mode required");
 if(GameObject.Find("SkyGameplayComposition")!=null)throw new System.Exception("Already installed");
 System.IO.Directory.CreateDirectory(R+"Configs");System.IO.Directory.CreateDirectory(R+"Prefabs");AssetDatabase.Refresh();
 var view=Object.FindFirstObjectByType<BoardView>();var old=view.Theme;var theme=Object.Instantiate(old);AssetDatabase.CreateAsset(theme,R+"Configs/GameplayTheme.asset");theme.cellSprite=S("BoardCell");theme.frameSprite=S("BoardFrame");theme.scaleFrameArtwork=true;theme.frameArtworkInsets=new Vector4(.14f,.14f,.14f,.17f);theme.overlay=Color.clear;theme.boardMaxWidth=1200;
 foreach(var m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include,FindObjectsSortMode.None)){var so=new SerializedObject(m);var f=so.FindProperty("theme");if(f!=null&&f.objectReferenceValue==old){f.objectReferenceValue=theme;so.ApplyModifiedPropertiesWithoutUndo();}}
 var board=Object.FindFirstObjectByType<BlockBlast.Grid.GridBoard>();var bs=new SerializedObject(board);var cell=(Component)bs.FindProperty("cellPrefab").objectReferenceValue;var copy=Object.Instantiate(cell.gameObject);Set(copy.GetComponent<CellView>(),"theme",theme);var prefab=PrefabUtility.SaveAsPrefabAsset(copy,R+"Prefabs/GameplayCell.prefab");Object.DestroyImmediate(copy);Set(board,"cellPrefab",prefab.GetComponent<BlockBlast.Grid.GridCell>());
 var canvas=view.GetComponentInParent<Canvas>();var safe=canvas.transform.Find("SafeArea");var root=new GameObject("SkyGameplayComposition",typeof(RectTransform),typeof(AspectRatioFitter));Area((RectTransform)root.transform,safe,0,0,1,1);var fit=root.GetComponent<AspectRatioFitter>();fit.aspectRatio=941f/1672;fit.aspectMode=AspectRatioFitter.AspectMode.FitInParent;
 var coords=new GameObject("DesignCoordinates",typeof(RectTransform),typeof(BlockBlast.Menu.MenuDesignScale));coords.transform.SetParent(root.transform,false);((RectTransform)coords.transform).sizeDelta=new Vector2(941,1672);var design=coords.transform;
 var bg=GameObject.Find("Background").GetComponent<SpriteRenderer>();bg.sprite=S("IslandBackground");
 var soft=Resources.FindObjectsOfTypeAll<SpriteRenderer>().FirstOrDefault(r=>r.gameObject.scene.IsValid()&&r.name=="BackgroundSoftLayer");if(soft!=null)soft.enabled=false;
 var boardRect=(RectTransform)view.transform;Area(boardRect,design,.08f,.315f,.84f,.455f);
 var frame=(SpriteRenderer)new SerializedObject(view).FindProperty("frame").objectReferenceValue;frame.sprite=S("BoardFrame");
 var tray=Object.FindFirstObjectByType<TrayController>();Area((RectTransform)tray.transform,design,.16f,.165f,.70f,.12f);var ti=tray.GetComponent<Image>();ti.sprite=S("PieceTray");ti.type=Image.Type.Simple;ti.color=Color.white;
 foreach(var im in tray.GetComponentsInChildren<Image>(true))if(im!=ti)im.enabled=false;
 var hud=Object.FindFirstObjectByType<GameHUD>();var oldHud=hud.transform.Find("ReferenceWoodHUD");oldHud.GetComponent<AspectRatioFitter>().enabled=false;
 var hs=new SerializedObject(hud);var best=(TextMeshProUGUI)hs.FindProperty("score").objectReferenceValue;var score=(TextMeshProUGUI)hs.FindProperty("highScore").objectReferenceValue;
 var bp=Art("BestPlate",design,"BestPlate",.04f,.905f,.28f,.075f);var sp=Art("ScorePlate",design,"ScorePlate",.32f,.89f,.38f,.105f);
 Area(best.rectTransform,bp.transform,.36f,.12f,.58f,.47f);Area(score.rectTransform,sp.transform,.15f,.14f,.7f,.43f);best.text=score.text="0";foreach(var t in new[]{best,score}){t.enableAutoSizing=true;t.fontSizeMin=18;t.fontSizeMax=64;t.color=new Color32(255,217,76,255);}
 Label(bp.transform,"Best",.57f);Label(sp.transform,"Score",.62f);hs.FindProperty("separateScorePlates").boolValue=true;hs.ApplyModifiedPropertiesWithoutUndo();
 var setting=oldHud.Find("Settings");Area((RectTransform)setting,design,.83f,.90f,.12f,.075f);setting.GetComponent<Image>().sprite=S("Settings");setting.GetComponent<Image>().type=Image.Type.Simple;foreach(var im in setting.GetComponentsInChildren<Image>(true))if(im.gameObject!=setting.gameObject)im.enabled=false;
 foreach(var im in oldHud.GetComponentsInChildren<Image>(true))im.enabled=false;
 var dragon=Object.FindFirstObjectByType<BlockBlast.Mascot.DragonMascot>();Area((RectTransform)dragon.transform,design,.005f,.035f,.23f,.16f);var ds=new SerializedObject(dragon);ds.FindProperty("topBar").objectReferenceValue=null;ds.ApplyModifiedPropertiesWithoutUndo();dragon.GetComponent<Image>().sprite=S("Dragon");dragon.GetComponent<Image>().raycastTarget=false;
 Art("DragonPerch",design,"DragonPerch",.01f,.005f,.25f,.065f).transform.SetAsFirstSibling();Art("BoardTraySteps",design,"BoardTraySteps",.39f,.275f,.22f,.045f).transform.SetAsFirstSibling();
 var names=new[]{"BlockBlue","BlockGold","BlockCoral","BlockGreen","BlockBlue","BlockPurple","BlockCoral","BlockPurple"};for(int i=0;i<8;i++){theme.blockSprites[i]=S(names[i]);theme.highlightedSprites[i]=S(names[i]);theme.disabledSprites[i]=S(names[i]);theme.blockUsesTint[i]=false;}
 EditorUtility.SetDirty(theme);EditorSceneManager.MarkSceneDirty(board.gameObject.scene);EditorSceneManager.SaveScene(board.gameObject.scene);AssetDatabase.SaveAssets();return "Static gameplay layout installed";
 }
}


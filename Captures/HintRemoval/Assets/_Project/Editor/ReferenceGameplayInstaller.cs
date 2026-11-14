
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using UnityEditor.U2D;
using TMPro;
using BlockBlast.UI;
using BlockBlast.Grid;
using BlockBlast.Shape;

public static class ReferenceGameplayInstaller
{
    private static Sprite Art(string name) => AssetDatabase.LoadAssetAtPath<Sprite>(ReferenceGameplayArt.Folder+"/"+name+".png");
    private static void Set(UnityEngine.Object obj,string field,UnityEngine.Object value)
    {var so=new SerializedObject(obj);so.FindProperty(field).objectReferenceValue=value;so.ApplyModifiedPropertiesWithoutUndo();}
    private static void Rect(Transform transform,Vector2 min,Vector2 max)
    {var r=(RectTransform)transform;r.anchorMin=min;r.anchorMax=max;r.offsetMin=r.offsetMax=Vector2.zero;r.localScale=Vector3.one;}
    private static RectTransform Node(string name,Transform parent)
    {var found=parent.Find(name);if(found!=null)return(RectTransform)found;var go=new GameObject(name,typeof(RectTransform));go.transform.SetParent(parent,false);Rect(go.transform,Vector2.zero,Vector2.one);return(RectTransform)go.transform;}
    private static void Text(TextMeshProUGUI text,float size)
    {text.fontSize=size;text.fontSizeMax=size;text.fontSizeMin=size*.65f;text.enableAutoSizing=true;text.fontStyle=FontStyles.Bold;text.color=Color.white;text.outlineWidth=.06f;}
    private static void SkinButton(Transform button,string art,Vector2 min,Vector2 max)
    {
        Rect(button,min,max);
        foreach(Transform child in button)child.gameObject.SetActive(false);
        var image=button.GetComponent<Image>();image.enabled=true;image.sprite=Art(art);image.type=Image.Type.Simple;image.color=Color.white;image.preserveAspect=false;
        button.GetComponent<Button>().targetGraphic=image;
    }
    public static void Apply(GameObject root)
    {
        var canvas=root.GetComponentInChildren<Canvas>(true);
        var scaler=canvas.GetComponent<CanvasScaler>();scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution=new Vector2(1080,2340);scaler.matchWidthOrHeight=.5f;
        var safe=canvas.transform.Find("SafeArea");
        var hud=safe.Find("HUD");Rect(hud,new Vector2(.025f,.80f),new Vector2(.975f,.95f));
        var surface=Node("ReferenceWoodHUD",hud);
        var aspect=surface.GetComponent<AspectRatioFitter>()??surface.gameObject.AddComponent<AspectRatioFitter>();
        aspect.aspectMode=AspectRatioFitter.AspectMode.FitInParent;aspect.aspectRatio=432f/97;
        foreach(string childName in new[]{"Pill","ScoreArea","HighScore","Settings","Hint"})
        {var child=hud.Find(childName);if(child!=null)child.SetParent(surface,false);}
        var pill=surface.Find("Pill");Rect(pill,Vector2.zero,Vector2.one);
        var art=pill.GetComponent<Image>();art.sprite=Art("HUDWood");art.type=Image.Type.Simple;art.color=Color.white;art.raycastTarget=false;
        var area=surface.Find("ScoreArea");Rect(area,new Vector2(.205f,.19f),new Vector2(.365f,.78f));
        area.Find("Trophy").gameObject.SetActive(false);
        var score=area.Find("Score").GetComponent<TextMeshProUGUI>();Rect(score.transform,Vector2.zero,Vector2.one);Text(score,78);
        var high=surface.Find("HighScore").GetComponent<TextMeshProUGUI>();Rect(high.transform,new Vector2(.415f,.20f),new Vector2(.69f,.77f));Text(high,46);
        SkinButton(surface.Find("Settings"),"SettingsButton",new Vector2(.722f,.216f),new Vector2(.831f,.804f));
        SkinButton(surface.Find("Hint"),"HintButton",new Vector2(.840f,.216f),new Vector2(.949f,.804f));
        var crown=Node("ReferenceCrown",surface);Rect(crown,new Vector2(.448f,.82f),new Vector2(.552f,1.18f));
        var crownImage=crown.GetComponent<Image>()??crown.gameObject.AddComponent<Image>();crownImage.sprite=Art("Crown");crownImage.color=Color.white;crownImage.preserveAspect=true;crownImage.raycastTarget=false;

        var viewport=safe.Find("BoardRegion");Rect(viewport,new Vector2(.055f,.295f),new Vector2(.945f,.795f));
        var board=root.GetComponentInChildren<GridBoard>(true);
        var frame=board.transform.Find("StoneFrameView");
        if(frame!=null)frame.gameObject.SetActive(false);
        var crest=board.transform.Find("ReferenceBoardCrest");
        if(crest!=null)UnityEngine.Object.DestroyImmediate(crest.gameObject);
        var panel=board.transform.Find("BoardPanelView");
        if(panel!=null)panel.GetComponent<SpriteRenderer>().color=new Color32(13,25,49,255);
        var tray=safe.Find("TrayPanel");Rect(tray,new Vector2(.045f,.055f),new Vector2(.955f,.28f));
        var trayImage=tray.GetComponent<Image>();trayImage.sprite=Art("TrayWood");trayImage.type=Image.Type.Simple;trayImage.color=Color.white;
        var trayView=tray.GetComponent<TrayController>();var serialized=new SerializedObject(trayView);
        serialized.FindProperty("anchorToSafeBottom").boolValue=false;serialized.ApplyModifiedPropertiesWithoutUndo();
        float[] left={.085f,.355f,.665f},right={.33f,.635f,.925f};
        for(int i=0;i<3;i++){
            var slot=tray.Find("slot_"+i);Rect(slot,new Vector2(left[i],.21f),new Vector2(right[i],.71f));
            slot.GetComponent<Image>().enabled=false;
        }
        EditorUtility.SetDirty(root);
    }
    [MenuItem("Block Blast/UI/Apply Reference Gameplay")]
    public static void Scene()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().isDirty)
            throw new InvalidOperationException("Unsaved scene changes: save or review them before running the installer.");
        var theme=AssetDatabase.LoadAssetAtPath<UITheme>("Assets/_Project/ScriptableObjects/UITheme.asset");
        theme.cellSprite=Art("CellBlue");theme.frameSprite=null;
        theme.boardCrest=null;
        theme.cellPixels=100;theme.gap=6;theme.padding=14;theme.frame=0;theme.boardMaxWidth=1024;theme.scaleFrameArtwork=false;
        theme.trayCellPixels=68;theme.overlay=new Color(27/255f,42/255f,58/255f,.10f);
        string[] names={"Blue","Yellow","Red","Green","Cyan","Purple","Orange","Pink"};
        theme.blockUsesTint = new bool[8];
        for(int i=0;i<8;i++){theme.blockSprites[i]=Art("Block"+names[i]);theme.highlightedSprites[i]=Art("Block"+names[i]+"Highlighted");theme.disabledSprites[i]=Art("Block"+names[i]+"Disabled");}
        theme.whiteBlock=Art("BlockWhite");
        EditorUtility.SetDirty(theme);
        var root=GameObject.Find("GameplayScreen");Undo.RegisterFullObjectHierarchyUndo(root,"Reference Gameplay");
        Apply(root);
        var bg=GameObject.Find("Background").GetComponent<SpriteRenderer>();bg.sprite=Art("BackgroundCliffs");bg.color=Color.white;
        var view=UnityEngine.Object.FindFirstObjectByType<BackgroundView>();Set(view,"background",bg);
        GameObject.Find("BackgroundSoftLayer").SetActive(false);
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        string prefab="Assets/_Project/Prefabs/UI/GameplayScreen.prefab";var contents=PrefabUtility.LoadPrefabContents(prefab);
        try{Apply(contents);PrefabUtility.SaveAsPrefabAsset(contents,prefab);}finally{PrefabUtility.UnloadPrefabContents(contents);}
        var cellPath="Assets/_Project/Prefabs/Shapes/GridCell.prefab";var cell=PrefabUtility.LoadPrefabContents(cellPath);
        try{var bgCell=cell.GetComponent<SpriteRenderer>();bgCell.sprite=theme.cellSprite;bgCell.drawMode=SpriteDrawMode.Sliced;bgCell.size=Vector2.one*.96f;
            var s=new SerializedObject(cell.GetComponent<GridCell>());var fill=(SpriteRenderer)s.FindProperty("fillRenderer").objectReferenceValue;
            fill.sprite=theme.whiteBlock;var scale=fill.transform.localScale;fill.transform.localScale=new Vector3(.96f/fill.sprite.bounds.size.x,.96f/fill.sprite.bounds.size.y,scale.z);
            PrefabUtility.SaveAsPrefabAsset(cell,cellPath);}finally{PrefabUtility.UnloadPrefabContents(cell);}
        var atlas=AssetDatabase.LoadAssetAtPath<SpriteAtlas>("Assets/_Project/UI/Gameplay.spriteatlas");
        SpriteAtlasExtensions.Add(atlas,new UnityEngine.Object[]{AssetDatabase.LoadAssetAtPath<DefaultAsset>(ReferenceGameplayArt.Folder)});
        EditorUtility.SetDirty(atlas);AssetDatabase.SaveAssets();EditorSceneManager.SaveScene(scene);
    }
}

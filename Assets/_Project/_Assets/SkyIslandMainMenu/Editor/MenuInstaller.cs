using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using BlockBlast.UI;
using BlockBlast.Mascot;
using BlockBlast.Menu;

public static class MenuInstaller
{
    const string Root = "Assets/_Project/_Assets/SkyIslandMainMenu/";
    static RectTransform Rect(string name, Transform parent)
    {
        var r = new GameObject(name, typeof(RectTransform)).GetComponent<RectTransform>();
        r.SetParent(parent, false); return r;
    }
    static void Full(RectTransform r) { r.anchorMin = Vector2.zero; r.anchorMax = Vector2.one; r.offsetMin = r.offsetMax = Vector2.zero; }
    static void Place(RectTransform r, Vector2 position, Vector2 size)
    {
        r.anchorMin = r.anchorMax = r.pivot = new Vector2(.5f, .5f);
        r.anchoredPosition = position; r.sizeDelta = size; r.localScale = Vector3.one; r.localRotation = Quaternion.identity;
    }
    static Sprite Sprite(string name) => AssetDatabase.LoadAllAssetsAtPath(Root + name + ".png").OfType<Sprite>().First();
    static Image Image(string name, Transform parent, Sprite sprite, Vector2 position, Vector2 size)
    {
        var r = Rect(name, parent); Place(r, position, size);
        var image = r.gameObject.AddComponent<Image>(); image.sprite = sprite; image.raycastTarget = false; image.preserveAspect = true;
        return image;
    }
    static Material Material(string name, string shader)
    {
        string path = Root + "Materials/" + name + ".mat";
        var material = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (material == null) { material = new Material(Shader.Find(shader)); AssetDatabase.CreateAsset(material, path); }
        return material;
    }
    static Sprite Dot()
    {
        string path = Root + "VFX/SoftLight.png";
        if (!File.Exists(path))
        {
            var t = new Texture2D(64, 64, TextureFormat.RGBA32, false);
            for (int y = 0; y < 64; y++) for (int x = 0; x < 64; x++)
            { float d = Vector2.Distance(new Vector2(x, y), new Vector2(31.5f, 31.5f)) / 31.5f; t.SetPixel(x, y, new Color(1, 1, 1, Mathf.Pow(Mathf.Clamp01(1-d), 2))); }
            t.Apply(); File.WriteAllBytes(path, t.EncodeToPNG()); UnityEngine.Object.DestroyImmediate(t); AssetDatabase.ImportAsset(path);
            var importer = (TextureImporter)AssetImporter.GetAtPath(path); importer.textureType = TextureImporterType.Sprite; importer.mipmapEnabled = false; importer.alphaIsTransparency = true; importer.SaveAndReimport();
        }
        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }
    static void Set(UnityEngine.Object obj, string property, UnityEngine.Object value)
    { var so = new SerializedObject(obj); so.FindProperty(property).objectReferenceValue = value; so.ApplyModifiedPropertiesWithoutUndo(); }
    [MenuItem("Block Blast/Main Menu/Install Reference Layout")]
    public static void Install()
    {
        if (EditorApplication.isPlaying) throw new InvalidOperationException("Stop Play Mode first.");
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (scene.path != "Assets/Scenes/MainMenu.unity") throw new InvalidOperationException("Open MainMenu first.");
        if (UnityEngine.Object.FindFirstObjectByType<MenuPresentation>() != null) throw new InvalidOperationException("Already installed; inspect existing layout instead of duplicating it.");
        foreach (string folder in new[]{"Materials","VFX","Animations","Prefabs"}) Directory.CreateDirectory(Root + folder);
        AssetDatabase.Refresh();
        var canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        var scaler = canvas.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(941, 1672); scaler.matchWidthOrHeight = .5f;
        var background = GameObject.Find("Background").GetComponent<SpriteRenderer>();
        background.enabled = false; // Keep original references available for rollback.
        var oldEnvironment = UnityEngine.Object.FindFirstObjectByType<BlockBlast.Environment.IslandEnvironment>();
        if (oldEnvironment != null) oldEnvironment.gameObject.SetActive(false);
        Camera.main.backgroundColor = new Color(.12f, .68f, .96f);
        var sky=Image("ExtendedSky",canvas.transform,Sprite("MenuIsland"),Vector2.zero,Vector2.zero);Full(sky.rectTransform);sky.preserveAspect=false;sky.material=Material("ExtendedSky","SkyBlock/MenuSky");sky.transform.SetAsFirstSibling();
        var safe = Rect("MenuSafeArea", canvas.transform); Full(safe); safe.gameObject.AddComponent<SafeArea>();
        var composition = Rect("ReferenceComposition", safe); composition.sizeDelta = new Vector2(941, 1672);
        var fit = composition.gameObject.AddComponent<AspectRatioFitter>(); fit.aspectRatio = 941f / 1672; fit.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        var backdrop = Image("MainIslandAndSky", composition, Sprite("MenuIsland"), Vector2.zero, new Vector2(941, 1672));
        Full(backdrop.rectTransform); backdrop.preserveAspect = false; backdrop.material = Material("IslandFlow", "SkyBlock/MenuIsland");
        // A nested dynamic canvas keeps moving characters/VFX separate from the static island geometry.
        var dynamic = Rect("AnimatedLayers", composition); Full(dynamic); dynamic.gameObject.AddComponent<Canvas>();
        dynamic.gameObject.AddComponent<GraphicRaycaster>();
        var coordinates = Rect("DesignCoordinates", dynamic); Place(coordinates, Vector2.zero, new Vector2(941, 1672));
        coordinates.gameObject.AddComponent<MenuDesignScale>();
        var environment = Rect("AmbientVFX", coordinates); Full(environment);
        var cloudSprites = AssetDatabase.LoadAllAssetsAtPath("Assets/_Project/Environment/Art/CloudLeafAtlas.png").OfType<Sprite>().ToArray();
        var cloud = cloudSprites.First(s => s.name.ToLowerInvariant().Contains("cloud"));
        var leaf = cloudSprites.First(s => s.name.ToLowerInvariant().Contains("leaf"));
        var clouds = new Image[4]; for (int i=0;i<4;i++) clouds[i]=Image("Cloud_"+i,environment,cloud,Vector2.zero,new Vector2(300+i*30,150));
        var logo = GameObject.Find("Logo_img").GetComponent<Image>(); logo.transform.SetParent(coordinates,false);
        Place(logo.rectTransform,new Vector2(0,525),new Vector2(660,603)); logo.sprite=Sprite("TitleLogo");logo.preserveAspect=true;logo.raycastTarget=false;
        logo.material=Material("LogoShine","SkyBlock/MenuShine");
        var human=GameObject.Find("img_character").GetComponent<Image>();human.transform.SetParent(coordinates,false);
        Place(human.rectTransform,new Vector2(-290,-85),new Vector2(340,372));human.sprite=Sprite("Explorer");human.preserveAspect=true;
        var dragon=GameObject.Find("img_dragon").GetComponent<Image>();dragon.transform.SetParent(coordinates,false);
        Place(dragon.rectTransform,new Vector2(300,-80),new Vector2(330,300));dragon.sprite=Sprite("MenuDragon");dragon.preserveAspect=true;
        var play=GameObject.Find("Play_btn").GetComponent<Button>();play.transform.SetParent(coordinates,false);
        Place((RectTransform)play.transform,new Vector2(0,-252),new Vector2(700,233));
        var playImage=play.GetComponent<Image>();playImage.sprite=Sprite("PlayButton");playImage.preserveAspect=true;
        var playMat=Material("PlayShine","SkyBlock/MenuShine");playMat.SetFloat("_Period",9);playMat.SetFloat("_Phase",4);playImage.material=playMat;
        var presentation=composition.gameObject.AddComponent<MenuPresentation>();
        presentation.composition=composition;presentation.playArt=(RectTransform)play.transform;
        presentation.dragon=dragon.rectTransform;presentation.explorer=human.rectTransform;presentation.island=backdrop;
        presentation.dragonIdle=dragon.GetComponent<DragonMascot>();presentation.clouds=clouds;
        var animator=human.gameObject.AddComponent<Animator>();animator.runtimeAnimatorController=HumanAnimation();animator.updateMode=AnimatorUpdateMode.UnscaledTime;presentation.explorerAnimator=animator;
        var effects=Rect("MagicAndMist",coordinates);Full(effects);var dot=Dot();
        presentation.motes=new Image[18];for(int i=0;i<18;i++)presentation.motes[i]=Image("Mote_"+i,effects,dot,Vector2.zero,Vector2.one*(i%3+5));
        presentation.smoke=new Image[3];for(int i=0;i<3;i++)presentation.smoke[i]=Image("ChimneySmoke_"+i,effects,cloud,Vector2.zero,new Vector2(55,40));
        presentation.leaves=new Image[3];for(int i=0;i<3;i++)presentation.leaves[i]=Image("Leaf_"+i,effects,leaf,Vector2.zero,new Vector2(18,12));
        presentation.mist=new Image[4];for(int i=0;i<4;i++)presentation.mist[i]=Image("WaterfallMist_"+i,effects,cloud,Vector2.zero,new Vector2(110,55));
        var settingImage=Image("SettingsButton",safe,Sprite("SettingsButton"),Vector2.zero,new Vector2(104,104));
        var sr=settingImage.rectTransform;sr.anchorMin=sr.anchorMax=new Vector2(1,1);sr.anchoredPosition=new Vector2(-68,-68);
        settingImage.raycastTarget=true;var setting=settingImage.gameObject.AddComponent<Button>();setting.targetGraphic=settingImage;
        settingImage.gameObject.AddComponent<ButtonAudio>();settingImage.gameObject.AddComponent<ButtonFeedback>();presentation.settings=setting;
        BuildSettings(canvas.transform,setting,play);
        var fade=Image("DepartureFade",canvas.transform,null,Vector2.zero,Vector2.zero);Full(fade.rectTransform);fade.color=new Color(.07f,.33f,.52f,1);fade.raycastTarget=false;
        presentation.fade=fade.gameObject.AddComponent<CanvasGroup>();presentation.fade.alpha=0;presentation.fade.blocksRaycasts=false;
        Set(play.GetComponent<PlaySceneLoader>(),"menuPresentation",presentation);
        var privacy=canvas.GetComponentInChildren<AdPrivacyButton>(true);if(privacy!=null){privacy.transform.SetParent(safe,false);var pr=(RectTransform)privacy.transform;Place(pr,new Vector2(-175,-55),new Vector2(220,36));pr.anchorMin=pr.anchorMax=new Vector2(1,1);}
        var loading=canvas.transform.Find("LoadingPanel");loading.SetAsLastSibling();
        EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();
        Selection.activeGameObject=composition.gameObject;
        Debug.Log("MainMenu reference layout installed; original background/environment retained inactive.");
    }
    static RuntimeAnimatorController HumanAnimation()
    {
        string path=Root+"Animations/ExplorerMenu.controller";
        var controller=AssetDatabase.LoadAssetAtPath<AnimatorController>(path);if(controller!=null)return controller;
        controller=AnimatorController.CreateAnimatorControllerAtPath(path);
        var idle=new AnimationClip{name="Idle"};
        idle.SetCurve("",typeof(Transform),"localScale.y",AnimationCurve.EaseInOut(0,1,2,1.008f));
        var curve=new AnimationCurve(new Keyframe(0,1),new Keyframe(2,1.008f),new Keyframe(4,1));idle.SetCurve("",typeof(Transform),"localScale.y",curve);
        var clipSettings=AnimationUtility.GetAnimationClipSettings(idle);clipSettings.loopTime=true;AnimationUtility.SetAnimationClipSettings(idle,clipSettings);
        AssetDatabase.CreateAsset(idle,Root+"Animations/ExplorerIdle.anim");controller.AddMotion(idle);
        var depart=new AnimationClip{name="Depart"};
        depart.SetCurve("",typeof(RectTransform),"m_AnchoredPosition.y",new AnimationCurve(new Keyframe(0,-85),new Keyframe(.7f,-85),new Keyframe(1,-48),new Keyframe(1.3f,-85),new Keyframe(1.6f,-85)));
        depart.SetCurve("",typeof(RectTransform),"m_AnchoredPosition.x",AnimationCurve.EaseInOut(0,-290,1.3f,-265));
        AssetDatabase.CreateAsset(depart,Root+"Animations/ExplorerDepart.anim");controller.AddMotion(depart);return controller;
    }
    static void BuildSettings(Transform parent,Button open,Button play)
    {
        var prefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Project/Prefabs/UI/GameplayScreen.prefab");
        var original=prefab.GetComponentInChildren<SettingsPanel>(true);var so=new SerializedObject(original);
        var source=(GameObject)so.FindProperty("popup").objectReferenceValue;
        var popup=UnityEngine.Object.Instantiate(source,parent,false);popup.name="MenuSettingsPopup";Full((RectTransform)popup.transform);
        var settings=parent.gameObject.AddComponent<MenuSettings>();settings.popup=popup;settings.open=open;settings.play=play;
        settings.close=Map<Button>(so,"close",source,popup);settings.music=Map<Slider>(so,"music",source,popup);settings.sfx=Map<Slider>(so,"sfx",source,popup);settings.haptic=Map<Toggle>(so,"haptic",source,popup);
        settings.close.onClick=new Button.ButtonClickedEvent();settings.music.onValueChanged=new Slider.SliderEvent();settings.sfx.onValueChanged=new Slider.SliderEvent();settings.haptic.onValueChanged=new Toggle.ToggleEvent();
        popup.SetActive(false);
        PrefabUtility.SaveAsPrefabAsset(popup,Root+"Prefabs/MenuSettingsPopup.prefab");
    }
    public static void Polish()
    {
        if(EditorApplication.isPlaying)throw new InvalidOperationException("Edit Mode required");
        var p=UnityEngine.Object.FindFirstObjectByType<MenuPresentation>();
        var canvas=p.GetComponentInParent<Canvas>();
        if(canvas.transform.Find("ExtendedSky")==null){var sky=Image("ExtendedSky",canvas.transform,Sprite("MenuIsland"),Vector2.zero,Vector2.zero);Full(sky.rectTransform);sky.preserveAspect=false;sky.material=Material("ExtendedSky","SkyBlock/MenuSky");sky.transform.SetAsFirstSibling();}
        if(p.mist==null||p.mist.Length==0){var cloud=p.clouds[0].sprite;p.mist=new Image[4];for(int i=0;i<4;i++)p.mist[i]=Image("WaterfallMist_"+i,p.motes[0].transform.parent,cloud,Vector2.zero,new Vector2(110,55));}
        p.playArt.sizeDelta=new Vector2(700,233);
        var privacy=canvas.GetComponentInChildren<AdPrivacyButton>(true);if(privacy!=null){var r=(RectTransform)privacy.transform;Place(r,new Vector2(-215,-55),new Vector2(220,36));r.anchorMin=r.anchorMax=new Vector2(1,1);}
        EditorSceneManager.MarkSceneDirty(p.gameObject.scene);EditorSceneManager.SaveScene(p.gameObject.scene);AssetDatabase.SaveAssets();
    }
    static T Map<T>(SerializedObject so,string name,GameObject source,GameObject clone) where T:Component
    {
        var reference=(Component)so.FindProperty(name).objectReferenceValue;
        string path=AnimationUtility.CalculateTransformPath(reference.transform,source.transform);
        return clone.transform.Find(path).GetComponent<T>();
    }
}

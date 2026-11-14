
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;
using BlockBlast.UI;
using BlockBlast.Grid;
using BlockBlast.Shape;
using BlockBlast.Managers;

public static class GameOverInstaller
{
    private static void Set(UnityEngine.Object target, string field, UnityEngine.Object value)
    {
        var serialized = new SerializedObject(target);
        serialized.FindProperty(field).objectReferenceValue = value;
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }
    private static RectTransform Node(string name, Transform parent, Vector2 min, Vector2 max)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var rect = (RectTransform)go.transform;
        rect.anchorMin = min; rect.anchorMax = max;
        rect.offsetMin = rect.offsetMax = Vector2.zero;
        return rect;
    }
    private static Image Surface(string name, Transform parent, Vector2 min, Vector2 max, Sprite sprite, Color color)
    {
        var rect = Node(name, parent, min, max);
        var image = rect.gameObject.AddComponent<Image>();
        image.sprite = sprite; image.color = color;
        if (sprite != null && sprite.border.sqrMagnitude > 0) image.type = Image.Type.Sliced;
        return image;
    }
    private static TextMeshProUGUI Label(string name, Transform parent, Vector2 min, Vector2 max,
        string value, float size, TMP_FontAsset font, Color color)
    {
        var rect = Node(name, parent, min, max);
        var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
        text.font = font; text.text = value; text.fontSize = size;
        text.fontStyle = FontStyles.Bold; text.alignment = TextAlignmentOptions.Center;
        text.enableAutoSizing = true; text.fontSizeMin = size * .8f; text.fontSizeMax = size;
        text.color = color; text.raycastTarget = false;
        return text;
    }
    private static Button Button(string name, Transform parent, Vector2 min, Vector2 max,
        string title, UITheme theme, TMP_FontAsset font, Color color)
    {
        var image = Surface(name, parent, min, max, theme.panelSprite, color);
        var button = image.gameObject.AddComponent<Button>();
        button.targetGraphic = image;
        image.gameObject.AddComponent<ButtonFeedback>();
        Label("Label", image.transform, new Vector2(.03f,.08f), new Vector2(.97f,.92f),
            title, 42, font, Color.white);
        return button;
    }
    public static void Install(GameObject root)
    {
        if (root.GetComponentInChildren<GameOverController>(true) != null)
            throw new InvalidOperationException("Game Over already installed; existing setup preserved.");
        var canvas = root.GetComponentInChildren<Canvas>(true);
        var hud = root.GetComponentInChildren<GameHUD>(true);
        var font = hud.GetComponentInChildren<TextMeshProUGUI>(true).font;
        var theme = AssetDatabase.LoadAssetAtPath<UITheme>("Assets/_Project/ScriptableObjects/UITheme.asset");
        var flow = root.AddComponent<GameOverController>();
        var popup = Node("GameOverPopup", canvas.transform, Vector2.zero, Vector2.one);
        var group = popup.gameObject.AddComponent<CanvasGroup>();
        Surface("Backdrop", popup, Vector2.zero, Vector2.one, null, new Color(.025f,.04f,.065f,.75f));
        var safe = Node("PopupSafeArea", popup, Vector2.zero, Vector2.one);
        safe.gameObject.AddComponent<SafeArea>();
        var panel = Surface("Panel", safe, new Vector2(.08f,.28f), new Vector2(.92f,.72f),
            theme.panelSprite, theme.panel).rectTransform;
        Label("Title", panel, new Vector2(.025f,.79f), new Vector2(.975f,.94f),
            "HẾT NƯỚC ĐI!", 72, font, Color.white);
        Label("ScoreLabel", panel, new Vector2(.1f,.67f), new Vector2(.9f,.75f),
            "ĐIỂM VÁN NÀY", 32, font, new Color(.7f,.8f,.95f));
        var glow = Surface("RecordGlow", panel, new Vector2(.06f,.44f),new Vector2(.94f,.66f),
            theme.panelSprite, Color.clear);
        glow.raycastTarget = false;
        var final = Label("FinalScore",panel,new Vector2(.1f,.46f),new Vector2(.9f,.65f),
            "0",110,font,Color.white);
        var high = Label("HighScore",panel,new Vector2(.05f,.34f),new Vector2(.95f,.44f),
            "KỶ LỤC  0",40,font,theme.recordGold);
        var crown = Node("NewRecordCrown",panel,new Vector2(.12f,.51f),new Vector2(.24f,.63f));
        // A small crown assembled from the existing particle sprite; no external artwork.
        Surface("Base",crown,new Vector2(.06f,.05f),new Vector2(.94f,.33f),theme.panelSprite,theme.recordGold).raycastTarget=false;
        for(int i=0;i<3;i++)
        {
            var tooth=Surface("Point"+i,crown,new Vector2(.04f+i*.32f,.31f),
                new Vector2(.32f+i*.32f,i==1?.98f:.78f),theme.whiteBlock,theme.recordGold);
            tooth.raycastTarget=false;
        }
        var replay = Button("Replay",panel,new Vector2(.08f,.18f),new Vector2(.92f,.30f),
            "Chơi lại",theme,font,new Color(.12f,.60f,.38f));
        var view = Button("ViewBoard",panel,new Vector2(.08f,.035f),new Vector2(.92f,.15f),
            "Tiếp tục xem board",theme,font,new Color(.20f,.30f,.46f));
        view.gameObject.SetActive(false);
        Set(flow,"theme",theme);Set(flow,"board",root.GetComponentInChildren<GridBoard>(true));
        Set(flow,"tray",root.GetComponentInChildren<TrayManager>(true));
        Set(flow,"scoreManager",root.GetComponentInChildren<GameManager>(true));
        Set(flow,"popup",popup.gameObject);Set(flow,"panel",panel);Set(flow,"group",group);
        Set(flow,"finalScore",final);Set(flow,"highScore",high);Set(flow,"crown",crown.gameObject);
        Set(flow,"recordGlow",glow);Set(flow,"replayButton",replay);Set(flow,"viewBoardButton",view);
        Set(flow,"hud",hud);
        Set(flow,"settings",root.GetComponentInChildren<SettingsPanel>(true));
        Set(flow,"clearPresentation",root.GetComponentInChildren<ClearPresentation>(true));
        Set(flow,"bubbles",root.GetComponentInChildren<BlockBlast.Effects.BubblePopEffectPlayer>(true));
        popup.gameObject.SetActive(false);
        EditorUtility.SetDirty(flow);
    }
    [MenuItem("Block Blast/UI/Install Game Over")]
    public static void InstallScene()
    {
        var root = GameObject.Find("GameplayScreen");
        if(root==null)throw new InvalidOperationException("GameplayScreen not found.");
        Undo.RegisterFullObjectHierarchyUndo(root,"Install Game Over");
        Install(root);
        EditorSceneManager.MarkSceneDirty(root.scene);
    }
}

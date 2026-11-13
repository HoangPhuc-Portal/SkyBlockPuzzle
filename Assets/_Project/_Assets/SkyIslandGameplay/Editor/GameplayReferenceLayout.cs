using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;
using BlockBlast.UI;

// Editor-only authoring pass. Coordinates match the supplied 941 x 1672 reference.
public static class GameplayReferenceLayout
{
    const string Root = "Assets/_Project/_Assets/SkyIslandGameplay/";
    static void Box(RectTransform rect, float x, float top, float width, float height)
    {
        rect.anchorMin = rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(.5f, .5f);
        rect.anchoredPosition = new Vector2(x + width / 2, -top - height / 2);
        rect.sizeDelta = new Vector2(width, height);
        rect.localScale = Vector3.one;
    }
    static void Set(Object target, string field, Object value)
    {
        var so = new SerializedObject(target);
        so.FindProperty(field).objectReferenceValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
    }
    static void Text(TextMeshProUGUI text, float x, float y, float w, float h, float size, bool gold)
    {
        Box(text.rectTransform, x, y, w, h);
        text.enableAutoSizing = true; text.fontSizeMin = 24; text.fontSizeMax = size;
        text.fontSize = size; text.fontStyle = FontStyles.Bold;
        text.alignment = TextAlignmentOptions.Center;
        text.color = gold ? new Color32(255, 220, 77, 255) : Color.white;
        text.margin = Vector4.zero; text.raycastTarget = false;
    }
    public static string Apply()
    {
        if (EditorApplication.isPlaying) throw new System.Exception("Edit Mode required");
        var design = GameObject.Find("DesignCoordinates").transform;
        var fit = design.parent.GetComponent<AspectRatioFitter>();
        fit.aspectRatio = 941f / 1672f; fit.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        var board = Object.FindFirstObjectByType<BoardView>();
        var theme = board.Theme;
        theme.padding = 0; theme.frame = 0; theme.gap = 4;
        theme.scaleFrameArtwork = false; theme.trayCellPixels = 76;
        var bs = new SerializedObject(board);
        bs.FindProperty("fitAuthoredViewport").boolValue = true;
        var oldFrame = (SpriteRenderer)bs.FindProperty("frame").objectReferenceValue;
        if (oldFrame != null) oldFrame.gameObject.SetActive(false);
        bs.FindProperty("frame").objectReferenceValue = null;
        bs.ApplyModifiedPropertiesWithoutUndo();
        Box((RectTransform)board.transform, 150, 455, 640, 640);
        Set(Object.FindFirstObjectByType<BackgroundView>(), "artworkViewport", design);
        var background = GameObject.Find("Background").GetComponent<SpriteRenderer>();
        var skyObject = GameObject.Find("GameplayExtendedSky");
        if (skyObject == null) skyObject = new GameObject("GameplayExtendedSky", typeof(SpriteRenderer));
        skyObject.transform.SetParent(background.transform.parent, false);
        skyObject.transform.position = background.transform.position;
        var sky = skyObject.GetComponent<SpriteRenderer>(); sky.sprite = background.sprite;
        sky.sortingLayerID = background.sortingLayerID; sky.sortingOrder = background.sortingOrder - 1;
        string skyPath = Root + "Configs/SkyExtension.mat";
        var skyMaterial = AssetDatabase.LoadAssetAtPath<Material>(skyPath);
        if (skyMaterial == null)
        {
            skyMaterial = new Material(Shader.Find("SkyBlock/GameplaySkyExtension"));
            AssetDatabase.CreateAsset(skyMaterial, skyPath);
        }
        sky.sharedMaterial = skyMaterial;
        Set(Object.FindFirstObjectByType<BackgroundView>(), "extendedSky", sky);
        var environment = Object.FindFirstObjectByType<BlockBlast.Environment.IslandEnvironment>();
        var es = new SerializedObject(environment);
        var environmentPath = Root + "Configs/ReferenceEnvironment.asset";
        var environmentSettings = AssetDatabase.LoadAssetAtPath<BlockBlast.Environment.IslandEnvironmentSettings>(environmentPath);
        if (environmentSettings == null)
        {
            environmentSettings = Object.Instantiate((BlockBlast.Environment.IslandEnvironmentSettings)es.FindProperty("settings").objectReferenceValue);
            AssetDatabase.CreateAsset(environmentSettings, environmentPath);
        }
        environmentSettings.cloudOpacity = .14f; environmentSettings.cloudWidth = .38f;
        environmentSettings.cloudSpeed = .006f;
        environmentSettings.waterAreas = new[] { new Vector4(.02f,.36f,.10f,.18f), new Vector4(.88f,.36f,.10f,.18f), new Vector4(.57f,0,.13f,.13f), new Vector4(.30f,.76f,.22f,.07f) };
        EditorUtility.SetDirty(environmentSettings); Set(environment, "settings", environmentSettings);
        var volume = Object.FindFirstObjectByType<UnityEngine.Rendering.Volume>();
        string profilePath = Root + "Configs/ReferencePostProcessing.asset";
        var profile = AssetDatabase.LoadAssetAtPath<UnityEngine.Rendering.VolumeProfile>(profilePath);
        if (profile == null)
        {
            AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(volume.sharedProfile), profilePath);
            profile = AssetDatabase.LoadAssetAtPath<UnityEngine.Rendering.VolumeProfile>(profilePath);
        }
        foreach (var effect in profile.components)
            if (effect is UnityEngine.Rendering.Universal.Tonemapping || effect is UnityEngine.Rendering.Universal.Vignette)
            { effect.active = false; EditorUtility.SetDirty(effect); }
        volume.sharedProfile = profile;
        var tray = Object.FindFirstObjectByType<TrayController>();
        Box((RectTransform)tray.transform, 135, 1190, 670, 205);
        var ts = new SerializedObject(tray); ts.FindProperty("anchorToSafeBottom").boolValue = false; ts.ApplyModifiedPropertiesWithoutUndo();
        var slots = ts.FindProperty("slots");
        for (int i = 0; i < slots.arraySize; i++)
        {
            var r = (RectTransform)((CanvasGroup)slots.GetArrayElementAtIndex(i).objectReferenceValue).transform;
            Box(r, 54 + i * 187, 28, 187, 150);
        }
        Box((RectTransform)design.Find("BestPlate"), 60, 60, 240, 100);
        Box((RectTransform)design.Find("ScorePlate"), 300, 20, 355, 165);
        Box((RectTransform)design.Find("Settings"), 785, 50, 115, 115);
        Box((RectTransform)design.Find("Hint"), 850, 185, 65, 65);
        var hs = new SerializedObject(Object.FindFirstObjectByType<GameHUD>());
        Text((TextMeshProUGUI)hs.FindProperty("score").objectReferenceValue, 85, 49, 145, 45, 40, true);
        Text((TextMeshProUGUI)hs.FindProperty("highScore").objectReferenceValue, 40, 72, 275, 75, 70, true);
        Text(design.Find("BestPlate/Best").GetComponent<TextMeshProUGUI>(), 90, 8, 135, 36, 34, false);
        Text(design.Find("ScorePlate/Score").GetComponent<TextMeshProUGUI>(), 70, 37, 215, 40, 38, false);
        Box((RectTransform)design.Find("BoardTraySteps"), 355, 1135, 230, 65);
        var dragon = Object.FindFirstObjectByType<BlockBlast.Mascot.DragonMascot>();
        Box((RectTransform)dragon.transform, 5, 1330, 235, 245);
        dragon.GetComponent<Image>().preserveAspect = true;
        Box((RectTransform)design.Find("DragonPerch"), 8, 1515, 250, 100);
        BuildFrame(design);
        EditorUtility.SetDirty(theme);
        EditorSceneManager.MarkSceneDirty(board.gameObject.scene);
        EditorSceneManager.SaveScene(board.gameObject.scene);
        AssetDatabase.SaveAssets();
        return "Reference layout saved";
    }
    static void BuildFrame(Transform parent)
    {
        var previous = parent.Find("ReferenceStoneFrame");
        if (previous != null) Object.DestroyImmediate(previous.gameObject);
        var root = new GameObject("ReferenceStoneFrame", typeof(RectTransform));
        root.transform.SetParent(parent, false);
        Box((RectTransform)root.transform, 75, 380, 790, 765);
        root.transform.SetAsFirstSibling();
        // Separate corners, center crests and straight rails: ornaments never stretch across the board.
        var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(Root + "BoardFrame.png");
        float[] sx = { 0, 65, 120, 200, 255, 320 }, sy = { 0, 68, 110, 128, 210, 256, 275, 327 };
        float[] dx = { 0, 94, 352, 438, 696, 790 }, dy = { 0, 80, 330, 350, 432, 680, 701, 765 };
        if (!AssetDatabase.IsValidFolder(Root + "FrameSlices")) AssetDatabase.CreateFolder(Root.TrimEnd('/'), "FrameSlices");
        for (int y = 0; y < 7; y++) for (int x = 0; x < 5; x++)
        {
            if (y > 0 && y < 6 && x != 0 && x != 4) continue;
            string name = "Segment_" + x + "_" + y;
            string path = Root + "FrameSlices/" + name + ".asset";
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite == null)
            {
                sprite = Sprite.Create(texture, new Rect(sx[x], 327 - sy[y + 1], sx[x + 1] - sx[x], sy[y + 1] - sy[y]), new Vector2(.5f, .5f), 100, 0, SpriteMeshType.FullRect);
                sprite.name = name; AssetDatabase.CreateAsset(sprite, path);
            }
            var part = new GameObject(name, typeof(RectTransform), typeof(Image)); part.transform.SetParent(root.transform, false);
            Box((RectTransform)part.transform, dx[x], dy[y], dx[x + 1] - dx[x], dy[y + 1] - dy[y]);
            var image = part.GetComponent<Image>(); image.sprite = sprite; image.raycastTarget = false;
        }
    }
}

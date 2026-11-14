using UnityEngine;

namespace BlockBlast.UI
{
    [CreateAssetMenu(menuName = "Block Blast/UI Theme")]
    public sealed class UITheme : ScriptableObject
    {
        [Header("Palette")]
        public Color panel = new Color32(22, 35, 58, 255);
        public Color cell = new Color32(42, 60, 95, 255);
        public Color bevelLight = new Color32(61, 84, 128, 255);
        public Color bevelDark = new Color32(30, 44, 72, 255);
        public Color ghostValid = new Color(74/255f, 222/255f, 128/255f, .6f);
        public Color ghostInvalid = new Color(239/255f, 68/255f, 68/255f, .6f);
        public Color overlay = new Color(27/255f, 42/255f, 58/255f, .55f);

        [Header("Layout (reference pixels)")]
        public float cellPixels = 96, gap = 6, padding = 20, frame = 28;
        public float boardMaxWidth = 1024, trayCellPixels = 68, bottomInset = 40;
        public bool scaleFrameArtwork;
        public Vector4 frameArtworkInsets = new Vector4(.11f, .162f, .09f, .104f);
        public Sprite boardCrest;
        public float boardCrestWidthRatio = .13f;
        [Header("Drag")]
        public float dragScale = 1.1f, dragAlpha = .9f, dragOffset = 100;
        [Header("Block interaction")]
        [Min(.01f)] public float dragLiftTime = .1f;
        [Range(0, 8)] public float dragGlowPixels = 3;
        [Range(0, 1)] public float dragGlowIntensity = .55f;
        [Range(0, .2f)] public float placeBounceStrength = .06f;
        [Min(.01f)] public float placementHighlightDuration = .18f;
        [Range(0, 1)] public float placementHighlightIntensity = .45f;
        public bool returnWithOvershoot = false;
        [Header("Animation (seconds)")]
        public float placementTime = .15f, snapTime = .12f, returnTime = .25f;
        public float scoreTime = .35f, flashTime = .08f, clearTime = .2f;
        public float shakePixels = 4, shakeTime = .15f, comboTime = .6f;
        [Header("Game Over")]
        public float gameOverDelay = .5f, gameOverOpenTime = .25f, gameOverCloseTime = .15f, recordFlashTime = 1.2f;
        public Color recordGold = new Color32(255, 213, 74, 255);
        [Header("Hint budget per successful placement")]
        [Min(1)] public int charge = 1;
        public float hintCooldown = 1, hintPulseTime = .3f;

        [Header("Sliced surfaces and effects")]
        public Sprite panelSprite, cellSprite, frameSprite, slotSprite, whiteBlock, squareParticle;
        [Header("Normal / hover / unavailable palette")]
        public Sprite[] blockSprites = new Sprite[8];
        public bool[] blockUsesTint = { false, false, false, false, false, false, true, true };
        public Color TintFor(int index) => blockUsesTint != null && index < blockUsesTint.Length &&
            blockUsesTint[index] ? blockColors[index] : Color.white;
        public Sprite[] highlightedSprites = new Sprite[8], disabledSprites = new Sprite[8];
        public Color[] blockColors = {
            new Color32(59,130,246,255), new Color32(250,204,21,255),
            new Color32(239,68,68,255), new Color32(34,197,94,255),
            new Color32(6,182,212,255), new Color32(168,85,247,255),
            new Color32(249,115,22,255), new Color32(236,72,153,255)
        };

        private void OnValidate()
        {
            gameOverDelay = Mathf.Max(0, gameOverDelay);
            gameOverOpenTime = Mathf.Max(.01f, gameOverOpenTime);
            gameOverCloseTime = Mathf.Max(.01f, gameOverCloseTime);
            recordFlashTime = Mathf.Max(.01f, recordFlashTime);
            cellPixels = Mathf.Max(1, cellPixels);
            gap = Mathf.Max(0, gap);
            trayCellPixels = Mathf.Max(60/.92f, trayCellPixels);
            charge = Mathf.Max(1, charge);
            placementTime = Mathf.Max(.01f, placementTime);
            snapTime = Mathf.Max(.01f, snapTime);
            returnTime = Mathf.Max(.01f, returnTime);
            hintPulseTime = Mathf.Max(.05f, hintPulseTime);
        }
    }
}

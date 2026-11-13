using UnityEngine;

namespace BlockBlast.Effects
{
    [CreateAssetMenu(menuName = "Block Blast/Bubble Pop Config")]
    public sealed class BubblePopConfig : ScriptableObject
    {
        [Header("Assets")]
        public BubblePopEffect effectPrefab;
        public Sprite bubbleSprite;
        public Sprite ringSprite;
        public BlockBlast.UI.UITheme theme;
        public bool useBlockColor=true;public float gravity=.3f;
        public Color bubbleColor = new Color(.65f, .95f, 1f, 1f);
        [Header("Golden line clear")]
        public Color energyColor = new Color(1f, .76f, .16f, 1f);
        [Range(.01f, .1f)] public float outlineWidth = .035f;
        [Min(.05f)] public float sweepDuration = .2f;

        [Header("Timing")]
        [Min(0f)] public float clearPreviewDuration = .08f;
        [Range(0f, 1f)] public float clearHighlightIntensity = .35f;
        [Range(0f, .5f)] public float multiClearIntensity = .18f;
        [Range(.5f, 1.5f)] public float clearEffectScale = 1f;
        [Tooltip("Pop duration after preview and wave delay.")]
        [Min(.05f)] public float duration = .48f;
        [Range(.05f, .9f)] public float inflateFraction = .3f;
        [Min(1f)] public float inflateScale = 1.15f;
        [Min(0f)] public float staggerDelay = .02f;
        [Min(0f)] public float maximumStagger = .16f;
        [Min(.01f)] public float ringDuration = .16f;
        public AnimationCurve inflateCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public AnimationCurve burstCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public AnimationCurve fadeCurve = AnimationCurve.Linear(0, 1, 1, 0);

        [Header("Burst")]
        [Range(1, 12)] public int bubbleCount = 6;
        [Min(.01f)] public float bubbleSize = .18f;
        [Min(0f)] public float travelDistance = .65f;
        [Min(.01f)] public float ringStartSize = .4f;
        [Min(.01f)] public float ringEndSize = 1.3f;
        [Min(1)] public int sortingOrderOffset = 3;

        [Header("Pool")]
        [Min(1)] public int prewarmCount = 64;
        [Min(1)] public int maximumCount = 256;

        public bool HasValidReferences => effectPrefab != null && bubbleSprite != null;

        private void OnValidate()
        {
            maximumCount = Mathf.Max(1, maximumCount);
            prewarmCount = Mathf.Clamp(prewarmCount, 1, maximumCount);
            duration = Mathf.Max(.05f, duration);
            ringDuration = Mathf.Max(.01f, ringDuration);
            clearPreviewDuration = Mathf.Max(0, clearPreviewDuration);
            staggerDelay = Mathf.Max(0, staggerDelay);
            maximumStagger = Mathf.Max(0, maximumStagger);
            if (!HasValidReferences) Debug.LogWarning("BubblePopConfig needs an effect prefab and bubble sprite.", this);
        }
    }
}

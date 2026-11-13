using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using BlockBlast.Mascot;

namespace BlockBlast.Menu
{
    public sealed class MenuPresentation : MonoBehaviour
    {
        public enum EffectQuality { Low, Medium, High }
        public EffectQuality quality = EffectQuality.Medium;
        public RectTransform composition, playArt, dragon, explorer;
        public Animator explorerAnimator;
        public DragonMascot dragonIdle;
        public MenuDragonAnimation dragonAnimation;
        public Button settings;
        public CanvasGroup fade;
        public Image[] clouds, motes, smoke, leaves, mist;
        public Image island;
        Vector3 playScale, dragonScale;
        Vector2 dragonPosition;
        Quaternion dragonRotation;
        float clock;
        bool reacting;
        static readonly int Depart = Animator.StringToHash("Depart");
        static readonly int Idle = Animator.StringToHash("Idle");
        public bool IsReacting => reacting;
        public float AnimationTime => clock;
        void Awake()
        {
            playScale = playArt.localScale; dragonScale = dragon.localScale;
            dragonPosition = dragon.anchoredPosition; dragonRotation = dragon.localRotation;
        }
        void OnEnable() { clock = 0; reacting = false; if (fade != null) fade.alpha = 0; }
        void Update()
        {
            clock += Mathf.Min(Time.unscaledDeltaTime, .1f);
            if (!reacting) playArt.localScale = playScale * (1 + .012f * Mathf.Sin(clock * Mathf.PI * 2 / 3));
            int level = (int)quality;
            for (int i = 0; i < clouds.Length; i++)
            {
                var c = clouds[i]; c.enabled = i < 2 + level;
                float phase = Mathf.Repeat(clock / (90 - i * 16) + i * .29f, 1);
                c.rectTransform.anchoredPosition = new Vector2(Mathf.Lerp(-720, 720, phase), i < 2 ? 560 - i * 230 : -650 + (i - 2) * 110);
                c.color = new Color(1, 1, 1, .48f * Mathf.Sin(phase * Mathf.PI));
            }
            for (int i = 0; i < motes.Length; i++)
            {
                var m = motes[i]; m.enabled = i < 6 + level * 6;
                float phase = Mathf.Repeat(clock / (4.5f + i * .11f) + i * .618f, 1);
                float x = -360 + (i * 137 % 720), y = -360 + (i * 71 % 550);
                if (reacting && i < 8) { x = dragon.anchoredPosition.x + Mathf.Sin(i * 2.4f) * 60; y = dragon.anchoredPosition.y + 90; }
                m.rectTransform.anchoredPosition = new Vector2(x + 18 * Mathf.Sin(clock + i), y + phase * 70);
                m.color = new Color(i % 2 == 0 ? .5f : 1, 1, .8f, Mathf.Sin(phase * Mathf.PI) * .7f);
            }
            for (int i = 0; i < smoke.Length; i++)
            {
                float p = Mathf.Repeat(clock / 7 + i / 3f, 1);
                smoke[i].enabled = level > 0;
                smoke[i].rectTransform.anchoredPosition = new Vector2(289 + p * 25, 310 + p * 90);
                smoke[i].rectTransform.localScale = Vector3.one * (.4f + p);
                smoke[i].color = new Color(1, 1, 1, Mathf.Sin(p * Mathf.PI) * .17f);
            }
            for (int i = 0; i < leaves.Length; i++)
            {
                float p = Mathf.Repeat(clock / (12 + i * 2) + i * .31f, 1);
                leaves[i].enabled = i < level + 1;
                leaves[i].rectTransform.anchoredPosition = new Vector2(Mathf.Lerp(-530, 530, p), -190 - i * 100 + 35 * Mathf.Sin(p * 8));
                leaves[i].rectTransform.localRotation = Quaternion.Euler(0, 0, p * 250);
                leaves[i].color = new Color(1, 1, 1, Mathf.Sin(p * Mathf.PI) * .75f);
            }
            for (int i = 0; mist != null && i < mist.Length; i++)
            {
                float p = Mathf.Repeat(clock / 3.8f + i * .25f, 1);
                mist[i].enabled = i < 2 + level;
                mist[i].rectTransform.anchoredPosition = new Vector2((i-1.5f)*38 + p*12, -710+p*30);
                mist[i].rectTransform.localScale = Vector3.one * (.65f + p*.6f);
                mist[i].color = new Color(.8f, 1, 1, Mathf.Sin(p*Mathf.PI)*.3f);
            }
        }
        // The sheet animator owns artwork; this timeline owns only the flight container.
        public IEnumerator PlayReaction()
        {
            if (reacting) yield break;
            reacting = true; settings.interactable = false;
            dragonIdle.enabled = false;
            if (dragonAnimation != null) dragonAnimation.PlayReaction();
            explorerAnimator.Play(Depart, 0, 0);
            float elapsed = 0;
            while (elapsed < 1.6f)
            {
                elapsed += Time.unscaledDeltaTime;
                float p = Mathf.Clamp01(elapsed / .2f);
                playArt.localScale = playScale * (1 - .08f * Mathf.Sin(p * Mathf.PI));
                float fly = Mathf.SmoothStep(0, 1, Mathf.InverseLerp(.7f, 1.3f, elapsed));
                dragon.anchoredPosition = dragonPosition + new Vector2(-24 * fly, 140 * fly);
                dragon.localRotation = dragonRotation * Quaternion.Euler(0, 0, 5 * Mathf.Sin(fly * Mathf.PI));
                dragon.localScale = dragonScale * (1 + .025f * Mathf.Sin(elapsed * 6));
                fade.alpha = Mathf.InverseLerp(1.3f, 1.6f, elapsed);
                yield return null;
            }
            playArt.localScale = playScale;
        }
        public void Restore()
        {
            reacting = false; settings.interactable = true; fade.alpha = 0;
            playArt.localScale = playScale; dragon.anchoredPosition = dragonPosition;
            dragon.localScale = dragonScale; dragon.localRotation = dragonRotation;
            if (dragonAnimation != null) dragonAnimation.ResetIdle();
            else dragonIdle.enabled = true;
            explorerAnimator.Play(Idle, 0, 0);
        }
        void OnDisable() { StopAllCoroutines(); if (playArt != null) playArt.localScale = playScale; }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
namespace BlockBlast.UI
{
    public sealed class ButtonFeedback : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        Vector3 originalScale;
        bool initialized, controlled;
        Coroutine tween;
        void Awake() => Initialize();
        void Initialize() { if (initialized) return; originalScale = transform.localScale; initialized = true; }
        public void UseControlledFeedback() { Initialize(); controlled = true; }
        public void OnPointerDown(PointerEventData e) { if (!controlled) Scale(.9f); }
        public void OnPointerUp(PointerEventData e) { if (!controlled) Scale(1); }
        public void OnPointerExit(PointerEventData e) { if (!controlled) Scale(1); }
        public void SetExpanded(bool expanded) => Scale(expanded ? 1.1f : 1);
        public void Pulse()
        {
            Initialize(); CancelTween();
            if (isActiveAndEnabled) tween = StartCoroutine(PulseRoutine());
        }
        IEnumerator PulseRoutine()
        {
            yield return Animate(1.1f);
            yield return new WaitForSecondsRealtime(.5f);
            yield return Animate(1);
            tween = null;
        }
        IEnumerator Animate(float factor)
        {
            var from = transform.localScale;
            var target = originalScale * factor;
            yield return UITween.RunRealtime(.1f, t => transform.localScale = Vector3.Lerp(from, target, UITween.Quad(t)));
            transform.localScale = target;
        }
        void Scale(float factor)
        {
            Initialize(); CancelTween();
            if (isActiveAndEnabled) tween = StartCoroutine(Animate(factor));
            else transform.localScale = originalScale;
        }
        void CancelTween() { if (tween != null) StopCoroutine(tween); tween = null; }
        public void ResetScale() { Initialize(); CancelTween(); transform.localScale = originalScale; }
        void OnDisable() => ResetScale();
    }
}

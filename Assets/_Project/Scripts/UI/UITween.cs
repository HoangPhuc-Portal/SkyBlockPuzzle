using System;using System.Collections;using UnityEngine;
namespace BlockBlast.UI { public static class UITween {
 public static float Back(float t){float u=t-1;return 1+2.70158f*u*u*u+1.70158f*u*u;}
 public static float Quad(float t)=>1-(1-t)*(1-t);
 // Wall-clock tween for modal transitions: no extra completion frame at timeScale zero.
 public static IEnumerator RunRealtime(float seconds,Action<float> frame,Action done=null){
 double start=Time.realtimeSinceStartupAsDouble;
 frame(0);
 while(Time.realtimeSinceStartupAsDouble-start<seconds){
 yield return null;
 frame(Mathf.Clamp01((float)((Time.realtimeSinceStartupAsDouble-start)/Math.Max(.001f,seconds))));
 }
 frame(1);done?.Invoke();
 }
 public static IEnumerator Run(float seconds,Action<float> frame,Action done=null){float t=0;while(t<seconds){t+=Time.unscaledDeltaTime;frame(Mathf.Clamp01(t/Mathf.Max(.001f,seconds)));yield return null;}frame(1);done?.Invoke();}
}}

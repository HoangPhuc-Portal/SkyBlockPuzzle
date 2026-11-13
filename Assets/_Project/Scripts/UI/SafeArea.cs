using UnityEngine;
namespace BlockBlast.UI {
[RequireComponent(typeof(RectTransform))] public sealed class SafeArea:MonoBehaviour {
 RectTransform rect; Rect last; Vector2Int screen;
 void Awake(){rect=(RectTransform)transform;}
 void OnEnable(){Canvas.willRenderCanvases+=Refresh;Refresh();}
 void OnDisable(){Canvas.willRenderCanvases-=Refresh;}
 void Refresh(){if(rect==null)rect=(RectTransform)transform;if(Screen.width<=0||Screen.height<=0)return;
 var area=Screen.safeArea;var size=new Vector2Int(Screen.width,Screen.height);if(last==area&&screen==size)return;last=area;screen=size;
 rect.anchorMin=new Vector2(area.xMin/size.x,area.yMin/size.y);rect.anchorMax=new Vector2(area.xMax/size.x,area.yMax/size.y);rect.offsetMin=rect.offsetMax=Vector2.zero;}
}}

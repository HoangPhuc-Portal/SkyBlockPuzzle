using UnityEngine;
namespace BlockBlast.Menu
{
    // Pixel positions remain in the 941x1672 reference coordinate system at every aspect ratio.
    [ExecuteAlways]
    public sealed class MenuDesignScale : MonoBehaviour
    {
        RectTransform rect, parent;
        void OnEnable() { rect=(RectTransform)transform;parent=(RectTransform)transform.parent;Canvas.willRenderCanvases+=Refresh;Refresh(); }
        void OnDisable() => Canvas.willRenderCanvases-=Refresh;
        void Refresh() { if(parent==null)return;float scale=parent.rect.width/941f;if(scale>0&&!Mathf.Approximately(rect.localScale.x,scale))rect.localScale=Vector3.one*scale; }
    }
}

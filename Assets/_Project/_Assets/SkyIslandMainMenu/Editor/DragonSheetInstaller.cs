using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using BlockBlast.Menu;

public static class DragonSheetInstaller
{
    const string Root="Assets/_Project/_Assets/SkyIslandMainMenu/";
    static Sprite[] Frames(string name) => AssetDatabase.LoadAllAssetsAtPath(Root+name+".png").OfType<Sprite>().OrderByDescending(s=>s.rect.y).ThenBy(s=>s.rect.x).ToArray();
    static AnimationClip Clip(string name,Sprite[] frames,float duration,bool loop,float size,float y)
    {
        string path=Root+"Animations/Dragon"+name+".anim";
        var clip=AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
        bool create=clip==null;if(create)clip=new AnimationClip{name=name,frameRate=30};
        var keys=new ObjectReferenceKeyframe[frames.Length+1];
        for(int i=0;i<frames.Length;i++)keys[i]=new ObjectReferenceKeyframe{time=i*duration/frames.Length,value=frames[i]};
        keys[frames.Length]=new ObjectReferenceKeyframe{time=duration,value=frames[frames.Length-1]};
        AnimationUtility.SetObjectReferenceCurve(clip,new EditorCurveBinding{path="",type=typeof(Image),propertyName="m_Sprite"},keys);
        clip.SetCurve("",typeof(RectTransform),"m_SizeDelta.x",AnimationCurve.Constant(0,duration,size));
        clip.SetCurve("",typeof(RectTransform),"m_SizeDelta.y",AnimationCurve.Constant(0,duration,size));
        clip.SetCurve("",typeof(RectTransform),"m_AnchoredPosition.y",AnimationCurve.Constant(0,duration,y));
        var settings=AnimationUtility.GetAnimationClipSettings(clip);settings.loopTime=loop;AnimationUtility.SetAnimationClipSettings(clip,settings);
        if(create)AssetDatabase.CreateAsset(clip,path);else EditorUtility.SetDirty(clip);return clip;
    }
    public static string Install()
    {
        if(EditorApplication.isPlaying)throw new Exception("Edit Mode required");
        var p=UnityEngine.Object.FindFirstObjectByType<MenuPresentation>();
        if(p==null||p.dragonAnimation!=null)throw new Exception("Missing menu or already installed");
        var idle=Frames("Dragon_Idle");var blink=Frames("Dragon_Blink");var play=Frames("Dragon_Play_Reaction");
        if(idle.Length!=32||blink.Length!=8||play.Length!=32)throw new Exception("Unexpected frame counts");
        var ordered=new[]{blink[0],blink[1],blink[2],blink[3],blink[7],blink[6],blink[5],blink[4]};
        var path=Root+"Animations/MenuDragon.controller";
        var controller=AssetDatabase.LoadAssetAtPath<AnimatorController>(path)??AnimatorController.CreateAnimatorControllerAtPath(path);
        var clips=new[]{Clip("Idle",idle,4,true,300,0),Clip("Blink",ordered,.28f,false,280,-5),Clip("PlayReaction",play,1.6f,false,450,-12)};
        var names=new[]{"Idle","Blink","PlayReaction"};
        for(int i=0;i<3;i++){var state=controller.layers[0].stateMachine.states.FirstOrDefault(s=>s.state.motion==clips[i]).state;if(state==null)state=controller.AddMotion(clips[i]);state.name=names[i];}
        var states=controller.layers[0].stateMachine.states;
        var exit=states.First(s=>s.state.name=="Blink").state.AddTransition(states.First(s=>s.state.name=="Idle").state);
        exit.hasExitTime=true;exit.exitTime=1;exit.duration=0;exit.hasFixedDuration=true;
        var go=new GameObject("DragonSheetArtwork",typeof(RectTransform),typeof(CanvasRenderer),typeof(Image),typeof(Animator),typeof(MenuDragonAnimation));
        go.transform.SetParent(p.dragon,false);var rect=(RectTransform)go.transform;rect.sizeDelta=new Vector2(300,300);
        var image=go.GetComponent<Image>();image.sprite=idle[0];image.preserveAspect=true;image.raycastTarget=false;
        var animator=go.GetComponent<Animator>();animator.runtimeAnimatorController=controller;animator.updateMode=AnimatorUpdateMode.UnscaledTime;animator.cullingMode=AnimatorCullingMode.AlwaysAnimate;
        p.dragonIdle.enabled=false;p.dragon.GetComponent<Image>().enabled=false;
        p.dragonAnimation=go.GetComponent<MenuDragonAnimation>();
        var button=p.dragon.GetComponent<Button>();if(button!=null){button.targetGraphic=p.dragon.GetComponent<Image>();button.enabled=false;}
        EditorSceneManager.MarkSceneDirty(p.gameObject.scene);EditorSceneManager.SaveScene(p.gameObject.scene);AssetDatabase.SaveAssets();
        return "Installed 32 Idle / 8 Blink / 32 Play frames; three states only";
    }
}

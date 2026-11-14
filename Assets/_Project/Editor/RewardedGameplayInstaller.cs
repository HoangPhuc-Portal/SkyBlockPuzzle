using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using BlockBlast.UI;
public static class RewardedGameplayInstaller
{
 static void Set(Object o,string k,Object v){var s=new SerializedObject(o);s.FindProperty(k).objectReferenceValue=v;s.ApplyModifiedPropertiesWithoutUndo();}
 static T Get<T>(Object o,string k) where T:Object{return new SerializedObject(o).FindProperty(k).objectReferenceValue as T;}
 static RectTransform Node(string n,Transform p,Vector2 a,Vector2 b){var r=new GameObject(n,typeof(RectTransform)).GetComponent<RectTransform>();r.SetParent(p,false);r.anchorMin=a;r.anchorMax=b;r.offsetMin=r.offsetMax=Vector2.zero;return r;}
 static TMP_Text Text(string n,Transform p,Vector2 a,Vector2 b,string value,TMP_FontAsset font,float size){var t=Node(n,p,a,b).gameObject.AddComponent<TextMeshProUGUI>();t.font=font;t.text=value;t.fontSize=size;t.enableAutoSizing=true;t.fontSizeMin=size*.65f;t.fontSizeMax=size;t.alignment=TextAlignmentOptions.Center;t.color=Color.white;t.raycastTarget=false;return t;}
 static Button Button(string n,Transform p,Vector2 a,Vector2 b,string label,UITheme theme,TMP_FontAsset font){var r=Node(n,p,a,b);var i=r.gameObject.AddComponent<Image>();i.sprite=theme.panelSprite;i.type=Image.Type.Sliced;i.color=new Color(.12f,.6f,.38f);var bt=r.gameObject.AddComponent<Button>();bt.targetGraphic=i;r.gameObject.AddComponent<ButtonFeedback>();Text("Label",r,Vector2.zero,Vector2.one,label,font,36);return bt;}
 public static void Install(GameObject root){
 if(root.GetComponent<RewardedGameplay>()!=null)throw new System.InvalidOperationException("Already installed");
 var game=root.GetComponentInChildren<GameOverController>(true);var settings=root.GetComponentInChildren<SettingsPanel>(true);var theme=Get<UITheme>(game,"theme");var font=Get<TextMeshProUGUI>(game,"finalScore").font;
 var reward=root.AddComponent<RewardedGameplay>();var provider=root.AddComponent<AdMobRewardedProvider>();
 var gp=Get<RectTransform>(game,"panel");var replay=Get<Button>(game,"replayButton");var rr=(RectTransform)replay.transform;rr.anchorMin=new Vector2(.08f,.035f);rr.anchorMax=new Vector2(.92f,.15f);
 var old=Get<Button>(game,"viewBoardButton");if(old!=null)old.gameObject.SetActive(false);
 var cont=Button("ContinueRewarded",gp,new Vector2(.08f,.18f),new Vector2(.92f,.30f),"TIẾP TỤC (XEM QUẢNG CÁO)",theme,font);
 var reviveStatus=Text("RewardStatus",gp,new Vector2(.06f,.30f),new Vector2(.94f,.345f),"",font,24);
 Set(reward,"provider",provider);Set(reward,"gameOver",game);Set(reward,"settings",settings);Set(settings,"rewards",reward);Set(reward,"continueButton",cont);Set(reward,"reviveStatus",reviveStatus);Set(game,"rewards",reward);
 }
}


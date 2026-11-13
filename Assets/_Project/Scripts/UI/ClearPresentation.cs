using System.Collections;using System.Collections.Generic;using UnityEngine;using TMPro;using BlockBlast.Grid;using BlockBlast.Core;
namespace BlockBlast.UI {
public sealed class ClearPresentation:MonoBehaviour {
 [SerializeField]UITheme theme;[SerializeField]BlockBlast.Effects.BubblePopConfig clearConfig;[SerializeField]GridBoard board;[SerializeField]TextMeshProUGUI combo;
 SpriteRenderer[] lines;Vector3[] lineStarts,lineEnds;int activeLines;Coroutine shake,pop,glow;Camera camera;Vector3 cameraRest;
 void Awake(){camera=Camera.main;int size=board.Model!=null?board.Model.Size:GridBoard.GRID_SIZE;lines=new SpriteRenderer[size*2];lineStarts=new Vector3[lines.Length];lineEnds=new Vector3[lines.Length];for(int i=0;i<lines.Length;i++){var go=new GameObject("ClearGlow"+i);go.transform.SetParent(transform);lines[i]=go.AddComponent<SpriteRenderer>();lines[i].sprite=theme.squareParticle;lines[i].sortingOrder=30;lines[i].enabled=false;}combo.gameObject.SetActive(false);}
 void OnEnable(){GameEvents.OnCellsCleared+=Clear;GameEvents.OnShapePlaced+=ResetEffects;GameEvents.OnGameOver+=ResetEffects;GameEvents.OnBoardReset+=BoardReset;}
 void BoardReset(GridBoard source){if(source==board)ResetEffects();}
 void OnDisable(){GameEvents.OnCellsCleared-=Clear;GameEvents.OnShapePlaced-=ResetEffects;GameEvents.OnGameOver-=ResetEffects;GameEvents.OnBoardReset-=BoardReset;ResetEffects();}
 public void ResetEffects(){StopAllCoroutines();if(camera!=null&&shake!=null)camera.transform.position=cameraRest;shake=pop=glow=null;activeLines=0;if(lines!=null)foreach(var line in lines)line.enabled=false;if(combo!=null)combo.gameObject.SetActive(false);}
 void Clear(GridBoard source,IReadOnlyList<ClearedCellInfo> cells){if(source!=board)return;int size=board.Model.Size;int[] rows=new int[size],cols=new int[size];var unique=new HashSet<Vector2Int>();foreach(var cell in cells){if(unique.Add(new Vector2Int(cell.Row,cell.Col))){rows[cell.Row]++;cols[cell.Col]++;}}int count=0;for(int i=0;i<size;i++){if(rows[i]==size){Line(count++,board.CellPosition(i,0),board.CellPosition(i,size-1));}if(cols[i]==size){Line(count++,board.CellPosition(0,i),board.CellPosition(size-1,i));}}
 if(glow!=null)StopCoroutine(glow);activeLines=count;glow=StartCoroutine(Sweep(count));
 if(shake!=null){StopCoroutine(shake);camera.transform.position=cameraRest;}cameraRest=camera.transform.position;float pixels=Vector2.Distance(camera.WorldToScreenPoint(board.transform.position),camera.WorldToScreenPoint(board.transform.position+Vector3.right));float amp=theme.shakePixels/Mathf.Max(1,pixels)*Mathf.Min(2,1+.15f*(count-1));shake=StartCoroutine(UITween.Run(theme.shakeTime,t=>camera.transform.position=cameraRest+new Vector3(Mathf.Sin(t*40),Mathf.Cos(t*35),0)*amp*(1-t),()=>{camera.transform.position=cameraRest;shake=null;}));
 var scoreModel=BlockBlast.Managers.GameManager.Instance;
 if(scoreModel!=null){if(pop!=null)StopCoroutine(pop);combo.text=(scoreModel.LastPerfectClear?"PERFECT CLEAR! +300\n":"")+(count==2?"DOUBLE CLEAR!\n":count==3?"TRIPLE CLEAR!\n":count>3?"AMAZING!\n":"")+(scoreModel.CurrentCombo>1?"COMBO x"+scoreModel.CurrentCombo+"\n":"")+"+"+scoreModel.LastScoreGain;combo.gameObject.SetActive(true);pop=StartCoroutine(UITween.Run(theme.comboTime,t=>{float s=t<.35f?Mathf.Lerp(.5f,1.2f,UITween.Quad(t/.35f)):Mathf.Lerp(1.2f,1,(t-.35f)/.65f);combo.transform.localScale=Vector3.one*s;combo.alpha=t<.6f?1:1-(t-.6f)/.4f;},()=>{combo.gameObject.SetActive(false);pop=null;}));}
 Haptics.Pulse();
 }
 // Reuse the existing line renderers as moving energy streaks.
 IEnumerator Sweep(int count){
 float preview=clearConfig!=null?clearConfig.clearPreviewDuration:.08f;
 float duration=clearConfig!=null?clearConfig.sweepDuration:.2f;
 float elapsed=0;
 while(elapsed<preview+duration){
 elapsed+=Time.deltaTime;float t=Mathf.Clamp01((elapsed-preview)/duration);
 for(int i=0;i<count;i++){
 var line=lines[i];line.enabled=elapsed>=preview;
 line.transform.position=Vector3.Lerp(lineStarts[i],lineEnds[i],t);
 Color color=clearConfig!=null?clearConfig.energyColor:new Color(1,.76f,.16f,1);
 color.a*=Mathf.Sin(t*Mathf.PI)*Mathf.Min(.9f,.7f+.05f*count);line.color=color;
 }
 yield return null;
 }
 foreach(var line in lines)line.enabled=false;activeLines=0;glow=null;
 }
 void Line(int index,Vector3 start,Vector3 end){
 var line=lines[index];line.enabled=false;lineStarts[index]=start;lineEnds[index]=end;
 line.transform.position=start;Vector3 direction=(end-start).normalized;
 line.transform.rotation=Quaternion.Euler(0,0,Mathf.Atan2(direction.y,direction.x)*Mathf.Rad2Deg);
 var size=theme.squareParticle.bounds.size;
 line.transform.localScale=new Vector3(board.CellSize*1.35f/size.x,board.CellSize*.12f/size.y,1);
 }
}}

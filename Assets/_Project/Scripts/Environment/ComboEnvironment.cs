using UnityEngine;
using System.Collections.Generic;
using BlockBlast.Core;
using BlockBlast.Grid;
using BlockBlast.Managers;
namespace BlockBlast.Environment
{
    /// <summary>Presentation only: consumes authoritative combo events; never writes gameplay state.</summary>
    public sealed class ComboEnvironment : MonoBehaviour
    {
        public ComboEnvironmentSettings settings;
        public Renderer boardFrame;
        public Camera viewCamera;
        public Sprite glowSprite;
        public Material energyMaterial;
        SpriteRenderer[] crystals, halos;
        ParticleSystem particles;
        float emission, ambient, rate, startEmission, startAmbient, startRate, targetEmission, targetAmbient, targetRate, progress=1, spawn;
        int emitted;
        bool initialized;
        MaterialPropertyBlock tint;
        static readonly int TintId=Shader.PropertyToID("_EnergyTint");
        readonly Dictionary<SpriteRenderer,Color> appliedTint=new Dictionary<SpriteRenderer,Color>();
        public int Level {get;private set;}
        public float Intensity => emission;
        public int ParticleCount => particles==null?0:particles.particleCount;
        public float CurrentRate => rate;
        void Awake()
        {
            tint=new MaterialPropertyBlock();
            if(settings==null||boardFrame==null||viewCamera==null||glowSprite==null||energyMaterial==null){Debug.LogError("ComboEnvironment references missing",this);enabled=false;return;}
            crystals=new SpriteRenderer[settings.crystalAnchors.Length];halos=new SpriteRenderer[crystals.Length];
            for(int i=0;i<crystals.Length;i++){crystals[i]=Make("CrystalEnergy_"+i);halos[i]=Make("CrystalHalo_"+i);}
            var go=new GameObject("ComboSparks");go.transform.SetParent(transform,false);particles=go.AddComponent<ParticleSystem>();
            particles.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
            var main=particles.main;main.playOnAwake=false;main.loop=true;main.simulationSpace=ParticleSystemSimulationSpace.World;main.maxParticles=Mathf.Clamp(settings.maximumParticleCount,0,64);main.startSpeed=0;main.startLifetime=1.4f;
            var auto=particles.emission;auto.enabled=false;var shape=particles.shape;shape.enabled=false;
            var color=particles.colorOverLifetime;color.enabled=true;
            var gradient=new Gradient();gradient.SetKeys(new[]{new GradientColorKey(Color.white,0),new GradientColorKey(Color.white,1)},new[]{new GradientAlphaKey(0,0),new GradientAlphaKey(1,.15f),new GradientAlphaKey(0,1)});color.color=gradient;
            var renderer=go.GetComponent<ParticleSystemRenderer>();renderer.sharedMaterial=energyMaterial;renderer.sortingOrder=36;
            initialized=true;particles.Play();
        }
        SpriteRenderer Make(string label){var go=new GameObject(label);go.transform.SetParent(transform,false);var r=go.AddComponent<SpriteRenderer>();r.sprite=glowSprite;r.sharedMaterial=energyMaterial;r.sortingOrder=35;r.enabled=false;return r;}
        void OnEnable(){GameEvents.OnComboChanged+=Combo;GameEvents.OnGameOver+=GameOver;GameEvents.OnBoardReset+=ResetBoard;if(initialized){if(tint==null)tint=new MaterialPropertyBlock();Sync();}}
        void Start(){Sync();}
        void Sync(){if(GameManager.Instance!=null&&!GameManager.Instance.IsGameOver)Combo(GameManager.Instance.CurrentCombo);else Combo(0);}
        void OnDisable()
        {
            GameEvents.OnComboChanged-=Combo;GameEvents.OnGameOver-=GameOver;GameEvents.OnBoardReset-=ResetBoard;
            if(initialized)Clear();
        }
        void ResetBoard(GridBoard board){Clear();}
        void GameOver(){Combo(0);}
        void Clear(){emission=ambient=rate=targetEmission=targetAmbient=targetRate=spawn=0;progress=1;Level=0;if(particles!=null)particles.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);if(crystals!=null)foreach(var r in crystals)r.enabled=false;if(halos!=null)foreach(var r in halos)r.enabled=false;}
        void Combo(int combo)
        {
            if(!initialized)return;
            Level=settings.effectsEnabled?settings.Level(combo):0;
            float quality=QualitySettings.GetQualityLevel()==0?settings.lowQualityMultiplier:1;
            startEmission=emission;startAmbient=ambient;startRate=rate;progress=0;
            targetEmission=settings.At(settings.crystalEmissionIntensity,Level)*quality;
            targetAmbient=settings.At(settings.ambientEffectIntensity,Level)*quality;
            targetRate=settings.At(settings.particleEmissionRate,Level)*quality;
            if(!particles.isPlaying)particles.Play();
        }
        void LateUpdate()
        {
            if(!initialized||boardFrame==null||viewCamera==null)return;
            if(!settings.effectsEnabled){Clear();return;}
            bool paused=Time.timeScale==0||(GameManager.Instance!=null&&GameManager.Instance.RewardModalOpen);
            if(paused){if(particles.isPlaying)particles.Pause();return;}
            if(particles.isPaused)particles.Play();
            float dt=Time.deltaTime;progress=Mathf.Min(1,progress+dt/Mathf.Max(.05f,settings.transitionDuration));float t=Mathf.SmoothStep(0,1,progress);
            emission=Mathf.Lerp(startEmission,targetEmission,t);ambient=Mathf.Lerp(startAmbient,targetAmbient,t);rate=Mathf.Lerp(startRate,targetRate,t);
            var bounds=boardFrame.bounds;Vector2 min=Vector2.one*float.MaxValue,max=Vector2.one*float.MinValue;
            for(int i=0;i<8;i++){var p=viewCamera.WorldToViewportPoint(bounds.center+Vector3.Scale(bounds.extents,new Vector3((i&1)==0?-1:1,(i&2)==0?-1:1,(i&4)==0?-1:1)));min=Vector2.Min(min,p);max=Vector2.Max(max,p);}
            float depth=viewCamera.nearClipPlane+1;
            float span=Vector3.Distance(viewCamera.ViewportToWorldPoint(new Vector3(min.x,min.y,depth)),viewCamera.ViewportToWorldPoint(new Vector3(max.x,min.y,depth)));
            for(int i=0;i<crystals.Length;i++)
            {
                var anchor=settings.crystalAnchors[i];var p=viewCamera.ViewportToWorldPoint(new Vector3(Mathf.Lerp(min.x,max.x,anchor.x),Mathf.Lerp(min.y,max.y,anchor.y),depth));
                Draw(crystals[i],p,span*.13f,emission);Draw(halos[i],p,span*.22f,ambient);
            }
            spawn+=dt*rate;
            while(spawn>=1)
            {
                spawn--;float phase=Mathf.Repeat(emitted*.618034f,1);float x=(emitted++%2==0?.055f:.945f);
                var p=viewCamera.ViewportToWorldPoint(new Vector3(Mathf.Lerp(min.x,max.x,x),Mathf.Lerp(min.y,max.y,.2f+phase*.58f),depth));
                var e=new ParticleSystem.EmitParams{position=p,velocity=viewCamera.transform.up*(span*.025f),startLifetime=1.4f,startSize=span*.022f,startColor=settings.energyColor};particles.Emit(e,1);
            }
        }
        void Draw(SpriteRenderer r,Vector3 position,float width,float alpha){r.enabled=alpha>.001f;if(!r.enabled)return;r.transform.position=position;r.transform.rotation=viewCamera.transform.rotation;r.transform.localScale=Vector3.one*(width/glowSprite.bounds.size.x);var c=settings.energyColor;c.a=alpha;if(!appliedTint.TryGetValue(r,out var last)||last!=c){tint.SetColor(TintId,c);r.SetPropertyBlock(tint);appliedTint[r]=c;}}
    }
}

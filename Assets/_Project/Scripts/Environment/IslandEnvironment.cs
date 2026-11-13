using UnityEngine;
using BlockBlast.Managers;
namespace BlockBlast.Environment
{
    /// <summary>Clouds behind gameplay; leaves in edge corridors above decorative frames. Fixed pool, no input.</summary>
    public sealed class IslandEnvironment : MonoBehaviour
    {
        [SerializeField] private IslandEnvironmentSettings settings;
        [SerializeField] private SpriteRenderer background;
        [SerializeField] private Camera viewCamera;
        [SerializeField] private bool gameplay;
        private SpriteRenderer[] clouds, leaves;
        private float[] ages, lifetimes, phases;
        private bool[] live;
        private Material originalMaterial;
        private MaterialPropertyBlock block, originalBlock;
        private System.Random random;
        private float elapsed, spawnClock, density;
        private Vector3 lower, upper;
        private bool initialized;
        private static readonly int TimeId=Shader.PropertyToID("_EnvironmentTime"), StrengthId=Shader.PropertyToID("_WaterStrength");
        private static readonly int[] AreaIds={Shader.PropertyToID("_WaterArea0"),Shader.PropertyToID("_WaterArea1"),Shader.PropertyToID("_WaterArea2"),Shader.PropertyToID("_WaterArea3")};
        public float AnimationTime => elapsed;
        public int PoolSize => leaves == null ? 0 : leaves.Length;
        public int ActiveLeaves { get { int n=0;if(live!=null)foreach(bool b in live)if(b)n++;return n; } }
        private void Awake()
        {
            if(settings==null||background==null||viewCamera==null){Debug.LogError("IslandEnvironment references missing",this);enabled=false;return;}
            random=new System.Random(1729);block=new MaterialPropertyBlock();originalBlock=new MaterialPropertyBlock();
            background.GetPropertyBlock(originalBlock);originalMaterial=background.sharedMaterial;
            density=settings.particleDensity*(settings.reduceOnLowQuality&&QualitySettings.GetQualityLevel()==0?.6f:1);
            clouds=new SpriteRenderer[4];for(int i=0;i<clouds.Length;i++)clouds[i]=Make("Cloud_"+i,settings.cloudSprite);
            leaves=new SpriteRenderer[Mathf.Clamp(settings.maximumActiveLeaves,0,12)];
            ages=new float[leaves.Length];lifetimes=new float[leaves.Length];phases=new float[leaves.Length];live=new bool[leaves.Length];
            for(int i=0;i<leaves.Length;i++)
            {
                leaves[i]=Make("Leaf_"+i,settings.leafSprite);
                // The stone/tray frame otherwise occludes the narrow edge corridors.
                // Screen-space popup UI stays above these world sprites.
                leaves[i].sortingOrder=background.sortingOrder+25;
                leaves[i].enabled=false;
            }
            // Seed different ages so decoration is visible on the very first rendered frame.
            int initial=Mathf.Min(leaves.Length,Mathf.CeilToInt(settings.initialLeaves*density));
            for(int i=0;i<initial;i++){Spawn();ages[i]=lifetimes[i]*Mathf.Lerp(.18f,.78f,initial>1?(float)i/(initial-1):.5f);}
            initialized=true;ApplyWater();Layout();Draw();
        }
        private SpriteRenderer Make(string name,Sprite sprite)
        {
            var go=new GameObject(name);go.transform.SetParent(transform,false);var r=go.AddComponent<SpriteRenderer>();r.sprite=sprite;
            r.sortingLayerID=background.sortingLayerID;r.sortingOrder=background.sortingOrder+1;return r;
        }
        private void OnEnable()
        {
            if(!initialized)return;
            // Unity hot reload restores serialized state, but not native property blocks or System.Random.
            if(block==null)block=new MaterialPropertyBlock();
            if(originalBlock==null){originalBlock=new MaterialPropertyBlock();background.GetPropertyBlock(originalBlock);}
            if(random==null)random=new System.Random(1729);
            ApplyWater();Draw();
        }
        private void ApplyWater(){if(settings.waterMaterial!=null)background.sharedMaterial=settings.waterMaterial;}
        private void OnDisable()
        {
            if(!initialized)return;
            foreach(var r in clouds)if(r!=null)r.enabled=false;
            foreach(var r in leaves)if(r!=null)r.enabled=false;
            if(background!=null){background.sharedMaterial=originalMaterial;background.SetPropertyBlock(originalBlock);}
        }
        private Vector3 Point(float x,float y)
        {
            var plane=new Plane(Vector3.forward,new Vector3(0,0,background.transform.position.z));
            var ray=viewCamera.ViewportPointToRay(new Vector3(x,y,0));return plane.Raycast(ray,out float distance)?ray.GetPoint(distance):background.transform.position;
        }
        private void Layout(){lower=Point(0,0);upper=Point(1,1);}
        private void LateUpdate()
        {
            if(!initialized)return;
            Layout();float dt=Time.deltaTime;
            if(settings.pauseWithGameplayModal&&gameplay&&GameManager.Instance!=null&&(GameManager.Instance.RewardModalOpen||GameManager.Instance.IsGameOver))dt=0;
            elapsed+=dt;
            spawnClock+=dt*settings.leafSpawnRate*density;
            if(spawnClock>=1){spawnClock%=1;Spawn();}
            for(int i=0;i<leaves.Length;i++)if(live[i]){ages[i]+=dt;if(ages[i]>=lifetimes[i])live[i]=false;}
            Draw();
        }
        private void Spawn()
        {
            for(int i=0;i<leaves.Length;i++)if(!live[i])
            {live[i]=true;ages[i]=0;lifetimes[i]=(.45f+(float)random.NextDouble()*.2f)/Mathf.Max(.01f,settings.leafMovementSpeed);phases[i]=(float)random.NextDouble()*6.28f;return;}
        }
        private void Place(SpriteRenderer r,float x,float y,float width,bool foreground=false)
        {
            if(foreground)
            {
                // Keep leaves ahead of opaque frame geometry as well as sprite sorting.
                float depth=viewCamera.nearClipPlane+1f;
                r.transform.position=viewCamera.ViewportToWorldPoint(new Vector3(x,y,depth));
                float span=Vector3.Distance(viewCamera.ViewportToWorldPoint(new Vector3(0,.5f,depth)),viewCamera.ViewportToWorldPoint(new Vector3(1,.5f,depth)));
                if(r.sprite!=null)r.transform.localScale=Vector3.one*(span*width/r.sprite.bounds.size.x);
                return;
            }
            r.transform.position=lower+new Vector3((upper.x-lower.x)*x,(upper.y-lower.y)*y,-.01f);
            if(r.sprite!=null)r.transform.localScale=Vector3.one*((upper.x-lower.x)*width/r.sprite.bounds.size.x);
        }
        private void Draw()
        {
            for(int i=0;i<clouds.Length;i++)
            {
                float range=settings.cloudMovementRange.x;
                float start=i==0?.2f:i==1?.75f:i==2?.7f:.25f;
                float x=Mathf.Repeat(start+range+elapsed*settings.cloudSpeed*(.7f+i*.17f),1+2*range)-range;
                float y=(gameplay?(i%2==0?.955f:.045f):(.74f+i*.065f))+Mathf.Sin(elapsed*.11f+i)*settings.cloudMovementRange.y;
                Place(clouds[i],x,y,settings.cloudWidth*(1+i*.06f));
                clouds[i].color=new Color(1,1,1,settings.cloudOpacity);clouds[i].enabled=i<Mathf.CeilToInt(clouds.Length*density)&&settings.cloudSprite!=null;
            }
            for(int i=0;i<leaves.Length;i++)
            {
                var r=leaves[i];r.enabled=live[i];if(!live[i])continue;
                float t=ages[i]/lifetimes[i],p=phases[i];
                // Narrow edge corridors avoid the board, score and controls even with transparent frames.
                float x=(i%2==0?.045f:.955f)+Mathf.Sin(t*6.28f+p)*.012f;
                float y=Mathf.Lerp(gameplay?.76f:.91f,gameplay?.13f:.08f,t);
                Place(r,x,y,settings.leafWidth,true);r.transform.rotation=viewCamera.transform.rotation*Quaternion.Euler(0,0,Mathf.Sin(t*5+p)*32+t*70);
                r.color=new Color(1,1,1,Mathf.SmoothStep(0,1,t*6)*Mathf.SmoothStep(0,1,(1-t)*6));
            }
            background.GetPropertyBlock(block);block.SetFloat(TimeId,elapsed*settings.waterAnimationSpeed);block.SetFloat(StrengthId,settings.waterStrength);
            for(int i=0;i<4;i++)block.SetVector(AreaIds[i],settings.waterAreas!=null&&i<settings.waterAreas.Length?settings.waterAreas[i]:Vector4.zero);
            background.SetPropertyBlock(block);
        }
    }
}

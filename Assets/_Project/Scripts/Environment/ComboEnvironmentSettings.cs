using UnityEngine;
namespace BlockBlast.Environment
{
    [CreateAssetMenu(menuName="Block Blast/Combo Environment Settings")]
    public sealed class ComboEnvironmentSettings : ScriptableObject
    {
        public int[] comboThresholds = {1,3,6};
        public float[] crystalEmissionIntensity = {0,.25f,.5f,.85f};
        public float[] particleEmissionRate = {0,3,7,12};
        public float[] ambientEffectIntensity = {0,.08f,.16f,.25f};
        [Min(.05f)] public float transitionDuration=.65f;
        [Range(0,64)] public int maximumParticleCount=24;
        [Range(0,1)] public float lowQualityMultiplier=.5f;
        public bool effectsEnabled=true;
        public Color energyColor=new Color(.25f,.8f,1,1);
        public Vector2[] crystalAnchors={new Vector2(.08f,.9f),new Vector2(.5f,.94f),new Vector2(.92f,.9f),new Vector2(.08f,.08f),new Vector2(.92f,.08f)};
        public int Level(int combo) { int n=0; if(comboThresholds!=null) foreach(int threshold in comboThresholds) if(combo>=threshold)n++; return Mathf.Min(3,n); }
        public float At(float[] values,int level) => values==null||values.Length==0?0:Mathf.Max(0,values[Mathf.Clamp(level,0,values.Length-1)]);
    }
}

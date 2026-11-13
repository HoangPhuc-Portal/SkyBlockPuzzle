using System;
using UnityEngine;
namespace BlockBlast.Difficulty
{
    public enum ViabilityPolicy { CompleteHand, PreferThreeMinimumTwo, PreferThreeInitialMove }
    [Serializable] public class DifficultyBand
    {
        public string name;
        [Range(0,1)] public float index;
        public Vector3 weights;
        public ViabilityPolicy viability=ViabilityPolicy.CompleteHand;
        public DifficultyBand(string n,float d,Vector3 w){name=n;index=d;weights=w;}
    }
    [CreateAssetMenu(menuName="Block Blast/Difficulty Configuration")]
    public sealed class DifficultyConfig : ScriptableObject
    {
        public bool enabledDda=true;
        [Header("Score progression: Developing, Skilled, Near Record, Advanced")]
        public Vector4 scoreRatios=new Vector4(.45f,.75f,.93f,1.10f);
        [Tooltip("First-run endpoints. Typical placement=1-9, one-line clear=10*combo, perfect clear adds 300.")]
        public float firstRunScore=600, firstRunPlacements=45, firstRunLines=12;
        public Vector3 firstRunWeights=new Vector3(.4f,.35f,.25f);
        public Vector3 indexWeights=new Vector3(.45f,.35f,.20f);
        [Header("Recent successful placements (no persistent/personal telemetry)")]
        [Range(3,20)] public int historyLength=6;
        public bool useSpeed=false;
        [Min(.1f)] public float fastSeconds=4.2f;
        [Range(0,.15f)] public float speedInfluence=.05f;
        [Range(0,1)] public float performanceForChallenge=.55f, opportunityForChallenge=.45f;
        public float comboReference=8;
        [Header("Board pressure")]
        public Vector2 occupancyRange=new Vector2(.45f,.9f);
        public Vector3 dangerThresholds=new Vector3(.35f,.6f,.8f);
        public Vector4 dangerWeights=new Vector4(.4f,.3f,.15f,.15f);
        [Min(1)] public float legalPlacementReference=12;
        [Header("Smoothing and relief (per new hand)")]
        [Range(.01f,1)] public float increaseStep=.12f, decreaseStep=.25f;
        [Range(0,1)] public float pressureAdjustment=.4f, hardRelief=.85f, easyFloor=.10f;
        [Min(1)] public int noClearReliefTurns=4, reliefHands=2;
        [Range(0,1)] public float struggleClearRate=.2f;
        [Min(1)] public int tinyHandLimit=3;
        [Range(1,3)] public int maxHardPerHand=2;
        [Range(0,1)] public float contextFloor=.15f;
        [Header("Bounded candidate and placement search")]
        [Range(1,30)] public int candidateAttempts=10;
        [Range(100,50000)] public int searchNodeBudget=5000;
        [Range(1,50)] public float searchMilliseconds=8;
        public bool logDecisions;
        public DifficultyBand[] bands={
            new DifficultyBand("Starter",0,new Vector3(.75f,.25f,0)),
            new DifficultyBand("Developing",.25f,new Vector3(.55f,.35f,.10f)),
            new DifficultyBand("Skilled",.5f,new Vector3(.35f,.45f,.20f)){viability=ViabilityPolicy.PreferThreeMinimumTwo},
            new DifficultyBand("Advanced",.75f,new Vector3(.20f,.45f,.35f)){viability=ViabilityPolicy.PreferThreeMinimumTwo},
            new DifficultyBand("Expert",1,new Vector3(.15f,.40f,.45f)){viability=ViabilityPolicy.PreferThreeInitialMove}
        };
        public static Vector3 Normalize(Vector3 weights)
        {
            weights=new Vector3(Mathf.Max(0,weights.x),Mathf.Max(0,weights.y),Mathf.Max(0,weights.z));
            float total=weights.x+weights.y+weights.z;return total>0?weights/total:Vector3.right;
        }
        void OnValidate()
        {
            scoreRatios.x=Mathf.Max(.01f,scoreRatios.x);scoreRatios.y=Mathf.Max(scoreRatios.x+.01f,scoreRatios.y);
            scoreRatios.z=Mathf.Max(scoreRatios.y+.01f,scoreRatios.z);scoreRatios.w=Mathf.Max(scoreRatios.z+.01f,scoreRatios.w);
            firstRunScore=Mathf.Max(1,firstRunScore);firstRunPlacements=Mathf.Max(1,firstRunPlacements);firstRunLines=Mathf.Max(1,firstRunLines);
            historyLength=Mathf.Clamp(historyLength,3,20);candidateAttempts=Mathf.Clamp(candidateAttempts,1,30);searchNodeBudget=Mathf.Clamp(searchNodeBudget,100,50000);
            searchMilliseconds=Mathf.Clamp(searchMilliseconds,1,50);maxHardPerHand=Mathf.Clamp(maxHardPerHand,1,3);
            if(bands!=null){Array.Sort(bands,(a,b)=>a.index.CompareTo(b.index));foreach(var band in bands)band.weights=Normalize(band.weights);}
        }
    }
}

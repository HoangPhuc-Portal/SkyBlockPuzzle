using UnityEngine;
namespace BlockBlast.Environment
{
    [CreateAssetMenu(menuName="Block Blast/Environment Settings")]
    public sealed class IslandEnvironmentSettings : ScriptableObject
    {
        public Sprite cloudSprite, leafSprite;
        public Material waterMaterial;
        [Header("Clouds (viewport units per second)")]
        [Range(0, .03f)] public float cloudSpeed=.006f;
        public Vector2 cloudMovementRange=new Vector2(.38f,.012f);
        [Range(0, .5f)] public float cloudOpacity=.22f;
        [Range(.2f,.9f)] public float cloudWidth=.65f;
        [Header("Water")]
        [Range(0,3)] public float waterAnimationSpeed=1;
        [Range(0,.5f)] public float waterStrength=.13f;
        public Vector4[] waterAreas=new Vector4[4]; // texture UV: x,y,width,height
        [Header("Leaves (fixed pool)")]
        [Range(0,2)] public float leafSpawnRate=.25f;
        [Range(.01f,.15f)] public float leafMovementSpeed=.045f;
        [Range(0,12)] public int maximumActiveLeaves=5;
        [Range(0,12)] public int initialLeaves=4;
        [Range(.02f,.09f)] public float leafWidth=.065f;
        [Range(0,1)] public float particleDensity=1;
        [Header("Lifecycle")]
        public bool pauseWithGameplayModal=true;
        public bool reduceOnLowQuality=true;
    }
}

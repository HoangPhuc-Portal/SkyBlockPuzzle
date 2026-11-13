using System.Collections.Generic;
using UnityEngine;
namespace BlockBlast.Data
{
    public enum RotationPolicy { AllRotations, NoRotation, UniqueRotations }
    public enum ShapeDifficulty { Easy, Medium, Hard }
    [CreateAssetMenu(fileName="NewShapeData",menuName="Block Blast/Shape Data")]
    public class ShapeData : ScriptableObject
    {
        // Legacy fields retained so existing assets can be migrated without losing cells.
        [HideInInspector] public int rows=3, columns=3;
        [SerializeField, HideInInspector] bool[] matrixData;
        [SerializeField] bool[] cells = new bool[9];
        [SerializeField, HideInInspector] bool migrated;
        [SerializeField, Range(3,5)] int designSize = 3;
        [Tooltip("Enable only for shapes whose cells intentionally touch at corners, such as diagonals.")]
        public bool allowCornerConnections;
        public int DesignSize => !migrated && matrixData != null
            ? Mathf.Clamp(Mathf.Max(designSize, rows, columns), 3, 5) : Mathf.Clamp(designSize, 3, 5);
        public Color blockColor=Color.cyan;
        public RotationPolicy rotationPolicy=RotationPolicy.UniqueRotations;
        public ShapeDifficulty difficulty=ShapeDifficulty.Medium;
        public bool[,] DesignMatrix()
        {
            int size=DesignSize;
            var result=new bool[size,size];
            if(!migrated && matrixData!=null) {
                for(int r=0;r<Mathf.Min(rows,size);r++)for(int c=0;c<Mathf.Min(columns,size);c++)
                    if(r*columns+c<matrixData.Length)result[size-1-r,c]=matrixData[r*columns+c];
            } else for(int i=0;i<size*size;i++)result[i/size,i%size]=cells!=null&&i<cells.Length&&cells[i];
            return result;
        }
        public bool Migrate()
        {
            if(migrated)return false;
            if(rows>5||columns>5)throw new System.InvalidOperationException("Legacy shape exceeds 5x5: "+name);
            var m=DesignMatrix();designSize=m.GetLength(0);cells=new bool[designSize*designSize];
            for(int i=0;i<cells.Length;i++)cells[i]=m[i/designSize,i%designSize];migrated=true;return true;
        }
        public void ResizeDesign(int size)
        {
            size=Mathf.Clamp(size,3,5);var m=DesignMatrix();int old=m.GetLength(0);
            for(int r=0;r<old;r++)for(int c=0;c<old;c++)
                if(m[r,c]&&(r>=size||c>=size))throw new System.InvalidOperationException("Clear cells outside the smaller grid before resizing.");
            var resized=new bool[size*size];
            for(int r=0;r<Mathf.Min(old,size);r++)for(int c=0;c<Mathf.Min(old,size);c++)resized[r*size+c]=m[r,c];
            cells=resized;designSize=size;migrated=true;
        }
        public bool IsValid => ShapeRuntimeData.Connected(DesignMatrix(), allowCornerConnections);
        public List<ShapeRuntimeData> Variants()
        {
            var result=new List<ShapeRuntimeData>();if(!IsValid)return result;
            var m=DesignMatrix();var keys=new HashSet<string>();
            for(int i=0;i<(rotationPolicy==RotationPolicy.NoRotation?1:4);i++){
                var v=new ShapeRuntimeData(this,m,i*90);
                if(rotationPolicy!=RotationPolicy.UniqueRotations||keys.Add(v.Key))result.Add(v);
                m=ShapeRuntimeData.Rotate(m);
            }return result;
        }
        public ShapeRuntimeData CreateRuntime(){var v=Variants();return v.Count==0?null:v[Random.Range(0,v.Count)];}
        // Compatibility for editor checks that construct legacy assets in memory.
        public bool[,] GetMatrix(){return new ShapeRuntimeData(this,DesignMatrix(),0).GetMatrix();}
    }
}

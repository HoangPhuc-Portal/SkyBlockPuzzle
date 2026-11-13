using System.Collections.Generic;
using UnityEngine;
namespace BlockBlast.Data
{
    public sealed class ShapeRuntimeData
    {
        readonly bool[,] matrix;
        public ShapeData Source {get;}
        public int Rotation {get;}
        public int rows => matrix.GetLength(0);
        public int columns => matrix.GetLength(1);
        public Color blockColor {get;}
        public IReadOnlyList<Vector2Int> Cells {get;}
        public string Key {get;}
        public ShapeRuntimeData(ShapeData source,bool[,] design,int rotation)
        {
            Source=source;Rotation=rotation;blockColor=source.blockColor;
            int minR=design.GetLength(0),minC=design.GetLength(1),maxR=-1,maxC=-1;
            for(int r=0;r<design.GetLength(0);r++)for(int c=0;c<design.GetLength(1);c++)if(design[r,c]){minR=Mathf.Min(minR,r);minC=Mathf.Min(minC,c);maxR=Mathf.Max(maxR,r);maxC=Mathf.Max(maxC,c);}
            matrix=new bool[Mathf.Max(0,maxR-minR+1),Mathf.Max(0,maxC-minC+1)];
            var coords=new List<Vector2Int>();var key=new System.Text.StringBuilder(rows+"x"+columns+":");
            // Board rows increase upwards; Inspector rows increase downwards.
            for(int r=0;r<rows;r++)for(int c=0;c<columns;c++){bool filled=design[maxR-r,minC+c];matrix[r,c]=filled;key.Append(filled?'1':'0');if(filled)coords.Add(new Vector2Int(r,c));}
            Cells=coords.AsReadOnly();Key=key.ToString();
        }
        internal bool[,] GetMatrix()=>matrix;
        public static implicit operator ShapeRuntimeData(ShapeData source)=>source==null?null:new ShapeRuntimeData(source,source.DesignMatrix(),0);
        public static bool[,] Rotate(bool[,] source)
        {
            int rows=source.GetLength(0),cols=source.GetLength(1);var result=new bool[cols,rows];
            for(int r=0;r<rows;r++)for(int c=0;c<cols;c++)result[c,rows-1-r]=source[r,c];return result;
        }
        public static bool Connected(bool[,] m, bool allowCorners=false)
        {
            int total=0;var q=new Queue<Vector2Int>();var seen=new HashSet<Vector2Int>();
            int rows=m.GetLength(0),cols=m.GetLength(1);
            for(int r=0;r<rows;r++)for(int c=0;c<cols;c++)if(m[r,c]){total++;if(q.Count==0)q.Enqueue(new Vector2Int(r,c));}
            if(total==0)return false;
            var directions=allowCorners
                ? new[]{Vector2Int.up,Vector2Int.down,Vector2Int.left,Vector2Int.right,new Vector2Int(1,1),new Vector2Int(1,-1),new Vector2Int(-1,1),new Vector2Int(-1,-1)}
                : new[]{Vector2Int.up,Vector2Int.down,Vector2Int.left,Vector2Int.right};
            while(q.Count>0){var p=q.Dequeue();if(!seen.Add(p))continue;foreach(var d in directions){var n=p+d;if(n.x>=0&&n.x<rows&&n.y>=0&&n.y<cols&&m[n.x,n.y]&&!seen.Contains(n))q.Enqueue(n);}}
            return seen.Count==total;
        }
    }
}

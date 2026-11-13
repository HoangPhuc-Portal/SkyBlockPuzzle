using UnityEngine;

namespace BlockBlast.Core
{
    // Immutable copy of the old visual; effects never retain or mutate a live GridCell.
    public readonly struct ClearedCellInfo
    {
        public readonly int Row;
        public readonly int Col;
        public readonly Vector3 Position;
        public readonly Sprite Sprite;
        public readonly Color Color;
        public readonly Vector3 Scale;
        public readonly Quaternion Rotation;
        public readonly int SortingLayerID;
        public readonly int SortingOrder;

        public ClearedCellInfo(int row, int col, Vector3 position, Sprite sprite, Color color,
            Vector3 scale, Quaternion rotation, int sortingLayerID, int sortingOrder)
        {
            Row = row; Col = col; Position = position; Sprite = sprite; Color = color;
            Scale = scale; Rotation = rotation; SortingLayerID = sortingLayerID; SortingOrder = sortingOrder;
        }
    }
}

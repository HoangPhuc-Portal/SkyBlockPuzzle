using System.Collections.Generic;
using UnityEngine;

namespace BlockBlast.Data
{
    [CreateAssetMenu(menuName = "Block Blast/Block Sprite Set")]
    public sealed class BlockSpriteSet : ScriptableObject
    {
        [SerializeField] private Sprite[] sprites = new Sprite[0];
        public IReadOnlyList<Sprite> Sprites => sprites;
        public int ValidCount
        {
            get
            {
                int count = 0;
                if (sprites != null) foreach (var sprite in sprites) if (sprite != null) count++;
                return count;
            }
        }

        // Selection is deterministic; the spawner supplies the random index once per piece.
        public Sprite SelectByIndex(int index)
        {
            int count = ValidCount;
            if (count == 0) return null;
            index = ((index % count) + count) % count;
            foreach (var sprite in sprites)
            {
                if (sprite == null) continue;
                if (index-- == 0) return sprite;
            }
            return null;
        }

        private void OnValidate()
        {
            if (ValidCount == 0) Debug.LogWarning("BlockSpriteSet has no usable sprites; tray will use its existing fallback sprite.", this);
            if (sprites == null) return;
            var unique = new HashSet<Sprite>();
            foreach (var sprite in sprites)
                if (sprite == null || !unique.Add(sprite))
                    Debug.LogWarning("BlockSpriteSet contains a null or duplicate sprite.", this);
        }
    }
}

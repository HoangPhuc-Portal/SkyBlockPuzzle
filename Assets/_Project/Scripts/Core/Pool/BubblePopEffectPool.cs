using System.Collections.Generic;
using BlockBlast.Effects;
using UnityEngine;

namespace BlockBlast.Core.Pool
{
    public sealed class BubblePopEffectPool
    {
        private readonly BubblePopConfig config;
        private readonly Transform parent;
        private readonly Stack<BubblePopEffect> available;
        private readonly HashSet<BubblePopEffect> leased;
        private readonly List<BubblePopEffect> instances;
        public int CreatedCount => instances.Count;
        public int ActiveCount => leased.Count;

        public BubblePopEffectPool(BubblePopConfig settings, Transform container)
        {
            config = settings;
            parent = container;
            int capacity = Mathf.Max(1, config.maximumCount);
            available = new Stack<BubblePopEffect>(capacity);
            leased = new HashSet<BubblePopEffect>();
            instances = new List<BubblePopEffect>(capacity);
            for (int i = 0; i < Mathf.Min(config.prewarmCount, capacity); i++) available.Push(Create());
        }

        private BubblePopEffect Create()
        {
            var effect = Object.Instantiate(config.effectPrefab, parent);
            effect.Initialize(config);
            instances.Add(effect);
            return effect;
        }

        public BubblePopEffect Rent()
        {
            if (available.Count == 0)
            {
                if (instances.Count >= Mathf.Max(1, config.maximumCount)) return null;
                // Grow in batches, rather than allocating for every cell of later clears.
                int batch = Mathf.Min(Mathf.Max(1, config.prewarmCount), config.maximumCount - instances.Count);
                for (int i = 0; i < batch; i++) available.Push(Create());
            }
            var effect = available.Pop();
            leased.Add(effect);
            return effect;
        }

        public void Return(BubblePopEffect effect)
        {
            if (effect == null || !leased.Remove(effect)) return;
            effect.ResetVisuals();
            available.Push(effect);
        }

        public void ReleaseAll()
        {
            foreach (var effect in leased)
            {
                effect.ResetVisuals();
                available.Push(effect);
            }
            leased.Clear();
        }
    }
}

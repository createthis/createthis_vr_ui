using UnityEngine;
using System.Collections.Generic;

namespace CreateThis.Unity {
    public static class TextureCache {
        private static int count;
        private static Dictionary<int, Texture2D> cache;

        private static void Initialize() {
            if (cache == null) {
                cache = new Dictionary<int, Texture2D>();
                count = 0;
            }
        }

        public static int Add(Texture2D texture) {
            Initialize();
            int id = count;
            cache.Add(id, texture);
            count++;
            return id;
        }

        public static Texture2D TextureById(int id) {
            Initialize();

            if (cache.ContainsKey(id)) return cache[id];
            return null;
        }
    }
}
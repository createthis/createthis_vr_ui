using UnityEngine;
using System;
using System.Collections.Generic;

namespace CreateThis.Unity {
    public static class MaterialCache {
        private struct Key : IEquatable<Key> {
            public float r;
            public float g;
            public float b;
            public float a;
            public bool renderMesh;
            public bool renderWireframe;
            public bool renderNormals;
            public int textureCacheId;

            public bool Equals(Key other) {
                return r == other.r &&
                       g == other.g &&
                       b == other.b &&
                       a == other.a &&
                       renderMesh == other.renderMesh &&
                       renderWireframe == other.renderWireframe &&
                       renderNormals == other.renderNormals &&
                       textureCacheId == other.textureCacheId;
            }

            public override int GetHashCode() {
                return r.GetHashCode() ^ g.GetHashCode() ^ b.GetHashCode() ^ a.GetHashCode() ^ renderMesh.GetHashCode() ^ renderWireframe.GetHashCode() ^ renderNormals.GetHashCode() ^ textureCacheId.GetHashCode();
            }
        }
        private static int materialCount;
        private static Dictionary<Key, Material> cache;

        private static void Initialize() {
            if (cache == null) {
                cache = new Dictionary<Key, Material>();
                materialCount = 0;
            }
        }

        private static string ColorToString(Color color) {
            Color32 color32 = color;
            string hex = color32.r.ToString("X2") + color32.g.ToString("X2") + color32.b.ToString("X2");
            return hex;
        }

        private static Key ColorAndStatesToKey(Color color, bool renderMesh, bool renderWireframe, bool renderNormals, int textureCacheId = -1) {
            Key key = new Key();
            key.r = color.r;
            key.g = color.g;
            key.b = color.b;
            key.a = color.a;
            key.renderMesh = renderMesh;
            key.renderWireframe = renderWireframe;
            key.renderNormals = renderNormals;
            key.textureCacheId = textureCacheId;
            return key;
        }

        private static Material ColorAndStateToMaterial(Color color, bool renderMesh, bool renderWireframe, bool renderNormals, int textureCacheId = -1) {
            Material materialInstance;
            materialInstance = new Material(Shader.Find("Standard"));

            /*
            if (renderMesh) {
                materialInstance = new Material(Shader.Find("VacuumShaders/The Amazing Wireframe/Geometry Shader"));
            } else {
                materialInstance = new Material(Shader.Find("Hidden/VacuumShaders/The Amazing Wireframe/Geometry Shader/Transparent/Simple/Diffuse"));
                materialInstance.SetInt("_Cull", 0);
                if (textureCacheId == -1) color.a = 0;
            }
            */

            materialInstance.SetColor("_Color", color);
            materialInstance.EnableKeyword("_EMISSION");
            Color wireframe = new Color();
            ColorUtility.TryParseHtmlString("#15FF3DFF", out wireframe);

            if (!renderWireframe) {
                wireframe.a = 0;
                materialInstance.SetFloat("_NHeight", 0);
                materialInstance.SetFloat("_NScale", 0);
            }

            if (renderNormals) {
                if (renderWireframe) {
                    materialInstance.SetFloat("_NHeight", 0.025f);
                    materialInstance.SetFloat("_NScale", 0.001f);
                }
            } else {
                materialInstance.SetFloat("_NHeight", 0);
                materialInstance.SetFloat("_NScale", 0);
            }

            materialInstance.SetColor("_V_WIRE_Color", wireframe);
            materialInstance.SetColor("_NColor", wireframe);

            materialInstance.name = "Material" + materialCount;
            if (textureCacheId != -1) {
                Texture2D mainTexture = TextureCache.TextureById(textureCacheId);
                materialInstance.mainTexture = mainTexture;
            }
            materialCount++;
            return materialInstance;
        }

        public static Material MaterialByColor(Color color, bool renderMesh, bool renderWireframe, bool renderNormals, int textureCacheId = -1) {
            Initialize();

            Key key = ColorAndStatesToKey(color, renderMesh, renderWireframe, renderNormals, textureCacheId);
            if (cache.ContainsKey(key)) return cache[key];

            Material material = ColorAndStateToMaterial(color, renderMesh, renderWireframe, renderNormals, textureCacheId);
            cache.Add(key, material);
            return material;
        }
    }
}
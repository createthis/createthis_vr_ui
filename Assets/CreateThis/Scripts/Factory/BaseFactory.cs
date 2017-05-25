using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CreateThis.Factory {
    public class BaseFactory : MonoBehaviour {
        public bool useVRTK;

        protected T SafeAddComponent<T>(GameObject target) where T : Component {
#if UNITY_EDITOR
            return Undo.AddComponent<T>(target);
#else
            return target.AddComponent<T>();
#endif
        }

        public virtual GameObject Generate() {
            return null;
        }
    }
}
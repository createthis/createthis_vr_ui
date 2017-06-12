using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CreateThis.Unity {
    public static class Undoable {
        public static T AddComponent<T>(GameObject target) where T : Component {
#if UNITY_EDITOR
            return Undo.AddComponent<T>(target);
#else
            return target.AddComponent<T>();
#endif
        }
    }
}
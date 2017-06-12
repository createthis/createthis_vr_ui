using System;
using System.Reflection;
using UnityEngine;

namespace CreateThis.Unity {
    public static class ScriptUtils {
        public static T CopyComponent<T>(T original, GameObject destination) where T : Component {
            Type type = original.GetType();
            Component copy = destination.AddComponent(type);
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields) {
                field.SetValue(copy, field.GetValue(original));
            }
            return copy as T;
        }
    }
}
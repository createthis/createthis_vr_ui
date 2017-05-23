using UnityEngine;
using UnityEditor;

namespace CreateThis.VR.UI.Container {
    [CustomEditor(typeof(ColumnContainer))]
    [CanEditMultipleObjects]

    public class ColumnContainerEditor : Editor {
        SerializedProperty padding;
        SerializedProperty spacing;
        SerializedProperty bounds;

        void OnEnable() {
            padding = serializedObject.FindProperty("padding");
            spacing = serializedObject.FindProperty("spacing");
            bounds = serializedObject.FindProperty("bounds");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorGUILayout.PropertyField(padding);
            EditorGUILayout.PropertyField(spacing);
            EditorGUILayout.PropertyField(bounds);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Move Children")) {
                if (target.GetType() == typeof(ColumnContainer)) {
                    ColumnContainer columnContainer = (ColumnContainer)target;
                    columnContainer.MoveChildren();
                }
            }
        }
    }
}
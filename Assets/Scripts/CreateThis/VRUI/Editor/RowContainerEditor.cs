using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RowContainer))]
[CanEditMultipleObjects]

public class RowContainerEditor : Editor {
    SerializedProperty padding;
    SerializedProperty spacing;
    SerializedProperty alignment;
    SerializedProperty bounds;
    SerializedProperty log;

    void OnEnable() {
        padding = serializedObject.FindProperty("padding");
        spacing = serializedObject.FindProperty("spacing");
        alignment = serializedObject.FindProperty("alignment");
        bounds = serializedObject.FindProperty("bounds");
        log = serializedObject.FindProperty("log");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.PropertyField(padding);
        EditorGUILayout.PropertyField(spacing);
        EditorGUILayout.PropertyField(alignment);
        EditorGUILayout.PropertyField(bounds);
        EditorGUILayout.PropertyField(log);

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Move Children")) {
            if (target.GetType() == typeof(RowContainer)) {
                RowContainer rowContainer = (RowContainer)target;
                rowContainer.MoveChildren();
            }
        }
    }
}

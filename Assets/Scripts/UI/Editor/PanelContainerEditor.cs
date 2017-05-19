using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PanelContainer))]
[CanEditMultipleObjects]

public class PanelContainerEditor : Editor {
    SerializedProperty minWidth;
    SerializedProperty minHeight;
    SerializedProperty bounds;

    void OnEnable() {
        minWidth = serializedObject.FindProperty("minWidth");
        minHeight = serializedObject.FindProperty("minHeight");
        bounds = serializedObject.FindProperty("bounds");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.PropertyField(minWidth);
        EditorGUILayout.PropertyField(minHeight);
        EditorGUILayout.PropertyField(bounds);

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Resize")) {
            if (target.GetType() == typeof(PanelContainer)) {
                PanelContainer panelContainer = (PanelContainer)target;
                panelContainer.Resize();
            }
        }
    }
}

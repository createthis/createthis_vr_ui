using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GrowButtonByTextMesh))]
[CanEditMultipleObjects]

public class GrowButtonByTextMeshEditor : Editor {
    SerializedProperty minWidth;
    SerializedProperty padding;
    SerializedProperty buttonBody;
    SerializedProperty textMesh;
    SerializedProperty alignment;
    SerializedProperty log;

    void OnEnable() {
        minWidth = serializedObject.FindProperty("minWidth");
        padding = serializedObject.FindProperty("padding");
        buttonBody = serializedObject.FindProperty("buttonBody");
        textMesh = serializedObject.FindProperty("textMesh");
        alignment = serializedObject.FindProperty("alignment");
        log = serializedObject.FindProperty("log");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.PropertyField(minWidth);
        EditorGUILayout.PropertyField(padding);
        EditorGUILayout.PropertyField(buttonBody);
        EditorGUILayout.PropertyField(textMesh);
        EditorGUILayout.PropertyField(alignment);
        EditorGUILayout.PropertyField(log);

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Resize")) {
            if (target.GetType() == typeof(GrowButtonByTextMesh)) {
                GrowButtonByTextMesh growButtonByTextMesh = (GrowButtonByTextMesh)target;
                growButtonByTextMesh.Resize();
            }
        }
    }
}

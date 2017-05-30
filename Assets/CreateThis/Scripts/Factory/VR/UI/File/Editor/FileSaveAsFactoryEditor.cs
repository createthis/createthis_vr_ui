using UnityEngine;
using UnityEditor;

namespace CreateThis.Factory.VR.UI.File {
    [CustomEditor(typeof(FileSaveAsFactory))]
    [CanEditMultipleObjects]

    public class FileSaveAsFactoryEditor : FileOpenFactoryEditor {
        SerializedProperty fileNameExtension;
        SerializedProperty keyboard;
        SerializedProperty sceneCamera;
        SerializedProperty offset;
        SerializedProperty minDistance;
        SerializedProperty hideOnAwake;

        protected override void OnEnable() {
            base.OnEnable();
            fileNameExtension = serializedObject.FindProperty("fileNameExtension");
            keyboard = serializedObject.FindProperty("keyboard");
            sceneCamera = serializedObject.FindProperty("sceneCamera");
            offset = serializedObject.FindProperty("offset");
            minDistance = serializedObject.FindProperty("minDistance");
            hideOnAwake = serializedObject.FindProperty("hideOnAwake");
        }

        protected override void BuildGenerateButton() {
            // Take out this if statement to set the value using setter when ever you change it in the inspector.
            // But then it gets called a couple of times when ever inspector updates
            // By having a button, you can control when the value goes through the setter and getter, your self.
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(FileSaveAsFactory)) {
                    FileSaveAsFactory factory = (FileSaveAsFactory)target;
                    factory.Generate();
                }
            }
        }

        protected override void AdditionalProperties() {
            base.AdditionalProperties();
            EditorGUILayout.PropertyField(fileNameExtension);
            EditorGUILayout.PropertyField(keyboard);
            EditorGUILayout.PropertyField(sceneCamera);
            EditorGUILayout.PropertyField(offset);
            EditorGUILayout.PropertyField(minDistance);
            EditorGUILayout.PropertyField(hideOnAwake);
        }
    }
}
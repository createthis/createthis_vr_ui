using UnityEngine;
using UnityEditor;
using CreateThis.Factory;

namespace CreateThis.Example {
    [CustomEditor(typeof(ExampleMasterUIFactory))]
    [CanEditMultipleObjects]

    public class ExampleMasterUIFactoryEditor : BaseFactoryEditor {
        SerializedProperty parent;
        SerializedProperty skyboxManager;
        SerializedProperty touchPadMenuController;
        SerializedProperty toolsLocalPosition;
        SerializedProperty keyboardLocalPosition;
        SerializedProperty fileOpenLocalPosition;
        SerializedProperty fileSaveAsLocalPosition;

        protected override void OnEnable() {
            base.OnEnable();
            parent = serializedObject.FindProperty("parent");
            skyboxManager = serializedObject.FindProperty("skyboxManager");
            touchPadMenuController = serializedObject.FindProperty("touchPadMenuController");
            toolsLocalPosition = serializedObject.FindProperty("toolsLocalPosition");
            keyboardLocalPosition = serializedObject.FindProperty("keyboardLocalPosition");
            fileOpenLocalPosition = serializedObject.FindProperty("fileOpenLocalPosition");
            fileSaveAsLocalPosition = serializedObject.FindProperty("fileSaveAsLocalPosition");
        }

        protected override void BuildGenerateButton() {
            // Take out this if statement to set the value using setter when ever you change it in the inspector.
            // But then it gets called a couple of times when ever inspector updates
            // By having a button, you can control when the value goes through the setter and getter, your self.
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(ExampleMasterUIFactory)) {
                    ExampleMasterUIFactory factory = (ExampleMasterUIFactory)target;
                    factory.Generate();
                }
            }
        }

        protected override void AdditionalProperties() {
            base.AdditionalProperties();
            EditorGUILayout.PropertyField(parent);
            EditorGUILayout.PropertyField(skyboxManager);
            EditorGUILayout.PropertyField(touchPadMenuController);
            EditorGUILayout.PropertyField(toolsLocalPosition);
            EditorGUILayout.PropertyField(keyboardLocalPosition);
            EditorGUILayout.PropertyField(fileOpenLocalPosition);
            EditorGUILayout.PropertyField(fileSaveAsLocalPosition);
        }
    }
}
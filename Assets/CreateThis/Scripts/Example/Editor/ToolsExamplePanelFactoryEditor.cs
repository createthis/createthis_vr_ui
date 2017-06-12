using UnityEngine;
using UnityEditor;
using CreateThis.Factory;

namespace CreateThis.Example {
    [CustomEditor(typeof(ToolsExamplePanelFactory))]
    [CanEditMultipleObjects]

    public class ToolsExamplePanelFactoryEditor : BaseFactoryEditor {
        SerializedProperty parent;
        SerializedProperty panelProfile;
        SerializedProperty panelContainerProfile;
        SerializedProperty momentaryButtonProfile;
        SerializedProperty toggleButtonProfile;
        SerializedProperty fileOpen;
        SerializedProperty fileSaveAs;
        SerializedProperty skyboxManager;

        protected override void OnEnable() {
            base.OnEnable();
            parent = serializedObject.FindProperty("parent");
            panelProfile = serializedObject.FindProperty("panelProfile");
            panelContainerProfile = serializedObject.FindProperty("panelContainerProfile");
            momentaryButtonProfile = serializedObject.FindProperty("momentaryButtonProfile");
            toggleButtonProfile = serializedObject.FindProperty("toggleButtonProfile");
            panelProfile = serializedObject.FindProperty("panelProfile");
            fileOpen = serializedObject.FindProperty("fileOpen");
            fileSaveAs = serializedObject.FindProperty("fileSaveAs");
            skyboxManager = serializedObject.FindProperty("skyboxManager");
        }

        protected override void BuildGenerateButton() {
            // Take out this if statement to set the value using setter when ever you change it in the inspector.
            // But then it gets called a couple of times when ever inspector updates
            // By having a button, you can control when the value goes through the setter and getter, your self.
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(ToolsExamplePanelFactory)) {
                    ToolsExamplePanelFactory factory = (ToolsExamplePanelFactory)target;
                    factory.Generate();
                }
            }
        }

        protected override void AdditionalProperties() {
            base.AdditionalProperties();
            EditorGUILayout.PropertyField(parent);
            EditorGUILayout.PropertyField(panelProfile);
            EditorGUILayout.PropertyField(panelContainerProfile);
            EditorGUILayout.PropertyField(momentaryButtonProfile);
            EditorGUILayout.PropertyField(toggleButtonProfile);
            EditorGUILayout.PropertyField(fileOpen);
            EditorGUILayout.PropertyField(fileSaveAs);
            EditorGUILayout.PropertyField(skyboxManager);
        }
    }
}
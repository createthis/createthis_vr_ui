using UnityEngine;
using UnityEditor;
using CreateThis.Factory;

namespace CreateThis.Example {
    [CustomEditor(typeof(ToolsExamplePanelFactory))]
    [CanEditMultipleObjects]

    public class ToolsExamplePanelFactoryEditor : BaseFactoryEditor {
        SerializedProperty parent;
        SerializedProperty buttonBody;
        SerializedProperty folderPrefab;
        SerializedProperty buttonMaterial;
        SerializedProperty panelMaterial;
        SerializedProperty highlight;
        SerializedProperty outline;
        SerializedProperty momentaryButtonClickDown;
        SerializedProperty momentaryButtonClickUp;
        SerializedProperty toggleButtonClickDown;
        SerializedProperty toggleButtonClickUp;
        SerializedProperty fontSize;
        SerializedProperty fontColor;
        SerializedProperty labelZ;
        SerializedProperty buttonZ;
        SerializedProperty bodyScale;
        SerializedProperty labelScale;
        SerializedProperty padding;
        SerializedProperty spacing;
        SerializedProperty buttonPadding;
        SerializedProperty buttonMinWidth;
        SerializedProperty buttonCharacterSize;
        SerializedProperty labelCharacterSize;
        SerializedProperty sceneCamera;
        SerializedProperty offset;
        SerializedProperty minDistance;
        SerializedProperty hideOnAwake;
        SerializedProperty fileOpen;
        SerializedProperty fileSaveAs;
        SerializedProperty skyboxManager;

        protected override void OnEnable() {
            base.OnEnable();
            parent = serializedObject.FindProperty("parent");
            buttonBody = serializedObject.FindProperty("buttonBody");
            folderPrefab = serializedObject.FindProperty("folderPrefab");
            buttonMaterial = serializedObject.FindProperty("buttonMaterial");
            panelMaterial = serializedObject.FindProperty("panelMaterial");
            highlight = serializedObject.FindProperty("highlight");
            outline = serializedObject.FindProperty("outline");
            momentaryButtonClickDown = serializedObject.FindProperty("momentaryButtonClickDown");
            momentaryButtonClickUp = serializedObject.FindProperty("momentaryButtonClickUp");
            toggleButtonClickDown = serializedObject.FindProperty("toggleButtonClickDown");
            toggleButtonClickUp = serializedObject.FindProperty("toggleButtonClickUp");
            fontSize = serializedObject.FindProperty("fontSize");
            fontColor = serializedObject.FindProperty("fontColor");
            labelZ = serializedObject.FindProperty("labelZ");
            buttonZ = serializedObject.FindProperty("buttonZ");
            bodyScale = serializedObject.FindProperty("bodyScale");
            labelScale = serializedObject.FindProperty("labelScale");
            padding = serializedObject.FindProperty("padding");
            spacing = serializedObject.FindProperty("spacing");
            buttonPadding = serializedObject.FindProperty("buttonPadding");
            buttonMinWidth = serializedObject.FindProperty("buttonMinWidth");
            buttonCharacterSize = serializedObject.FindProperty("buttonCharacterSize");
            labelCharacterSize = serializedObject.FindProperty("labelCharacterSize");
            sceneCamera = serializedObject.FindProperty("sceneCamera");
            offset = serializedObject.FindProperty("offset");
            minDistance = serializedObject.FindProperty("minDistance");
            hideOnAwake = serializedObject.FindProperty("hideOnAwake");
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
            EditorGUILayout.PropertyField(buttonBody);
            EditorGUILayout.PropertyField(folderPrefab);
            EditorGUILayout.PropertyField(buttonMaterial);
            EditorGUILayout.PropertyField(panelMaterial);
            EditorGUILayout.PropertyField(highlight);
            EditorGUILayout.PropertyField(outline);
            EditorGUILayout.PropertyField(momentaryButtonClickDown);
            EditorGUILayout.PropertyField(momentaryButtonClickUp);
            EditorGUILayout.PropertyField(toggleButtonClickDown);
            EditorGUILayout.PropertyField(toggleButtonClickUp);
            EditorGUILayout.PropertyField(fontSize);
            EditorGUILayout.PropertyField(fontColor);
            EditorGUILayout.PropertyField(labelZ);
            EditorGUILayout.PropertyField(buttonZ);
            EditorGUILayout.PropertyField(bodyScale);
            EditorGUILayout.PropertyField(labelScale);
            EditorGUILayout.PropertyField(padding);
            EditorGUILayout.PropertyField(spacing);
            EditorGUILayout.PropertyField(buttonPadding);
            EditorGUILayout.PropertyField(buttonMinWidth);
            EditorGUILayout.PropertyField(buttonCharacterSize);
            EditorGUILayout.PropertyField(labelCharacterSize);
            EditorGUILayout.PropertyField(sceneCamera);
            EditorGUILayout.PropertyField(offset);
            EditorGUILayout.PropertyField(minDistance);
            EditorGUILayout.PropertyField(hideOnAwake);
            EditorGUILayout.PropertyField(fileOpen);
            EditorGUILayout.PropertyField(fileSaveAs);
            EditorGUILayout.PropertyField(skyboxManager);
        }
    }
}
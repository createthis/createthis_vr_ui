using UnityEngine;
using UnityEditor;

namespace CreateThis.Factory.VR.UI.File {
    [CustomEditor(typeof(FileOpenFactory))]
    [CanEditMultipleObjects]

    public class FileOpenFactoryEditor : BaseFactoryEditor {
        SerializedProperty parent;
        SerializedProperty buttonBody;
        SerializedProperty folderPrefab;
        SerializedProperty buttonMaterial;
        SerializedProperty panelMaterial;
        SerializedProperty highlight;
        SerializedProperty outline;
        SerializedProperty buttonClickDown;
        SerializedProperty buttonClickUp;
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
        SerializedProperty kineticScrollerSpacing;
        SerializedProperty scrollerHeight;
        SerializedProperty searchPattern;
        SerializedProperty panelProfile;

        protected override void OnEnable() {
            base.OnEnable();
            parent = serializedObject.FindProperty("parent");
            buttonBody = serializedObject.FindProperty("buttonBody");
            folderPrefab = serializedObject.FindProperty("folderPrefab");
            buttonMaterial = serializedObject.FindProperty("buttonMaterial");
            panelMaterial = serializedObject.FindProperty("panelMaterial");
            highlight = serializedObject.FindProperty("highlight");
            outline = serializedObject.FindProperty("outline");
            buttonClickDown = serializedObject.FindProperty("buttonClickDown");
            buttonClickUp = serializedObject.FindProperty("buttonClickUp");
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
            kineticScrollerSpacing = serializedObject.FindProperty("kineticScrollerSpacing");
            scrollerHeight = serializedObject.FindProperty("scrollerHeight");
            searchPattern = serializedObject.FindProperty("searchPattern");
            panelProfile = serializedObject.FindProperty("panelProfile");
        }

        protected override void BuildGenerateButton() {
            // Take out this if statement to set the value using setter when ever you change it in the inspector.
            // But then it gets called a couple of times when ever inspector updates
            // By having a button, you can control when the value goes through the setter and getter, your self.
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(FileOpenFactory)) {
                    FileOpenFactory factory = (FileOpenFactory)target;
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
            EditorGUILayout.PropertyField(buttonClickDown);
            EditorGUILayout.PropertyField(buttonClickUp);
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
            EditorGUILayout.PropertyField(kineticScrollerSpacing);
            EditorGUILayout.PropertyField(scrollerHeight);
            EditorGUILayout.PropertyField(searchPattern);
            EditorGUILayout.PropertyField(panelProfile);
        }
    }
}
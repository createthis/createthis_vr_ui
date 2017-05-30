using UnityEngine;
using UnityEditor;

namespace CreateThis.Factory.VR.UI {
    [CustomEditor(typeof(KeyboardFactory))]
    [CanEditMultipleObjects]

    public class KeyboardFactoryEditor : BaseFactoryEditor {
        SerializedProperty parent;
        SerializedProperty buttonBody;
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
        SerializedProperty keyMinWidth;
        SerializedProperty keyCharacterSize;
        SerializedProperty numLockCharacterSize;
        SerializedProperty spaceMinWidth;
        SerializedProperty returnMinWidth;
        SerializedProperty spacerWidth;
        SerializedProperty modeKeyMinWidth;
        SerializedProperty wideKeyMinWidth;
        SerializedProperty sceneCamera;
        SerializedProperty offset;
        SerializedProperty minDistance;
        SerializedProperty hideOnAwake;

        protected override void OnEnable() {
            base.OnEnable();
            parent = serializedObject.FindProperty("parent");
            buttonBody = serializedObject.FindProperty("buttonBody");
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
            keyMinWidth = serializedObject.FindProperty("keyMinWidth");
            keyCharacterSize = serializedObject.FindProperty("keyCharacterSize");
            numLockCharacterSize = serializedObject.FindProperty("numLockCharacterSize");
            spaceMinWidth = serializedObject.FindProperty("spaceMinWidth");
            returnMinWidth = serializedObject.FindProperty("returnMinWidth");
            spacerWidth = serializedObject.FindProperty("spacerWidth");
            modeKeyMinWidth = serializedObject.FindProperty("modeKeyMinWidth");
            wideKeyMinWidth = serializedObject.FindProperty("wideKeyMinWidth");
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
                if (target.GetType() == typeof(KeyboardFactory)) {
                    KeyboardFactory factory = (KeyboardFactory)target;
                    factory.Generate();
                }
            }
        }

        protected override void AdditionalProperties() {
            base.AdditionalProperties();
            EditorGUILayout.PropertyField(parent);
            EditorGUILayout.PropertyField(buttonBody);
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
            EditorGUILayout.PropertyField(keyMinWidth);
            EditorGUILayout.PropertyField(keyCharacterSize);
            EditorGUILayout.PropertyField(numLockCharacterSize);
            EditorGUILayout.PropertyField(spaceMinWidth);
            EditorGUILayout.PropertyField(returnMinWidth);
            EditorGUILayout.PropertyField(spacerWidth);
            EditorGUILayout.PropertyField(modeKeyMinWidth);
            EditorGUILayout.PropertyField(wideKeyMinWidth);
            EditorGUILayout.PropertyField(sceneCamera);
            EditorGUILayout.PropertyField(offset);
            EditorGUILayout.PropertyField(minDistance);
            EditorGUILayout.PropertyField(hideOnAwake);
        }
    }
}
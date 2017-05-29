using UnityEngine;
using UnityEditor;

namespace CreateThis.Factory.VR.UI.Button {
    [CustomEditor(typeof(ButtonBaseFactory))]
    [CanEditMultipleObjects]

    public abstract class ButtonBaseFactoryEditor : BaseFactoryEditor {
        SerializedProperty parent;
        SerializedProperty buttonText;
        SerializedProperty buttonBody;
        SerializedProperty material;
        SerializedProperty highlight;
        SerializedProperty outline;
        SerializedProperty buttonClickDown;
        SerializedProperty buttonClickUp;
        SerializedProperty alignment;
        SerializedProperty fontSize;
        SerializedProperty fontColor;
        SerializedProperty labelZ;
        SerializedProperty bodyScale;
        SerializedProperty labelScale;
        SerializedProperty minWidth;
        SerializedProperty padding;
        SerializedProperty characterSize;
        SerializedProperty panel;

        protected override void OnEnable() {
            base.OnEnable();
            parent = serializedObject.FindProperty("parent");
            buttonText = serializedObject.FindProperty("buttonText");
            buttonBody = serializedObject.FindProperty("buttonBody");
            material = serializedObject.FindProperty("material");
            highlight = serializedObject.FindProperty("highlight");
            outline = serializedObject.FindProperty("outline");
            buttonClickDown = serializedObject.FindProperty("buttonClickDown");
            buttonClickUp = serializedObject.FindProperty("buttonClickUp");
            alignment = serializedObject.FindProperty("alignment");
            fontSize = serializedObject.FindProperty("fontSize");
            fontColor = serializedObject.FindProperty("fontColor");
            labelZ = serializedObject.FindProperty("labelZ");
            bodyScale = serializedObject.FindProperty("bodyScale");
            labelScale = serializedObject.FindProperty("labelScale");
            minWidth = serializedObject.FindProperty("minWidth");
            padding = serializedObject.FindProperty("padding");
            characterSize = serializedObject.FindProperty("characterSize");
            panel = serializedObject.FindProperty("panel");
        }

        protected override void BuildGenerateButton() {
            // Take out this if statement to set the value using setter when ever you change it in the inspector.
            // But then it gets called a couple of times when ever inspector updates
            // By having a button, you can control when the value goes through the setter and getter, your self.
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(ButtonBaseFactory)) {
                    ButtonBaseFactory buttonFactory = (ButtonBaseFactory)target;
                    buttonFactory.Generate();
                }
            }
        }

        protected override void AdditionalProperties() {
            base.AdditionalProperties();
            EditorGUILayout.PropertyField(parent);
            EditorGUILayout.PropertyField(buttonText);
            EditorGUILayout.PropertyField(buttonBody);
            EditorGUILayout.PropertyField(material);
            EditorGUILayout.PropertyField(highlight);
            EditorGUILayout.PropertyField(outline);
            EditorGUILayout.PropertyField(buttonClickDown);
            EditorGUILayout.PropertyField(buttonClickUp);
            EditorGUILayout.PropertyField(alignment);
            EditorGUILayout.PropertyField(fontSize);
            EditorGUILayout.PropertyField(fontColor);
            EditorGUILayout.PropertyField(labelZ);
            EditorGUILayout.PropertyField(bodyScale);
            EditorGUILayout.PropertyField(labelScale);
            EditorGUILayout.PropertyField(minWidth);
            EditorGUILayout.PropertyField(padding);
            EditorGUILayout.PropertyField(characterSize);
            EditorGUILayout.PropertyField(panel);
        }
    }
}

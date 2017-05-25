using UnityEngine;
using UnityEditor;

namespace CreateThis.Factory.VR.UI {
    [CustomEditor(typeof(KeyboardFactory))]
    [CanEditMultipleObjects]

    public class KeyboardFactoryEditor : Editor {
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
        SerializedProperty bodyScale;
        SerializedProperty labelScale;

        protected void OnEnable() {
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
            bodyScale = serializedObject.FindProperty("bodyScale");
            labelScale = serializedObject.FindProperty("labelScale");
        }

        protected void BuildGenerateButton() {
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

        protected void AdditionalProperties() {
            // put your properties here in the override
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

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
            EditorGUILayout.PropertyField(bodyScale);
            EditorGUILayout.PropertyField(labelScale);
            AdditionalProperties();

            serializedObject.ApplyModifiedProperties();

            BuildGenerateButton();
        }
    }
}

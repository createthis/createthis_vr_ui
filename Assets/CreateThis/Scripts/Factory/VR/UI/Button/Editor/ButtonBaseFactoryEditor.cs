using UnityEngine;
using UnityEditor;

namespace CreateThis.Factory.VR.UI.Button {
    [CustomEditor(typeof(ButtonBaseFactory))]
    [CanEditMultipleObjects]

    public abstract class ButtonBaseFactoryEditor : BaseFactoryEditor {
        SerializedProperty parent;
        SerializedProperty buttonText;
        SerializedProperty buttonProfile;
        SerializedProperty alignment;
        SerializedProperty panel;

        protected override void OnEnable() {
            base.OnEnable();
            parent = serializedObject.FindProperty("parent");
            buttonText = serializedObject.FindProperty("buttonText");
            buttonProfile = serializedObject.FindProperty("buttonProfile");
            alignment = serializedObject.FindProperty("alignment");
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
            EditorGUILayout.PropertyField(buttonProfile);
            EditorGUILayout.PropertyField(alignment);
            EditorGUILayout.PropertyField(panel);
        }
    }
}

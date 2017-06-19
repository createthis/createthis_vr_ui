#if COLOR_PICKER
using UnityEngine;
using UnityEditor;

namespace CreateThis.Factory.VR.UI.ColorPicker {
    [CustomEditor(typeof(ColorPickerFactory))]
    [CanEditMultipleObjects]

    public class ColorPickerFactoryEditor : BaseFactoryEditor {
        SerializedProperty parent;
        SerializedProperty colorPickerProfile;

        protected override void OnEnable() {
            base.OnEnable();
            parent = serializedObject.FindProperty("parent");
            colorPickerProfile = serializedObject.FindProperty("colorPickerProfile");
        }

        protected override void BuildGenerateButton() {
            // Take out this if statement to set the value using setter when ever you change it in the inspector.
            // But then it gets called a couple of times when ever inspector updates
            // By having a button, you can control when the value goes through the setter and getter, your self.
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(ColorPickerFactory)) {
                    ColorPickerFactory factory = (ColorPickerFactory)target;
                    factory.Generate();
                }
            }
        }

        protected override void AdditionalProperties() {
            base.AdditionalProperties();
            EditorGUILayout.PropertyField(parent);
            EditorGUILayout.PropertyField(colorPickerProfile);
        }
    }
}
#endif
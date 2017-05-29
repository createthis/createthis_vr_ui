using UnityEngine;
using UnityEditor;

namespace CreateThis.Factory.VR.UI.Button {
    [CustomEditor(typeof(KeyboardButtonFactory))]
    [CanEditMultipleObjects]
    public abstract class KeyboardButtonFactoryEditor : MomentaryButtonFactoryEditor {
        SerializedProperty keyboard;

        protected override void OnEnable() {
            base.OnEnable();
            keyboard = serializedObject.FindProperty("keyboard");
        }

        protected override void BuildGenerateButton() {
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(KeyboardButtonFactory)) {
                    KeyboardButtonFactory buttonFactory = (KeyboardButtonFactory)target;
                    buttonFactory.Generate();
                }
            }
        }

        protected override void AdditionalProperties() {
            base.AdditionalProperties();
            EditorGUILayout.PropertyField(keyboard);
        }
    }
}

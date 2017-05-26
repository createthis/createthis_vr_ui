using UnityEngine;
using UnityEditor;

namespace CreateThis.Factory.VR.UI.Button {
    [CustomEditor(typeof(KeyboardMomentaryKeyButtonFactory))]
    [CanEditMultipleObjects]
    public class KeyboardMomentaryKeyButtonFactoryEditor : KeyboardButtonFactoryEditor {
        SerializedProperty keyboard;
        SerializedProperty value;

        protected override void OnEnable() {
            base.OnEnable();
            keyboard = serializedObject.FindProperty("keyboard");
            value = serializedObject.FindProperty("value");
        }

        protected override void BuildGenerateButton() {
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(KeyboardMomentaryKeyButtonFactory)) {
                    KeyboardMomentaryKeyButtonFactory buttonFactory = (KeyboardMomentaryKeyButtonFactory)target;
                    buttonFactory.Generate();
                }
            }
        }

        protected override void AdditionalProperties() {
            EditorGUILayout.PropertyField(keyboard);
            EditorGUILayout.PropertyField(value);
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
        }
    }
}

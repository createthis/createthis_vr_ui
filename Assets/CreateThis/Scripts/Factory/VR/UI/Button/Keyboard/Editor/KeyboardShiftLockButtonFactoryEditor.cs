using UnityEngine;
using UnityEditor;

namespace CreateThis.Factory.VR.UI.Button {
    [CustomEditor(typeof(KeyboardShiftLockButtonFactory))]
    [CanEditMultipleObjects]
    public class KeyboardShiftLockButtonFactoryEditor : KeyboardButtonFactoryEditor {
        SerializedProperty on;

        protected override void OnEnable() {
            base.OnEnable();
            on = serializedObject.FindProperty("on");
        }

        protected override void BuildGenerateButton() {
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(KeyboardShiftLockButtonFactory)) {
                    KeyboardShiftLockButtonFactory buttonFactory = (KeyboardShiftLockButtonFactory)target;
                    buttonFactory.Generate();
                }
            }
        }

        protected override void AdditionalProperties() {
            base.AdditionalProperties();
            EditorGUILayout.PropertyField(on);
        }
    }
}

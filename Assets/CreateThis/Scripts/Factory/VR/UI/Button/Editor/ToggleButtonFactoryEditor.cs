using UnityEngine;
using UnityEditor;

namespace CreateThis.Factory.VR.UI.Button {
    [CustomEditor(typeof(ToggleButtonFactory))]
    [CanEditMultipleObjects]
    public class ToggleButtonFactoryEditor : ButtonBaseFactoryEditor {
        SerializedProperty on;

        protected override void OnEnable() {
            base.OnEnable();
            on = serializedObject.FindProperty("on");
        }

        protected override void BuildGenerateButton() {
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(ToggleButtonFactory)) {
                    ToggleButtonFactory buttonFactory = (ToggleButtonFactory)target;
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
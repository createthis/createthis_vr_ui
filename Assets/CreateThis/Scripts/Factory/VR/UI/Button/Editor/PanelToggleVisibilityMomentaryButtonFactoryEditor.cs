using UnityEngine;
using UnityEditor;

namespace CreateThis.Factory.VR.UI.Button {
    [CustomEditor(typeof(PanelToggleVisibilityMomentaryButtonFactory))]
    [CanEditMultipleObjects]
    public class PanelToggleVisibilityMomentaryButtonFactoryEditor : MomentaryButtonFactoryEditor {
        SerializedProperty panelToToggle;

        protected override void OnEnable() {
            base.OnEnable();
            panelToToggle = serializedObject.FindProperty("panelToToggle");
        }

        protected override void BuildGenerateButton() {
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(PanelToggleVisibilityMomentaryButtonFactory)) {
                    PanelToggleVisibilityMomentaryButtonFactory buttonFactory = (PanelToggleVisibilityMomentaryButtonFactory)target;
                    buttonFactory.Generate();
                }
            }
        }

        protected override void AdditionalProperties() {
            base.AdditionalProperties();
            EditorGUILayout.PropertyField(panelToToggle);
        }
    }
}

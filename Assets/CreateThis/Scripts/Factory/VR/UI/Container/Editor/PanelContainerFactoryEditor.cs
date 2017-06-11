using UnityEngine;
using UnityEditor;

namespace CreateThis.Factory.VR.UI.Container {
    [CustomEditor(typeof(PanelContainerFactory))]
    [CanEditMultipleObjects]
    public class PanelContainerFactoryEditor : ContainerBaseFactoryEditor {
        SerializedProperty panelContainerProfile;

        protected override void OnEnable() {
            base.OnEnable();
            panelContainerProfile = serializedObject.FindProperty("panelContainerProfile");
        }

        protected override void BuildGenerateButton() {
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(PanelContainerFactory)) {
                    PanelContainerFactory containerFactory = (PanelContainerFactory)target;
                    containerFactory.Generate();
                }
            }
        }

        protected override void AdditionalProperties() {
            EditorGUILayout.PropertyField(panelContainerProfile);
        }
    }
}

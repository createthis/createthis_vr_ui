using UnityEngine;
using UnityEditor;

namespace CreateThis.Factory.VR.UI.Container {
    [CustomEditor(typeof(RowContainerFactory))]
    [CanEditMultipleObjects]
    public class RowContainerFactoryEditor : ContainerBaseFactoryEditor {
        SerializedProperty alignment;

        protected override void OnEnable() {
            base.OnEnable();
            alignment = serializedObject.FindProperty("alignment");
        }

        protected override void BuildGenerateButton() {
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(RowContainerFactory)) {
                    RowContainerFactory containerFactory = (RowContainerFactory)target;
                    containerFactory.Generate();
                }
            }
        }

        protected override void AdditionalProperties() {
            EditorGUILayout.PropertyField(alignment);
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
        }
    }
}

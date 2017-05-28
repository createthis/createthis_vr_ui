using UnityEngine;
using UnityEditor;

namespace CreateThis.Factory.VR.UI.Button {
    [CustomEditor(typeof(DriveButtonFactory))]
    [CanEditMultipleObjects]
    public class DriveButtonFactoryEditor : MomentaryButtonFactoryEditor {
        SerializedProperty filePanel;

        protected override void OnEnable() {
            base.OnEnable();
            filePanel = serializedObject.FindProperty("filePanel");
        }

        protected override void BuildGenerateButton() {
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(DriveButtonFactory)) {
                    DriveButtonFactory buttonFactory = (DriveButtonFactory)target;
                    buttonFactory.Generate();
                }
            }
        }

        protected override void AdditionalProperties() {
            EditorGUILayout.PropertyField(filePanel);
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
        }
    }
}

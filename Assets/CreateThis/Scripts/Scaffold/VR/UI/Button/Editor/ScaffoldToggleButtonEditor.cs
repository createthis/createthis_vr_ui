using UnityEngine;
using UnityEditor;

namespace CreateThis.Scaffold.VR.UI.Button {
    [CustomEditor(typeof(ScaffoldToggleButton))]
    [CanEditMultipleObjects]
    public class ScaffoldToggleButtonEditor : ScaffoldButtonBaseEditor {
        SerializedProperty on;

        protected override void OnEnable() {
            base.OnEnable();
            on = serializedObject.FindProperty("on");

        }

        protected override void BuildGenerateButton() {
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(ScaffoldToggleButton)) {
                    ScaffoldToggleButton scaffoldButton = (ScaffoldToggleButton)target;
                    scaffoldButton.Generate();
                }
            }
        }

        protected override void AdditionalProperties() {
            EditorGUILayout.PropertyField(on);
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
        }
    }
}

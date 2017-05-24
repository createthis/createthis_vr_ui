using UnityEngine;
using UnityEditor;

namespace CreateThis.Scaffold.VR.UI.Button {
    [CustomEditor(typeof(ScaffoldMomentaryButton))]
    [CanEditMultipleObjects]
    public class ScaffoldMomentaryButtonEditor : ScaffoldButtonBaseEditor {

        protected override void BuildGenerateButton() {
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(ScaffoldMomentaryButton)) {
                    ScaffoldMomentaryButton scaffoldButton = (ScaffoldMomentaryButton)target;
                    scaffoldButton.Generate();
                }
            }
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
        }
    }
}

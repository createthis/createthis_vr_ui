using UnityEngine;
using UnityEditor;

namespace CreateThis.Factory.VR.UI.Button {
    [CustomEditor(typeof(MomentaryButtonFactory))]
    [CanEditMultipleObjects]
    public class MomentaryButtonFactoryEditor : ButtonBaseFactoryEditor {

        protected override void BuildGenerateButton() {
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(MomentaryButtonFactory)) {
                    MomentaryButtonFactory buttonFactory = (MomentaryButtonFactory)target;
                    buttonFactory.Generate();
                }
            }
        }        
    }
}

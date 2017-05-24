using UnityEngine;
using UnityEditor;

namespace CreateThis.Factory.VR.UI.Container {
    [CustomEditor(typeof(ColumnContainerFactory))]
    [CanEditMultipleObjects]
    public class ColumnContainerFactoryEditor : ContainerBaseFactoryEditor {
        protected override void BuildGenerateButton() {
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(ColumnContainerFactory)) {
                    ColumnContainerFactory containerFactory = (ColumnContainerFactory)target;
                    containerFactory.Generate();
                }
            }
        }
    }
}

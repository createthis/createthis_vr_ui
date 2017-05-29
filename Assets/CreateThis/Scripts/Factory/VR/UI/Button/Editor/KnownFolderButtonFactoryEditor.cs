using UnityEngine;
using UnityEditor;

namespace CreateThis.Factory.VR.UI.Button {
    [CustomEditor(typeof(KnownFolderButtonFactory))]
    [CanEditMultipleObjects]
    public class KnownFolderButtonFactoryEditor : MomentaryButtonFactoryEditor {
        SerializedProperty filePanel;
        SerializedProperty knownFolder;

        protected override void OnEnable() {
            base.OnEnable();
            filePanel = serializedObject.FindProperty("filePanel");
            knownFolder = serializedObject.FindProperty("knownFolder");
        }

        protected override void BuildGenerateButton() {
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(KnownFolderButtonFactory)) {
                    MomentaryButtonFactory buttonFactory = (KnownFolderButtonFactory)target;
                    buttonFactory.Generate();
                }
            }
        }

        protected override void AdditionalProperties() {
            base.AdditionalProperties();
            EditorGUILayout.PropertyField(filePanel);
            EditorGUILayout.PropertyField(knownFolder);
        }
    }
}

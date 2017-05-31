using UnityEngine;
using UnityEditor;

namespace CreateThis.Factory.VR.UI.Button {
    [CustomEditor(typeof(SkyboxButtonFactory))]
    [CanEditMultipleObjects]
    public class SkyboxButtonFactoryEditor : ToggleButtonFactoryEditor {
        SerializedProperty skybox;
        SerializedProperty skyboxManager;

        protected override void OnEnable() {
            base.OnEnable();
            skybox = serializedObject.FindProperty("skybox");
            skyboxManager = serializedObject.FindProperty("skyboxManager");
        }

        protected override void BuildGenerateButton() {
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(SkyboxButtonFactory)) {
                    SkyboxButtonFactory buttonFactory = (SkyboxButtonFactory)target;
                    buttonFactory.Generate();
                }
            }
        }

        protected override void AdditionalProperties() {
            base.AdditionalProperties();
            EditorGUILayout.PropertyField(skybox);
            EditorGUILayout.PropertyField(skyboxManager);
        }
    }
}

using UnityEngine;
using UnityEditor;
using CreateThis.Factory.VR.UI.Button;

namespace CreateThis.Example {
    [CustomEditor(typeof(ExampleSkyboxButtonFactory))]
    [CanEditMultipleObjects]
    public class ExampleSkyboxButtonFactoryEditor : ToggleButtonFactoryEditor {
        SerializedProperty skybox;
        SerializedProperty skyboxManager;

        protected override void OnEnable() {
            base.OnEnable();
            skybox = serializedObject.FindProperty("skybox");
            skyboxManager = serializedObject.FindProperty("skyboxManager");
        }

        protected override void BuildGenerateButton() {
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(ExampleSkyboxButtonFactory)) {
                    ExampleSkyboxButtonFactory buttonFactory = (ExampleSkyboxButtonFactory)target;
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
using UnityEngine;
using UnityEditor;
using CreateThis.Factory;

namespace CreateThis.Example {
    [CustomEditor(typeof(ExampleMasterUIFactory))]
    [CanEditMultipleObjects]

    public class MMVR_MasterUIFactoryEditor : BaseFactoryEditor {
        SerializedProperty parent;
        SerializedProperty skyboxManager;
        SerializedProperty touchPadMenuController;

        protected override void OnEnable() {
            base.OnEnable();
            parent = serializedObject.FindProperty("parent");
            skyboxManager = serializedObject.FindProperty("skyboxManager");
            touchPadMenuController = serializedObject.FindProperty("touchPadMenuController");
        }

        protected override void BuildGenerateButton() {
            // Take out this if statement to set the value using setter when ever you change it in the inspector.
            // But then it gets called a couple of times when ever inspector updates
            // By having a button, you can control when the value goes through the setter and getter, your self.
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(ExampleMasterUIFactory)) {
                    ExampleMasterUIFactory factory = (ExampleMasterUIFactory)target;
                    factory.Generate();
                }
            }
        }

        protected override void AdditionalProperties() {
            base.AdditionalProperties();
            EditorGUILayout.PropertyField(parent);
            EditorGUILayout.PropertyField(skyboxManager);
            EditorGUILayout.PropertyField(touchPadMenuController);
        }
    }
}
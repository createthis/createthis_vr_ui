using UnityEngine;
using UnityEditor;

namespace CreateThis.Factory.VR.UI.Container {
    [CustomEditor(typeof(ContainerBaseFactory))]
    [CanEditMultipleObjects]

    public abstract class ContainerBaseFactoryEditor : Editor {
        SerializedProperty padding;
        SerializedProperty spacing;
        SerializedProperty parent;

        protected virtual void OnEnable() {
            padding = serializedObject.FindProperty("padding");
            spacing = serializedObject.FindProperty("spacing");
            parent = serializedObject.FindProperty("parent");
        }

        protected virtual void BuildGenerateButton() {
            // Take out this if statement to set the value using setter when ever you change it in the inspector.
            // But then it gets called a couple of times when ever inspector updates
            // By having a button, you can control when the value goes through the setter and getter, your self.
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(ContainerBaseFactory)) {
                    ContainerBaseFactory containerFactory = (ContainerBaseFactory)target;
                    containerFactory.Generate();
                }
            }
        }

        protected virtual void AdditionalProperties() {
            // put your properties here in the override
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorGUILayout.PropertyField(padding);
            EditorGUILayout.PropertyField(spacing);
            EditorGUILayout.PropertyField(parent);
            AdditionalProperties();

            serializedObject.ApplyModifiedProperties();

            BuildGenerateButton();
        }
    }
}

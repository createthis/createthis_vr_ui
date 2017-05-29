using UnityEngine;
using UnityEditor;

namespace CreateThis.Factory {
    [CustomEditor(typeof(BaseFactory))]
    [CanEditMultipleObjects]

    public abstract class BaseFactoryEditor : Editor {
        SerializedProperty useVRTK;

        protected virtual void OnEnable() {
            useVRTK = serializedObject.FindProperty("useVRTK");
        }

        protected virtual void BuildGenerateButton() {
            // Take out this if statement to set the value using setter when ever you change it in the inspector.
            // But then it gets called a couple of times when ever inspector updates
            // By having a button, you can control when the value goes through the setter and getter, your self.
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(BaseFactory)) {
                    BaseFactory factory = (BaseFactory)target;
                    factory.Generate();
                }
            }
        }

        protected virtual void AdditionalProperties() {
            EditorGUILayout.PropertyField(useVRTK);
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            AdditionalProperties();

            serializedObject.ApplyModifiedProperties();

            BuildGenerateButton();
        }
    }
}

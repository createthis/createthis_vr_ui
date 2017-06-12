using UnityEngine;
using UnityEditor;

namespace CreateThis.Factory.VR.UI {
    [CustomEditor(typeof(KeyboardFactory))]
    [CanEditMultipleObjects]

    public class KeyboardFactoryEditor : BaseFactoryEditor {
        SerializedProperty parent;
        SerializedProperty panelProfile;
        SerializedProperty panelContainerProfile;
        SerializedProperty momentaryButtonProfile;
        SerializedProperty toggleButtonProfile;
        SerializedProperty keyMinWidth;
        SerializedProperty keyCharacterSize;
        SerializedProperty numLockCharacterSize;
        SerializedProperty spaceMinWidth;
        SerializedProperty returnMinWidth;
        SerializedProperty spacerWidth;
        SerializedProperty modeKeyMinWidth;
        SerializedProperty wideKeyMinWidth;

        protected override void OnEnable() {
            base.OnEnable();
            parent = serializedObject.FindProperty("parent");
            panelProfile = serializedObject.FindProperty("panelProfile");
            panelContainerProfile = serializedObject.FindProperty("panelContainerProfile");
            momentaryButtonProfile = serializedObject.FindProperty("momentaryButtonProfile");
            toggleButtonProfile = serializedObject.FindProperty("toggleButtonProfile");
            keyMinWidth = serializedObject.FindProperty("keyMinWidth");
            keyCharacterSize = serializedObject.FindProperty("keyCharacterSize");
            numLockCharacterSize = serializedObject.FindProperty("numLockCharacterSize");
            spaceMinWidth = serializedObject.FindProperty("spaceMinWidth");
            returnMinWidth = serializedObject.FindProperty("returnMinWidth");
            spacerWidth = serializedObject.FindProperty("spacerWidth");
            modeKeyMinWidth = serializedObject.FindProperty("modeKeyMinWidth");
            wideKeyMinWidth = serializedObject.FindProperty("wideKeyMinWidth");
        }

        protected override void BuildGenerateButton() {
            // Take out this if statement to set the value using setter when ever you change it in the inspector.
            // But then it gets called a couple of times when ever inspector updates
            // By having a button, you can control when the value goes through the setter and getter, your self.
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(KeyboardFactory)) {
                    KeyboardFactory factory = (KeyboardFactory)target;
                    factory.Generate();
                }
            }
        }

        protected override void AdditionalProperties() {
            base.AdditionalProperties();
            EditorGUILayout.PropertyField(parent);
            EditorGUILayout.PropertyField(panelProfile);
            EditorGUILayout.PropertyField(panelContainerProfile);
            EditorGUILayout.PropertyField(momentaryButtonProfile);
            EditorGUILayout.PropertyField(toggleButtonProfile);
            EditorGUILayout.PropertyField(keyMinWidth);
            EditorGUILayout.PropertyField(keyCharacterSize);
            EditorGUILayout.PropertyField(numLockCharacterSize);
            EditorGUILayout.PropertyField(spaceMinWidth);
            EditorGUILayout.PropertyField(returnMinWidth);
            EditorGUILayout.PropertyField(spacerWidth);
            EditorGUILayout.PropertyField(modeKeyMinWidth);
            EditorGUILayout.PropertyField(wideKeyMinWidth);
        }
    }
}
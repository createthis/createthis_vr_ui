using UnityEngine;
using UnityEditor;

namespace CreateThis.VR.UI.Button {
    [CustomEditor(typeof(ToggleButton))]
    [CanEditMultipleObjects]

    public class ToggleButtonEditor : Editor {
        SerializedProperty buttonClickDown;
        SerializedProperty buttonClickUp;
        SerializedProperty buttonBody;
        SerializedProperty buttonText;
        SerializedProperty onClick;
        SerializedProperty onClickBool;
        SerializedProperty on;
        SerializedProperty log;
        SerializedProperty clickOnTriggerExit;

        void OnEnable() {
            buttonClickDown = serializedObject.FindProperty("buttonClickDown");
            buttonClickUp = serializedObject.FindProperty("buttonClickUp");
            buttonBody = serializedObject.FindProperty("buttonBody");
            buttonText = serializedObject.FindProperty("buttonText");
            onClick = serializedObject.FindProperty("onClick");
            onClickBool = serializedObject.FindProperty("onClickBool");
            on = serializedObject.FindProperty("on");
            log = serializedObject.FindProperty("log");
            clickOnTriggerExit = serializedObject.FindProperty("clickOnTriggerExit");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorGUILayout.PropertyField(buttonClickDown);
            EditorGUILayout.PropertyField(buttonClickUp);
            EditorGUILayout.PropertyField(buttonBody);
            EditorGUILayout.PropertyField(buttonText);
            EditorGUILayout.PropertyField(onClick);
            EditorGUILayout.PropertyField(onClickBool);
            EditorGUILayout.PropertyField(clickOnTriggerExit);
            EditorGUILayout.PropertyField(log);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(on);
            if (EditorGUI.EndChangeCheck()) {
                if (target.GetType() == typeof(ToggleButton)) {
                    ToggleButton physicalToggleButtonController = (ToggleButton)target;
                    physicalToggleButtonController.On = on.boolValue;
                }
            }

            serializedObject.ApplyModifiedProperties();

            // Take out this if statement to set the value using setter when ever you change it in the inspector.
            // But then it gets called a couple of times when ever inspector updates
            // By having a button, you can control when the value goes through the setter and getter, your self.
            if (GUILayout.Button("Force Reinitialize")) {
                if (target.GetType() == typeof(ToggleButton)) {
                    ToggleButton toggleButton = (ToggleButton)target;
                    toggleButton.Initialize(true);
                }
            }
        }
    }
}

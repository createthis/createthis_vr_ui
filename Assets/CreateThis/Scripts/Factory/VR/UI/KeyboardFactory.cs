using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using CreateThis.Factory.VR.UI.Button;
using CreateThis.Factory.VR.UI.Container;
using CreateThis.VR.UI;
using CreateThis.VR.UI.Panel;

namespace CreateThis.Factory.VR.UI {
    public class KeyboardFactory : BaseFactory {
        public GameObject parent;
        public GameObject buttonBody;
        public Material buttonMaterial;
        public Material panelMaterial;
        public Material highlight;
        public Material outline;
        public AudioClip buttonClickDown;
        public AudioClip buttonClickUp;
        public int fontSize;
        public Color fontColor;
        public float labelZ;
        public float buttonZ;
        public Vector3 bodyScale;
        public Vector3 labelScale;
        public float padding;
        public float spacing;

        protected Keyboard keyboard;
        protected GameObject keyboardInstance;
        protected GameObject panelLowerCase;
        protected GameObject panelUpperCase;
        protected GameObject panelNumber;
        protected GameObject panelSymbol;
        private GameObject disposable;

        protected GameObject Button(GameObject parent, string buttonText) {
            MomentaryButtonFactory factory = SafeAddComponent<MomentaryButtonFactory>(disposable);
            factory.parent = parent;
            factory.buttonBody = buttonBody;
            factory.buttonText = buttonText;
            factory.material = buttonMaterial;
            factory.highlight = highlight;
            factory.outline = outline;
            factory.buttonClickDown = buttonClickDown;
            factory.buttonClickUp = buttonClickUp;
            factory.alignment = TextAlignment.Left;
            factory.fontSize = fontSize;
            factory.fontColor = fontColor;
            factory.labelZ = labelZ;
            factory.bodyScale = bodyScale;
            factory.labelScale = labelScale;
            GameObject button = factory.Generate();
            Vector3 localPosition = button.transform.localPosition;
            localPosition.z = buttonZ;
            button.transform.localPosition = localPosition;
            return button;
        }

        protected GameObject Row(GameObject parent) {
            RowContainerFactory factory = SafeAddComponent<RowContainerFactory>(disposable);
            factory.parent = parent;
            factory.padding = padding;
            factory.spacing = spacing;
            factory.alignment = TextAlignment.Center;
            return factory.Generate();
        }

        protected GameObject Column(GameObject parent) {
            ColumnContainerFactory factory = SafeAddComponent<ColumnContainerFactory>(disposable);
            factory.parent = parent;
            factory.padding = padding;
            factory.spacing = spacing;
            return factory.Generate();
        }

        protected GameObject Panel(GameObject parent) {
            PanelContainerFactory factory = SafeAddComponent<PanelContainerFactory>(disposable);
            factory.parent = parent;
            factory.panelBody = buttonBody;
            factory.material = panelMaterial;
            factory.highlight = highlight;
            factory.outline = outline;
            factory.fontColor = fontColor;
            factory.bodyScale = bodyScale;
            return factory.Generate();
        }

        protected GameObject ButtonRow(GameObject parent, List<string> buttonLabels) {
            GameObject row = Row(parent);
            foreach (string buttonLabel in buttonLabels) {
                Button(row, buttonLabel);
            }
            return row;
        }

        private void CreateDisposable(GameObject parent) {
            if (disposable) return;
            disposable = new GameObject();
#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(disposable, "Created Disposable");
#endif
            disposable.name = "disposable";
            disposable.transform.parent = parent.transform;
        }

        protected void CreateKeyboard(GameObject parent) {
            if (keyboardInstance) return;
            keyboardInstance = new GameObject();
#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(keyboardInstance, "Created Button");
#endif
            keyboardInstance.name = "keyboard";
            keyboardInstance.transform.parent = parent.transform;
            keyboardInstance.transform.localPosition = Vector3.zero;
            keyboardInstance.transform.localRotation = Quaternion.identity;

            keyboard = SafeAddComponent<Keyboard>(keyboardInstance);
        }

        protected void PanelLowerCase(GameObject parent) {
            if (panelLowerCase) return;
            Debug.Log("parent=" + parent);

            GameObject panel = Panel(parent);
            GameObject column = Column(panel);
            GameObject row1 = ButtonRow(column, new List<string> {
                "q", "w", "e", "r", "t", "y", "u", "i", "o", "p"
            });
            GameObject row2 = ButtonRow(column, new List<string> {
                "a", "s", "d", "f", "g", "h", "j", "k", "l"
            });
            GameObject row3 = ButtonRow(column, new List<string> {
                "⇧", "z", "x", "c", "v", "b", "n", "m", "⌫"
            });
            GameObject row4 = ButtonRow(column, new List<string> {
                "123", "", "space", "return"
            });

            keyboard.panelLowerCase = panel.GetComponent<StandardPanel>();
            panelLowerCase = panel;
        }

        public override GameObject Generate() {
            base.Generate();

#if UNITY_EDITOR
            Undo.SetCurrentGroupName("KeyboardFactory Generate");
            int group = Undo.GetCurrentGroup();

            Undo.RegisterCompleteObjectUndo(this, "KeyboardFactory state");
#endif
            CreateDisposable(parent);
            CreateKeyboard(parent);
            Debug.Log("keyboardInstance=" + keyboardInstance);
            PanelLowerCase(keyboardInstance);
            //panelUpperCase = Panel(keyboard);
            //panelNumber = Panel(keyboard);
            //panelSymbol = Panel(keyboard);

#if UNITY_EDITOR
            Undo.DestroyObjectImmediate(disposable);
            Undo.CollapseUndoOperations(group);
#else
            Destroy(disposable);
#endif
            return keyboardInstance;
        }
    }
}

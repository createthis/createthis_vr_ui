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

        protected GameObject KeyboardButton(GameObject parent, string buttonText) {
            KeyboardButtonFactory factory = SafeAddComponent<KeyboardButtonFactory>(disposable);
            factory.parent = parent;
            factory.buttonBody = buttonBody;
            factory.buttonText = buttonText;
            factory.material = buttonMaterial;
            factory.highlight = highlight;
            factory.outline = outline;
            factory.buttonClickDown = buttonClickDown;
            factory.buttonClickUp = buttonClickUp;
            factory.alignment = TextAlignment.Center;
            factory.fontSize = fontSize;
            factory.fontColor = fontColor;
            factory.labelZ = labelZ;
            factory.bodyScale = bodyScale;
            factory.labelScale = labelScale;
            factory.keyboard = keyboard;
            factory.value = buttonText;
            GameObject button = factory.Generate();
            Vector3 localPosition = button.transform.localPosition;
            localPosition.z = buttonZ;
            button.transform.localPosition = localPosition;
            return button;
        }

        protected GameObject Row(GameObject parent, string name = null, TextAlignment alignment = TextAlignment.Center) {
            RowContainerFactory factory = SafeAddComponent<RowContainerFactory>(disposable);
            factory.containerName = name;
            factory.parent = parent;
            factory.padding = padding;
            factory.spacing = spacing;
            factory.alignment = alignment;
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

        protected GameObject DisplayRow(GameObject parent) {
            GameObject row = Row(parent, "DisplayRow", TextAlignment.Left);
            GameObject label = EmptyChild(row, "Display");
            label.transform.localScale = labelScale;
            Vector3 localPosition = new Vector3(0, 0, labelZ);
            label.transform.localPosition = localPosition;
            TextMesh textMesh = SafeAddComponent<TextMesh>(label);
            textMesh.text = "keyboard display";
            textMesh.fontSize = fontSize;
            textMesh.color = fontColor;
            textMesh.anchor = TextAnchor.MiddleCenter;

            KeyboardLabel keyboardLabel = SafeAddComponent<KeyboardLabel>(label);
            keyboardLabel.keyboard = keyboard;
            keyboardLabel.textMesh = textMesh;

            return row;
        }

        protected GameObject ButtonRow(GameObject parent, List<string> buttonLabels) {
            GameObject row = Row(parent);
            foreach (string buttonLabel in buttonLabels) {
                KeyboardButton(row, buttonLabel);
            }
            return row;
        }

        private void CreateDisposable(GameObject parent) {
            if (disposable) return;
            disposable = EmptyChild(parent, "disposable");
        }

        protected void CreateKeyboard(GameObject parent) {
            if (keyboardInstance) return;
            keyboardInstance = EmptyChild(parent, "keyboard");

            keyboard = SafeAddComponent<Keyboard>(keyboardInstance);
        }

        protected void PanelLowerCase(GameObject parent) {
            if (panelLowerCase) return;

            GameObject panel = Panel(parent);
            StandardPanel standardPanel = SafeAddComponent<StandardPanel>(panel);

            GameObject column = Column(panel);

            DisplayRow(column);

            ButtonRow(column, new List<string> {
                "q", "w", "e", "r", "t", "y", "u", "i", "o", "p"
            });
            ButtonRow(column, new List<string> {
                "a", "s", "d", "f", "g", "h", "j", "k", "l"
            });
            ButtonRow(column, new List<string> {
                "⇧", "z", "x", "c", "v", "b", "n", "m", "⌫"
            });
            ButtonRow(column, new List<string> {
                "123", "", "space", "return"
            });

            panelLowerCase = panel;
            keyboard.panelLowerCase = panelLowerCase.GetComponent<StandardPanel>();
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

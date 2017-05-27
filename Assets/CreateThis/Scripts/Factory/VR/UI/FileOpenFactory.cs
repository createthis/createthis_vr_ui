using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using CreateThis.Factory.VR.UI.Button;
using CreateThis.Factory.VR.UI.Container;
using CreateThis.VR.UI;
using CreateThis.VR.UI.Panel;
using CreateThis.VR.UI.Container;

namespace CreateThis.Factory.VR.UI {
    public class FileOpenFactory : KeyboardKey {
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
        public float buttonPadding;
        public float keyMinWidth;
        public float keyCharacterSize;

        protected GameObject fileOpenPanel;
        private GameObject disposable;

        protected void SetKeyboardButtonValues(KeyboardButtonFactory factory, StandardPanel panel, GameObject parent) {
            factory.parent = parent;
            factory.buttonBody = buttonBody;
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
            factory.minWidth = keyMinWidth;
            factory.padding = buttonPadding;
            factory.characterSize = keyCharacterSize;
            factory.panel = panel;
        }

        protected void SetKeyboardButtonPosition(GameObject button) {
            Vector3 localPosition = button.transform.localPosition;
            localPosition.z = buttonZ;
            button.transform.localPosition = localPosition;
        }

        protected GameObject GenerateKeyboardButtonAndSetPosition(KeyboardButtonFactory factory) {
            GameObject button = factory.Generate();
            SetKeyboardButtonPosition(button);
            return button;
        }

        protected GameObject KeyboardButton(StandardPanel panel, GameObject parent, string buttonText, float minWidth = -1) {
            KeyboardMomentaryKeyButtonFactory factory = SafeAddComponent<KeyboardMomentaryKeyButtonFactory>(disposable);
            SetKeyboardButtonValues(factory, panel, parent);
            factory.buttonText = buttonText;
            factory.value = buttonText;
            if (minWidth != -1) factory.minWidth = minWidth;
            return GenerateKeyboardButtonAndSetPosition(factory);
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

        protected GameObject Panel(GameObject parent, string name) {
            PanelContainerFactory factory = SafeAddComponent<PanelContainerFactory>(disposable);
            factory.parent = parent;
            factory.containerName = name;
            factory.panelBody = buttonBody;
            factory.material = panelMaterial;
            factory.highlight = highlight;
            factory.outline = outline;
            factory.fontColor = fontColor;
            factory.bodyScale = bodyScale;
            GameObject panel = factory.Generate();

            StandardPanel standardPanel = SafeAddComponent<StandardPanel>(panel);
            standardPanel.grabTarget = standardPanel.transform;

            Rigidbody rigidbody = SafeAddComponent<Rigidbody>(panel);
            rigidbody.isKinematic = true;

            return panel;
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

            //FIXME: file open path
            KeyboardLabel keyboardLabel = SafeAddComponent<KeyboardLabel>(label);
            keyboardLabel.textMesh = textMesh;

            BoxCollider boxCollider = SafeAddComponent<BoxCollider>(label);

            UpdateBoxColliderFromTextMesh updateBoxCollider = SafeAddComponent<UpdateBoxColliderFromTextMesh>(label);
            updateBoxCollider.textMesh = textMesh;
            updateBoxCollider.boxCollider = boxCollider;

            return row;
        }

        protected void ButtonByKey(StandardPanel panel, GameObject parent, Key key) {
            switch (key.type) {
                case KeyType.Key:
                    KeyboardButton(panel, parent, key.value);
                    break;
                default:
                    Debug.Log("unhandled key type=" + key.type);
                    break;
            }
        }

        protected GameObject ButtonRow(StandardPanel panel, GameObject parent, List<Key> keys, TextAlignment alignment = TextAlignment.Center) {
            GameObject row = Row(parent, null, alignment);
            foreach (Key key in keys) {
                ButtonByKey(panel, row, key);
            }
            return row;
        }

        private void CreateDisposable(GameObject parent) {
            if (disposable) return;
            disposable = EmptyChild(parent, "disposable");
        }

        protected void PanelHeader(StandardPanel panel, GameObject parent) {
            ButtonRow(panel, parent, new List<Key> {
                Key.Done("done")
            }, TextAlignment.Right);

            DisplayRow(parent);
        }

        protected void FileOpenPanel(GameObject parent) {
            if (fileOpenPanel) return;

            GameObject panel = Panel(parent, "FileOpenPanel");
            GameObject column = Column(panel);
            StandardPanel standardPanel = panel.GetComponent<StandardPanel>();

            PanelHeader(standardPanel, column);

            // FIXME: Add label
            // FIXME: Add drive buttons
            // FIXME: Add special buttons
            ButtonRow(standardPanel, column, new List<Key> {
                K("q"), K("w"), K("e"), K("r"), K("t"), K("y"), K("u"), K("i"), K("o"), K("p")
            });

            fileOpenPanel = panel;
        }

        public override GameObject Generate() {
            base.Generate();

#if UNITY_EDITOR
            Undo.SetCurrentGroupName("FileOpenFactory Generate");
            int group = Undo.GetCurrentGroup();

            Undo.RegisterCompleteObjectUndo(this, "FileOpenFactory state");
#endif
            CreateDisposable(parent);
            
            FileOpenPanel(parent);

#if UNITY_EDITOR
            Undo.DestroyObjectImmediate(disposable);
            Undo.CollapseUndoOperations(group);
#else
            Destroy(disposable);
#endif
            return fileOpenPanel;
        }
    }
}
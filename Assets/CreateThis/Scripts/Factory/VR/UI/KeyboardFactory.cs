using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using CreateThis.Factory.VR.UI.Button;
using CreateThis.Factory.VR.UI.Container;
using CreateThis.VR.UI;
#if VRTK
using CreateThis.VRTK;
#endif
using CreateThis.VR.UI.Panel;
using CreateThis.VR.UI.Container;

namespace CreateThis.Factory.VR.UI {
    public class KeyboardFactory : KeyboardKey {
        public GameObject parent;
        public PanelProfile panelProfile;
        public PanelContainerProfile panelContainerProfile;
        public ButtonProfile momentaryButtonProfile;
        public ButtonProfile toggleButtonProfile;
        public float buttonZ;
        public float padding;
        public float spacing;
        public float keyMinWidth;
        public float keyCharacterSize;
        public float numLockCharacterSize;
        public float spaceMinWidth;
        public float returnMinWidth;
        public float spacerWidth;
        public float modeKeyMinWidth;
        public float wideKeyMinWidth;

        private Keyboard keyboard;
        private GameObject keyboardInstance;
        private GameObject panelLowerCase;
        private GameObject panelUpperCase;
        private GameObject panelNumber;
        private GameObject panelSymbol;
        private GameObject disposable;
        private ButtonProfile keyProfile;
        private ButtonProfile wideKeyProfile;
        private ButtonProfile spaceKeyProfile;
        private ButtonProfile modeKeyProfile;
        private ButtonProfile numLockKeyProfile;
        private ButtonProfile returnKeyProfile;

        private static T CopyComponent<T>(T original, GameObject destination) where T : Component {
            global::System.Type type = original.GetType();
            Component copy = destination.AddComponent(type);
            global::System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (global::System.Reflection.FieldInfo field in fields) {
                field.SetValue(copy, field.GetValue(original));
            }
            return copy as T;
        }

        private PanelProfile CreateSubPanelProfile(GameObject parent) {
            PanelProfile profile = Defaults.GetProfile(panelProfile);
            PanelProfile subPanelProfile = CopyComponent(profile, parent);
            subPanelProfile.hideOnAwake = false;
            return subPanelProfile;
        }

        private void CreateKeyProfile() {
            ButtonProfile buttonProfile = Defaults.GetMomentaryButtonProfile(momentaryButtonProfile);
            keyProfile = CopyComponent(buttonProfile, disposable);
            keyProfile.characterSize = keyCharacterSize;
        }

        private void CreateWideKeyProfile() {
            ButtonProfile buttonProfile = Defaults.GetMomentaryButtonProfile(momentaryButtonProfile);
            wideKeyProfile = CopyComponent(buttonProfile, disposable);
            wideKeyProfile.characterSize = keyCharacterSize;
            wideKeyProfile.minWidth = wideKeyMinWidth;
        }

        private void CreateSpaceKeyProfile() {
            ButtonProfile buttonProfile = Defaults.GetMomentaryButtonProfile(momentaryButtonProfile);
            spaceKeyProfile = CopyComponent(buttonProfile, disposable);
            spaceKeyProfile.characterSize = keyCharacterSize;
            spaceKeyProfile.minWidth = spaceMinWidth;
        }

        private void CreateModeKeyProfile() {
            ButtonProfile buttonProfile = Defaults.GetToggleButtonProfile(toggleButtonProfile);
            modeKeyProfile = CopyComponent(buttonProfile, disposable);
            modeKeyProfile.characterSize = keyCharacterSize;
            modeKeyProfile.minWidth = modeKeyMinWidth;
        }

        private void CreateNumLockKeyProfile() {
            ButtonProfile buttonProfile = Defaults.GetToggleButtonProfile(toggleButtonProfile);
            numLockKeyProfile = CopyComponent(buttonProfile, disposable);
            numLockKeyProfile.characterSize = numLockCharacterSize;
            numLockKeyProfile.minWidth = modeKeyMinWidth;
        }

        private void CreateReturnKeyProfile() {
            ButtonProfile buttonProfile = Defaults.GetMomentaryButtonProfile(momentaryButtonProfile);
            returnKeyProfile = CopyComponent(buttonProfile, disposable);
            returnKeyProfile.characterSize = keyCharacterSize;
            returnKeyProfile.minWidth = returnMinWidth;
        }

        protected void SetButtonValues(ButtonBaseFactory factory, StandardPanel panel, GameObject parent) {
#if VRTK
            factory.useVRTK = useVRTK;
#endif
            factory.parent = parent;
            factory.buttonProfile = keyProfile;
            factory.alignment = TextAlignment.Center;
            factory.panel = panel;
        }

        protected void SetKeyboardButtonValues(KeyboardButtonFactory factory, StandardPanel panel, GameObject parent) {
            SetButtonValues(factory, panel, parent);
            factory.keyboard = keyboard;
        }

        protected void SetKeyboardToggleButtonValues(KeyboardToggleButtonFactory factory, StandardPanel panel, GameObject parent) {
            SetButtonValues(factory, panel, parent);
            factory.keyboard = keyboard;
        }

        protected void SetKeyboardButtonPosition(GameObject button) {
            Vector3 localPosition = button.transform.localPosition;
            localPosition.z = buttonZ;
            button.transform.localPosition = localPosition;
        }

        protected GameObject GenerateKeyboardButtonAndSetPosition(BaseFactory factory) {
            GameObject button = factory.Generate();
            SetKeyboardButtonPosition(button);
            return button;
        }

        protected GameObject KeyboardButton(StandardPanel panel, GameObject parent, ButtonProfile profile, string buttonText) {
            KeyboardMomentaryKeyButtonFactory factory = SafeAddComponent<KeyboardMomentaryKeyButtonFactory>(disposable);
            SetKeyboardButtonValues(factory, panel, parent);
            factory.buttonText = buttonText;
            factory.value = buttonText;
            factory.buttonProfile = profile;
            return GenerateKeyboardButtonAndSetPosition(factory);
        }

        protected GameObject KeyboardSpaceButton(StandardPanel panel, GameObject parent, string buttonText) {
            KeyboardMomentaryKeyButtonFactory factory = SafeAddComponent<KeyboardMomentaryKeyButtonFactory>(disposable);
            SetKeyboardButtonValues(factory, panel, parent);
            factory.buttonText = buttonText;
            factory.value = " ";
            factory.buttonProfile = spaceKeyProfile;
            return GenerateKeyboardButtonAndSetPosition(factory);
        }

        protected GameObject KeyboardShiftLockButton(StandardPanel panel, GameObject parent, string buttonText, bool on) {
            KeyboardShiftLockButtonFactory factory = SafeAddComponent<KeyboardShiftLockButtonFactory>(disposable);
            SetKeyboardToggleButtonValues(factory, panel, parent);
            factory.buttonText = buttonText;
            factory.on = on;
            factory.buttonProfile = modeKeyProfile;
            return GenerateKeyboardButtonAndSetPosition(factory);
        }

        protected GameObject KeyboardABCButton(StandardPanel panel, GameObject parent, string buttonText) {
            KeyboardShiftLockButtonFactory factory = SafeAddComponent<KeyboardShiftLockButtonFactory>(disposable);
            SetKeyboardToggleButtonValues(factory, panel, parent);
            factory.buttonText = buttonText;
            factory.on = false;
            factory.buttonProfile = numLockKeyProfile;
            return GenerateKeyboardButtonAndSetPosition(factory);
        }

        protected GameObject KeyboardNumLockButton(StandardPanel panel, GameObject parent, string buttonText) {
            KeyboardNumLockButtonFactory factory = SafeAddComponent<KeyboardNumLockButtonFactory>(disposable);
            SetKeyboardButtonValues(factory, panel, parent);
            factory.buttonText = buttonText;
            factory.buttonProfile = numLockKeyProfile;
            return GenerateKeyboardButtonAndSetPosition(factory);
        }

        protected GameObject KeyboardReturnButton(StandardPanel panel, GameObject parent, string buttonText) {
            KeyboardReturnButtonFactory factory = SafeAddComponent<KeyboardReturnButtonFactory>(disposable);
            SetKeyboardButtonValues(factory, panel, parent);
            factory.buttonText = buttonText;
            factory.buttonProfile = returnKeyProfile;
            return GenerateKeyboardButtonAndSetPosition(factory);
        }

        protected GameObject KeyboardDoneButton(StandardPanel panel, GameObject parent, string buttonText) {
            KeyboardDoneButtonFactory factory = SafeAddComponent<KeyboardDoneButtonFactory>(disposable);
            SetKeyboardButtonValues(factory, panel, parent);
            factory.buttonText = buttonText;
            factory.buttonProfile = returnKeyProfile;
            return GenerateKeyboardButtonAndSetPosition(factory);
        }

        protected GameObject KeyboardSymbolButton(StandardPanel panel, GameObject parent, string buttonText) {
            KeyboardSymbolButtonFactory factory = SafeAddComponent<KeyboardSymbolButtonFactory>(disposable);
            SetKeyboardButtonValues(factory, panel, parent);
            factory.buttonText = buttonText;
            factory.buttonProfile = numLockKeyProfile;
            return GenerateKeyboardButtonAndSetPosition(factory);
        }

        protected GameObject KeyboardBackspaceButton(StandardPanel panel, GameObject parent, string buttonText) {
            KeyboardBackspaceButtonFactory factory = SafeAddComponent<KeyboardBackspaceButtonFactory>(disposable);
            SetKeyboardButtonValues(factory, panel, parent);
            factory.buttonText = buttonText;
            return GenerateKeyboardButtonAndSetPosition(factory);
        }

        protected GameObject ButtonSpacer(GameObject parent, float width) {
            ButtonProfile profile = Defaults.GetMomentaryButtonProfile(momentaryButtonProfile);
            ButtonSpacerFactory factory = SafeAddComponent<ButtonSpacerFactory>(disposable);
            factory.parent = parent;
            Vector3 size = profile.bodyScale;
            size.x = width;
            factory.size = size;
            GameObject button = factory.Generate();
            SetKeyboardButtonPosition(button);
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

        protected GameObject Panel(GameObject parent, string name) {
            PanelContainerProfile profile = Defaults.GetProfile(panelContainerProfile);
            PanelContainerFactory factory = SafeAddComponent<PanelContainerFactory>(disposable);
            factory.parent = parent;
            factory.containerName = name;
            factory.panelContainerProfile = profile;
            GameObject panel = factory.Generate();

            PanelProfile subPanelProfile = CreateSubPanelProfile(panel);
            StandardPanel standardPanel = SafeAddComponent<StandardPanel>(panel);
            standardPanel.panelProfile = subPanelProfile;
            standardPanel.grabTarget = keyboard.transform;

#if VRTK
            if (useVRTK) {
                CreateThis_VRTK_Interactable interactable = SafeAddComponent<CreateThis_VRTK_Interactable>(panel);
                CreateThis_VRTK_GrabAttach grabAttach = SafeAddComponent<CreateThis_VRTK_GrabAttach>(panel);
                interactable.isGrabbable = true;
                interactable.grabAttachMechanicScript = grabAttach;
            }
#endif

            Rigidbody rigidbody = SafeAddComponent<Rigidbody>(panel);
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            return panel;
        }

        protected GameObject DisplayRow(GameObject parent) {
            ButtonProfile profile = Defaults.GetMomentaryButtonProfile(momentaryButtonProfile);

            GameObject row = Row(parent, "DisplayRow", TextAlignment.Left);
            GameObject label = EmptyChild(row, "Display");
            label.transform.localScale = profile.labelScale;
            Vector3 localPosition = new Vector3(0, 0, profile.labelZ);
            label.transform.localPosition = localPosition;
            TextMesh textMesh = SafeAddComponent<TextMesh>(label);
            textMesh.text = "keyboard display";
            textMesh.fontSize = profile.fontSize;
            textMesh.color = profile.fontColor;
            textMesh.anchor = TextAnchor.MiddleCenter;

            KeyboardLabel keyboardLabel = SafeAddComponent<KeyboardLabel>(label);
            keyboardLabel.keyboard = keyboard;
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
                    KeyboardButton(panel, parent, keyProfile, key.value);
                    break;
                case KeyType.Wide:
                    KeyboardButton(panel, parent, wideKeyProfile, key.value);
                    break;
                case KeyType.Space:
                    KeyboardSpaceButton(panel, parent, key.value);
                    break;
                case KeyType.ShiftLock:
                    KeyboardShiftLockButton(panel, parent, key.value, key.on);
                    break;
                case KeyType.ABC:
                    KeyboardABCButton(panel, parent, key.value);
                    break;
                case KeyType.NumLock:
                    KeyboardNumLockButton(panel, parent, key.value);
                    break;
                case KeyType.Symbol:
                    KeyboardSymbolButton(panel, parent, key.value);
                    break;
                case KeyType.Return:
                    KeyboardReturnButton(panel, parent, key.value);
                    break;
                case KeyType.Backspace:
                    KeyboardBackspaceButton(panel, parent, key.value);
                    break;
                case KeyType.Done:
                    KeyboardDoneButton(panel, parent, key.value);
                    break;
                case KeyType.Spacer:
                    ButtonSpacer(parent, key.width);
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

        protected void CreateKeyboard(GameObject parent) {
            if (keyboardInstance) return;
            keyboardInstance = EmptyChild(parent, "keyboard");

            keyboard = SafeAddComponent<Keyboard>(keyboardInstance);
            keyboard.panelProfile = panelProfile;
        }

        protected void PanelHeader(StandardPanel panel, GameObject parent) {
            ButtonRow(panel, parent, new List<Key> {
                Key.Done("done")
            }, TextAlignment.Right);

            DisplayRow(parent);
        }

        protected void PanelLowerCase(GameObject parent) {
            if (panelLowerCase) return;

            GameObject panel = Panel(parent, "PanelLowerCase");
            GameObject column = Column(panel);
            StandardPanel standardPanel = panel.GetComponent<StandardPanel>();

            PanelHeader(standardPanel, column);

            ButtonRow(standardPanel, column, new List<Key> {
                K("q"), K("w"), K("e"), K("r"), K("t"), K("y"), K("u"), K("i"), K("o"), K("p")
            });
            ButtonRow(standardPanel, column, new List<Key> {
                K("a"), K("s"), K("d"), K("f"), K("g"), K("h"), K("j"), K("k"), K("l")
            });
            ButtonRow(standardPanel, column, new List<Key> {
                Key.ShiftLock("⇧", false), Key.Spacer(spacerWidth), K("z"), K("x"), K("c"), K("v"), K("b"), K("n"), K("m"), Key.Spacer(spacerWidth), Key.Backspace("⇦")
            });
            ButtonRow(standardPanel, column, new List<Key> {
                Key.NumLock("123"), Key.Spacer(spacerWidth), Key.Space("space"), Key.Return("return")
            });

            panelLowerCase = panel;
            keyboard.panelLowerCase = standardPanel;
        }

        protected void PanelUpperCase(GameObject parent) {
            if (panelUpperCase) return;

            GameObject panel = Panel(parent, "PanelUpperCase");
            GameObject column = Column(panel);
            StandardPanel standardPanel = panel.GetComponent<StandardPanel>();

            PanelHeader(standardPanel, column);

            ButtonRow(standardPanel, column, new List<Key> {
                K("Q"), K("W"), K("E"), K("R"), K("T"), K("Y"), K("U"), K("I"), K("O"), K("P")
            });
            ButtonRow(standardPanel, column, new List<Key> {
                K("A"), K("S"), K("D"), K("F"), K("G"), K("H"), K("J"), K("K"), K("L")
            });
            ButtonRow(standardPanel, column, new List<Key> {
                Key.ShiftLock("⇧", true), Key.Spacer(spacerWidth), K("Z"), K("X"), K("C"), K("V"), K("B"), K("N"), K("M"), Key.Spacer(spacerWidth), Key.Backspace("⇦")
            });
            ButtonRow(standardPanel, column, new List<Key> {
                Key.NumLock("123"), Key.Spacer(spacerWidth), Key.Space("space"), Key.Return("return")
            });

            panelUpperCase = panel;
            keyboard.panelUpperCase = standardPanel;
        }

        protected void PanelNumber(GameObject parent) {
            if (panelNumber) return;

            GameObject panel = Panel(parent, "PanelNumber");
            GameObject column = Column(panel);
            StandardPanel standardPanel = panel.GetComponent<StandardPanel>();

            PanelHeader(standardPanel, column);

            ButtonRow(standardPanel, column, new List<Key> {
                K("1"), K("2"), K("3"), K("4"), K("5"), K("6"), K("7"), K("8"), K("9"), K("0")
            });
            ButtonRow(standardPanel, column, new List<Key> {
                K("-"), K("/"), K(":"), K(";"), K("("), K(")"), K("$"), K("&"), K("@"), K("\"")
            });
            ButtonRow(standardPanel, column, new List<Key> {
                Key.Symbol("#+="), Key.Spacer(spacerWidth), W("."), W(","), W("?"), W("!"), W("'"), Key.Spacer(spacerWidth), Key.Backspace("⇦")
            });
            ButtonRow(standardPanel, column, new List<Key> {
                Key.ABC("ABC"), Key.Spacer(spacerWidth), Key.Space("space"), Key.Return("return")
            });

            panelNumber = panel;
            keyboard.panelNumber = standardPanel;
        }

        protected void PanelSymbol(GameObject parent) {
            if (panelSymbol) return;

            GameObject panel = Panel(parent, "PanelSymbol");
            GameObject column = Column(panel);
            StandardPanel standardPanel = panel.GetComponent<StandardPanel>();

            PanelHeader(standardPanel, column);

            ButtonRow(standardPanel, column, new List<Key> {
                K("["), K("]"), K("{"), K("}"), K("#"), K("%"), K("^"), K("*"), K("+"), K("=")
            });
            ButtonRow(standardPanel, column, new List<Key> {
                K("_"), K("\\"), K("|"), K("~"), K("<"), K(">"), K("€"), K("£"), K("¥"), K("•")
            });
            ButtonRow(standardPanel, column, new List<Key> {
                Key.NumLock("123"), Key.Spacer(spacerWidth), W("."), W(","), W("?"), W("!"), W("'"), Key.Spacer(spacerWidth), Key.Backspace("⇦")
            });
            ButtonRow(standardPanel, column, new List<Key> {
                Key.ABC("ABC"), Key.Spacer(spacerWidth), Key.Space("space"), Key.Return("return")
            });

            panelSymbol = panel;
            keyboard.panelSymbol = standardPanel;
        }

        public override GameObject Generate() {
            base.Generate();

#if UNITY_EDITOR
            Undo.SetCurrentGroupName("KeyboardFactory Generate");
            int group = Undo.GetCurrentGroup();

            Undo.RegisterCompleteObjectUndo(this, "KeyboardFactory state");
#endif
            CreateDisposable(parent);

            CreateKeyProfile();
            CreateWideKeyProfile();
            CreateSpaceKeyProfile();
            CreateModeKeyProfile();
            CreateNumLockKeyProfile();
            CreateReturnKeyProfile();

            CreateKeyboard(parent);
            PanelLowerCase(keyboardInstance);
            PanelUpperCase(keyboardInstance);
            PanelNumber(keyboardInstance);
            PanelSymbol(keyboardInstance);

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
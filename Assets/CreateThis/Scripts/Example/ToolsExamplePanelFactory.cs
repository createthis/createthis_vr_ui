using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using CreateThis.VRTK;
using CreateThis.VR.UI.Panel;
using CreateThis.VR.UI.Container;
using CreateThis.Factory;
using CreateThis.Factory.VR.UI.Button;
using CreateThis.Factory.VR.UI.Container;

namespace CreateThis.Example {
    public class ToolsExamplePanelFactory : BaseFactory {
        public GameObject parent;
        public GameObject buttonBody;
        public GameObject folderPrefab;
        public Material buttonMaterial;
        public Material panelMaterial;
        public Material highlight;
        public Material outline;
        public AudioClip momentaryButtonClickDown;
        public AudioClip momentaryButtonClickUp;
        public AudioClip toggleButtonClickDown;
        public AudioClip toggleButtonClickUp;
        public int fontSize;
        public Color fontColor;
        public float labelZ;
        public float buttonZ;
        public Vector3 bodyScale;
        public Vector3 labelScale;
        public float padding;
        public float spacing;
        public float buttonPadding;
        public float buttonMinWidth;
        public float buttonCharacterSize;
        public float labelCharacterSize;
        public Camera sceneCamera;
        public Vector3 offset;
        public float minDistance;
        public bool hideOnAwake;

        private GameObject disposable;
        private GameObject toolsPanelInstance;
        private StandardPanel toolsPanel;

        protected void SetMomentaryButtonValues(MomentaryButtonFactory factory, StandardPanel panel, GameObject parent) {
            factory.useVRTK = useVRTK;
            factory.parent = parent;
            factory.buttonBody = buttonBody;
            factory.material = buttonMaterial;
            factory.highlight = highlight;
            factory.outline = outline;
            factory.buttonClickDown = momentaryButtonClickDown;
            factory.buttonClickUp = momentaryButtonClickUp;
            factory.alignment = TextAlignment.Center;
            factory.fontSize = fontSize;
            factory.fontColor = fontColor;
            factory.labelZ = labelZ;
            factory.bodyScale = bodyScale;
            factory.labelScale = labelScale;
            factory.minWidth = buttonMinWidth;
            factory.padding = buttonPadding;
            factory.characterSize = buttonCharacterSize;
            factory.panel = panel;
        }

        protected void SetToggleButtonValues(ToggleButtonFactory factory, StandardPanel panel, GameObject parent) {
            factory.useVRTK = useVRTK;
            factory.parent = parent;
            factory.buttonBody = buttonBody;
            factory.material = buttonMaterial;
            factory.highlight = highlight;
            factory.outline = outline;
            factory.buttonClickDown = toggleButtonClickDown;
            factory.buttonClickUp = toggleButtonClickUp;
            factory.alignment = TextAlignment.Center;
            factory.fontSize = fontSize;
            factory.fontColor = fontColor;
            factory.labelZ = labelZ;
            factory.bodyScale = bodyScale;
            factory.labelScale = labelScale;
            factory.minWidth = buttonMinWidth;
            factory.padding = buttonPadding;
            factory.characterSize = buttonCharacterSize;
            factory.panel = panel;
        }

        protected void SetButtonPosition(GameObject button) {
            Vector3 localPosition = button.transform.localPosition;
            localPosition.z = buttonZ;
            button.transform.localPosition = localPosition;
        }

        protected GameObject GenerateMomentaryButtonAndSetPosition(MomentaryButtonFactory factory) {
            GameObject button = factory.Generate();
            SetButtonPosition(button);
            return button;
        }

        protected GameObject GenerateToggleButtonAndSetPosition(ToggleButtonFactory factory) {
            GameObject button = factory.Generate();
            SetButtonPosition(button);
            return button;
        }

        protected GameObject MomentaryButton(StandardPanel panel, GameObject parent, string buttonText) {
            PanelHidingMomentaryButtonFactory factory = SafeAddComponent<PanelHidingMomentaryButtonFactory>(disposable);
            SetMomentaryButtonValues(factory, panel, parent);
            factory.buttonText = buttonText;
            return GenerateMomentaryButtonAndSetPosition(factory);
        }

        protected GameObject ToggleButton(StandardPanel panel, GameObject parent, string buttonText) {
            ToggleButtonFactory factory = SafeAddComponent<ToggleButtonFactory>(disposable);
            SetToggleButtonValues(factory, panel, parent);
            factory.buttonText = buttonText;
            return GenerateToggleButtonAndSetPosition(factory);
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
            standardPanel.grabTarget = panel.transform;
            standardPanel.sceneCamera = sceneCamera;
            standardPanel.offset = offset;
            standardPanel.minDistance = minDistance;
            standardPanel.hideOnAwake = hideOnAwake;
            toolsPanel = standardPanel;

            if (useVRTK) {
                CreateThis_VRTK_Interactable interactable = SafeAddComponent<CreateThis_VRTK_Interactable>(panel);
                CreateThis_VRTK_GrabAttach grabAttach = SafeAddComponent<CreateThis_VRTK_GrabAttach>(panel);
                interactable.isGrabbable = true;
                interactable.grabAttachMechanicScript = grabAttach;
            }

            Rigidbody rigidbody = SafeAddComponent<Rigidbody>(panel);
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            return panel;
        }

        protected GameObject Label(GameObject parent, string name, string text) {
            GameObject label = EmptyChild(parent, name);
            label.transform.localScale = labelScale;
            Vector3 localPosition = new Vector3(0, 0, labelZ);
            label.transform.localPosition = localPosition;
            TextMesh textMesh = SafeAddComponent<TextMesh>(label);
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = fontColor;
            textMesh.characterSize = labelCharacterSize;
            textMesh.anchor = TextAnchor.MiddleCenter;

            BoxCollider boxCollider = SafeAddComponent<BoxCollider>(label);

            UpdateBoxColliderFromTextMesh updateBoxCollider = SafeAddComponent<UpdateBoxColliderFromTextMesh>(label);
            updateBoxCollider.textMesh = textMesh;
            updateBoxCollider.boxCollider = boxCollider;
            return label;
        }

        private void CreateDisposable(GameObject parent) {
            if (disposable) return;
            disposable = EmptyChild(parent, "disposable");
        }

        private GameObject FileLabelRow(GameObject parent) {
            GameObject row = Row(parent, "FileLabelRow", TextAlignment.Left);
            Label(row, "FileLabel", "File");
            return row;
        }

        private GameObject FileRow(GameObject parent) {
            GameObject row = Row(parent, "FileRow", TextAlignment.Left);
            MomentaryButton(toolsPanel, row, "Open");
            MomentaryButton(toolsPanel, row, "SaveAs");
            return row;
        }

        private GameObject ChangeSkyboxLabelRow(GameObject parent) {
            GameObject row = Row(parent, "ChangeSkyboxLabelRow", TextAlignment.Left);
            Label(row, "ChangeSkyboxLabel", "Change Skybox");
            return row;
        }

        private GameObject ChangeSkyboxRow(GameObject parent) {
            GameObject row = Row(parent, "ChangeSkyboxRow", TextAlignment.Left);
            ToggleButton(toolsPanel, row, "Blue Sky");
            ToggleButton(toolsPanel, row, "Stars");
            return row;
        }

        private void CreateToolsPanel(GameObject parent) {
            toolsPanelInstance = Panel(parent, "ToolsPanelInstance");
            GameObject column = Column(toolsPanelInstance);
            FileLabelRow(column);
            FileRow(column);
            ChangeSkyboxLabelRow(column);
            ChangeSkyboxRow(column);
        }

        public override GameObject Generate() {
            base.Generate();

#if UNITY_EDITOR
            Undo.SetCurrentGroupName("ToolsExamplePanelFactory Generate");
            int group = Undo.GetCurrentGroup();

            Undo.RegisterCompleteObjectUndo(this, "ToolsExamplePanelFactory state");
#endif
            CreateDisposable(parent);
            CreateToolsPanel(parent);

#if UNITY_EDITOR
            Undo.DestroyObjectImmediate(disposable);
            Undo.CollapseUndoOperations(group);
#else
            Destroy(disposable);
#endif
            return toolsPanelInstance;
        }
    }
}
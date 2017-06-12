﻿using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if VRTK
using CreateThis.VRTK;
#endif
using CreateThis.Unity;
using CreateThis.VR.UI;
using CreateThis.VR.UI.Panel;
using CreateThis.VR.UI.File;
using CreateThis.VR.UI.Container;
using CreateThis.Factory;
using CreateThis.Factory.VR.UI.Button;
using CreateThis.Factory.VR.UI.Container;

namespace CreateThis.Example {
    public class ToolsExamplePanelFactory : BaseFactory {
        public GameObject parent;
        public PanelProfile panelProfile;
        public PanelContainerProfile panelContainerProfile;
        public ButtonProfile momentaryButtonProfile;
        public ButtonProfile toggleButtonProfile;
        public FileOpen fileOpen;
        public FileSaveAs fileSaveAs;
        public SkyboxManager skyboxManager;

        private GameObject disposable;
        private GameObject toolsPanelInstance;
        private StandardPanel toolsPanel;

        protected void SetMomentaryButtonValues(MomentaryButtonFactory factory, StandardPanel panel, GameObject parent) {
#if VRTK
            factory.useVRTK = useVRTK;
#endif
            factory.parent = parent;
            factory.buttonProfile = momentaryButtonProfile;
            factory.alignment = TextAlignment.Center;
            factory.panel = panel;
        }

        protected void SetToggleButtonValues(ToggleButtonFactory factory, StandardPanel panel, GameObject parent) {
#if VRTK
            factory.useVRTK = useVRTK;
#endif
            factory.parent = parent;
            factory.buttonProfile = toggleButtonProfile;
            factory.alignment = TextAlignment.Center;
            factory.panel = panel;
        }

        protected void SetButtonPosition(GameObject button) {
            PanelContainerProfile profile = Defaults.GetProfile(panelContainerProfile);
            Vector3 localPosition = button.transform.localPosition;
            localPosition.z = profile.buttonZ;
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

        protected GameObject PanelToggleVisibilityMomentaryButton(StandardPanel panel, GameObject parent, string buttonText, PanelBase panelToToggle) {
            PanelToggleVisibilityMomentaryButtonFactory factory = Undoable.AddComponent<PanelToggleVisibilityMomentaryButtonFactory>(disposable);
            SetMomentaryButtonValues(factory, panel, parent);
            factory.buttonText = buttonText;
            factory.panelToToggle = panelToToggle;
            return GenerateMomentaryButtonAndSetPosition(factory);
        }

        protected GameObject SkyboxButton(StandardPanel panel, GameObject parent, string buttonText, string skybox) {
            SkyboxButtonFactory factory = Undoable.AddComponent<SkyboxButtonFactory>(disposable);
            SetToggleButtonValues(factory, panel, parent);
            factory.buttonText = buttonText;
            factory.skybox = skybox;
            factory.skyboxManager = skyboxManager;
            return GenerateToggleButtonAndSetPosition(factory);
        }

        protected GameObject Row(GameObject parent, string name = null, TextAlignment alignment = TextAlignment.Center) {
            PanelContainerProfile profile = Defaults.GetProfile(panelContainerProfile);
            RowContainerFactory factory = Undoable.AddComponent<RowContainerFactory>(disposable);
            factory.containerName = name;
            factory.parent = parent;
            factory.padding = profile.padding;
            factory.spacing = profile.spacing;
            factory.alignment = alignment;
            return factory.Generate();
        }

        protected GameObject Column(GameObject parent) {
            PanelContainerProfile profile = Defaults.GetProfile(panelContainerProfile);
            ColumnContainerFactory factory = Undoable.AddComponent<ColumnContainerFactory>(disposable);
            factory.parent = parent;
            factory.padding = profile.padding;
            factory.spacing = profile.spacing;
            return factory.Generate();
        }

        protected GameObject Panel(GameObject parent, string name) {
            PanelContainerFactory factory = Undoable.AddComponent<PanelContainerFactory>(disposable);
            factory.parent = parent;
            factory.containerName = name;
            factory.panelContainerProfile = panelContainerProfile;
            GameObject panel = factory.Generate();

            StandardPanel standardPanel = Undoable.AddComponent<StandardPanel>(panel);
            standardPanel.grabTarget = panel.transform;
            standardPanel.panelProfile = panelProfile;
            toolsPanel = standardPanel;

#if VRTK
            if (useVRTK) {
                CreateThis_VRTK_Interactable interactable = SafeAddComponent<CreateThis_VRTK_Interactable>(panel);
                CreateThis_VRTK_GrabAttach grabAttach = SafeAddComponent<CreateThis_VRTK_GrabAttach>(panel);
                interactable.isGrabbable = true;
                interactable.grabAttachMechanicScript = grabAttach;
            }
#endif

            Rigidbody rigidbody = Undoable.AddComponent<Rigidbody>(panel);
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            return panel;
        }

        protected GameObject Label(GameObject parent, string name, string text) {
            GameObject label = EmptyChild(parent, name);

            ButtonProfile profile = Defaults.GetMomentaryButtonProfile(momentaryButtonProfile);
            PanelContainerProfile pcProfile = Defaults.GetProfile(panelContainerProfile);
            label.transform.localScale = profile.labelScale;
            Vector3 localPosition = new Vector3(0, 0, profile.labelZ);
            label.transform.localPosition = localPosition;
            TextMesh textMesh = Undoable.AddComponent<TextMesh>(label);
            textMesh.text = text;
            textMesh.fontSize = profile.fontSize;
            textMesh.color = profile.fontColor;
            textMesh.characterSize = pcProfile.labelCharacterSize;
            textMesh.anchor = TextAnchor.MiddleCenter;

            BoxCollider boxCollider = Undoable.AddComponent<BoxCollider>(label);

            UpdateBoxColliderFromTextMesh updateBoxCollider = Undoable.AddComponent<UpdateBoxColliderFromTextMesh>(label);
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
            PanelToggleVisibilityMomentaryButton(toolsPanel, row, "Open", fileOpen);
            PanelToggleVisibilityMomentaryButton(toolsPanel, row, "SaveAs", fileSaveAs);
            return row;
        }

        private GameObject ChangeSkyboxLabelRow(GameObject parent) {
            GameObject row = Row(parent, "ChangeSkyboxLabelRow", TextAlignment.Left);
            Label(row, "ChangeSkyboxLabel", "Change Skybox");
            return row;
        }

        private GameObject ChangeSkyboxRow(GameObject parent) {
            GameObject row = Row(parent, "ChangeSkyboxRow", TextAlignment.Left);
            SkyboxButton(toolsPanel, row, "Blue Sky", "bluesky");
            SkyboxButton(toolsPanel, row, "Sunset", "sunset");
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
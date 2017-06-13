using UnityEngine;
using CreateThis.Factory.VR.UI.Button;
using CreateThis.Factory.VR.UI.Scroller;
using CreateThis.Factory.VR.UI.Container;
using CreateThis.System;
#if VRTK
using CreateThis.VRTK;
#endif
using CreateThis.Unity;
using CreateThis.VR.UI;
using CreateThis.VR.UI.Panel;
using CreateThis.VR.UI.File;
using CreateThis.VR.UI.Interact;
using CreateThis.VR.UI.Scroller;
using CreateThis.VR.UI.Container;

namespace CreateThis.Factory.VR.UI.File {
    public abstract class FileBaseFactory : BaseFactory {
        public GameObject parent;
        public PanelProfile panelProfile;
        public PanelContainerProfile panelContainerProfile;
        public ButtonProfile momentaryButtonProfile;
        public ButtonProfile toggleButtonProfile;
        public FilePanelProfile filePanelProfile;

        protected GameObject filePanelContainerInstance;
        protected Rigidbody filePanelContainerRigidbody;
        protected GameObject filePanelInstance;
        protected FileBase filePanel;
        private Drives drives;
        protected GameObject disposable;
        private GameObject currentPathLabel;
        private KineticScroller kineticScroller;
        protected GameObject kineticScrollerItem;
        protected GameObject kineticScrollerInstance;

        protected void SetButtonValues(MomentaryButtonFactory factory, StandardPanel panel, GameObject parent) {
#if VRTK
            factory.useVRTK = useVRTK;
#endif
            factory.parent = parent;
            factory.buttonProfile = momentaryButtonProfile;
            factory.alignment = TextAlignment.Center;
            factory.panel = panel;
        }

        protected void SetKeyboardButtonPosition(GameObject button) {
            PanelContainerProfile profile = Defaults.GetProfile(panelContainerProfile);
            Vector3 localPosition = button.transform.localPosition;
            localPosition.z = profile.buttonZ;
            button.transform.localPosition = localPosition;
        }

        protected GameObject GenerateKeyboardButtonAndSetPosition(MomentaryButtonFactory factory) {
            GameObject button = factory.Generate();
            SetKeyboardButtonPosition(button);
            return button;
        }

        protected GameObject DriveButton(FileBase panel, GameObject parent, ButtonProfile buttonProfile, string buttonText) {
            DriveButtonFactory factory = Undoable.AddComponent<DriveButtonFactory>(disposable);
            SetButtonValues(factory, panel, parent);
            factory.buttonText = buttonText;
            factory.filePanel = panel;
            factory.buttonProfile = buttonProfile;
            return GenerateKeyboardButtonAndSetPosition(factory);
        }

        protected GameObject KnownFolderButton(FileBase panel, GameObject parent, ButtonProfile buttonProfile, string buttonText, KnownFolder knownFolder) {
            KnownFolderButtonFactory factory = Undoable.AddComponent<KnownFolderButtonFactory>(disposable);
            SetButtonValues(factory, panel, parent);
            factory.buttonText = buttonText;
            factory.filePanel = panel;
            factory.buttonProfile = buttonProfile;
            factory.knownFolder = knownFolder;
            return GenerateKeyboardButtonAndSetPosition(factory);
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

        protected virtual FileBase AddFilePanel(GameObject panel) {
            return Undoable.AddComponent<FileBase>(panel);
        }

        protected GameObject Panel(GameObject parent, string name) {
            PanelContainerProfile profile = Defaults.GetProfile(panelContainerProfile);
            FilePanelProfile fpProfile = Defaults.GetProfile(filePanelProfile);
            PanelContainerFactory factory = Undoable.AddComponent<PanelContainerFactory>(disposable);
            factory.parent = parent;
            factory.containerName = name;
            factory.panelContainerProfile = profile;
            GameObject panel = factory.Generate();

            filePanel = AddFilePanel(panel);
            filePanel.grabTarget = filePanelContainerInstance.transform;
            filePanel.folderPrefab = fpProfile.folderPrefab;
            filePanel.kineticScrollItemPrefab = kineticScrollerItem;
            filePanel.height = fpProfile.scrollerHeight;
            filePanel.searchPattern = fpProfile.searchPattern;
            filePanel.panelProfile = panelProfile;

#if VRTK
            if (useVRTK) {
                CreateThis_VRTK_Interactable interactable = Undoable.AddComponent<CreateThis_VRTK_Interactable>(panel);
                CreateThis_VRTK_GrabAttach grabAttach = Undoable.AddComponent<CreateThis_VRTK_GrabAttach>(panel);
                interactable.isGrabbable = true;
                interactable.grabAttachMechanicScript = grabAttach;
            }
#endif

            drives = Undoable.AddComponent<Drives>(panel);

            Rigidbody rigidbody = Undoable.AddComponent<Rigidbody>(panel);
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            return panel;
        }

        protected GameObject Label(GameObject parent, string name, string text) {
            PanelContainerProfile pcProfile = Defaults.GetProfile(panelContainerProfile);
            ButtonProfile profile = Defaults.GetMomentaryButtonProfile(momentaryButtonProfile);
            GameObject label = EmptyChild(parent, name);
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

        protected GameObject CurrentPathRow(GameObject parent) {
            GameObject row = Row(parent, "CurrentPathRow", TextAlignment.Left);
            Label(row, "PathLabel", "                 Path");
            currentPathLabel = Label(row, "CurrentPathLabel", "C:/Foo/Blah/Stuff");
            filePanel.currentPathLabel = currentPathLabel;
            return row;
        }

        protected GameObject DriveButtonRow(GameObject parent) {
            GameObject row = Row(parent, "DriveButtonRow", TextAlignment.Left);
            Label(row, "DrivesLabel", "              Drives");
            ButtonProfile profile = Defaults.GetMomentaryButtonProfile(momentaryButtonProfile);

            GameObject driveButtonPrefab = DriveButton(filePanel, row, profile, "C");
            drives.driveButtonPrefab = driveButtonPrefab;
            return row;
        }

        protected GameObject SpecialFoldersRow(GameObject parent) {
            GameObject row = Row(parent, "SpecialFoldersRow", TextAlignment.Left);
            Label(row, "SpecialFoldersLabel", "Special Folders");
            ButtonProfile profile = Defaults.GetMomentaryButtonProfile(momentaryButtonProfile);

            KnownFolderButton(filePanel, row, profile, "Documents", KnownFolder.Documents);
            KnownFolderButton(filePanel, row, profile, "Downloads", KnownFolder.Downloads);
            KnownFolderButton(filePanel, row, profile, "Desktop", KnownFolder.Desktop);

            return row;
        }

        protected void CreateDisposable(GameObject parent) {
            if (disposable) return;
            disposable = EmptyChild(parent, "disposable");
        }

        protected GameObject CreateKineticScrollerItem(GameObject parent) {
            KineticScrollerItemFactory kineticScrollerItemFactory = Undoable.AddComponent<KineticScrollerItemFactory>(disposable);
#if VRTK
            kineticScrollerItemFactory.useVRTK = useVRTK;
#endif
            ButtonProfile profile = Defaults.GetMomentaryButtonProfile(momentaryButtonProfile);

            kineticScrollerItemFactory.parent = parent;
            kineticScrollerItemFactory.material = profile.material;
            kineticScrollerItemFactory.highlight = profile.highlight;
            kineticScrollerItemFactory.outline = profile.outline;
            kineticScrollerItemFactory.fontColor = profile.fontColor;
            return kineticScrollerItemFactory.Generate();
        }

        protected GameObject CreateKineticScroller(GameObject parent) {
            FilePanelProfile fpProfile = Defaults.GetProfile(filePanelProfile);
            kineticScrollerInstance = EmptyChild(parent, "KineticScroller");
            kineticScroller = Undoable.AddComponent<KineticScroller>(kineticScrollerInstance);
            kineticScroller.space = fpProfile.kineticScrollerSpacing;
            filePanel.kineticScroller = kineticScroller;

            Rigidbody rigidbody = Undoable.AddComponent<Rigidbody>(kineticScrollerInstance);
            rigidbody.useGravity = false;

            ButtonProfile profile = Defaults.GetMomentaryButtonProfile(momentaryButtonProfile);

            Selectable selectable = Undoable.AddComponent<Selectable>(kineticScrollerInstance);
            selectable.highlightMaterial = profile.highlight;
            selectable.outlineMaterial = profile.outline;
            selectable.textColor = profile.fontColor;
            selectable.unselectedMaterials = new Material[] { profile.material };
            selectable.recursive = true;

            ConfigurableJoint configurableJoint = Undoable.AddComponent<ConfigurableJoint>(kineticScrollerInstance);
            configurableJoint.connectedBody = filePanelContainerRigidbody;
            configurableJoint.anchor = Vector3.zero;
            configurableJoint.xMotion = ConfigurableJointMotion.Limited;
            configurableJoint.yMotion = ConfigurableJointMotion.Locked;
            configurableJoint.zMotion = ConfigurableJointMotion.Locked;
            configurableJoint.angularXMotion = ConfigurableJointMotion.Locked;
            configurableJoint.angularYMotion = ConfigurableJointMotion.Locked;
            configurableJoint.angularZMotion = ConfigurableJointMotion.Locked;
            configurableJoint.breakForce = float.PositiveInfinity;
            configurableJoint.breakTorque = float.PositiveInfinity;

            return kineticScrollerInstance;
        }

        protected virtual void PanelHeader(StandardPanel panel, GameObject parent) {
            CurrentPathRow(parent);
        }

        protected virtual void FilePanel(GameObject parent) {
            if (filePanelInstance) return;

            FilePanelProfile fpProfile = Defaults.GetProfile(filePanelProfile);
            filePanelInstance = EmptyChild(parent, "FileOpenPanel");
            Vector3 localPosition = filePanelInstance.transform.localPosition;
            localPosition.y = -fpProfile.scrollerHeight * 1.50f;
            filePanelInstance.transform.localPosition = localPosition;

            Rigidbody rigidbody = Undoable.AddComponent<Rigidbody>(filePanelInstance);
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            GameObject panel = Panel(filePanelInstance, "DrivesPanel");
            GameObject column = Column(panel);

            PanelHeader(filePanel, column);
            DriveButtonRow(column);
            SpecialFoldersRow(column);

            panel.transform.localPosition = Vector3.zero;
        }
    }
}
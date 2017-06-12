using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
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
    public class FileOpenFactory : BaseFactory {
        public GameObject parent;
        public PanelProfile panelProfile;
        public PanelContainerProfile panelContainerProfile;
        public ButtonProfile momentaryButtonProfile;
        public ButtonProfile toggleButtonProfile;
        public GameObject folderPrefab;
        public float kineticScrollerSpacing;
        public float scrollerHeight;
        public string searchPattern;

        private GameObject fileOpenContainerInstance;
        private Rigidbody fileOpenContainerRigidbody;
        private GameObject fileOpenInstance;
        private FileOpen fileOpenPanel;
        private Drives drives;
        private GameObject disposable;
        private GameObject currentPathLabel;
        private KineticScroller kineticScroller;
        private GameObject kineticScrollerItem;
        private GameObject kineticScrollerInstance;

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

        protected GameObject DriveButton(FileOpen panel, GameObject parent, ButtonProfile buttonProfile, string buttonText) {
            DriveButtonFactory factory = Undoable.AddComponent<DriveButtonFactory>(disposable);
            SetButtonValues(factory, panel, parent);
            factory.buttonText = buttonText;
            factory.filePanel = panel;
            factory.buttonProfile = buttonProfile;
            return GenerateKeyboardButtonAndSetPosition(factory);
        }

        protected GameObject KnownFolderButton(FileOpen panel, GameObject parent, ButtonProfile buttonProfile, string buttonText, KnownFolder knownFolder) {
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

        protected GameObject Panel(GameObject parent, string name) {
            PanelContainerProfile profile = Defaults.GetProfile(panelContainerProfile);
            PanelContainerFactory factory = Undoable.AddComponent<PanelContainerFactory>(disposable);
            factory.parent = parent;
            factory.containerName = name;
            factory.panelContainerProfile = profile;
            GameObject panel = factory.Generate();

            fileOpenPanel = Undoable.AddComponent<FileOpen>(panel);
            fileOpenPanel.grabTarget = fileOpenContainerInstance.transform;
            fileOpenPanel.folderPrefab = folderPrefab;
            fileOpenPanel.kineticScrollItemPrefab = kineticScrollerItem;
            fileOpenPanel.height = scrollerHeight;
            fileOpenPanel.searchPattern = searchPattern;
            fileOpenPanel.panelProfile = panelProfile;

#if VRTK
            if (useVRTK) {
                CreateThis_VRTK_Interactable interactable = SafeAddComponent<CreateThis_VRTK_Interactable>(panel);
                CreateThis_VRTK_GrabAttach grabAttach = SafeAddComponent<CreateThis_VRTK_GrabAttach>(panel);
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
            fileOpenPanel.currentPathLabel = currentPathLabel;
            return row;
        }

        protected GameObject DriveButtonRow(GameObject parent) {
            GameObject row = Row(parent, "DriveButtonRow", TextAlignment.Left);
            Label(row, "DrivesLabel", "              Drives");
            ButtonProfile profile = Defaults.GetMomentaryButtonProfile(momentaryButtonProfile);

            GameObject driveButtonPrefab = DriveButton(fileOpenPanel, row, profile, "C");
            drives.driveButtonPrefab = driveButtonPrefab;
            return row;
        }

        protected GameObject SpecialFoldersRow(GameObject parent) {
            GameObject row = Row(parent, "SpecialFoldersRow", TextAlignment.Left);
            Label(row, "SpecialFoldersLabel", "Special Folders");
            ButtonProfile profile = Defaults.GetMomentaryButtonProfile(momentaryButtonProfile);

            KnownFolderButton(fileOpenPanel, row, profile, "Documents", KnownFolder.Documents);
            KnownFolderButton(fileOpenPanel, row, profile, "Downloads", KnownFolder.Downloads);
            KnownFolderButton(fileOpenPanel, row, profile, "Desktop", KnownFolder.Desktop);

            return row;
        }

        private void CreateDisposable(GameObject parent) {
            if (disposable) return;
            disposable = EmptyChild(parent, "disposable");
        }

        private GameObject CreateKineticScrollerItem(GameObject parent) {
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

        private GameObject CreateKineticScroller(GameObject parent) {
            kineticScrollerInstance = EmptyChild(parent, "KineticScroller");
            kineticScroller = Undoable.AddComponent<KineticScroller>(kineticScrollerInstance);
            kineticScroller.space = kineticScrollerSpacing;
            fileOpenPanel.kineticScroller = kineticScroller;

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
            configurableJoint.connectedBody = fileOpenContainerRigidbody;
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

        protected void PanelHeader(StandardPanel panel, GameObject parent) {
            CurrentPathRow(parent);
        }

        protected void FileOpenPanel(GameObject parent) {
            if (fileOpenInstance) return;

            fileOpenInstance = EmptyChild(parent, "FileOpenPanel");
            Vector3 localPosition = fileOpenInstance.transform.localPosition;
            localPosition.y = -scrollerHeight * 1.50f;
            fileOpenInstance.transform.localPosition = localPosition;

            Rigidbody rigidbody = Undoable.AddComponent<Rigidbody>(fileOpenInstance);
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            GameObject panel = Panel(fileOpenInstance, "DrivesPanel");
            GameObject column = Column(panel);

            PanelHeader(fileOpenPanel, column);
            DriveButtonRow(column);
            SpecialFoldersRow(column);

            panel.transform.localPosition = Vector3.zero;
        }

        public override GameObject Generate() {
            base.Generate();

#if UNITY_EDITOR
            Undo.SetCurrentGroupName("FileOpenFactory Generate");
            int group = Undo.GetCurrentGroup();

            Undo.RegisterCompleteObjectUndo(this, "FileOpenFactory state");
#endif
            CreateDisposable(parent);

            fileOpenContainerInstance = EmptyChild(parent, "FileOpenContainer");
            fileOpenContainerRigidbody = Undoable.AddComponent<Rigidbody>(fileOpenContainerInstance);
            fileOpenContainerRigidbody.useGravity = false;
            fileOpenContainerRigidbody.isKinematic = true;

            kineticScrollerItem = CreateKineticScrollerItem(fileOpenContainerInstance);
            kineticScrollerItem.SetActive(false);

            FileOpenPanel(fileOpenContainerInstance);

            CreateKineticScroller(fileOpenContainerInstance);

#if UNITY_EDITOR
            Undo.DestroyObjectImmediate(disposable);
            Undo.CollapseUndoOperations(group);
#else
            Destroy(disposable);
#endif
            return fileOpenInstance;
        }
    }
}
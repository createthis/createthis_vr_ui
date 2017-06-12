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
using CreateThis.VR.UI.Button;
using CreateThis.VR.UI.Panel;
using CreateThis.VR.UI.File;
using CreateThis.VR.UI.Interact;
using CreateThis.VR.UI.Scroller;
using CreateThis.VR.UI.Container;

namespace CreateThis.Factory.VR.UI.File {
    public class FileSaveAsFactory : BaseFactory {
        public GameObject parent;
        public PanelProfile panelProfile;
        public PanelContainerProfile panelContainerProfile;
        public ButtonProfile momentaryButtonProfile;
        public ButtonProfile toggleButtonProfile;
        public GameObject folderPrefab;
        public float buttonZ;
        public float padding;
        public float spacing;
        public float labelCharacterSize;
        public float kineticScrollerSpacing;
        public float scrollerHeight;
        public string searchPattern;
        public string fileNameExtension;
        public Keyboard keyboard;

        protected GameObject fileSaveAsContainerInstance;
        protected Rigidbody fileSaveAsContainerRigidbody;
        protected GameObject fileSaveAsInstance;
        protected FileSaveAs fileSaveAsPanel;
        private Drives drives;
        private GameObject disposable;
        private GameObject currentPathLabel;
        private KineticScroller kineticScroller;
        private GameObject kineticScrollerItem;
        private GameObject kineticScrollerInstance;
        private SaveAsFileNameButton fileNameButton;

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
            Vector3 localPosition = button.transform.localPosition;
            localPosition.z = buttonZ;
            button.transform.localPosition = localPosition;
        }

        protected GameObject GenerateKeyboardButtonAndSetPosition(MomentaryButtonFactory factory) {
            GameObject button = factory.Generate();
            SetKeyboardButtonPosition(button);
            return button;
        }

        protected GameObject DriveButton(FileSaveAs panel, GameObject parent, ButtonProfile buttonProfile, string buttonText) {
            DriveButtonFactory factory = Undoable.AddComponent<DriveButtonFactory>(disposable);
            SetButtonValues(factory, panel, parent);
            factory.buttonText = buttonText;
            factory.filePanel = panel;
            factory.buttonProfile = buttonProfile;
            return GenerateKeyboardButtonAndSetPosition(factory);
        }

        protected GameObject FileNameButton(FileSaveAs panel, GameObject parent, ButtonProfile buttonProfile, string buttonText) {
            SaveAsFileNameButtonFactory factory = Undoable.AddComponent<SaveAsFileNameButtonFactory>(disposable);
            SetButtonValues(factory, panel, parent);
            factory.buttonText = buttonText;
            factory.filePanel = panel;
            factory.buttonProfile = buttonProfile;
            return GenerateKeyboardButtonAndSetPosition(factory);
        }

        protected GameObject SaveAsButton(FileSaveAs panel, GameObject parent, ButtonProfile buttonProfile, string buttonText) {
            SaveAsButtonFactory factory = Undoable.AddComponent<SaveAsButtonFactory>(disposable);
            SetButtonValues(factory, panel, parent);
            factory.buttonText = buttonText;
            factory.filePanel = panel;
            factory.buttonProfile = buttonProfile;
            return GenerateKeyboardButtonAndSetPosition(factory);
        }

        protected GameObject KnownFolderButton(FileSaveAs panel, GameObject parent, ButtonProfile buttonProfile, string buttonText, KnownFolder knownFolder) {
            KnownFolderButtonFactory factory = Undoable.AddComponent<KnownFolderButtonFactory>(disposable);
            SetButtonValues(factory, panel, parent);
            factory.buttonText = buttonText;
            factory.filePanel = panel;
            factory.buttonProfile = buttonProfile;
            factory.knownFolder = knownFolder;
            return GenerateKeyboardButtonAndSetPosition(factory);
        }

        protected GameObject Row(GameObject parent, string name = null, TextAlignment alignment = TextAlignment.Center) {
            RowContainerFactory factory = Undoable.AddComponent<RowContainerFactory>(disposable);
            factory.containerName = name;
            factory.parent = parent;
            factory.padding = padding;
            factory.spacing = spacing;
            factory.alignment = alignment;
            return factory.Generate();
        }

        protected GameObject Column(GameObject parent) {
            ColumnContainerFactory factory = Undoable.AddComponent<ColumnContainerFactory>(disposable);
            factory.parent = parent;
            factory.padding = padding;
            factory.spacing = spacing;
            return factory.Generate();
        }

        protected GameObject Panel(GameObject parent, string name) {
            PanelContainerProfile profile = Defaults.GetProfile(panelContainerProfile);
            PanelContainerFactory factory = Undoable.AddComponent<PanelContainerFactory>(disposable);
            factory.parent = parent;
            factory.containerName = name;
            factory.panelContainerProfile = profile;
            GameObject panel = factory.Generate();

            fileSaveAsPanel = Undoable.AddComponent<FileSaveAs>(panel);
            fileSaveAsPanel.grabTarget = fileSaveAsContainerInstance.transform;
            fileSaveAsPanel.folderPrefab = folderPrefab;
            fileSaveAsPanel.kineticScrollItemPrefab = kineticScrollerItem;
            fileSaveAsPanel.height = scrollerHeight;
            fileSaveAsPanel.keyboard = keyboard;
            fileSaveAsPanel.searchPattern = searchPattern;
            fileSaveAsPanel.panelProfile = panelProfile;

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
            ButtonProfile profile = Defaults.GetMomentaryButtonProfile(momentaryButtonProfile);
            GameObject label = EmptyChild(parent, name);
            label.transform.localScale = profile.labelScale;
            Vector3 localPosition = new Vector3(0, 0, profile.labelZ);
            label.transform.localPosition = localPosition;
            TextMesh textMesh = Undoable.AddComponent<TextMesh>(label);
            textMesh.text = text;
            textMesh.fontSize = profile.fontSize;
            textMesh.color = profile.fontColor;
            textMesh.characterSize = labelCharacterSize;
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
            fileSaveAsPanel.currentPathLabel = currentPathLabel;
            return row;
        }

        protected GameObject DriveButtonRow(GameObject parent) {
            GameObject row = Row(parent, "DriveButtonRow", TextAlignment.Left);
            Label(row, "DrivesLabel", "              Drives");
            ButtonProfile profile = Defaults.GetMomentaryButtonProfile(momentaryButtonProfile);

            GameObject driveButtonPrefab = DriveButton(fileSaveAsPanel, row, profile, "C");
            drives.driveButtonPrefab = driveButtonPrefab;
            return row;
        }

        protected GameObject SaveAsButtonRow(GameObject parent) {
            GameObject row = Row(parent, "SaveAsButtonRow", TextAlignment.Right);
            ButtonProfile profile = Defaults.GetMomentaryButtonProfile(momentaryButtonProfile);

            SaveAsButton(fileSaveAsPanel, row, profile, "Save As");
            return row;
        }

        protected GameObject FileNameButtonRow(GameObject parent) {
            GameObject row = Row(parent, "FileNameButtonRow", TextAlignment.Left);
            Label(row, "FileNameLabel", "         FileName");
            ButtonProfile profile = Defaults.GetMomentaryButtonProfile(momentaryButtonProfile);

            GameObject fileNameObject = FileNameButton(fileSaveAsPanel, row, profile, "FileName");
            fileNameButton = fileNameObject.GetComponent<SaveAsFileNameButton>();
            fileSaveAsPanel.fileNameButton = fileNameButton;
            Label(row, "ExtLabel", fileNameExtension);
            return row;
        }

        protected GameObject SpecialFoldersRow(GameObject parent) {
            GameObject row = Row(parent, "SpecialFoldersRow", TextAlignment.Left);
            Label(row, "SpecialFoldersLabel", "Special Folders");
            ButtonProfile profile = Defaults.GetMomentaryButtonProfile(momentaryButtonProfile);

            KnownFolderButton(fileSaveAsPanel, row, profile, "Documents", KnownFolder.Documents);
            KnownFolderButton(fileSaveAsPanel, row, profile, "Downloads", KnownFolder.Downloads);
            KnownFolderButton(fileSaveAsPanel, row, profile, "Desktop", KnownFolder.Desktop);

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
            fileSaveAsPanel.kineticScroller = kineticScroller;

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
            configurableJoint.connectedBody = fileSaveAsContainerRigidbody;
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
            SaveAsButtonRow(parent);
            CurrentPathRow(parent);
        }

        protected void FileSaveAsPanel(GameObject parent) {
            if (fileSaveAsInstance) return;

            fileSaveAsInstance = EmptyChild(parent, "FileSaveAsPanel");
            Vector3 localPosition = fileSaveAsInstance.transform.localPosition;
            localPosition.y = -scrollerHeight * 1.75f;
            fileSaveAsInstance.transform.localPosition = localPosition;

            Rigidbody rigidbody = Undoable.AddComponent<Rigidbody>(fileSaveAsInstance);
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            GameObject panel = Panel(fileSaveAsInstance, "DrivesPanel");
            GameObject column = Column(panel);

            PanelHeader(fileSaveAsPanel, column);
            FileNameButtonRow(column);
            DriveButtonRow(column);
            SpecialFoldersRow(column);

            panel.transform.localPosition = Vector3.zero;
        }

        public override GameObject Generate() {
            base.Generate();

#if UNITY_EDITOR
            Undo.SetCurrentGroupName("FileSaveAsFactory Generate");
            int group = Undo.GetCurrentGroup();

            Undo.RegisterCompleteObjectUndo(this, "FileSaveAsFactory state");
#endif
            CreateDisposable(parent);

            fileSaveAsContainerInstance = EmptyChild(parent, "FileSaveAsContainer");
            fileSaveAsContainerRigidbody = Undoable.AddComponent<Rigidbody>(fileSaveAsContainerInstance);
            fileSaveAsContainerRigidbody.useGravity = false;
            fileSaveAsContainerRigidbody.isKinematic = true;

            kineticScrollerItem = CreateKineticScrollerItem(fileSaveAsContainerInstance);
            kineticScrollerItem.SetActive(false);

            FileSaveAsPanel(fileSaveAsContainerInstance);

            CreateKineticScroller(fileSaveAsContainerInstance);

#if UNITY_EDITOR
            Undo.DestroyObjectImmediate(disposable);
            Undo.CollapseUndoOperations(group);
#else
            Destroy(disposable);
#endif
            return fileSaveAsInstance;
        }
    }
}
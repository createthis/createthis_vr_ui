using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using CreateThis.Factory.VR.UI.Button;
using CreateThis.Unity;
using CreateThis.VR.UI;
using CreateThis.VR.UI.Button;
using CreateThis.VR.UI.Panel;
using CreateThis.VR.UI.File;

namespace CreateThis.Factory.VR.UI.File {
    public class FileSaveAsFactory : FileBaseFactory {
        public string fileNameExtension;
        public Keyboard keyboard;

        protected FileSaveAs fileSaveAs;
        private SaveAsFileNameButton fileNameButton;

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

        protected override FileBase AddFilePanel(GameObject panel) {
            fileSaveAs = Undoable.AddComponent<FileSaveAs>(panel);
            fileSaveAs.keyboard = keyboard;
            return fileSaveAs;
        }

        protected GameObject SaveAsButtonRow(GameObject parent) {
            GameObject row = Row(parent, "SaveAsButtonRow", TextAlignment.Right);
            ButtonProfile profile = Defaults.GetMomentaryButtonProfile(momentaryButtonProfile);

            SaveAsButton(fileSaveAs, row, profile, "Save As");
            return row;
        }

        protected GameObject FileNameButtonRow(GameObject parent) {
            GameObject row = Row(parent, "FileNameButtonRow", TextAlignment.Left);
            Label(row, "FileNameLabel", "         FileName");
            ButtonProfile profile = Defaults.GetMomentaryButtonProfile(momentaryButtonProfile);

            GameObject fileNameObject = FileNameButton(fileSaveAs, row, profile, "FileName");
            fileNameButton = fileNameObject.GetComponent<SaveAsFileNameButton>();
            fileSaveAs.fileNameButton = fileNameButton;
            Label(row, "ExtLabel", fileNameExtension);
            return row;
        }

        protected override void PanelHeader(StandardPanel panel, GameObject parent) {
            SaveAsButtonRow(parent);
            CurrentPathRow(parent);
        }

        protected override void FilePanel(GameObject parent) {
            if (filePanelInstance) return;

            FilePanelProfile fpProfile = Defaults.GetProfile(filePanelProfile);
            filePanelInstance = EmptyChild(parent, "FileSaveAsPanel");
            Vector3 localPosition = filePanelInstance.transform.localPosition;
            localPosition.y = -fpProfile.scrollerHeight * 1.75f;
            filePanelInstance.transform.localPosition = localPosition;

            Rigidbody rigidbody = Undoable.AddComponent<Rigidbody>(filePanelInstance);
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            GameObject panel = Panel(filePanelInstance, "DrivesPanel");
            GameObject column = Column(panel);

            PanelHeader(filePanel, column);
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

            filePanelContainerInstance = EmptyChild(parent, "FileSaveAsContainer");
            filePanelContainerRigidbody = Undoable.AddComponent<Rigidbody>(filePanelContainerInstance);
            filePanelContainerRigidbody.useGravity = false;
            filePanelContainerRigidbody.isKinematic = true;

            kineticScrollerItem = CreateKineticScrollerItem(filePanelContainerInstance);
            kineticScrollerItem.SetActive(false);

            FilePanel(filePanelContainerInstance);

            CreateKineticScroller(filePanelContainerInstance);

#if UNITY_EDITOR
            Undo.DestroyObjectImmediate(disposable);
            Undo.CollapseUndoOperations(group);
#else
            Destroy(disposable);
#endif
            return filePanelInstance;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Events;
#endif
using CreateThis.Unity;
using CreateThis.Factory;
using CreateThis.Factory.VR.UI;
using CreateThis.Factory.VR.UI.File;
using CreateThis.VR.UI;
using CreateThis.VR.UI.File;
using CreateThis.VR.UI.Panel;
using CreateThis.VR.UI.Controller;

namespace CreateThis.Example {
    public class ExampleMasterUIFactory : BaseFactory {
        public GameObject parent;
        public ExampleSkyboxManager skyboxManager;
        public TouchPadMenuController touchPadMenuController;
        public Vector3 fileOpenLocalPosition = new Vector3(-1.329f, 0, 0);
        public Vector3 fileSaveAsLocalPosition = new Vector3(-0.754f, 0, 0);
        public Vector3 toolsLocalPosition = new Vector3(-0.3367146f, 0.3599027f, 0);
        public Vector3 keyboardLocalPosition = new Vector3(0.357f, 0, 0);

        private GameObject disposable;
        private GameObject keyboardInstance;
        private Keyboard keyboard;
        private GameObject fileSaveAsInstance;
        private FileSaveAs fileSaveAs;
        private GameObject fileOpenInstance;
        private FileOpen fileOpen;
        private GameObject toolsInstance;

        private GameObject CreateKeyboard() {
            KeyboardFactory factory = Undoable.AddComponent<KeyboardFactory>(disposable);
            factory.parent = parent;
            GameObject panel = factory.Generate();
            keyboardInstance = panel;
            Vector3 localPosition = keyboardInstance.transform.localPosition;
            localPosition.x = keyboardLocalPosition.x;
            keyboardInstance.transform.localPosition = localPosition;
            keyboard = keyboardInstance.GetComponent<Keyboard>();
            return panel;
        }

        private GameObject CreateFileSaveAs() {
            FileSaveAsFactory factory = Undoable.AddComponent<FileSaveAsFactory>(disposable);
            factory.parent = parent;
            factory.keyboard = keyboard;
            GameObject panel = factory.Generate();
            fileSaveAsInstance = panel;
            GameObject fileSaveAsContainer = panel.transform.parent.gameObject;
            fileSaveAsContainer.transform.localPosition = fileSaveAsLocalPosition;
            fileSaveAs = fileSaveAsInstance.transform.Find("DrivesPanel").GetComponent<FileSaveAs>();
            return panel;
        }

        private GameObject CreateFileOpen() {
            FileOpenFactory factory = Undoable.AddComponent<FileOpenFactory>(disposable);
            factory.parent = parent;
            GameObject panel = factory.Generate();
            fileOpenInstance = panel;
            GameObject fileOpenContainer = panel.transform.parent.gameObject;
            fileOpenContainer.transform.localPosition = fileOpenLocalPosition;
            fileOpen = fileOpenInstance.transform.Find("DrivesPanel").GetComponent<FileOpen>();
            return panel;
        }

        private GameObject CreateToolsPanel() {
            ToolsExamplePanelFactory factory = Undoable.AddComponent<ToolsExamplePanelFactory>(disposable);
            factory.parent = parent;
            factory.fileOpen = fileOpen;
            factory.fileSaveAs = fileSaveAs;
            factory.skyboxManager = skyboxManager;
            GameObject panel = factory.Generate();
            toolsInstance = panel;
            Vector3 localPosition = toolsInstance.transform.localPosition;
            localPosition.x = toolsLocalPosition.x;
            toolsInstance.transform.localPosition = localPosition;

#if UNITY_EDITOR
            var touchPadButtons = touchPadMenuController.touchPadButtons;
            for (int i=0; i < touchPadButtons[0].onSelected.GetPersistentEventCount(); i++) {
                UnityEventTools.RemovePersistentListener(touchPadButtons[0].onSelected, 0);
            }
            UnityEventTools.AddPersistentListener(touchPadButtons[0].onSelected, toolsInstance.GetComponent<StandardPanel>().ToggleVisible);
            touchPadMenuController.touchPadButtons = touchPadButtons;
#endif

            return panel;
        }

        private void CreateDisposable() {
            if (disposable) return;
            disposable = EmptyChild(parent, "disposable");
        }

        private void CleanParent() {
            var children = new List<GameObject>();
            foreach (Transform child in parent.transform) children.Add(child.gameObject);
#if UNITY_EDITOR
            children.ForEach(child => Undo.DestroyObjectImmediate(child));
#else
            children.ForEach(child => GameObject.DestroyImmediate(child));
#endif
        }

        public override GameObject Generate() {
            base.Generate();

#if UNITY_EDITOR
            Undo.SetCurrentGroupName("MasterFactory Generate");
            int group = Undo.GetCurrentGroup();

            Undo.RegisterCompleteObjectUndo(this, "MasterFactory state");
#endif
            CleanParent();

            CreateDisposable();
            CreateKeyboard();
            CreateFileSaveAs();
            CreateFileOpen();
            CreateToolsPanel();

#if UNITY_EDITOR
            Undo.DestroyObjectImmediate(disposable);
            Undo.CollapseUndoOperations(group);
#else
            Destroy(disposable);
#endif
            return parent;
        }
    }
}
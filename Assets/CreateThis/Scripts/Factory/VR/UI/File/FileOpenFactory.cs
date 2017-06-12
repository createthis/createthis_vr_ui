using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using CreateThis.Unity;
using CreateThis.VR.UI.File;

namespace CreateThis.Factory.VR.UI.File {
    public class FileOpenFactory : FileBaseFactory {
        protected override FileBase AddFilePanel(GameObject panel) {
            return Undoable.AddComponent<FileOpen>(panel);
        }

        public override GameObject Generate() {
            base.Generate();

#if UNITY_EDITOR
            Undo.SetCurrentGroupName("FileOpenFactory Generate");
            int group = Undo.GetCurrentGroup();

            Undo.RegisterCompleteObjectUndo(this, "FileOpenFactory state");
#endif
            CreateDisposable(parent);

            filePanelContainerInstance = EmptyChild(parent, "FileOpenContainer");
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
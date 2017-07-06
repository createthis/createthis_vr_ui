using UnityEngine;
using CreateThis.VR.UI.UnityEvent;

namespace CreateThis.VR.UI.File {
    public class FileOpen : FileBase {
        public FilePathEvent onOpen;

        protected override void ClickedFile(string path, Transform controller, int controllerIndex) {
            onOpen.Invoke(path, controller, controllerIndex);
        }
    }
}
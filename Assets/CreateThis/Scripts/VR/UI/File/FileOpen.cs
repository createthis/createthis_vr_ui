using CreateThis.VR.UI.UnityEvent;

namespace CreateThis.VR.UI.File {
    public class FileOpen : FileBase {
        public FilePathEvent onOpen;

        protected new void ClickedFile(string path) {
            onOpen.Invoke(path);
        }
    }
}
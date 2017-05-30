using System.IO;
using UnityEngine;
using CreateThis.VR.UI.UnityEvent;
using CreateThis.VR.UI.Button;
using CreateThis.VR.UI.Panel;

namespace CreateThis.VR.UI.File {
    public class FileSaveAs : FileBase {
        public SaveAsFileNameButton fileNameButton;
        public Keyboard keyboard;
        public FilePathEvent onSaveAs;

        private string filename;

        public void SetFilename(string value) {
            filename = value;
            fileNameButton.SetPath(filename);
        }

        public void KeyboardCallback(string filename) {
            SetFilename(filename);
            keyboard.SetVisible(false);
            this.SetVisible(true);
        }

        public void FileNameClick() {
            this.SetVisible(false);
            keyboard.SetBuffer(filename);
            keyboard.doneCallback = KeyboardCallback;
            keyboard.SetVisible(true);
        }

        public void SaveAs() {
            string path = Path.Combine(currentPath, filename);
            onSaveAs.Invoke(path);
        }

        protected new void ClickedFile(string path) {
            SetFilename(Path.GetFileNameWithoutExtension(path));
        }
    }
}
using System.IO;
using UnityEngine;
using CreateThis.VR.UI.UnityEvent;
using CreateThis.VR.UI.Panel;

namespace CreateThis.VR.UI.File {
    public class FileSaveAs : FileBase {
        public TextMesh filenameButtonTextMesh;
        public StandardPanel fileSaveAsPanel;
        public StandardPanel keyboardPanel;
        public Keyboard keyboard;
        public FilePathEvent onSaveAs;

        private string filename;

        public void SetFilename(string value) {
            filename = value;
            filenameButtonTextMesh.text = filename;
        }

        public void KeyboardCallback(string filename) {
            SetFilename(filename);
            keyboardPanel.SetVisible(false);
            fileSaveAsPanel.SetVisible(true);
        }

        public void FileNameClick() {
            fileSaveAsPanel.SetVisible(false);
            keyboard.SetBuffer(filename);
            keyboard.doneCallback = KeyboardCallback;
            keyboardPanel.SetVisible(true);
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
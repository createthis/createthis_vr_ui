using System.IO;
using UnityEngine;
using CreateThis.VR.UI.UnityEvent;
using CreateThis.VR.UI.Button;

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

        public void KeyboardCallback(string filename, Transform controller) {
            SetFilename(filename);
            keyboard.SetController(controller);
            keyboard.SetVisible(false);
            this.controller = controller;
            this.SetVisible(true);
        }

        public void FileNameClick(Transform controller) {
            this.SetVisible(false);
            keyboard.SetController(controller);
            keyboard.SetBuffer(filename);
            keyboard.doneCallback = KeyboardCallback;
            keyboard.SetVisible(true);
        }

        public void SaveAs(Transform controller, int controllerIndex) {
            string path = Path.Combine(currentPath, filename);
            onSaveAs.Invoke(path, controller, controllerIndex);
        }

        protected override void ClickedFile(string path, Transform controller, int controllerIndex) {
            SetFilename(Path.GetFileNameWithoutExtension(path));
        }
    }
}
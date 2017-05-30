using UnityEngine;
using CreateThis.VR.UI.File;

namespace CreateThis.VR.UI.Button {
    public class FileNameButton : MomentaryButton {
        public FileBase filePanel;

        private string path;

        protected override void ClickHandler(Transform controller, int controllerIndex) {
            base.ClickHandler(controller, controllerIndex);
            filePanel.ChangeDirectory(path);
        }

        public void SetPath(string value) {
            path = value;
            buttonText.GetComponent<TextMesh>().text = value;
        }
    }
}

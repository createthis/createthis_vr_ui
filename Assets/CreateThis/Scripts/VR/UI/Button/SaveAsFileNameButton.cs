using UnityEngine;
using CreateThis.VR.UI.File;

namespace CreateThis.VR.UI.Button {
    public class SaveAsFileNameButton : MomentaryButton {
        public FileSaveAs filePanel;

        protected override void ClickHandler(Transform controller, int controllerIndex) {
            base.ClickHandler(controller, controllerIndex);
            filePanel.FileNameClick();
        }

        public void SetPath(string value) {
            buttonText.GetComponent<TextMesh>().text = value;
        }
    }
}

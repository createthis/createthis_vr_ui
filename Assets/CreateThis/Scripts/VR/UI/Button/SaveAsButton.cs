using UnityEngine;
using CreateThis.VR.UI.File;

namespace CreateThis.VR.UI.Button {
    public class SaveAsButton : MomentaryButton {
        public FileSaveAs filePanel;

        protected override void ClickHandler(Transform controller, int controllerIndex) {
            base.ClickHandler(controller, controllerIndex);
            filePanel.SaveAs(controller, controllerIndex);
        }
    }
}

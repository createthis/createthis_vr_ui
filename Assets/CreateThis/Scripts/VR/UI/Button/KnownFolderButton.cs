using UnityEngine;
using CreateThis.System;
using CreateThis.VR.UI.File;

namespace CreateThis.VR.UI.Button {
    public class KnownFolderButton : MomentaryButton {
        public FileBase filePanel;
        public KnownFolder knownFolder;

        protected override void ClickHandler(Transform controller, int controllerIndex) {
            base.ClickHandler(controller, controllerIndex);
            filePanel.ChangeDirectoryToKnownFolder(knownFolder);
        }
    }
}

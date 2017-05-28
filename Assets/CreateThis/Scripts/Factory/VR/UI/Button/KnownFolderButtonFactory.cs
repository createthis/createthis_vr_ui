using UnityEngine;
using CreateThis.System;
using CreateThis.VR.UI.File;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class KnownFolderButtonFactory : MomentaryButtonFactory {
        public FileBase filePanel;
        public KnownFolder knownFolder;

        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            KnownFolderButton button = SafeAddComponent<KnownFolderButton>(target);
            if (audioSourceDown) button.buttonClickDown = audioSourceDown;
            if (audioSourceUp) button.buttonClickUp = audioSourceUp;
            button.buttonBody = buttonBodyInstance;
            button.buttonText = buttonTextLabelInstance;
            button.filePanel = filePanel;
            if (panel) button.panel = panel;
            button.knownFolder = knownFolder;
        }
    }
}
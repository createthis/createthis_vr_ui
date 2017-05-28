using UnityEngine;
using CreateThis.VR.UI.File;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class DriveButtonFactory : MomentaryButtonFactory {
        public FileBase filePanel;

        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            DriveButton button = SafeAddComponent<DriveButton>(target);
            if (audioSourceDown) button.buttonClickDown = audioSourceDown;
            if (audioSourceUp) button.buttonClickUp = audioSourceUp;
            button.buttonBody = buttonBodyInstance;
            button.buttonText = buttonTextLabelInstance;
            button.filePanel = filePanel;
            if (panel) button.panel = panel;
        }
    }
}
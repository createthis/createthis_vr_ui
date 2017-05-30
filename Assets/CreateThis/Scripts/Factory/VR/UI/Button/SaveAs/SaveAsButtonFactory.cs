using UnityEngine;
using CreateThis.VR.UI.File;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class SaveAsButtonFactory : MomentaryButtonFactory {
        public FileSaveAs filePanel;

        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            SaveAsButton button = SafeAddComponent<SaveAsButton>(target);
            if (audioSourceDown) button.buttonClickDown = audioSourceDown;
            if (audioSourceUp) button.buttonClickUp = audioSourceUp;
            button.buttonBody = buttonBodyInstance;
            button.buttonText = buttonTextLabelInstance;
            button.filePanel = filePanel;
            button.clickOnTriggerExit = true;
            if (panel) button.panel = panel;
        }
    }
}

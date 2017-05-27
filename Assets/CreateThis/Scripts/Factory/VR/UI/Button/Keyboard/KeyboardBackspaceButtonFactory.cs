using UnityEngine;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class KeyboardBackspaceButtonFactory : KeyboardButtonFactory {
        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            KeyboardBackspaceButton button = SafeAddComponent<KeyboardBackspaceButton>(target);
            if (audioSourceDown) button.buttonClickDown = audioSourceDown;
            if (audioSourceUp) button.buttonClickUp = audioSourceUp;
            button.buttonBody = buttonBodyInstance;
            button.buttonText = buttonTextLabelInstance;
            button.keyboard = keyboard;
            button.clickOnTriggerExit = true;
            if (panel) button.panel = panel;
        }
    }
}

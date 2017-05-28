using UnityEngine;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class KeyboardDoneButtonFactory : KeyboardButtonFactory {
        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            KeyboardDoneButton button = SafeAddComponent<KeyboardDoneButton>(target);
            if (audioSourceDown) button.buttonClickDown = audioSourceDown;
            if (audioSourceUp) button.buttonClickUp = audioSourceUp;
            button.buttonBody = buttonBodyInstance;
            button.buttonText = buttonTextLabelInstance;
            button.keyboard = keyboard;
            button.clickOnTriggerExit = true;
            button.repeat = true;
            if (panel) button.panel = panel;
        }
    }
}

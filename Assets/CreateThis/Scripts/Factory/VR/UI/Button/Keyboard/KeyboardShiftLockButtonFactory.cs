using UnityEngine;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class KeyboardShiftLockButtonFactory : KeyboardButtonFactory {
        public bool on;

        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            KeyboardShiftLockButton button = SafeAddComponent<KeyboardShiftLockButton>(target);
            if (audioSourceDown) button.buttonClickDown = audioSourceDown;
            if (audioSourceUp) button.buttonClickUp = audioSourceUp;
            button.buttonBody = buttonBodyInstance;
            button.buttonText = buttonTextLabelInstance;
            button.keyboard = keyboard;
            button.On = on;
            button.clickOnTriggerExit = true;
            if (panel) button.panel = panel;
        }
    }
}
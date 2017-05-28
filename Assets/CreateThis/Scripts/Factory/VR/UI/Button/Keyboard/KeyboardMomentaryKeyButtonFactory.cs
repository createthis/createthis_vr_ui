using UnityEngine;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class KeyboardMomentaryKeyButtonFactory : KeyboardButtonFactory {
        public string value;

        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            KeyboardMomentaryKeyButton button = SafeAddComponent<KeyboardMomentaryKeyButton>(target);
            if (audioSourceDown) button.buttonClickDown = audioSourceDown;
            if (audioSourceUp) button.buttonClickUp = audioSourceUp;
            button.buttonBody = buttonBodyInstance;
            button.buttonText = buttonTextLabelInstance;
            button.keyboard = keyboard;
            button.value = value;
            button.repeat = true;
            if (panel) button.panel = panel;
        }
    }
}
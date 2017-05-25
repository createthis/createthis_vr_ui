using UnityEngine;
using CreateThis.VR.UI;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class KeyboardButtonFactory : MomentaryButtonFactory {
        public Keyboard keyboard;
        public string value;

        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            KeyboardButton button = SafeAddComponent<KeyboardButton>(target);
            if (audioSourceDown) button.buttonClickDown = audioSourceDown;
            if (audioSourceUp) button.buttonClickUp = audioSourceUp;
            button.buttonBody = buttonBodyInstance;
            button.buttonText = buttonTextLabelInstance;
            button.keyboard = keyboard;
            button.value = value;
        }
    }
}
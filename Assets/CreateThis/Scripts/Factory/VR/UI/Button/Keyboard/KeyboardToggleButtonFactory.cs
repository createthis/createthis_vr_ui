using UnityEngine;
using CreateThis.VR.UI;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class KeyboardToggleButtonFactory : ToggleButtonFactory {
        public Keyboard keyboard;

        public void PopulateButton(KeyboardToggleButton button, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            base.PopulateButton(button, audioSourceDown, audioSourceUp);
            button.keyboard = keyboard;
        }

        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            KeyboardToggleButton button = SafeAddComponent<KeyboardToggleButton>(target);
            PopulateButton(button, audioSourceDown, audioSourceUp);
        }
    }
}
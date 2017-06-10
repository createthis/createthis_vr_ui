using UnityEngine;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class TriggerExitKeyboardToggleButtonFactory : KeyboardToggleButtonFactory {
        public new void PopulateButton(KeyboardToggleButton button, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            base.PopulateButton(button, audioSourceDown, audioSourceUp);
            button.clickOnTriggerExit = true;
        }

        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            ToggleButton button = SafeAddComponent<KeyboardToggleButton>(target);
            PopulateButton(button, audioSourceDown, audioSourceUp);
        }
    }
}

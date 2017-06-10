using UnityEngine;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class TriggerExitKeyboardButtonFactory : KeyboardButtonFactory {
        public override void PopulateButton(KeyboardMomentaryButton button, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            base.PopulateButton(button, audioSourceDown, audioSourceUp);
            button.clickOnTriggerExit = true;
        }

        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            KeyboardMomentaryButton button = SafeAddComponent<KeyboardMomentaryButton>(target);
            PopulateButton(button, audioSourceDown, audioSourceUp);
        }
    }
}

using UnityEngine;
using CreateThis.Unity;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class RepeatingKeyboardButtonFactory : KeyboardButtonFactory {
        public override void PopulateButton(KeyboardMomentaryButton button, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            base.PopulateButton(button, audioSourceDown, audioSourceUp);
            button.repeat = true;
        }

        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            KeyboardMomentaryButton button = Undoable.AddComponent<KeyboardMomentaryButton>(target);
            PopulateButton(button, audioSourceDown, audioSourceUp);
        }
    }
}
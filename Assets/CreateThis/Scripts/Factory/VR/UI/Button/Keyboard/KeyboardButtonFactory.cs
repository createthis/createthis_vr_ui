using UnityEngine;
using CreateThis.Unity;
using CreateThis.VR.UI;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class KeyboardButtonFactory : MomentaryButtonFactory {
        public Keyboard keyboard;

        public virtual void PopulateButton(KeyboardMomentaryButton button, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            base.PopulateButton(button, audioSourceDown, audioSourceUp);
            button.keyboard = keyboard;
        }

        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            KeyboardMomentaryButton button = Undoable.AddComponent<KeyboardMomentaryButton>(target);
            PopulateButton(button, audioSourceDown, audioSourceUp);
        }
    }
}
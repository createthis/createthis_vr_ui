using UnityEngine;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class KeyboardDoneButtonFactory : TriggerExitKeyboardButtonFactory {
        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            KeyboardDoneButton button = SafeAddComponent<KeyboardDoneButton>(target);
            PopulateButton(button, audioSourceDown, audioSourceUp);
        }
    }
}

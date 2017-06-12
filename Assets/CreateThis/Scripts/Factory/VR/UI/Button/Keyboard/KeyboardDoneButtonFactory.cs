using UnityEngine;
using CreateThis.Unity;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class KeyboardDoneButtonFactory : TriggerExitKeyboardButtonFactory {
        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            KeyboardDoneButton button = Undoable.AddComponent<KeyboardDoneButton>(target);
            PopulateButton(button, audioSourceDown, audioSourceUp);
        }
    }
}

using UnityEngine;
using CreateThis.Unity;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class KeyboardNumLockButtonFactory : TriggerExitKeyboardButtonFactory {
        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            KeyboardNumLockButton button = Undoable.AddComponent<KeyboardNumLockButton>(target);
            PopulateButton(button, audioSourceDown, audioSourceUp);
        }
    }
}

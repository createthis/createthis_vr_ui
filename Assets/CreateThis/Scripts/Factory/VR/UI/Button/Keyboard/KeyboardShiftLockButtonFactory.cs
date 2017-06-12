using UnityEngine;
using CreateThis.Unity;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class KeyboardShiftLockButtonFactory : TriggerExitKeyboardToggleButtonFactory {
        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            KeyboardShiftLockButton button = Undoable.AddComponent<KeyboardShiftLockButton>(target);
            PopulateButton(button, audioSourceDown, audioSourceUp);
        }
    }
}
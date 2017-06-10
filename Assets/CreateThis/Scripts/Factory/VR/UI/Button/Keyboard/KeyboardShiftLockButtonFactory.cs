using UnityEngine;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class KeyboardShiftLockButtonFactory : TriggerExitKeyboardToggleButtonFactory {
        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            KeyboardShiftLockButton button = SafeAddComponent<KeyboardShiftLockButton>(target);
            PopulateButton(button, audioSourceDown, audioSourceUp);
        }
    }
}
using UnityEngine;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class KeyboardReturnButtonFactory : RepeatingKeyboardButtonFactory {
        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            KeyboardReturnButton button = SafeAddComponent<KeyboardReturnButton>(target);
            PopulateButton(button, audioSourceDown, audioSourceUp);
        }
    }
}

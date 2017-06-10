using UnityEngine;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class KeyboardBackspaceButtonFactory : RepeatingKeyboardButtonFactory {
        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            KeyboardBackspaceButton button = SafeAddComponent<KeyboardBackspaceButton>(target);
            PopulateButton(button, audioSourceDown, audioSourceUp);
        }
    }
}

using UnityEngine;
using CreateThis.Unity;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class KeyboardReturnButtonFactory : RepeatingKeyboardButtonFactory {
        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            KeyboardReturnButton button = Undoable.AddComponent<KeyboardReturnButton>(target);
            PopulateButton(button, audioSourceDown, audioSourceUp);
        }
    }
}

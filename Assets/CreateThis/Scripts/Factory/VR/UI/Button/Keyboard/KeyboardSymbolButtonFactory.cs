using UnityEngine;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class KeyboardSymbolButtonFactory : TriggerExitKeyboardButtonFactory {
        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            KeyboardSymbolButton button = SafeAddComponent<KeyboardSymbolButton>(target);
            PopulateButton(button, audioSourceDown, audioSourceUp);
        }
    }
}

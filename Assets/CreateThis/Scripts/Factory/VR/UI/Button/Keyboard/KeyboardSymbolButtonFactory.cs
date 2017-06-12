using UnityEngine;
using CreateThis.Unity;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class KeyboardSymbolButtonFactory : TriggerExitKeyboardButtonFactory {
        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            KeyboardSymbolButton button = Undoable.AddComponent<KeyboardSymbolButton>(target);
            PopulateButton(button, audioSourceDown, audioSourceUp);
        }
    }
}

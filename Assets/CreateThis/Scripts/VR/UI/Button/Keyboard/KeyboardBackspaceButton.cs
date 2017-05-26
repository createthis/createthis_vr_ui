using UnityEngine;

namespace CreateThis.VR.UI.Button {
    public class KeyboardBackspaceButton : KeyboardMomentaryButton {
        protected override void ClickHandler(Transform controller, int controllerIndex) {
            keyboard.Backspace();
        }
    }
}

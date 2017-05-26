using UnityEngine;

namespace CreateThis.VR.UI.Button {
    public class KeyboardMomentaryKeyButton : KeyboardMomentaryButton {
        public string value;

        protected override void ClickHandler(Transform controller, int controllerIndex) {
            keyboard.PressKey(value);
        }
    }
}

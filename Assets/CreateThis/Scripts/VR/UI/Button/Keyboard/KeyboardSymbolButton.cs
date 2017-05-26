using UnityEngine;

namespace CreateThis.VR.UI.Button {
    public class KeyboardSymbolButton : KeyboardMomentaryButton {
        protected override void ClickHandler(Transform controller, int controllerIndex) {
            keyboard.Symbol();
        }
    }
}

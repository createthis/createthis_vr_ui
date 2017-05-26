using UnityEngine;

namespace CreateThis.VR.UI.Button {
    public class KeyboardReturnButton : KeyboardMomentaryButton {
        protected override void ClickHandler(Transform controller, int controllerIndex) {
            keyboard.Return();
        }
    }
}

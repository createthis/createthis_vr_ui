using UnityEngine;

namespace CreateThis.VR.UI.Button {
    public class KeyboardDoneButton : KeyboardMomentaryButton {
        protected override void ClickHandler(Transform controller, int controllerIndex) {
            keyboard.Done(controller);
        }
    }
}

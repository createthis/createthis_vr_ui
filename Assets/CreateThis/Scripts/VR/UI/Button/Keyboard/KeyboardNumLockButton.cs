using UnityEngine;

namespace CreateThis.VR.UI.Button {
    public class KeyboardNumLockButton : KeyboardMomentaryButton {
        protected override void ClickHandler(Transform controller, int controllerIndex) {
            keyboard.NumLock(true);
        }
    }
}

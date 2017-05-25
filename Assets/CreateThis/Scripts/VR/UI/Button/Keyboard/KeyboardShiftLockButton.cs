using UnityEngine;

namespace CreateThis.VR.UI.Button {
    public class KeyboardShiftLockButton : ToggleButton {
        public Keyboard keyboard;

        protected override void ClickHandler(Transform controller, int controllerIndex) {
            keyboard.ShiftLock(On);
        }
    }
}

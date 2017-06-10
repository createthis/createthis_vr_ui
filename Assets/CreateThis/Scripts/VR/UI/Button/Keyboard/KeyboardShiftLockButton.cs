using UnityEngine;

namespace CreateThis.VR.UI.Button {
    public class KeyboardShiftLockButton : KeyboardToggleButton {
        protected override void HitTravelLimitHandler(Transform controller, int controllerIndex) {
            // ShiftLock does NOT change the On state when it hits the travel limit. It's On state may only be set externally.
            if (!clickOnTriggerExit) {
                ClickHandler(controller, controllerIndex);
            }
        }

        protected override void ClickHandler(Transform controller, int controllerIndex) {
            keyboard.ShiftLock(!On);
        }
    }
}

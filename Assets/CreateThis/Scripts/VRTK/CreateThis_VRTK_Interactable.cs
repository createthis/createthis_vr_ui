#if VRTK
using UnityEngine;
using VRTK;
using CreateThis.VR.UI.Interact;

namespace CreateThis.VRTK {
    public class CreateThis_VRTK_Interactable : VRTK_InteractableObject {
        private int controllerIndex;

        public override void StartTouching(GameObject touchingObject) {
            base.StartTouching(touchingObject);
            VRTK_ControllerReference controllerReference = VRTK_ControllerReference.GetControllerReference(touchingObject);
            controllerIndex = (int)VRTK_ControllerReference.GetRealIndex(controllerReference);
            if (!GetComponent<Touchable>()) return;
            Touchable[] touchables = GetComponents<Touchable>();
            foreach(Touchable touchable in touchables) {
                touchable.OnTouchStart(touchingObject.transform, controllerIndex);
            }
        }

        public override void StopTouching(GameObject touchingObject) {
            base.StopTouching(touchingObject);
            if (!GetComponent<Touchable>()) return;
            Touchable[] touchables = GetComponents<Touchable>();
            foreach (Touchable touchable in touchables) {
                touchable.OnTouchStop(touchingObject.transform, controllerIndex);
            }
            controllerIndex = -1;
        }
    }
}
#endif
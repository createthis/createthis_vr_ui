using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using CreateThis.VR.UI.Interact;

namespace CreateThis.VRTK {
    public class CreateThis_VRTK_Interactable : VRTK_InteractableObject {
        private GameObject touchingObject;
        private int controllerIndex;

        public override void StartTouching(GameObject touchingObject) {
            base.StartTouching(touchingObject);
            VRTK_ControllerReference controllerReference = VRTK_ControllerReference.GetControllerReference(touchingObject);
            controllerIndex = (int)VRTK_ControllerReference.GetRealIndex(controllerReference);
            this.touchingObject = touchingObject;
            GetComponent<ITouchable>().OnTouchEnter(touchingObject.transform, controllerIndex);
        }

        public override void StopTouching(GameObject touchingObject) {
            base.StopTouching(touchingObject);
            GetComponent<ITouchable>().OnTouchExit(touchingObject.transform, controllerIndex);
            touchingObject = null;
            controllerIndex = -1;
        }

        protected override void Update() {
            base.Update();
            if (touchingObject) {
                GetComponent<ITouchable>().OnTouchStay(touchingObject.transform, controllerIndex);
            }
        }
    }
}
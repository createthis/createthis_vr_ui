using VRTK;
using VRTK.GrabAttachMechanics;

namespace CreateThis.VRTK {
    using UnityEngine;
    using CreateThis.VR.UI.Interact;

    public class CreateThis_VRTK_GrabAttach : VRTK_BaseGrabAttach {
        private GameObject grabbingObject;
        private GameObject givenGrabbedObject;

        /// <summary>
        /// The StartGrab method sets up the grab attach mechanic as soon as an object is grabbed. It is also responsible for creating the joint on the grabbed object.
        /// </summary>
        /// <param name="grabbingObject">The object that is doing the grabbing.</param>
        /// <param name="givenGrabbedObject">The object that is being grabbed.</param>
        /// <param name="givenControllerAttachPoint">The point on the grabbing object that the grabbed object should be attached to after grab occurs.</param>
        /// <returns>Is true if the grab is successful, false if the grab is unsuccessful.</returns>
        public override bool StartGrab(GameObject grabbingObject, GameObject givenGrabbedObject, Rigidbody givenControllerAttachPoint) {
            if (base.StartGrab(grabbingObject, givenGrabbedObject, givenControllerAttachPoint)) {
                grabbedObjectScript.isKinematic = true; // not sure why this is necessary. Bug in VRTK perhaps. Seems to turn off kinematic sometimes.
                this.grabbingObject = grabbingObject;
                this.givenGrabbedObject = givenGrabbedObject;

                VRTK_ControllerReference controllerReference = VRTK_ControllerReference.GetControllerReference(grabbingObject);
                int controllerIndex = (int)VRTK_ControllerReference.GetRealIndex(controllerReference);
                givenGrabbedObject.GetComponent<Grabbable>().OnGrabStart(grabbingObject.transform, controllerIndex);
                return true;
            }
            return false;
        }

        /// <summary>
        /// The StopGrab method ends the grab of the current object and cleans up the state.
        /// </summary>
        /// <param name="applyGrabbingObjectVelocity">If true will apply the current velocity of the grabbing object to the grabbed object on release.</param>
        public override void StopGrab(bool applyGrabbingObjectVelocity) {
            VRTK_ControllerReference controllerReference = VRTK_ControllerReference.GetControllerReference(grabbingObject);
            int controllerIndex = (int)VRTK_ControllerReference.GetRealIndex(controllerReference);
            givenGrabbedObject.GetComponent<Grabbable>().OnGrabStop(grabbingObject.transform, controllerIndex);
            ReleaseObject(applyGrabbingObjectVelocity);
            base.StopGrab(applyGrabbingObjectVelocity);
        }

        protected override void Initialise() {
            tracked = false;
            climbable = false;
            kinematic = true;
        }
    }
}
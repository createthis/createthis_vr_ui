using UnityEngine;

namespace CreateThis.VR.UI.Interact {
    public abstract class Grabbable : MonoBehaviour {
        protected Transform controller;
        protected int controllerIndex;
        protected bool grabbing = false;

        // functions that can be called via the messaging system
        public virtual void OnGrabStart(Transform controller, int controllerIndex) {
            this.controller = controller;
            this.controllerIndex = controllerIndex;
            grabbing = true;
        }

        public virtual void OnGrabUpdate(Transform controller, int controllerIndex) {

        }

        public virtual void OnGrabStop(Transform controller, int controllerIndex) {
            grabbing = false;
        }

        protected virtual void Update() {
            if (grabbing) {
                OnGrabUpdate(controller, controllerIndex);
            }
        }
    }
}
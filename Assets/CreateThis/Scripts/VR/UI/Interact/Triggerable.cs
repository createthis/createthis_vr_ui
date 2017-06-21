using UnityEngine;

namespace CreateThis.VR.UI.Interact {
    public abstract class Triggerable : MonoBehaviour {
        protected Transform controller;
        protected int controllerIndex;
        protected bool triggering = false;

        // functions that can be called via the messaging system
        public virtual void OnTriggerDown(Transform controller, int controllerIndex) {
            this.controller = controller;
            this.controllerIndex = controllerIndex;
            triggering = true;
        }

        public virtual void OnTriggerUpdate(Transform controller, int controllerIndex) {

        }

        public virtual void OnTriggerUp(Transform controller, int controllerIndex) {
            triggering = false;
        }

        protected virtual void Update() {
            if (triggering) {
                OnTriggerUpdate(controller, controllerIndex);
            }
        }
    }
}
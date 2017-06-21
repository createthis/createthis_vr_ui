using UnityEngine;

namespace CreateThis.VR.UI.Interact {
    public abstract class Touchable : MonoBehaviour {
        protected Transform controller;
        protected int controllerIndex;
        protected bool touching = false;

        // functions that can be called via the messaging system
        public virtual void OnTouchStart(Transform controller, int controllerIndex) {
            this.controller = controller;
            this.controllerIndex = controllerIndex;
            touching = true;
        }

        public virtual void OnTouchUpdate(Transform controller, int controllerIndex) {

        }

        public virtual void OnTouchStop(Transform controller, int controllerIndex) {
            touching = false;
        }

        protected virtual void Update() {
            if (touching) {
                OnTouchUpdate(controller, controllerIndex);
            }
        }
    }
}
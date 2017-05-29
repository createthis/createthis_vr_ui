using UnityEngine;

namespace CreateThis.VR.UI.Interact {
    public abstract class Triggerable : MonoBehaviour {
        // functions that can be called via the messaging system
        public virtual void OnTriggerDown(Transform controller, int controllerIndex) { }
        public virtual void OnTriggerUp(Transform controller, int controllerIndex) { }
    }
}

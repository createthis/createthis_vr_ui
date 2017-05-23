using UnityEngine;

namespace CreateThis.VR.UI.Interact {
    public abstract class Grabbable : MonoBehaviour {
        // functions that can be called via the messaging system
        public virtual void OnGrabStart(Transform controller, int controllerIndex) { }
        public virtual void OnGrabUpdate(Transform controller, int controllerIndex) { }
        public virtual void OnGrabStop(Transform controller, int controllerIndex) { }
    }
}

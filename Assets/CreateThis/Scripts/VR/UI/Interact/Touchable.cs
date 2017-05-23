using UnityEngine;

namespace CreateThis.VR.UI.Interact {
    public abstract class Touchable : MonoBehaviour {
        // functions that can be called via the messaging system
        public virtual void OnTouchStart(Transform controller, int controllerIndex) { }
        public virtual void OnTouchUpdate(Transform controller, int controllerIndex) { }
        public virtual void OnTouchStop(Transform controller, int controllerIndex) { }
    }
}

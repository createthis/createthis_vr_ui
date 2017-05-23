using UnityEngine;

namespace CreateThis.VR.UI.Interact {
    public interface ITouchable {
        // functions that can be called via the messaging system
        void OnTouchStart(Transform controller, int controllerIndex);
        void OnTouchUpdate(Transform controller, int controllerIndex);
        void OnTouchStop(Transform controller, int controllerIndex);
    }
}

using UnityEngine;

namespace CreateThis.VR.UI.Interact {
    public interface ITouchable {
        // functions that can be called via the messaging system
        void OnTouchEnter(Transform controller, int controllerIndex);
        void OnTouchStay(Transform controller, int controllerIndex);
        void OnTouchExit(Transform controller, int controllerIndex);
    }
}

using UnityEngine;

namespace CreateThis.VR.UI.Interact {
    public interface IGrabbable {
        // functions that can be called via the messaging system
        void OnGrabStart(Transform controller, int controllerIndex);
        void OnGrabUpdate(Transform controller, int controllerIndex);
        void OnGrabStop(Transform controller, int controllerIndex);
    }
}

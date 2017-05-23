using UnityEngine;

namespace CreateThis.VR.UI.Interact {
    public interface ITriggerable {
        // functions that can be called via the messaging system
        void OnTriggerDown(Transform controller, int controllerIndex);
        void OnTriggerUpdate(Transform controller, int controllerIndex);
        void OnTriggerUp(Transform controller, int controllerIndex);
    }
}

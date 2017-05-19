using UnityEngine.EventSystems;

namespace CreateThis.VR.UI.Event {
    public interface IChild3dWidgetResized : IEventSystemHandler {
        // functions that can be called via the messaging system
        void Child3dWidgetResized();
    }
}

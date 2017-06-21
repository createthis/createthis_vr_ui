using UnityEngine;
using CreateThis.VR.UI.Interact;

namespace CreateThis.VR.UI.Scroller {
    public class KineticScrollerItem : Grabbable {
        public KineticScroller kineticScroller;

        public override void OnGrabStart(Transform controller, int controllerIndex) {
            kineticScroller.fileObjectGrabbed = gameObject;
            kineticScroller.OnGrabStart(controller, controllerIndex);
        }

        public override void OnGrabStop(Transform controller, int controllerIndex) {
            kineticScroller.OnGrabStop(controller, controllerIndex);
        }

        // Use this for initialization
        void Start() {

        }
    }
}

#if COLOR_PICKER
using UnityEngine;
using CreateThis.VR.UI.Interact;

namespace CreateThis.VR.UI.ColorPicker {
    public class ColorPickerThumbTouchable : Touchable {
        public bool fixY;
        public Transform thumb;
        public float thumbOffset = 0.0025f;

        private Vector3 lastPosition;

        public override void OnTouchStart(Transform controller, int controllerIndex) {
            base.OnTouchStart(controller, controllerIndex);
            Move(controller.position);
            touching = true;
        }

        public override void OnTouchUpdate(Transform controller, int controllerIndex) {
            base.OnTouchUpdate(controller, controllerIndex);
            Move(controller.position);
        }

        public override void OnTouchStop(Transform controller, int controllerIndex) {
            base.OnTouchStop(controller, controllerIndex);
            Move(controller.position);
            touching = false;
        }

        public void Move(Vector3 position) {
            Vector3 point = GetComponent<Collider>().ClosestPointOnBounds(position);
            Vector3 localPoint = transform.InverseTransformPoint(point);
            Vector3 size = GetComponent<BoxCollider>().size;
            if (localPoint.x < -size.x / 2) localPoint.x = -size.x / 2;
            if (localPoint.x > size.x / 2) localPoint.x = size.x / 2;
            if (fixY) {
                localPoint.y = 0;
            } else {
                if (localPoint.y < -size.y / 2) localPoint.y = -size.y / 2;
                if (localPoint.y > size.y / 2) localPoint.y = size.y / 2;
            }
            point = transform.TransformPoint(localPoint);
            point = -transform.forward * thumbOffset + point;
            SetThumbPosition(point);
            // -(localPoint - 45) / 90
            Vector3 hsbPoint = -(localPoint - (new Vector3(1, 1, 0) * size.x / 2)) / size.x;
            SendMessage("OnDrag", hsbPoint);
        }

        void SetDragPoint(Vector3 hsbPoint) {
            SendMessage("OnDrag", hsbPoint);
            Vector3 size = GetComponent<BoxCollider>().size;
            // 45 - (90 * hsbPoint)
            Vector3 localPoint = (new Vector3(1, 1, 0) * size.x / 2) - (size.x * hsbPoint);
            if (fixY) localPoint.y = 0;
            Vector3 point = transform.TransformPoint(localPoint);
            point = -transform.forward * thumbOffset + point;
            SetThumbPosition(point);
        }

        void SetThumbPosition(Vector3 point) {
            thumb.position = point;
        }

        // Use this for initialization
        void Start() {

        }
    }
}
#endif
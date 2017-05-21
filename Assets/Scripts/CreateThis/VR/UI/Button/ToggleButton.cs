using UnityEngine;
using CreateThis.VR.UI.UnityEvent;

namespace CreateThis.VR.UI.Button {
    [ExecuteInEditMode]
    public class ToggleButton : ButtonBase {
        public BoolEvent onClickBool;

        [SerializeField]
        public bool On {
            get { return on; }
            set {
                Initialize();
                on = value;
                ResetPosition();
            }
        }
        public bool log;

        [SerializeField]
        private bool on;
        private float onStartingZ;

        protected new void ClickHandler(Transform controller, int controllerIndex) {
            onClick.Invoke(controller, controllerIndex);
            onClickBool.Invoke(on);
        }

        protected new void HitTravelLimitHandler(Transform controller, int controllerIndex) {
            on = !on;
            base.HitTravelLimitHandler(controller, controllerIndex);
        }

        protected new float UpdateZHandler(float newZ) {
            if (on && newZ < onStartingZ) {
                newZ = onStartingZ;
            }
            return newZ;
        }

        protected new void ResetPosition() {
            if (on) {
                buttonBody.transform.position = transform.TransformPoint(startingBodyButtonLocalPosition + Vector3.forward * onStartingZ);
                buttonText.transform.position = transform.TransformPoint(startingTextButtonLocalPosition + Vector3.forward * onStartingZ);
            } else {
                buttonBody.transform.position = transform.TransformPoint(startingBodyButtonLocalPosition);
                buttonText.transform.position = transform.TransformPoint(startingTextButtonLocalPosition);
            }
            hitTravelLimit = false;
        }

        protected new void StartingLocalPositionHandler(float buttonBodyDepth) {
            onStartingZ = buttonBodyDepth / 2;
            if (!on) {
                startingBodyButtonLocalPosition = transform.InverseTransformPoint(buttonBody.transform.position);
                startingTextButtonLocalPosition = transform.InverseTransformPoint(buttonText.transform.position);
            } else {
                startingBodyButtonLocalPosition = transform.InverseTransformPoint(buttonBody.transform.position + buttonBody.transform.forward * -1 * onStartingZ);
                startingTextButtonLocalPosition = transform.InverseTransformPoint(buttonText.transform.position + buttonText.transform.forward * -1 * onStartingZ);
            }
        }
    }
}

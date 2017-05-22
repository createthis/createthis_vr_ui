using UnityEngine;
using CreateThis.VR.UI.UnityEvent;

namespace CreateThis.VR.UI.Button {
    public abstract class ButtonBase : MonoBehaviour {
        public AudioSource buttonClickDown;
        public AudioSource buttonClickUp;
        public GameObject buttonBody;
        public GameObject buttonText;
        public GrabEvent onClick;
        public bool clickOnTriggerExit;

        private bool pushing;
        protected bool hitTravelLimit;
        protected Vector3 startingBodyButtonLocalPosition;
        protected Vector3 startingTextButtonLocalPosition;
        private float buttonBodyDepth;
        private float travelLimit;
        private float firstUpdateIgnoreThreshold;
        private bool hasInitialized = false;

        public void OnSelectedEnter(Transform controller, int controllerIndex) {
            pushing = true;
            UpdatePosition(controller, controllerIndex, true);
        }

        public void OnSelectedUpdate(Transform controller, int controllerIndex) {
            UpdatePosition(controller, controllerIndex);
        }

        public void OnSelectedExit(Transform controller, int controllerIndex) {
            if (!pushing) return;
            pushing = false;
            bool tmpHitTravelLimit = hitTravelLimit; // hitTravelLimit is set to false in ResetPosition.
            if (buttonClickUp && hitTravelLimit) buttonClickUp.Play();
            ResetPosition();
            if (tmpHitTravelLimit) {
                SelectedExitAfterHitTravelLimitHandler(controller, controllerIndex);
                if (clickOnTriggerExit) {
                    ClickHandler(controller, controllerIndex);
                }
            }
        }

        protected void ResetPosition() {
            buttonBody.transform.position = transform.TransformPoint(startingBodyButtonLocalPosition);
            buttonText.transform.position = transform.TransformPoint(startingTextButtonLocalPosition);
            hitTravelLimit = false;
        }

        protected void SelectedExitAfterHitTravelLimitHandler(Transform controller, int controllerIndex) {

        }

        protected void ClickHandler(Transform controller, int controllerIndex) {
            onClick.Invoke(controller, controllerIndex);
        }

        protected void HitTravelLimitHandler(Transform controller, int controllerIndex) {
            if (!clickOnTriggerExit) {
                ClickHandler(controller, controllerIndex);
            }
        }

        protected float UpdateZHandler(float newZ) {
            return newZ;
        }

        private void UpdatePosition(Transform controller, int controllerIndex, bool firstUpdate = false) {
            if (!pushing) return;
            Vector3 localControllerPosition = transform.InverseTransformPoint(controller.position);
            float newZ = localControllerPosition.z - startingTextButtonLocalPosition.z;

            if (firstUpdate && newZ > firstUpdateIgnoreThreshold) {
                pushing = false;
                return;
            }

            if (firstUpdate) {
                StartCoroutine(Haptic.LongVibration(controllerIndex, 0.01f, 0.5f));
            }

            newZ = UpdateZHandler(newZ);

            if (newZ > travelLimit) {
                newZ = travelLimit;
                if (!hitTravelLimit) {
                    if (buttonClickDown) buttonClickDown.Play();
                    StartCoroutine(Haptic.LongVibration(controllerIndex, 0.1f, 1f));
                    HitTravelLimitHandler(controller, controllerIndex);
                }
                hitTravelLimit = true;
            }
            buttonBody.transform.position = transform.TransformPoint(startingBodyButtonLocalPosition + Vector3.forward * newZ);
            buttonText.transform.position = transform.TransformPoint(startingTextButtonLocalPosition + Vector3.forward * newZ);
        }

        protected void StartingLocalPositionHandler(float buttonBodyDepth) {
            startingBodyButtonLocalPosition = transform.InverseTransformPoint(buttonBody.transform.position);
            startingTextButtonLocalPosition = transform.InverseTransformPoint(buttonText.transform.position);
        }

        public void Initialize(bool force = false) {
            if (hasInitialized && !force) return;

            buttonBodyDepth = GetComponent<BoxCollider>().size.z;
            travelLimit = buttonBodyDepth * 0.8f;
            StartingLocalPositionHandler(buttonBodyDepth);
            firstUpdateIgnoreThreshold = buttonBodyDepth * 0.7f;
            hitTravelLimit = false;
            hasInitialized = true;
        }

        // Use this for initialization
        void Start() {
            Initialize();
        }

        // Update is called once per frame
        void Update() {
        }
    }
}
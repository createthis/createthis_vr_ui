using System.Collections;
using UnityEngine;
using CreateThis.VR.UI.UnityEvent;

namespace CreateThis.VR.UI.Button {
    public class MomentaryButton : MonoBehaviour {
        public AudioSource buttonClickDown;
        public AudioSource buttonClickUp;
        public GameObject buttonBody;
        public GameObject buttonText;
        public GrabEvent onClick;
        public bool clickOnTriggerExit;
        public bool repeat;

        private bool pushing;
        private bool hitTravelLimit;
        private Vector3 startingBodyButtonLocalPosition;
        private Vector3 startingTextButtonLocalPosition;
        private float buttonBodyDepth;
        private float travelLimit;
        private float firstUpdateIgnoreThreshold;
        private float initialRepeatDelayInSeconds = 0.5f;
        private float repeatDelayInSeconds = 0.1f;
        private IEnumerator repeatCoroutine;

        public void OnSelectedEnter(Transform controller, int controllerIndex) {
            pushing = true;
            UpdatePosition(controller, controllerIndex, true);
        }

        public void OnSelectedUpdate(Transform controller, int controllerIndex) {
            UpdatePosition(controller, controllerIndex);
        }

        public void OnSelectedExit(Transform controller, int controllerIndex) {
            pushing = false;
            bool tmpHitTravelLimit = hitTravelLimit; // hitTravelLimit is set to false in ResetPosition.
            ResetPosition();
            if (tmpHitTravelLimit) {
                if (repeat) {
                    StopCoroutine(repeatCoroutine);
                }
                if (clickOnTriggerExit) {
                    onClick.Invoke(controller, controllerIndex);
                }
            }
        }

        private void ResetPosition() {
            buttonBody.transform.position = transform.TransformPoint(startingBodyButtonLocalPosition);
            buttonText.transform.position = transform.TransformPoint(startingTextButtonLocalPosition);
            hitTravelLimit = false;
            if (buttonClickUp) buttonClickUp.Play();
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

            if (newZ > travelLimit) {
                newZ = travelLimit;
                if (!hitTravelLimit) {
                    if (buttonClickDown) {
                        buttonClickDown.Play();
                        StartCoroutine(Haptic.LongVibration(controllerIndex, 0.1f, 1f));
                        if (!clickOnTriggerExit) {
                            onClick.Invoke(controller, controllerIndex);
                        }
                        if (repeat) {
                            repeatCoroutine = RepeatCoroutine(controller, controllerIndex);
                            StartCoroutine(repeatCoroutine);
                        }
                    }
                }
                hitTravelLimit = true;
            }
            buttonBody.transform.position = transform.TransformPoint(startingBodyButtonLocalPosition + Vector3.forward * newZ);
            buttonText.transform.position = transform.TransformPoint(startingTextButtonLocalPosition + Vector3.forward * newZ);
        }

        private IEnumerator RepeatCoroutine(Transform controller, int controllerIndex) {
            yield return new WaitForSeconds(initialRepeatDelayInSeconds);
            while (true) {
                StartCoroutine(Haptic.LongVibration(controllerIndex, 0.1f, 1f));
                onClick.Invoke(controller, controllerIndex);
                yield return new WaitForSeconds(repeatDelayInSeconds);
            }
        }

        // Use this for initialization
        void Start() {
            startingBodyButtonLocalPosition = transform.InverseTransformPoint(buttonBody.transform.position);
            startingTextButtonLocalPosition = transform.InverseTransformPoint(buttonText.transform.position);
            buttonBodyDepth = GetComponent<BoxCollider>().size.z;
            travelLimit = buttonBodyDepth * 0.8f;
            firstUpdateIgnoreThreshold = buttonBodyDepth * 0.7f;
            hitTravelLimit = false;
        }

        // Update is called once per frame
        void Update() {
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PhysicalToggleButton : MonoBehaviour {
    public AudioSource buttonClickDown;
    public AudioSource buttonClickUp;
    public GameObject buttonBody;
    public GameObject buttonText;
    public MyGrabEvent onClick;
    public MyBoolEvent onClickBool;
    public bool clickOnTriggerExit;

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
    private bool pushing;
    private bool hitTravelLimit;
    private Vector3 startingBodyButtonLocalPosition;
    private Vector3 startingTextButtonLocalPosition;
    private float buttonBodyDepth;
    private float travelLimit;
    private float firstUpdateIgnoreThreshold;
    private float onStartingZ;

    private bool hasInitialized = false;


    public void OnSelectedEnter(Transform controller, int controllerIndex) {
        pushing = true;
        UpdatePosition(controller, controllerIndex, true);
    }

    public void OnSelectedUpdate(Transform controller, int controllerIndex) {
        UpdatePosition(controller, controllerIndex);
    }

    public void OnSelectedExit(Transform controller, int controllerIndex) {
        if (pushing) {
            pushing = false;
            bool tmpHitTravelLimit = hitTravelLimit; // hitTravelLimit is set to false in ResetPosition.
            if (buttonClickUp && hitTravelLimit) buttonClickUp.Play();
            ResetPosition();
            if (tmpHitTravelLimit && clickOnTriggerExit) {
                onClick.Invoke(controller, controllerIndex);
                onClickBool.Invoke(on);
            }
        }
    }

    private void ResetPosition() {
        if (on) {
            buttonBody.transform.position = transform.TransformPoint(startingBodyButtonLocalPosition + Vector3.forward * onStartingZ);
            buttonText.transform.position = transform.TransformPoint(startingTextButtonLocalPosition + Vector3.forward * onStartingZ);
        } else {
            buttonBody.transform.position = transform.TransformPoint(startingBodyButtonLocalPosition);
            buttonText.transform.position = transform.TransformPoint(startingTextButtonLocalPosition);
        }
        hitTravelLimit = false;
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
            StartCoroutine(HapticLib.LongVibration(controllerIndex, 0.01f, 0.5f));
        }

        if (on && newZ < onStartingZ) {
            newZ = onStartingZ;
        }

        if (newZ > travelLimit) {
            newZ = travelLimit;
            if (!hitTravelLimit) {
                on = !on;
                if (buttonClickDown) {
                    buttonClickDown.Play();
                    StartCoroutine(HapticLib.LongVibration(controllerIndex, 0.1f, 1f));
                    if (!clickOnTriggerExit) {
                        onClick.Invoke(controller, controllerIndex);
                        onClickBool.Invoke(on);
                    }
                }
            }
            hitTravelLimit = true;
        }
        buttonBody.transform.position = transform.TransformPoint(startingBodyButtonLocalPosition + Vector3.forward * newZ);
        buttonText.transform.position = transform.TransformPoint(startingTextButtonLocalPosition + Vector3.forward * newZ);
    }

    public void Initialize(bool force = false) {
        if (hasInitialized && !force) return;
        
        buttonBodyDepth = GetComponent<BoxCollider>().size.z;
        travelLimit = buttonBodyDepth * 0.8f;
        onStartingZ = buttonBodyDepth/2;
        if (!on) {
            startingBodyButtonLocalPosition = transform.InverseTransformPoint(buttonBody.transform.position);
            startingTextButtonLocalPosition = transform.InverseTransformPoint(buttonText.transform.position);
        } else {
            startingBodyButtonLocalPosition = transform.InverseTransformPoint(buttonBody.transform.position + buttonBody.transform.forward * -1 * onStartingZ);
            startingTextButtonLocalPosition = transform.InverseTransformPoint(buttonText.transform.position + buttonText.transform.forward * -1 * onStartingZ);
        }
        firstUpdateIgnoreThreshold = buttonBodyDepth * 0.7f;
        hitTravelLimit = false;
        hasInitialized = true;
    }

    // Use this for initialization
    void Start () {
        Initialize();
    }

    // Update is called once per frame
    void Update () {
		
	}
}

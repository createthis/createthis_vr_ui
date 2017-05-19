using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticScroller : MonoBehaviour {
    public FixedJoint fixedJoint;
    public float space = 0.1f;
    public delegate void ClickAction(GameObject fileObject);
    public static event ClickAction OnClicked;
    public GameObject fileObjectGrabbed;

    private float movementThresholdForClick = 0.01f;
    private List<GameObject> list;
    private float height;
    private bool listChanged = false;
    private new Rigidbody rigidbody;
    private ConfigurableJoint slidingJoint;
    private Vector3 dragStartPosition;
    private bool hasInitialized = false;

    public void OnTriggerDown(Transform controller, int controllerIndex) {
        Rigidbody controllerRigidbody = controller.gameObject.GetComponent<Rigidbody>();
        if (!controllerRigidbody) {
            controllerRigidbody = controller.gameObject.AddComponent<Rigidbody>();
            controllerRigidbody.isKinematic = true;
            controllerRigidbody.useGravity = true;
        }
        dragStartPosition = controller.position;
        fixedJoint = gameObject.AddComponent<FixedJoint>();
        fixedJoint.anchor = transform.InverseTransformPoint(controller.position);
        fixedJoint.connectedBody = controllerRigidbody;
        fixedJoint.breakForce = Mathf.Infinity;
        fixedJoint.breakTorque = Mathf.Infinity;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    public void OnTriggerUpdate(Transform controller, int controllerIndex) {
    
    }

    public void OnTriggerUp(Transform controller, int controllerIndex) {
        if (!fixedJoint) return;
        fixedJoint.connectedBody = null;
        Destroy(fixedJoint);

        float distance = Vector3.Distance(dragStartPosition, controller.position);
        if (distance <= movementThresholdForClick) {
            if (OnClicked != null) {
                controller.parent.GetComponent<TouchController>().ClearPickup();
                OnClicked(fileObjectGrabbed);
            }
        } else {
            SteamVR_Controller.Device device = SteamVR_Controller.Input((int)controllerIndex);
            rigidbody.velocity = device.velocity;
            rigidbody.angularVelocity = device.angularVelocity;
        }
        fileObjectGrabbed = null;
    }

    public void SetHeight(float height) {
        this.height = height;
        listChanged = true;
        Initialize();
    }

    public void SetList(List<GameObject> myList) {
        list = myList;
        listChanged = true;
        Initialize();
    }

    public void Initialize() {
        if (!hasInitialized) {
            rigidbody = GetComponent<Rigidbody>();
            slidingJoint = GetComponent<ConfigurableJoint>();
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            hasInitialized = true;
        }
        if (!listChanged) return;

        if (list != null) {
            ScaleGameObjects();
            UpdateSlidingJoint();
        }
        listChanged = false;
    }

    private void UpdateSlidingJoint() {
        gameObject.transform.localPosition = Vector3.zero;
        float width = Width();
        slidingJoint.anchor = new Vector3(0, 0, 0);
        slidingJoint.autoConfigureConnectedAnchor = false;
        slidingJoint.connectedAnchor = new Vector3(-width/2, 0, 0);
        SoftJointLimit limit = slidingJoint.linearLimit;
        limit.limit = width/2;
        slidingJoint.linearLimit = limit;
    }

    private float Width() {
        float width = 0;
        List<float> widths = new List<float>();

        for (int i = 0; i < list.Count-1; i++) {
            widths.Add(height);
        }

        width += PanelLib.SumWithSpacing(widths, space);

        return width;
    }

    private void PositionGameObject(GameObject myGameObject, int index) {
        myGameObject.transform.parent = gameObject.transform;
        myGameObject.transform.localPosition = new Vector3(index * (height + space), 0, 0);
        myGameObject.transform.localRotation = Quaternion.identity;
    }

    private void ScaleGameObject(GameObject myGameObject) {
        Transform[] children = DetachReattachLib.DetachChildren(myGameObject);

        // scale to fit
        Bounds bounds = myGameObject.GetComponent<MeshFilter>().mesh.bounds;
        float x = bounds.size.x;
        float y = bounds.size.y;
        float z = bounds.size.z;
        float max = x;
        if (y > max) max = y;
        if (z > max) max = z;

        float newScale = RatioLib.SolveForD(max, 1f, height);
        myGameObject.transform.localScale = new Vector3(newScale, newScale, newScale);

        DetachReattachLib.ReattachChildren(children, myGameObject);
    }

    private void ScaleGameObjects() {
        for (int i = 0; i < list.Count; i++) {
            PositionGameObject(list[i], i);
            ScaleGameObject(list[i]);
        }
    }

    // Use this for initialization
    void Start() {
        Initialize();
    }

    // Update is called once per frame
    void Update() {

    }
}

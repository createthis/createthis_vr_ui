using UnityEngine;
using CreateThis.VR.UI.Interact;

namespace CreateThis.VR.UI.Controller {
    public class TouchController : MonoBehaviour {
        public Material unselected;
        public Material controllerMaterial;
        public GameObject pointerConePrefab;
        public string hardware;

        private Valve.VR.EVRButtonId touchPadButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
        private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
        private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

        private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
        private SteamVR_TrackedObject trackedObj;
        public GameObject pickup;
        public bool dragging;
        public bool triggerDown;
        public bool deferredTriggerExit;
        public bool noTriggerDownNext;
        private GameObject pointerConeInstance;
        private GameObject spawnPoint;
        private Vector3 validationPoint;


        // Use this for initialization
        void Start() {
            deferredTriggerExit = false;
            trackedObj = GetComponent<SteamVR_TrackedObject>();
            Debug.Log("TouchController[" + trackedObj.index + "] start");
            DetectVRHardware();
            spawnPoint = new GameObject();
            spawnPoint.name = "SpawnPoint";
            spawnPoint.transform.parent = this.transform;
            pointerConeInstance = Instantiate(pointerConePrefab, this.transform.position, this.transform.rotation);
            pointerConeInstance.transform.parent = this.transform;
            UpdatePointerCone(true);

            SteamVR_Events.RenderModelLoaded.Listen(OnRenderModelLoaded);
        }

        public void DetectVRHardware() {
            string model = UnityEngine.VR.VRDevice.model != null ? UnityEngine.VR.VRDevice.model : "";
            if (model.IndexOf("Rift") >= 0) {
                hardware = "oculus_touch";
            } else {
                hardware = "htc_vive";
            }
            Debug.Log("hardware=" + hardware);
        }

        public Quaternion SpawnLocalRotation() {
            return Quaternion.identity;
        }

        public Vector3 SpawnLocalPosition() {
            return Vector3.zero;
        }

        public void UpdateSpawnPoint() {
            spawnPoint.transform.localRotation = SpawnLocalRotation();
            spawnPoint.transform.localPosition = SpawnLocalPosition();

            BoxCollider collider = GetComponent<BoxCollider>();
            Vector3 defaultCenter = new Vector3(0, 0, -0.02f);
            
            collider.center = defaultCenter;
        }

        public void UpdatePointerCone(bool first = true) {
            if (!first) return;

            pointerConeInstance.transform.localRotation = Quaternion.Euler(0, 0, 0);
            pointerConeInstance.transform.localPosition = new Vector3(0, 0, -0.020f);
            
            UpdateSpawnPoint();
        }

        private void OnRenderModelLoaded(SteamVR_RenderModel model, bool connected) {
            Renderer[] renderers = model.gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (Renderer renderer in renderers) {
                renderer.material = controllerMaterial;
                //renderer.enabled = false;
            }
        }

        public void ClearPickup() { // null pickup when user clicks a button on the menu
            pickup = null;
            noTriggerDownNext = true;
        }

        // Update is called once per frame
        void Update() {
            UpdatePointerCone();
            if (controller == null) {
                Debug.Log("[" + trackedObj.index + "] Controller not initialized");
                return;
            }
            BoxCollider collider = GetComponent<BoxCollider>();
            validationPoint = transform.TransformPoint(collider.center) + transform.up * 10.0f; // some point outside the mesh

            // Handle Trigger Down
            if (!dragging && controller.GetPressDown(triggerButton) && !controller.GetPress(touchPadButton)) {
                //Debug.Log("[" + trackedObj.index + "] trigger pressed at " + this.transform.position.ToString());
                if (pickup != null && pickup.GetComponent<ITriggerable>() != null) {
                    pickup.GetComponent<ITriggerable>().OnTriggerDown(spawnPoint.transform, (int)trackedObj.index);
                    if (noTriggerDownNext) {
                        noTriggerDownNext = false;
                    } else {
                        triggerDown = true;
                    }
                }
            }

            if (controller.GetPressUp(triggerButton)) {
                if (pickup != null && pickup.GetComponent<ITriggerable>() != null) {
                    pickup.GetComponent<ITriggerable>().OnTriggerUp(spawnPoint.transform, (int)trackedObj.index);
                    triggerDown = false;
                    if (deferredTriggerExit) {
                        pickup = null; // this happens when holding the trigger while moving away from a collider, then releasing outside the collider.
                        deferredTriggerExit = false;
                    }
                }
                if (pickup == null) {
                    triggerDown = false;
                }
            }

            if (controller.GetPressDown(gripButton) && pickup != null) {
                Debug.Log("[" + trackedObj.index + "] grip pressed");
                if (pickup.GetComponent<IGrabbable>() != null) {
                    pickup.GetComponent<IGrabbable>().OnGrabStart(spawnPoint.transform, (int)trackedObj.index);
                    dragging = true;
                    pickup.GetComponent<Rigidbody>().isKinematic = true;
                }
                Debug.Log("[" + trackedObj.index + "] Dragging true");
            }

            if (dragging && pickup.GetComponent<IGrabbable>() != null) {
                pickup.GetComponent<IGrabbable>().OnGrabUpdate(spawnPoint.transform, (int)trackedObj.index);
            }

            if (triggerDown && pickup && pickup.GetComponent<ITriggerable>() != null) {
                pickup.GetComponent<ITriggerable>().OnTriggerUpdate(spawnPoint.transform, (int)trackedObj.index);
            }

            if (controller.GetPressUp(gripButton) && pickup != null && dragging == true) {
                Debug.Log("[" + trackedObj.index + "] Grip release, pickup.tag=" + pickup.tag);
                if (pickup.GetComponent<IGrabbable>() != null) {
                    pickup.GetComponent<IGrabbable>().OnGrabStop(spawnPoint.transform, (int)trackedObj.index);
                    dragging = false;
                    if (pickup) pickup = null;
                }
                Debug.Log("[" + trackedObj.index + "] Dragging false");
            }
        }

        private void OnTriggerEnter(Collider collider) {
            if (collider.GetComponent<ITouchable>() != null) {
                collider.GetComponent<ITouchable>().OnTouchStart(spawnPoint.transform, (int)trackedObj.index);
            }
            if (!dragging && !triggerDown) {
                pickup = collider.gameObject;
            }
        }

        private void OnTriggerStay(Collider collider) {
            if (collider.GetComponent<ITouchable>() != null) {
                collider.GetComponent<ITouchable>().OnTouchUpdate(spawnPoint.transform, (int)trackedObj.index);
            }
        }

        private void OnTriggerExit(Collider collider) {
            if (collider.GetComponent<ITouchable>() != null) {
                collider.GetComponent<ITouchable>().OnTouchStop(spawnPoint.transform, (int)trackedObj.index);
            }
            if (!dragging && !triggerDown) {
                if (pickup != null && collider != null && pickup != collider.gameObject) {
                    // Warm hand off - do nothing.
                } else {
                    //Debug.Log("OnTriggerExit");

                    if (pickup && !StillInside(collider.transform.position)) {
                        pickup = null;
                    }
                }
            }
            if (triggerDown) deferredTriggerExit = true;
        }

        private bool StillInside(Vector3 target) {
            RaycastHit[] hits;

            Vector3 dir = target - validationPoint;
            float dist = dir.magnitude;
            //Debug.Log("Validating if the contact has really exited... Raycast with distance: " + dist);

            hits = Physics.RaycastAll(validationPoint, dir.normalized, dist);
            //Debug.DrawRay(validationPoint, dir.normalized * dist, Color.white, 40.0f);
            foreach (RaycastHit hit in hits) {
                //Debug.Log("hit.collider.gameobject.name=" + hit.collider.gameObject.name);
                //Debug.Break();
                if (hit.collider.gameObject == gameObject) {
                    //Debug.Log("The contact seems to still be inside.");
                    return true;
                }
            }

            //Debug.Log("The contact seems to have left");
            return false;
        }
    }
}
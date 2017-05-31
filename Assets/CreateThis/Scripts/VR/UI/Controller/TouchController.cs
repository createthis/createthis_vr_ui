using System.Collections.Generic;
using UnityEngine;
using CreateThis.VR.UI.Interact;

namespace CreateThis.VR.UI.Controller {
    public class TouchController : MonoBehaviour {
        public Material unselected;
        public Material controllerMaterial;
        public GameObject pointerConePrefab;
        public string hardware;
        public float pointerConeZOffset;
        public List<Collider> touching; // public for debugging
        public GameObject grabbedObject; // public for debugging

        private Valve.VR.EVRButtonId touchPadButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
        private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
        private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

        private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
        private SteamVR_TrackedObject trackedObj;
        private GameObject pointerConeInstance;
        private GameObject spawnPoint;

        // Use this for initialization
        void Start() {
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

        public GameObject GetSpawnPoint() {
            return spawnPoint;
        }

        public int GetControllerIndex() {
            return (int)trackedObj.index;
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
            Vector3 defaultCenter = new Vector3(0, 0, pointerConeZOffset);
            
            collider.center = defaultCenter;
        }

        public void UpdatePointerCone(bool first = true) {
            if (!first) return;

            pointerConeInstance.transform.localRotation = Quaternion.Euler(0, 0, 0);
            pointerConeInstance.transform.localPosition = new Vector3(0, 0, pointerConeZOffset);
            
            UpdateSpawnPoint();
        }

        private void OnRenderModelLoaded(SteamVR_RenderModel model, bool connected) {
            Renderer[] renderers = model.gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (Renderer renderer in renderers) {
                renderer.material = controllerMaterial;
                //renderer.enabled = false;
            }
        }

        public void ClearTouching() {
            touching.Clear();
        }

        protected void HandleTriggerDown() {
            foreach (Collider touched in touching) {
                if (touched.GetComponent<Triggerable>()) {
                    touched.GetComponent<Triggerable>().OnTriggerDown(spawnPoint.transform, (int)trackedObj.index);
                }
            }
        }

        protected void HandleTriggerUp() {
            foreach (Collider touched in touching) {
                if (touched.GetComponent<Triggerable>()) {
                    touched.GetComponent<Triggerable>().OnTriggerUp(spawnPoint.transform, (int)trackedObj.index);
                }
            }
        }

        protected void HandleGripDown() {
            foreach (Collider touched in touching) {
                if (touched.GetComponent<Grabbable>()) {
                    touched.GetComponent<Grabbable>().OnGrabStart(spawnPoint.transform, (int)trackedObj.index);
                    grabbedObject = touched.gameObject;
                }
            }
        }

        protected void HandleGripUp() {
            if (!grabbedObject) return;
            if (grabbedObject.GetComponent<Grabbable>()) {
                grabbedObject.GetComponent<Grabbable>().OnGrabStop(spawnPoint.transform, (int)trackedObj.index);
                grabbedObject = null;
            }
        }

        // Update is called once per frame
        void Update() {
            UpdatePointerCone();
            if (controller == null) {
                Debug.Log("[" + trackedObj.index + "] Controller not initialized");
                return;
            }

            if (controller.GetPressDown(triggerButton) && !controller.GetPress(touchPadButton)) {
                HandleTriggerDown();
            }

            if (controller.GetPressUp(triggerButton)) {
                HandleTriggerUp();
            }

            if (controller.GetPressDown(gripButton)) {
                Debug.Log("[" + trackedObj.index + "] Grip down");
                HandleGripDown();
            }

            if (controller.GetPressUp(gripButton)) {
                Debug.Log("[" + trackedObj.index + "] Grip up");
                HandleGripUp();
            }
        }

        private void OnTriggerEnter(Collider collider) {
            Debug.Log("OnTriggerEnter collider.name=" + collider.name);
            if (!touching.Contains(collider)) {
                touching.Add(collider);
            }
            if (collider.GetComponent<Touchable>()) {
                Touchable[] touchables = collider.GetComponents<Touchable>();
                foreach (Touchable touchable in touchables) {
                    touchable.OnTouchStart(spawnPoint.transform, (int)trackedObj.index);
                }
            }
        }

        private void OnTriggerExit(Collider collider) {
            Debug.Log("OnTriggerExit collider.name=" + collider.name);
            if (touching.Contains(collider)) {
                touching.Remove(collider);
            }
            if (collider.GetComponent<Touchable>()) {
                Touchable[] touchables = collider.GetComponents<Touchable>();
                foreach (Touchable touchable in touchables) {
                    touchable.OnTouchStop(spawnPoint.transform, (int)trackedObj.index);
                }
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using CreateThis.Unity;
using CreateThis.VR.UI.Interact;
using CreateThis.VR.UI.UnityEvent;

namespace CreateThis.VR.UI.Controller {
    public class TouchPadMenuController : MonoBehaviour {
        [global::System.Serializable]
        public class TouchPadButton {
            public string label;
            public GameObject displayObject;
            public UseEvent onSelected;
        }

        public Material material;
        public Material highlight;
        public Material outline;
        public Vector3 offset;
        public int numVerts = 46;
        public TouchPadButton[] touchPadButtons;
        public float touchPadButtonSpacingInDegrees;
        public float innerRadius;
        public float outerRadius;
        public float rotationOffset;

        private SteamVR_Controller.Device device { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
        private SteamVR_TrackedObject trackedObj;
        private SteamVR_TrackedController trackedController;
        private List<GameObject> menuPlaneInstances = new List<GameObject>();
        private List<Mesh> meshList = new List<Mesh>();
        private int degreesPerButton;
        private GameObject[] touchPadButtonInstances;
        private int lastSelectedIndex;
        private DonutSliceMesh donutSliceMesh;
        private GameObject menuPlane;

        private void CreateMenuPlane() {
            menuPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            menuPlane.transform.parent = transform;
            menuPlane.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

            Rigidbody rigidbody = menuPlane.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            Selectable selectable = menuPlane.AddComponent<Selectable>();
            selectable.unselectedMaterials = new Material[] { material };
            selectable.highlightMaterial = highlight;
            selectable.outlineMaterial = outline;

            menuPlane.SetActive(false);
        }

        void Start() {
            //Debug.Log("TouchPadController start");
            donutSliceMesh = new DonutSliceMesh();
            CreateMenuPlane();
            donutSliceMesh.numVerts = numVerts;
            trackedObj = GetComponent<SteamVR_TrackedObject>();
            trackedController = GetComponent<SteamVR_TrackedController>();
            trackedController.PadClicked += Controller_PadClicked;
            trackedController.PadUnclicked += Controller_PadUnclicked;

            degreesPerButton = (int)360f / touchPadButtons.Length;

            touchPadButtonInstances = new GameObject[touchPadButtons.Length];

            for (int i = 0; i < touchPadButtons.Length; i++) {
                meshList.Add(donutSliceMesh.Build(innerRadius, outerRadius, degreesPerButton - touchPadButtonSpacingInDegrees));
            }
        }

        private bool SecondClockwiseToFirst(Vector3 v1, Vector2 v2) {
            return -v1.x * v2.y + v1.y * v2.x > 0;
        }

        private int MenuPlaneIndexOfPoint(Vector2 point) {
            // http://stackoverflow.com/questions/13652518/efficiently-find-points-inside-a-circle-sector
            // http://answers.unity3d.com/questions/823090/equivalent-of-degree-to-vector2-in-unity.html
            for (int i = 0; i < touchPadButtons.Length; i++) {
                Vector3 startVector = Quaternion.AngleAxis((degreesPerButton + touchPadButtonSpacingInDegrees) * -i + rotationOffset, Vector3.forward) * Vector3.up;
                Vector3 endVector = Quaternion.AngleAxis((degreesPerButton + touchPadButtonSpacingInDegrees) * -(i + 1) + rotationOffset, Vector3.forward) * Vector3.up;

                if (SecondClockwiseToFirst(startVector, point) && !SecondClockwiseToFirst(endVector, point)) {
                    return i;
                }
            }
            return -1;
        }

        private GameObject DisplayObjectOfIndex(int i) {
            return touchPadButtons[i].displayObject;
        }

        private void Update() {
            if (menuPlaneInstances.Count == 0) return;
            int selectedIndex = MenuPlaneIndexOfPoint(device.GetAxis());
            if (selectedIndex == lastSelectedIndex) return;
            else lastSelectedIndex = selectedIndex;

            for (int i = 0; i < touchPadButtons.Length; i++) {
                GameObject menuPlaneInstance = menuPlaneInstances[i];

                if (menuPlaneInstance.GetComponent<Selectable>()) {
                    if (selectedIndex == i) {
                        menuPlaneInstance.GetComponent<Selectable>().SetSelected(true);
                    } else {
                        menuPlaneInstance.GetComponent<Selectable>().SetSelected(false);
                    }
                }
            }
        }

        private void Controller_PadClicked(object sender, ClickedEventArgs e) {
            //Debug.Log("Touchpad Pressed " + device.GetAxis().x + " " + device.GetAxis().y);
            if (menuPlaneInstances.Count != 0) return;
            int selectedIndex = MenuPlaneIndexOfPoint(device.GetAxis());
            for (int i = 0; i < touchPadButtons.Length; i++) {
                GameObject menuPlaneInstance = Instantiate(menuPlane, this.transform.position, this.transform.rotation);
                menuPlaneInstance.SetActive(true);
                MeshFilter meshFilter = menuPlaneInstance.GetComponent<MeshFilter>();
                meshFilter.mesh = meshList[i];
                menuPlaneInstance.GetComponent<Selectable>().Initialize();

                menuPlaneInstance.transform.parent = this.transform;
                menuPlaneInstance.transform.Rotate(90, 0, degreesPerButton * -i + rotationOffset);
                menuPlaneInstance.transform.localPosition = offset;
                menuPlaneInstances.Add(menuPlaneInstance);

                touchPadButtonInstances[i] = Instantiate(DisplayObjectOfIndex(i), this.transform.position, this.transform.rotation);
                if (touchPadButtonInstances[i].GetComponent<Selectable>()) {
                    touchPadButtonInstances[i].GetComponent<Selectable>().Initialize();
                }
                touchPadButtonInstances[i].transform.Rotate(90, 0, 0);
                touchPadButtonInstances[i].transform.parent = menuPlaneInstance.transform;
                float angle = degreesPerButton / 2 - degreesPerButton - rotationOffset;
                touchPadButtonInstances[i].transform.localPosition = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up * (outerRadius - innerRadius) + new Vector3(0, 0, -0.25f);

                if (menuPlaneInstance.GetComponent<Selectable>()) {
                    if (selectedIndex == i) {
                        menuPlaneInstance.GetComponent<Selectable>().SetSelected(true);
                    } else {
                        menuPlaneInstance.GetComponent<Selectable>().SetSelected(false);
                    }
                }
                lastSelectedIndex = selectedIndex;
            }
        }

        private void Controller_PadUnclicked(object sender, ClickedEventArgs e) {
            //Debug.Log("Touchpad Unpressed " + device.GetAxis().x + " " + device.GetAxis().y);
            if (menuPlaneInstances.Count != 0) {
                int selectedIndex = MenuPlaneIndexOfPoint(device.GetAxis());
                TouchController touchController = GetComponent<TouchController>();
                Transform controller = touchController.GetSpawnPoint().transform;
                int controllerIndex = touchController.GetControllerIndex();
                touchPadButtons[selectedIndex].onSelected.Invoke(controller, controllerIndex);
                foreach (GameObject menuPlaneInstance in menuPlaneInstances) {
                    Destroy(menuPlaneInstance);
                }
                menuPlaneInstances = new List<GameObject>();

                for (int i = 0; i < touchPadButtons.Length; i++) {
                    Destroy(touchPadButtonInstances[i]);
                }
            }
        }
    }
}
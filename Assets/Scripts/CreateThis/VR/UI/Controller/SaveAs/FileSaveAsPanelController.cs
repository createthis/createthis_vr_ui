using UnityEngine;

namespace CreateThis.VR.UI.Controller.SaveAs {
    public class FileSaveAsPanelController : MonoBehaviour {
        public bool visible;
        public Camera sceneCamera;
        public Transform controller;
        public Vector3 offset;
        public float minDistance;
        public GameObject kineticScroller;

        private void Awake() {
            visible = false;
            gameObject.SetActive(false);
        }

        public void SetVisible(bool value) {
            visible = value;
            gameObject.SetActive(visible);
            if (visible) {
                transform.position = PanelUtils.Position(sceneCamera, controller, offset, minDistance);
                transform.rotation = PanelUtils.Rotation(sceneCamera, transform.position);
            }
            kineticScroller.SetActive(visible);
        }

        public void ToggleVisible() {
            SetVisible(!visible);
        }
    }
}
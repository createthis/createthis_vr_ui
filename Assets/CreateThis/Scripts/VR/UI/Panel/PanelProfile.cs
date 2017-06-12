using UnityEngine;

namespace CreateThis.VR.UI {
    public class PanelProfile : MonoBehaviour {
        public Camera sceneCamera;
        public Vector3 offset = new Vector3(0, 0.05f, 0.025f);
        public float minDistance = 0.5f;
        public bool hideOnAwake = true;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}
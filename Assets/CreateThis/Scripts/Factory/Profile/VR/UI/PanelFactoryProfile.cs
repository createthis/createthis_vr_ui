using UnityEngine;

namespace CreateThis.Factory.VR.UI {
    public class PanelFactoryProfile : MonoBehaviour {
        public Camera sceneCamera;
        public Vector3 offset;
        public float minDistance;
        public bool hideOnAwake;

        public PanelFactoryData GetPanelFactoryData() {
            PanelFactoryData data = new PanelFactoryData();
            data.sceneCamera = sceneCamera;
            data.offset = offset;
            data.minDistance = minDistance;
            data.hideOnAwake = hideOnAwake;
            return data;
        }

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}
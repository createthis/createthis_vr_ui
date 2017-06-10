using UnityEngine;
using CreateThis.Factory;
using CreateThis.Factory.VR.UI;

namespace CreateThis.VR.UI {
    public class VRUI_Defaults : MonoBehaviour {
        [Header("Panel Defaults")]
        public Camera sceneCamera;
        public Vector3 offset;
        public float minDistance;
        public bool hideOnAwake;

        void OnValidate() {
            if (FactoryDefaults.panel == null) FactoryDefaults.panel = new PanelFactoryData();
            FactoryDefaults.panel.sceneCamera = sceneCamera;
            FactoryDefaults.panel.offset = offset;
            FactoryDefaults.panel.minDistance = minDistance;
            FactoryDefaults.panel.hideOnAwake = hideOnAwake;
        }

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}
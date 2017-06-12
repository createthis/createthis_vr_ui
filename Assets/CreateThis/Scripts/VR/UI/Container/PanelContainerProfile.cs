using UnityEngine;

namespace CreateThis.VR.UI {
    public class PanelContainerProfile : MonoBehaviour {
        public GameObject panelBody;
        public Material material;
        public Material highlight;
        public Material outline;
        public Color fontColor = Color.white;
        public Vector3 bodyScale = new Vector3(1, 0.025f, 0.025f);
        public float minWidth;
        public float minHeight;
        public float buttonZ = -0.025f;
        public float padding = 0.02f;
        public float spacing = 0.02f;
        public float labelCharacterSize = 0.9f;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}
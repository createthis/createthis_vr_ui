using UnityEngine;

namespace CreateThis.VR.UI {
    public class ButtonProfile : MonoBehaviour {
        public GameObject buttonBody;
        public AudioClip buttonClickDown;
        public AudioClip buttonClickUp;
        public Material material;
        public Material highlight;
        public Material outline;
        public int fontSize = 60;
        public Color fontColor = Color.white;
        public float labelZ = -0.01370001f;
        public Vector3 bodyScale = new Vector3(1, 0.025f, 0.025f);
        public Vector3 labelScale = new Vector3(0.004f, 0.004f, 0.004f);
        public float minWidth = 0.025f;
        public float padding = 0.002f;
        public float characterSize = 0.8f;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}
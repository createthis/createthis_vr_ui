using UnityEngine;

namespace CreateThis.VR.UI {
    public class ButtonProfile : MonoBehaviour {
        public GameObject buttonBody;
        public AudioClip buttonClickDown;
        public AudioClip buttonClickUp;
        public Material material;
        public Material highlight;
        public Material outline;
        public int fontSize;
        public Color fontColor;
        public float labelZ;
        public Vector3 bodyScale;
        public Vector3 labelScale;
        public float minWidth;
        public float padding;
        public float characterSize;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}
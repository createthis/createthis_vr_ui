#if COLOR_PICKER
using UnityEngine;

namespace CreateThis.VR.UI.ColorPicker {
    public class ColorPickerProfile : MonoBehaviour {
        public GameObject thumbBody;
        public Material thumbMaterial;
        public Material colorHueMaterial;
        public Material solidColorMaterial;
        public Material colorSaturationBrightnessMaterial;
        public Vector3 boxColliderSize = new Vector3(0.212f, 0.15f, 0);
        public Vector3 sbThumbLocalPosition = new Vector3(-0.108f, 0.009f, 0f);
        public Vector3 sbThumbScale = new Vector3(0.01f, 0.01f, 0);
        public Vector3 hueThumbLocalPosition = new Vector3(-0.108f, 0.009f, 0f);
        public Vector3 hueThumbScale = new Vector3(0.005f, 0.03f, 0);
        public Vector3 huePickerLocalPosition = new Vector3(-0.0005f, 0.0573f, 0f);
        public Vector3 huePickerScale = new Vector3(0.208f, 0.03f, 0);
        public Vector3 colorIndicatorLocalPosition = new Vector3(0.0545f, -0.0227f, 0f);
        public Vector3 colorIndicatorScale = new Vector3(0.1f, 0.1f, 0);
        public Vector3 colorSBLocalPosition = new Vector3(-0.0545f, -0.0227f, 0f);
        public Vector3 colorSBScale = new Vector3(0.1f, 0.1f, 0);




        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}
#endif
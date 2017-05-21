using UnityEngine;

namespace CreateThis.VR.UI.Controller.Keyboard {
    public class KeyboardLabelController : MonoBehaviour {
        public TextMesh textMesh;
        public KeyboardController keyboardController;
        private string lastBuffer;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            string buffer = keyboardController.GetBuffer();
            if (lastBuffer != buffer) {
                lastBuffer = buffer;
                textMesh.text = buffer;
            }
        }
    }
}

using UnityEngine;

namespace CreateThis.VR.UI {
    public class KeyboardLabel : MonoBehaviour {
        public TextMesh textMesh;
        public Keyboard keyboard;

        void OnEnable() {
            Keyboard.OnBufferChanged += UpdateText;
        }

        void OnDisable() {
            Keyboard.OnBufferChanged -= UpdateText;
        }

        public void UpdateText() {
            string buffer = keyboard.GetBuffer();
            textMesh.text = buffer;
        }

        // Use this for initialization
        void Start() {
        }

        // Update is called once per frame
        void Update() {
        }
    }
}

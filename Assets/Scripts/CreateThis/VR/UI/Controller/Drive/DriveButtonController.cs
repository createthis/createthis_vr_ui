using UnityEngine;
using CreateThis.VR.UI.File;

namespace CreateThis.VR.UI.Controller {
    public class DriveButtonController : MonoBehaviour {
        public FileOpen fileOpen;
        public TextMesh textMeshLabel;

        private string path;

        public void Click() {
            fileOpen.ChangeDirectory(path);
        }

        public void SetPath(string value) {
            path = value;
            textMeshLabel.text = value;
        }

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}
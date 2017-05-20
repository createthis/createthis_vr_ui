using UnityEngine;

namespace CreateThis.VR.UI.Controller {
    public class SaveAsDriveButtonController : MonoBehaviour {
        public FileSaveAsController fileSaveAsController;
        public TextMesh textMeshLabel;

        private string path;

        public void Click() {
            fileSaveAsController.ChangeDirectory(path);
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
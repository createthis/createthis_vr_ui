using UnityEngine;

namespace CreateThis.VR.UI {
    public class VRUI_Defaults : MonoBehaviour {
        public PanelProfile panelProfile;
        public PanelContainerProfile panelContainerProfile;
        public ButtonProfile momentaryButtonProfile;
        public ButtonProfile toggleButtonProfile;
        public FilePanelProfile filePanelProfile;

        void OnValidate() {
            if (panelProfile != null) Defaults.panel = panelProfile;
            if (panelContainerProfile != null) Defaults.panelContainer = panelContainerProfile;
            if (momentaryButtonProfile != null) Defaults.momentaryButton = momentaryButtonProfile;
            if (toggleButtonProfile != null) Defaults.toggleButton = toggleButtonProfile;
            if (filePanelProfile != null) Defaults.filePanel = filePanelProfile;
        }

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}
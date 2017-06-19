using UnityEngine;
#if COLOR_PICKER
using CreateThis.VR.UI.ColorPicker;
#endif

namespace CreateThis.VR.UI {
    public class VRUI_Defaults : MonoBehaviour {
        public PanelProfile panelProfile;
        public PanelContainerProfile panelContainerProfile;
        public ButtonProfile momentaryButtonProfile;
        public ButtonProfile toggleButtonProfile;
        public FilePanelProfile filePanelProfile;
#if COLOR_PICKER
        public ColorPickerProfile colorPickerProfile;
#endif
        private void Initialize() {
            if (panelProfile != null) Defaults.panel = panelProfile;
            if (panelContainerProfile != null) Defaults.panelContainer = panelContainerProfile;
            if (momentaryButtonProfile != null) Defaults.momentaryButton = momentaryButtonProfile;
            if (toggleButtonProfile != null) Defaults.toggleButton = toggleButtonProfile;
            if (filePanelProfile != null) Defaults.filePanel = filePanelProfile;
#if COLOR_PICKER
            if (colorPickerProfile != null) Defaults.colorPicker = colorPickerProfile;
#endif
            Defaults.Changed();
        }

#if UNITY_EDITOR
        void OnValidate() {
            Initialize();
        }
#else
        void Awake() {
            Initialize();
        }
#endif

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}
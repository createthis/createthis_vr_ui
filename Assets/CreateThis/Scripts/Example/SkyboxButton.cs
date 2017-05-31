using UnityEngine;
using CreateThis.VR.UI.Interact;

namespace CreateThis.VR.UI.Button {
    public class SkyboxButton : ToggleButton {
        public string skybox;
        public SkyboxManager skyboxManager;

        protected override void HitTravelLimitHandler(Transform controller, int controllerIndex) {
            // ShiftLock does NOT change the On state when it hits the travel limit. It's On state may only be set externally.
            if (!clickOnTriggerExit) {
                ClickHandler(controller, controllerIndex);
            }
        }

        protected override void ClickHandler(Transform controller, int controllerIndex) {
            skyboxManager.SetSkybox(skybox);
        }

        private void SkyboxChanged() {
            if (skyboxManager.skybox == skybox) {
                GetComponent<Selectable>().SetStickySelected(true);
                if (On != true) On = true;
            } else {
                GetComponent<Selectable>().SetStickySelected(false);
                if (On != false) On = false;
            }
            GetComponent<Selectable>().SetSelected(GetComponent<Selectable>().selected);
        }

        // Use this for initialization
        void Start() {
            Initialize();
            SkyboxManager.OnSkyboxChanged += SkyboxChanged;
            SkyboxChanged();
        }
    }
}

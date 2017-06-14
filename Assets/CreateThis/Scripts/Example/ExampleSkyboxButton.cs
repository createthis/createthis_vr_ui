using UnityEngine;
using CreateThis.VR.UI.Interact;
#if UNITY_EDITOR
using UnityEditor;
#endif
using CreateThis.VR.UI.Button;

namespace CreateThis.Example {
    public class ExampleSkyboxButton : ToggleButton {
        public string skybox;
        public ExampleSkyboxManager skyboxManager;

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
#if UNITY_EDITOR
            if (EditorApplication.isPlaying) {
#endif
                if (skyboxManager.skybox == skybox) {
                    GetComponent<Selectable>().SetStickySelected(true);
                    if (On != true) On = true;
                } else {
                    GetComponent<Selectable>().SetStickySelected(false);
                    if (On != false) On = false;
                }
                GetComponent<Selectable>().SetSelected(GetComponent<Selectable>().selected);
#if UNITY_EDITOR
            }
#endif
        }

        // Use this for initialization
        void Start() {
            Initialize();
            ExampleSkyboxManager.OnSkyboxChanged += SkyboxChanged;
            SkyboxChanged();
        }
    }
}

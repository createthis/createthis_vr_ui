using UnityEngine;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class SkyboxButtonFactory : ToggleButtonFactory {
        public string skybox;
        public SkyboxManager skyboxManager;

        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            SkyboxButton button = SafeAddComponent<SkyboxButton>(target);
            if (audioSourceDown) button.buttonClickDown = audioSourceDown;
            if (audioSourceUp) button.buttonClickUp = audioSourceUp;
            button.buttonBody = buttonBodyInstance;
            button.buttonText = buttonTextLabelInstance;
            button.On = on;
            button.clickOnTriggerExit = true;
            button.skybox = skybox;
            button.skyboxManager = skyboxManager;
            if (panel) button.panel = panel;
        }
    }
}
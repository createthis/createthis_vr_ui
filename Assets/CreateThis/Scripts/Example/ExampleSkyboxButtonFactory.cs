using UnityEngine;
using CreateThis.Unity;
using CreateThis.Factory.VR.UI.Button;

namespace CreateThis.Example {
    public class ExampleSkyboxButtonFactory : ToggleButtonFactory {
        public string skybox;
        public ExampleSkyboxManager skyboxManager;

        public void PopulateButton(ExampleSkyboxButton button, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            base.PopulateButton(button, audioSourceDown, audioSourceUp);
            button.clickOnTriggerExit = true;
            button.skybox = skybox;
            button.skyboxManager = skyboxManager;
        }

        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            ExampleSkyboxButton button = Undoable.AddComponent<ExampleSkyboxButton>(target);
            PopulateButton(button, audioSourceDown, audioSourceUp);
        }
    }
}
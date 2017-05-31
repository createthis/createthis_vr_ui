using System;
using UnityEngine;
using CreateThis.VR.UI.Panel;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class PanelToggleVisibilityMomentaryButtonFactory : MomentaryButtonFactory {
        public PanelBase panelToToggle;

        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            PanelToggleVisibilityMomentaryButton button = SafeAddComponent<PanelToggleVisibilityMomentaryButton>(target);
            if (audioSourceDown) button.buttonClickDown = audioSourceDown;
            if (audioSourceUp) button.buttonClickUp = audioSourceUp;
            button.buttonBody = buttonBodyInstance;
            button.buttonText = buttonTextLabelInstance;
            button.clickOnTriggerExit = true;
            button.panelToToggle = panelToToggle;

            if (panel) button.panel = panel;
        }
    }
}

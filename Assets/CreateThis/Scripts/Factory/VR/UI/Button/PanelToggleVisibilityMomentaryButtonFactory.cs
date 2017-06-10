using UnityEngine;
using CreateThis.VR.UI.Panel;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class PanelToggleVisibilityMomentaryButtonFactory : MomentaryButtonFactory {
        public PanelBase panelToToggle;

        public void PopulateButton(PanelToggleVisibilityMomentaryButton button, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            base.PopulateButton(button, audioSourceDown, audioSourceUp);
            button.clickOnTriggerExit = true;
            button.panelToToggle = panelToToggle;
        }

        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            PanelToggleVisibilityMomentaryButton button = SafeAddComponent<PanelToggleVisibilityMomentaryButton>(target);
            PopulateButton(button, audioSourceDown, audioSourceUp);
        }
    }
}

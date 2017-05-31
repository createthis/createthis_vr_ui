using System;
using UnityEngine;
using CreateThis.VR.UI.Panel;

namespace CreateThis.VR.UI.Button {
    public class PanelToggleVisibilityMomentaryButton : MomentaryButton {
        public PanelBase panelToToggle;

        protected override void ClickHandler(Transform controller, int controllerIndex) {
            onClick.Invoke(controller, controllerIndex);
            panelToToggle.ToggleVisible(controller, controllerIndex);
        }
    }
}

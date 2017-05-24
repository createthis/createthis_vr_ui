using UnityEngine;
using CreateThis.VR.UI.Button;

namespace CreateThis.Scaffold.VR.UI.Button {
    public class ScaffoldToggleButton : ScaffoldButtonBase {
        public bool on;

        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            ToggleButton button = SafeAddComponent<ToggleButton>(target);
            if (audioSourceDown) button.buttonClickDown = audioSourceDown;
            if (audioSourceUp) button.buttonClickUp = audioSourceUp;
            button.buttonBody = buttonBodyInstance;
            button.buttonText = buttonTextLabelInstance;
            button.On = on;
        }
    }
}
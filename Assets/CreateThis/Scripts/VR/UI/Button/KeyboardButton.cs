using System.Collections;
using UnityEngine;

namespace CreateThis.VR.UI.Button {
    public class KeyboardButton : MomentaryButton {
        public Keyboard keyboard;
        public string value;

        protected override void ClickHandler(Transform controller, int controllerIndex) {
            keyboard.PressKey(value);
        }

        public new void Initialize(bool force = false) {
            base.Initialize(force);
            repeat = true;
        }
    }
}

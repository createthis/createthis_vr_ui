using UnityEngine;
using CreateThis.Unity;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class ToggleButtonFactory : ButtonBaseFactory {
        protected override ButtonBehavior buttonBehavior {
            get { return ButtonBehavior.Toggle; }
            set { }
        }
        public bool on;

        public void PopulateButton(ToggleButton button, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            base.PopulateButton(button, audioSourceDown, audioSourceUp);
            button.On = on;
        }

        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            ToggleButton button = Undoable.AddComponent<ToggleButton>(target);
            PopulateButton(button, audioSourceDown, audioSourceUp);
        }
    }
}
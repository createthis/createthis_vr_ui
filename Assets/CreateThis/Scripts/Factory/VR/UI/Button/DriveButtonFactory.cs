using UnityEngine;
using CreateThis.Unity;
using CreateThis.VR.UI.File;
using CreateThis.VR.UI.Button;

namespace CreateThis.Factory.VR.UI.Button {
    public class DriveButtonFactory : MomentaryButtonFactory {
        public FileBase filePanel;

        public void PopulateButton(DriveButton button, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            base.PopulateButton(button, audioSourceDown, audioSourceUp);
            button.filePanel = filePanel;
        }

        protected override void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            DriveButton button = Undoable.AddComponent<DriveButton>(target);
            PopulateButton(button, audioSourceDown, audioSourceUp);
        }
    }
}
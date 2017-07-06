using System.Collections;
using UnityEngine;

namespace CreateThis.VR.UI.Panel {
    public class NotificationPanel : PanelBase {
        public TextMesh notificationLabel;

        private void Opaque() {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            Color c = meshRenderer.materials[0].color;
            c.a = 1.0f;
            meshRenderer.materials[0].color = c;
        }

        private IEnumerator Fade() {
            yield return new WaitForSeconds(2);
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            for (float f = 1f; f >= 0; f -= 0.1f) {
                Color c = meshRenderer.materials[0].color;
                c.a = f;
                meshRenderer.materials[0].color = c;
                yield return null;
            }
            SetVisible(false);
        }

        public void DisplayMessage(string message, Transform controller, int controllerIndex) {
            notificationLabel.text = message;
            SetVisible(true, controller, controllerIndex);
            StartCoroutine(Fade());
        }

        public new void SetVisible(bool value, Transform controller, int controllerIndex) {
            base.SetVisible(value, controller, controllerIndex);
            if (value) Opaque();
        }
    }
}
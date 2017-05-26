using UnityEngine;

namespace CreateThis.VR.UI.Controller {
    public class TranslucentController : MonoBehaviour {
        public Material controllerMaterial;

        // Use this for initialization
        void Start() {
            SteamVR_Events.RenderModelLoaded.Listen(OnRenderModelLoaded);
        }

        private void OnRenderModelLoaded(SteamVR_RenderModel model, bool connected) {
            Renderer[] renderers = model.gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (Renderer renderer in renderers) {
                renderer.material = controllerMaterial;
            }
        }
    }
}
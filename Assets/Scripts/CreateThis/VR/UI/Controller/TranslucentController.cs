using UnityEngine;

namespace CreateThis.VR.UI.Controller {
    public class TranslucentController : MonoBehaviour {
        public Material controllerMaterial;

        private Valve.VR.EVRButtonId touchPadButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
        private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
        private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

        private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
        private SteamVR_TrackedObject trackedObj;

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
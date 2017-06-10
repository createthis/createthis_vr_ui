using UnityEngine;
using CreateThis.Factory;
using CreateThis.Factory.VR.UI;

namespace CreateThis.VR.UI {
    public class VRUI_Defaults : MonoBehaviour {
        public PanelFactoryProfile panelFactoryProfile;

        void OnValidate() {
            if (panelFactoryProfile != null) FactoryDefaults.panel = panelFactoryProfile;
        }

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}
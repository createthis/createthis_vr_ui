using UnityEngine;
using CreateThis.VR.UI.Container;

namespace CreateThis.Factory.VR.UI.Container {
    public class PanelContainerFactory : ContainerBaseFactory {
        public float minWidth;
        public float minHeight;

        protected override void AddContainer(GameObject target) {
            PanelContainer container = SafeAddComponent<PanelContainer>(target);
            target.name = "Panel";
            container.minWidth = minWidth;
            container.minHeight = minHeight;
        }
    }
}

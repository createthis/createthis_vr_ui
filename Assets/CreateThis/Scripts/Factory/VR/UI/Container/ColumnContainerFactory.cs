using UnityEngine;
using CreateThis.VR.UI.Container;

namespace CreateThis.Factory.VR.UI.Container {
    public class ColumnContainerFactory : ContainerBaseFactory {
        protected override void AddContainer(GameObject target) {
            ColumnContainer container = SafeAddComponent<ColumnContainer>(target);
            if (containerName == null) target.name = "Column";
            container.padding = padding;
            container.spacing = spacing;
        }
    }
}

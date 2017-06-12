using UnityEngine;
using CreateThis.Unity;
using CreateThis.VR.UI.Container;

namespace CreateThis.Factory.VR.UI.Container {
    public class RowContainerFactory : ContainerBaseFactory {
        public TextAlignment alignment;

        protected override void AddContainer(GameObject target) {
            RowContainer container = Undoable.AddComponent<RowContainer>(target);
            if (containerName == null) target.name = "Row";
            container.padding = padding;
            container.spacing = spacing;
            container.alignment = alignment;
        }
    }
}
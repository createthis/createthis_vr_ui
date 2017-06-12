using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using CreateThis.Unity;
using CreateThis.VR.UI;
using CreateThis.VR.UI.Container;
using CreateThis.VR.UI.Interact;

namespace CreateThis.Factory.VR.UI.Container {
    public class PanelContainerFactory : ContainerBaseFactory {
        public PanelContainerProfile panelContainerProfile;

        protected override void AddContainer(GameObject target) {
            PanelContainer container = Undoable.AddComponent<PanelContainer>(target);
            PanelContainerProfile profile = Defaults.GetProfile(panelContainerProfile);

            if (containerName == null) target.name = "Panel";
            else target.name = containerName;
            container.minWidth = profile.minWidth;
            container.minHeight = profile.minHeight;
        }

        protected override void CreateContainer() {
            if (containerInstance) return;
            PanelContainerProfile profile = Defaults.GetProfile(panelContainerProfile);
            containerInstance = Instantiate(profile.panelBody);

#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(containerInstance, "Created Container");
#endif
            containerInstance.SetActive(true);
            containerInstance.transform.localScale = profile.bodyScale;
            containerInstance.transform.parent = parent.transform;
            containerInstance.transform.localPosition = Vector3.zero;
            containerInstance.transform.localRotation = Quaternion.identity;
            if (containerName == null) containerInstance.name = "Container";

            BoxCollider boxCollider = containerInstance.GetComponent<BoxCollider>(); // this comes from the cube/prefab the panel is created from
            if (!boxCollider) Debug.Log("no box collider");
            boxCollider.isTrigger = true;

            MeshRenderer meshRenderer = containerInstance.GetComponent<MeshRenderer>();
            meshRenderer.materials = new Material[1] { profile.material };

            Selectable selectable = containerInstance.AddComponent<Selectable>();
            selectable.highlightMaterial = profile.highlight;
            selectable.outlineMaterial = profile.outline;
            selectable.textColor = profile.fontColor;
            selectable.unselectedMaterials = new Material[] { profile.material };

            AddContainer(containerInstance);
        }
    }
}

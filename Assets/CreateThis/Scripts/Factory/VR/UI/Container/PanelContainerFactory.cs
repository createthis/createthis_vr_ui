using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using CreateThis.VR.UI.Container;
using CreateThis.VR.UI.Interact;

namespace CreateThis.Factory.VR.UI.Container {
    public class PanelContainerFactory : ContainerBaseFactory {
        public float minWidth;
        public float minHeight;
        public GameObject panelBody;
        public Material material;
        public Material highlight;
        public Material outline;
        public Color fontColor;
        public Vector3 bodyScale;

        protected override void AddContainer(GameObject target) {
            PanelContainer container = SafeAddComponent<PanelContainer>(target);
            if (containerName == null) target.name = "Panel";
            else target.name = containerName;
            container.minWidth = minWidth;
            container.minHeight = minHeight;
        }

        protected override void CreateContainer() {
            if (containerInstance) return;
            containerInstance = Instantiate(panelBody);

#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(containerInstance, "Created Container");
#endif
            containerInstance.SetActive(true);
            containerInstance.transform.localScale = bodyScale;
            containerInstance.transform.parent = parent.transform;
            containerInstance.transform.localPosition = Vector3.zero;
            containerInstance.transform.localRotation = Quaternion.identity;
            if (containerName == null) containerInstance.name = "Container";

            BoxCollider boxCollider = containerInstance.GetComponent<BoxCollider>(); // this comes from the cube/prefab the panel is created from
            if (!boxCollider) Debug.Log("no box collider");
            boxCollider.isTrigger = true;

            MeshRenderer meshRenderer = containerInstance.GetComponent<MeshRenderer>();
            meshRenderer.materials = new Material[1] { material };

            Selectable selectable = containerInstance.AddComponent<Selectable>();
            selectable.highlightMaterial = highlight;
            selectable.outlineMaterial = outline;
            selectable.textColor = fontColor;
            selectable.unselectedMaterials = new Material[] { material };

            AddContainer(containerInstance);
        }
    }
}

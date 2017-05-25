using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CreateThis.Factory.VR.UI.Container {
    public abstract class ContainerBaseFactory : BaseFactory {
        public float padding = 0.02f;
        public float spacing = 0.01f;
        public GameObject parent;

        protected GameObject containerInstance;

        protected virtual void AddContainer(GameObject target) {
            // override
        }

        protected virtual void CreateContainer() {
            if (containerInstance) return;
            containerInstance = new GameObject();

#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(containerInstance, "Created Container");
#endif
            containerInstance.transform.localScale = Vector3.one;
            containerInstance.transform.parent = parent.transform;
            containerInstance.transform.localPosition = Vector3.zero;
            containerInstance.transform.localRotation = Quaternion.identity;
            containerInstance.name = "Container";

            AddContainer(containerInstance);
        }

        public override GameObject Generate() {
            base.Generate();
            
#if UNITY_EDITOR
            Undo.SetCurrentGroupName("ContainerFactory Generate");
            int group = Undo.GetCurrentGroup();

            Undo.RegisterCompleteObjectUndo(this, "ContainerFactory state");
#endif
            CreateContainer();
#if UNITY_EDITOR
            Undo.CollapseUndoOperations(group);
#endif
            return containerInstance;
        }
    }
}

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CreateThis.Factory.VR.UI.Container {
    public abstract class ContainerBaseFactory : BaseFactory {
        public float padding = 0.02f;
        public float spacing = 0.01f;
        public string containerName;
        public GameObject parent;

        protected GameObject containerInstance;

        protected virtual void AddContainer(GameObject target) {
            // override
        }

        protected virtual void CreateContainer() {
            if (containerInstance) return;
            string childName = containerName != null ? containerName : "Container";
            containerInstance = EmptyChild(parent, childName);

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

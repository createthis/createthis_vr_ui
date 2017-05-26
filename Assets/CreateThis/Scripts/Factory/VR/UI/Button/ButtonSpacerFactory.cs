using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CreateThis.Factory.VR.UI.Button {
    public class ButtonSpacerFactory : BaseFactory {
        public GameObject parent;
        public Vector3 size;

        protected GameObject instance;

        private void Create() {
            if (instance) return;
            instance = EmptyChild(parent, "spacer");
            BoxCollider boxCollider = instance.AddComponent<BoxCollider>();
            boxCollider.size = size;
        }

        public override GameObject Generate() {
            base.Generate();
            
#if UNITY_EDITOR
            Undo.SetCurrentGroupName("ButtonSpacerFactory Generate");
            int group = Undo.GetCurrentGroup();

            Undo.RegisterCompleteObjectUndo(this, "ButtonSpacerFactory state");
#endif
            Create();
#if UNITY_EDITOR
            Undo.CollapseUndoOperations(group);
#endif
            return instance;
        }
    }
}
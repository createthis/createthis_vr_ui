using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if VRTK
using CreateThis.VRTK;
#endif
using CreateThis.VR.UI.Interact;
using CreateThis.VR.UI.Scroller;

namespace CreateThis.Factory.VR.UI.Scroller {
    public class KineticScrollerItemFactory : BaseFactory {
        public GameObject parent;
        public Material material;
        public Material highlight;
        public Material outline;
        public Color fontColor;
        public KineticScroller kineticScroller;

        protected GameObject kineticScrollerItemInstance;

        public override GameObject Generate() {
            base.Generate();

#if UNITY_EDITOR
            Undo.SetCurrentGroupName("FileOpenFactory Generate");
            int group = Undo.GetCurrentGroup();

            Undo.RegisterCompleteObjectUndo(this, "FileOpenFactory state");
#endif
            kineticScrollerItemInstance = EmptyChild(parent, "kineticScrollerItem");

            KineticScrollerItem kineticScrollerItem = SafeAddComponent<KineticScrollerItem>(kineticScrollerItemInstance);
            kineticScrollerItem.kineticScroller = kineticScroller;

            SafeAddComponent<MeshFilter>(kineticScrollerItemInstance);
            SafeAddComponent<MeshRenderer>(kineticScrollerItemInstance);
            BoxCollider boxCollider = SafeAddComponent<BoxCollider>(kineticScrollerItemInstance);
            boxCollider.isTrigger = true;

            Selectable selectable = SafeAddComponent<Selectable>(kineticScrollerItemInstance);
            selectable.highlightMaterial = highlight;
            selectable.outlineMaterial = outline;
            selectable.textColor = fontColor;
            selectable.unselectedMaterials = new Material[] { material };
            selectable.recursive = true;

#if VRTK
            if (useVRTK) {
                CreateThis_VRTK_Interactable interactable = SafeAddComponent<CreateThis_VRTK_Interactable>(kineticScrollerItemInstance);
                CreateThis_VRTK_GrabAttach grabAttach = SafeAddComponent<CreateThis_VRTK_GrabAttach>(kineticScrollerItemInstance);
                interactable.isGrabbable = true;
                interactable.grabAttachMechanicScript = grabAttach;
            }
#endif

            Rigidbody rigidBody = SafeAddComponent<Rigidbody>(kineticScrollerItemInstance);
            rigidBody.useGravity = false;
            rigidBody.isKinematic = true;

#if UNITY_EDITOR
            Undo.CollapseUndoOperations(group);
#endif
            return kineticScrollerItemInstance;
        }
    }
}
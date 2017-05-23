using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using CreateThis.Unity;
using CreateThis.VR.UI.Event;

namespace CreateThis.VR.UI.Container {
    [ExecuteInEditMode]
    public class PanelContainer : MonoBehaviour, IChild3dWidgetResized {
        public float minWidth;
        public float minHeight;
        public Bounds bounds;

        private bool hasInitialized = false;
        private bool dirty = false;

        public void Resize() {
#if UNITY_EDITOR
            Undo.SetCurrentGroupName("PanelContainer Resize");
            int group = Undo.GetCurrentGroup();
#endif

            CalculateBounds();

            float maxWidth = bounds.size.x;
            float maxHeight = bounds.size.y;

#if UNITY_EDITOR
            for (int i = 0; i < gameObject.transform.childCount; i++) {
                Undo.RecordObject(gameObject.transform.GetChild(i), "Resize Child");
            }
#endif

            Transform[] children = DetachReattach.DetachChildren(gameObject);

#if UNITY_EDITOR
            Undo.RecordObject(transform, "Resize Panel");
#endif

            PanelUtils.PanelResizeWidth panelResizeWidth = PanelUtils.ResizeWidth(gameObject, maxWidth, 0);
            PanelUtils.PanelResizeHeight panelResizeHeight = PanelUtils.ResizeHeight(gameObject, maxHeight, 0);
            transform.localScale = new Vector3(panelResizeWidth.xScale, panelResizeHeight.yScale, transform.localScale.z);

            DetachReattach.ReattachChildren(children, gameObject);

            transform.position = transform.position + transform.right * panelResizeWidth.xOffset + transform.up * panelResizeHeight.yOffset;

#if UNITY_EDITOR
            Undo.CollapseUndoOperations(group);
#endif

            dirty = false;
        }

        public void Child3dWidgetResized() {
            Resize();
        }

        protected virtual void OnTransformChildrenChanged() {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying) return;
#endif
            if (!DetachReattach.DetachInProgress(gameObject)) {
                dirty = true;
            }
        }

        public void CalculateBounds() {
            float maxWidth = 0;
            float maxHeight = 0;

            for (int i = 0; i < transform.childCount; i++) {
                Transform child = transform.GetChild(i);
                float height = ObjectBounds.WorldHeight(child.gameObject);
                float width = ObjectBounds.WorldWidth(child.gameObject);

                if (width > maxWidth) maxWidth = width;
                if (height > maxHeight) maxHeight = height;
            }

            bounds.size = new Vector3(maxWidth, maxHeight, 0);
        }

        public void Initialize() {
            if (hasInitialized) return;
            bounds = new Bounds();
            hasInitialized = true;
            Resize();
        }

        // Use this for initialization
        void Start() {
            Initialize();
        }

        // Update is called once per frame
        void Update() {
            if (dirty) {
                Resize();
                dirty = false;
            }
        }
    }
}

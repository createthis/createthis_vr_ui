using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.EventSystems;
using CreateThis.Unity;
using CreateThis.VR.UI.Panel;
using CreateThis.VR.UI.Event;

namespace CreateThis.VR.UI.Container {
    [ExecuteInEditMode]
    public class ColumnContainer : MonoBehaviour, IChild3dWidgetResized {
        [Header("Organizes child objects vertically with spacing between")]
        public float padding = 0.02f;
        public float spacing = 0.01f;
        public Bounds bounds;

        private bool hasInitialized = false;
        private bool dirty = false;

        public float ColumnHeight() {
            float height = padding * 2;
            List<float> heights = new List<float>();

            foreach (Transform child in transform) {
                if (!child.gameObject.activeSelf) continue;
                heights.Add(ObjectBounds.WorldHeight(child.gameObject));
            }

            height += PanelLib.SumWithSpacing(heights, spacing);

            return height;
        }

        public void Child3dWidgetResized() {
            MoveChildren();
        }

        protected virtual void OnTransformChildrenChanged() {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying) return;
#endif
            if (!DetachReattach.DetachInProgress(gameObject)) {
                dirty = true;
            }
        }

        private void BroadcastContainerWidth() {
#if UNITY_EDITOR
            Undo.SetCurrentGroupName("ColumnContainer BroadcastContainerWidth");
            int group = Undo.GetCurrentGroup();
#endif

            for (int i = 0; i < transform.childCount; i++) {
                Transform child = transform.GetChild(i);
                RowContainer rowContainer = child.GetComponent<RowContainer>();
                if (rowContainer) {
#if UNITY_EDITOR
                    Undo.RecordObject(child.transform, "Child SetParentContainerWidth");
#endif
                    rowContainer.SetParentContainerWidth(bounds.size.x);
                }
            }
#if UNITY_EDITOR
            Undo.CollapseUndoOperations(group);
#endif
        }

        public void MoveChildren() {
            float heightSoFar = padding;
            float maxWidth = 0;

            float columnHeight = ColumnHeight();

#if UNITY_EDITOR
            Undo.SetCurrentGroupName("ColumnContainer MoveChildren");
            int group = Undo.GetCurrentGroup();
#endif

            for (int i = 0; i < transform.childCount; i++) {
                Transform child = transform.GetChild(i);
                if (!child.gameObject.activeSelf) continue;

                float height = ObjectBounds.WorldHeight(child.gameObject);
                float width = ObjectBounds.WorldWidth(child.gameObject);

                if (width > maxWidth) maxWidth = width;

                float localY = TransformWithoutRotation.WorldDistanceToLocalDistanceY(-heightSoFar - height / 2 + columnHeight / 2, gameObject);
#if UNITY_EDITOR
                Undo.RecordObject(child.transform, "Move Child");
#endif
                child.localPosition = new Vector3(child.localPosition.x, localY, child.localPosition.z);

                if (i == transform.childCount - 1) {
                    heightSoFar += height + padding;
                } else {
                    heightSoFar += height + spacing;
                }
            }

            bounds.size = new Vector3(maxWidth, columnHeight, 0);
#if UNITY_EDITOR
            Undo.CollapseUndoOperations(group);
#endif
            BroadcastContainerWidth();

            ExecuteEvents.Execute<IChild3dWidgetResized>(transform.parent.gameObject, null, (x, y) => x.Child3dWidgetResized());
        }

        public void Initialize() {
            if (hasInitialized) return;
            bounds = new Bounds();
            hasInitialized = true;
            MoveChildren();
        }

        // Use this for initialization
        void Start() {
            Initialize();
        }

        // Update is called once per frame
        void Update() {
            if (dirty) {
                MoveChildren();
                dirty = false;
            }
        }
    }
}

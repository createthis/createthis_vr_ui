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
    public class RowContainer : MonoBehaviour, IChild3dWidgetResized {
        [Header("Organizes child objects horizontally with spacing between")]
        public float padding = 0.02f;
        public float spacing = 0.01f;
        public TextAlignment alignment;
        public Bounds bounds;
        public bool log;

        private bool hasInitialized = false;
        private bool dirty = false;
        private float parentContainerWidth;

        public float RowWidth() {
            float width = padding * 2;
            List<float> widths = new List<float>();

            foreach (Transform child in transform) {
                if (!child.gameObject.activeSelf) continue;
                widths.Add(PanelLib.GetWorldWidth(child.gameObject));
            }

            width += PanelLib.SumWithSpacing(widths, spacing);

            return width;
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

        private void CalculateOffset() {
            float localRowWidth = PanelLib.WorldDistanceToLocalDistance(RowWidth(), gameObject);
            float xOffset;

            switch (alignment) {
                case TextAlignment.Left:
                    xOffset = localRowWidth / 2 - parentContainerWidth / 2;
                    if (log) Debug.Log("CalculateOffset RowWidth=" + RowWidth() + ",localRowWidth=" + localRowWidth + ",parentContainerWidth=" + parentContainerWidth + ",xOffset=" + xOffset);
                    transform.localPosition = new Vector3(xOffset * transform.localScale.x, transform.localPosition.y, transform.localPosition.z);
                    break;
                case TextAlignment.Center:
                    transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
                    break;
                case TextAlignment.Right:
                    xOffset = -localRowWidth / 2 + parentContainerWidth / 2;
                    if (log) Debug.Log("CalculateOffset RowWidth=" + RowWidth() + ",localRowWidth=" + localRowWidth + ",parentContainerWidth=" + parentContainerWidth + ",xOffset=" + xOffset);
                    transform.localPosition = new Vector3(xOffset * transform.localScale.x, transform.localPosition.y, transform.localPosition.z);
                    break;
            }
        }

        public void SetParentContainerWidth(float value) {
            parentContainerWidth = PanelLib.WorldDistanceToLocalDistance(value, gameObject);
            if (log) Debug.Log("SetParentContainerWidth value=" + value + ",parentContainerWidth=" + parentContainerWidth);
            CalculateOffset();
        }

        public void MoveChildren() {
            float widthSoFar = padding;
            float maxHeight = 0;

            float rowWidth = RowWidth();

#if UNITY_EDITOR
            Undo.SetCurrentGroupName("RowContainer MoveChildren");
            int group = Undo.GetCurrentGroup();
#endif

            for (int i = 0; i < transform.childCount; i++) {
                Transform child = transform.GetChild(i);
                if (!child.gameObject.activeSelf) continue;

                float width = PanelLib.GetWorldWidth(child.gameObject);
                float height = PanelLib.GetWorldHeight(child.gameObject);

                if (height > maxHeight) maxHeight = height;

                float localX = PanelLib.WorldDistanceToLocalDistance(widthSoFar + width / 2 - rowWidth / 2, gameObject);

#if UNITY_EDITOR
                Undo.RecordObject(child.transform, "Move Child");
#endif
                child.localPosition = new Vector3(localX, 0, child.localPosition.z);

                if (i == transform.childCount - 1) {
                    widthSoFar += width + padding;
                } else {
                    widthSoFar += width + spacing;
                }
            }

            bounds.size = new Vector3(rowWidth, maxHeight, 0);

#if UNITY_EDITOR
            Undo.CollapseUndoOperations(group);
#endif

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

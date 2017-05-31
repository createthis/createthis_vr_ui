using UnityEngine;
using CreateThis.VR.UI.Interact;

namespace CreateThis.VR.UI.Panel {
    public interface IPanel {
        // interface members
        void ZeroNotSelectableCount();
        void SetSelectable(bool value);
        void SetVisible(bool value);
        void ToggleVisible(Transform controller, int controllerIndex);
    }

    public abstract class PanelBase : Grabbable, IPanel {
        public bool visible;
        public Camera sceneCamera;
        public Transform controller;
        public Vector3 offset;
        public float minDistance;
        public bool hideOnAwake;
        public Transform grabTarget;

        private int notSelectableCount;
        private BoxCollider boxCollider;
        private Selectable selectable;
        private bool hasInitialized = false;
        private Transform oldParent;

        public override void OnGrabStart(Transform controller, int controllerIndex) {
            oldParent = grabTarget.parent;
            grabTarget.parent = controller;
        }

        public override void OnGrabStop(Transform controller, int controllerIndex) {
            grabTarget.parent = oldParent;
        }

        public void ZeroNotSelectableCount() {
            Initialize();

            notSelectableCount = 0;
            if (boxCollider) boxCollider.enabled = true;
        }

        public void SetSelectable(bool value) {
            Initialize();

            if (value) {
                if (notSelectableCount > 0) notSelectableCount--;
            } else {
                notSelectableCount++;
            }
            if (notSelectableCount == 0) {
                boxCollider.enabled = true;
            } else {
                boxCollider.enabled = false;
                selectable.SetSelected(false);
            }
        }

        private void Awake() {
            if (hideOnAwake) {
                visible = false;
                gameObject.SetActive(false);
            }
        }

        private GameObject GetTarget() {
            return (grabTarget) ? grabTarget.gameObject : gameObject;
        }

        public void SetVisible(bool value) {
            bool oldValue = visible;
            visible = value;

            GameObject target = GetTarget();
            target.SetActive(visible);
            if (visible) {
                if (oldValue != value) ZeroNotSelectableCount();
                Vector3 noYOffset = offset;
                noYOffset.y = 0;
                Vector3 positionWithoutYOffset = PanelUtils.Position(sceneCamera, controller, noYOffset, minDistance);
                target.transform.position = PanelUtils.Position(sceneCamera, controller, offset, minDistance);
                target.transform.rotation = PanelUtils.Rotation(sceneCamera, positionWithoutYOffset);
            }
        }

        public void ToggleVisible(Transform controller, int controllerIndex) {
            this.controller = controller;
            SetVisible(!visible);
        }

        public void Initialize() {
            if (hasInitialized) return;
            boxCollider = GetComponent<BoxCollider>();
            selectable = GetComponent<Selectable>();
            notSelectableCount = 0;
            hasInitialized = true;
        }

        // Use this for initialization
        void Start() {
            Initialize();
        }

        // Update is called once per frame
        void Update() {

        }
    }
}
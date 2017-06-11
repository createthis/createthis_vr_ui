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
        public Transform controller;
        public Transform grabTarget;
        public PanelProfile panelProfile;

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
            PanelManager.AddPanel(this);
            if (panelProfile.hideOnAwake) {
                visible = false;
                GameObject target = GetTarget();
                target.SetActive(false);
            }
        }

        private GameObject GetTarget() {
            return (grabTarget) ? grabTarget.gameObject : gameObject;
        }

        public void SetVisible(bool value) {
            bool oldValue = visible;
            visible = value;

            if (value) PanelManager.HideAllPanels(this);

            GameObject target = GetTarget();
            target.SetActive(visible);
            if (visible) {
                if (oldValue != value) ZeroNotSelectableCount();
                Vector3 noYOffset = panelProfile.offset;
                noYOffset.y = 0;
                Vector3 positionWithoutYOffset = PanelUtils.Position(panelProfile.sceneCamera, controller, noYOffset, panelProfile.minDistance);
                target.transform.position = PanelUtils.Position(panelProfile.sceneCamera, controller, panelProfile.offset, panelProfile.minDistance);
                target.transform.rotation = PanelUtils.Rotation(panelProfile.sceneCamera, positionWithoutYOffset);
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
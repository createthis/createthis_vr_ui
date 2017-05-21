using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreateThis.VR.UI.Panel;

namespace CreateThis.VR.UI.Controller.Keyboard {
    public class KeyboardPanelController : MonoBehaviour {
        public bool visible;
        public Camera sceneCamera;
        public Transform controller;
        public Vector3 offset;
        public PanelController panelController;
        public float minDistance;
        private Action<string> callback;

        private void Awake() {
            visible = false;
            gameObject.SetActive(false);
        }

        public void SetCallback(Action<string> callback) {
            this.callback = callback;
        }

        public void SetBuffer(string filename) {
            GetComponent<KeyboardController>().SetBuffer(filename);
        }

        public void Done() {
            string filename = GetComponent<KeyboardController>().GetBuffer();
            callback(filename);
        }

        public void SetVisible(bool value) {
            bool oldValue = visible;
            visible = value;
            gameObject.SetActive(visible);
            if (visible) {
                panelController.gameObject.transform.localPosition = Vector3.zero;
                if (oldValue != value) panelController.ZeroNotSelectableCount();
                Vector3 noYOffset = offset;
                noYOffset.y = 0;
                Vector3 positionWithoutYOffset = PanelUtils.Position(sceneCamera, controller, noYOffset, minDistance);
                transform.position = PanelUtils.Position(sceneCamera, controller, offset, minDistance);
                transform.rotation = PanelUtils.Rotation(sceneCamera, positionWithoutYOffset);
            }
        }

        public void ToggleVisible() {
            SetVisible(!visible);
        }
    }
}
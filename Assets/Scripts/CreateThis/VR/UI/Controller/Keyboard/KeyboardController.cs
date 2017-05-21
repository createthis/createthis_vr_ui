using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreateThis.VR.UI.Panel;

namespace CreateThis.VR.UI.Controller.Keyboard {
    public class KeyboardController : MonoBehaviour {
        public PanelController tabletLowerCaseController;
        public PanelController tabletUpperCaseController;
        public PanelController tabletNumberController;
        public PanelController tabletSymbolController;

        private string buffer;
        private bool shiftLock = false;
        private bool numLock = false;

        private void BufferChanged() {
            //bufferTextMesh.text = buffer;
        }

        public void PressKey(string value) {
            buffer += value;
            BufferChanged();
        }

        public void Space() {
            buffer += " ";
            BufferChanged();
        }

        public void ShiftLock(bool value) {
            shiftLock = value;
            // FIXME: Display different tablet
            if (shiftLock) {
                tabletLowerCaseController.gameObject.SetActive(false);
                tabletNumberController.gameObject.SetActive(false);
                tabletSymbolController.gameObject.SetActive(false);
                tabletUpperCaseController.gameObject.SetActive(true);
            } else {
                tabletLowerCaseController.gameObject.SetActive(true);
                tabletUpperCaseController.gameObject.SetActive(false);
                tabletNumberController.gameObject.SetActive(false);
                tabletSymbolController.gameObject.SetActive(false);
            }
            BufferChanged();
        }

        public void NumLock(bool value) {
            numLock = value;
            if (numLock) {
                tabletLowerCaseController.gameObject.SetActive(false);
                tabletNumberController.gameObject.SetActive(true);
                tabletSymbolController.gameObject.SetActive(false);
                tabletUpperCaseController.gameObject.SetActive(false);
            } else {
                // undefined
            }
            BufferChanged();
        }

        public void Symbol() {
            tabletLowerCaseController.gameObject.SetActive(false);
            tabletNumberController.gameObject.SetActive(false);
            tabletSymbolController.gameObject.SetActive(true);
            tabletUpperCaseController.gameObject.SetActive(false);
            BufferChanged();
        }

        public void Return() {
            buffer += "\n";
            BufferChanged();
        }

        public void Backspace() {
            if (buffer.Length == 0) return;
            buffer = buffer.Substring(0, buffer.Length - 1);
            BufferChanged();
        }

        public string GetBuffer() {
            return buffer;
        }

        public void SetBuffer(string value) {
            buffer = value;
            BufferChanged();
        }

        // Use this for initialization
        void Start() {
            shiftLock = false;
            numLock = false;

            tabletLowerCaseController.gameObject.SetActive(true);
            tabletLowerCaseController.gameObject.transform.localPosition = Vector3.zero;
            tabletUpperCaseController.gameObject.SetActive(false);
            tabletUpperCaseController.gameObject.transform.localPosition = Vector3.zero;
            tabletNumberController.gameObject.SetActive(false);
            tabletNumberController.gameObject.transform.localPosition = Vector3.zero;
            tabletSymbolController.gameObject.SetActive(false);
            tabletSymbolController.gameObject.transform.localPosition = Vector3.zero;
        }

        // Update is called once per frame
        void Update() {

        }
    }
}
using System;
using UnityEngine;
using CreateThis.VR.UI.Panel;

namespace CreateThis.VR.UI {
    public class Keyboard : MonoBehaviour {
        public StandardPanel panelLowerCase;
        public StandardPanel panelUpperCase;
        public StandardPanel panelNumber;
        public StandardPanel panelSymbol;
        public delegate void BufferChangedAction();
        public static event BufferChangedAction OnBufferChanged;
        public Action<string> doneCallback;


        private string buffer;
        private bool shiftLock = false;
        private bool numLock = false;

        private void BufferChanged() {
            if (OnBufferChanged != null)
                OnBufferChanged();
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
            if (shiftLock) {
                panelLowerCase.gameObject.SetActive(false);
                panelNumber.gameObject.SetActive(false);
                panelSymbol.gameObject.SetActive(false);
                panelUpperCase.gameObject.SetActive(true);
            } else {
                panelLowerCase.gameObject.SetActive(true);
                panelUpperCase.gameObject.SetActive(false);
                panelNumber.gameObject.SetActive(false);
                panelSymbol.gameObject.SetActive(false);
            }
            BufferChanged();
        }

        public void NumLock(bool value) {
            numLock = value;
            if (numLock) {
                panelLowerCase.gameObject.SetActive(false);
                panelNumber.gameObject.SetActive(true);
                panelSymbol.gameObject.SetActive(false);
                panelUpperCase.gameObject.SetActive(false);
            } else {
                // undefined
            }
            BufferChanged();
        }

        public void Symbol() {
            panelLowerCase.gameObject.SetActive(false);
            panelNumber.gameObject.SetActive(false);
            panelSymbol.gameObject.SetActive(true);
            panelUpperCase.gameObject.SetActive(false);
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

        public void Done() {
            doneCallback(buffer);
        }

        // Use this for initialization
        void Start() {
            shiftLock = false;
            numLock = false;

            panelLowerCase.gameObject.SetActive(true);
            panelLowerCase.gameObject.transform.localPosition = Vector3.zero;
            panelUpperCase.gameObject.SetActive(false);
            panelUpperCase.gameObject.transform.localPosition = Vector3.zero;
            panelNumber.gameObject.SetActive(false);
            panelNumber.gameObject.transform.localPosition = Vector3.zero;
            panelSymbol.gameObject.SetActive(false);
            panelSymbol.gameObject.transform.localPosition = Vector3.zero;
        }

        // Update is called once per frame
        void Update() {

        }
    }
}
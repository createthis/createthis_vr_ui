#if COLOR_PICKER
using UnityEngine;

namespace CreateThis.VR.UI.ColorPicker {
    public abstract class ColorPickerIOBase : MonoBehaviour {
        public Color color; // public for debugging and to set initial color at startup.

        protected virtual void OnColorChange(HSBColor color) {
            this.color = color.ToColor();
        }

        protected virtual void SetColorPickerColor(Color color) {
            HSBColor hsbColor = HSBColor.FromColor(color);
            transform.BroadcastMessage("SetColor", hsbColor, SendMessageOptions.DontRequireReceiver);
        }

        protected virtual void Start() {
            SetColorPickerColor(this.color);
        }
    }
}
#endif
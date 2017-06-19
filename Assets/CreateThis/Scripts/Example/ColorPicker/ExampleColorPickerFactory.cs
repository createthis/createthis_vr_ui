#if COLOR_PICKER
using UnityEngine;
using CreateThis.Unity;
using CreateThis.Factory.VR.UI.ColorPicker;

namespace CreateThis.Example.ColorPicker {
    public class ExampleColorPickerFactory : ColorPickerFactory {
        protected override void CreateColorPickerIO(GameObject parent) {
            ExampleColorPickerIO colorPickerIO = Undoable.AddComponent<ExampleColorPickerIO>(parent);
            colorPickerIO.color = Color.blue;
        }
    }
}
#endif
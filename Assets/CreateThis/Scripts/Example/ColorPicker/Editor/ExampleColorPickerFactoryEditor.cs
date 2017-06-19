#if COLOR_PICKER
using UnityEditor;
using CreateThis.Factory.VR.UI.ColorPicker;

namespace CreateThis.Example.ColorPicker {
    [CustomEditor(typeof(ExampleColorPickerFactory))]
    [CanEditMultipleObjects]

    public class ExampleColorPickerFactoryEditor : ColorPickerFactoryEditor {
    }
}
#endif
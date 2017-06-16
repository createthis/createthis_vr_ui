#if COLOR_PICKER
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using CreateThis.Unity;
using CreateThis.VR.UI;

namespace CreateThis.Factory.VR.UI {
    public class ColorPickerFactory : BaseFactory {
        public GameObject parent;
        public ButtonProfile buttonProfile;

        private GameObject colorPickerInstance;

        private GameObject CreateColorSaturationBrightnessThumb(GameObject parent) {
            GameObject thumb = EmptyChild(parent, "ColorSaturationBrightnessThumb");
            MeshFilter meshFilter = Undoable.AddComponent<MeshFilter>(thumb);
            ButtonProfile profile = Defaults.GetMomentaryButtonProfile(buttonProfile);
            meshFilter.mesh = profile.buttonBody.GetComponent<MeshFilter>().mesh;
            thumb.transform.localPosition = new Vector3(-0.108f, 0.009f, -1.74f);
            thumb.transform.localScale = new Vector3(0.03703703f, 0.01355932f, 0);
            MeshRenderer meshRenderer = Undoable.AddComponent<MeshRenderer>(thumb);

            return thumb;
        }

        private GameObject CreateColorPicker(GameObject parent) {
            GameObject colorPicker = EmptyChild(parent, "ColorPicker");
            Undoable.AddComponent<BoxCollider>(colorPicker);
            return colorPicker;
        }

        public override GameObject Generate() {
            base.Generate();
            
#if UNITY_EDITOR
            Undo.SetCurrentGroupName("ColorPickerFactory Generate");
            int group = Undo.GetCurrentGroup();

            Undo.RegisterCompleteObjectUndo(this, "ColorPickerFactory state");
#endif
            colorPickerInstance = CreateColorPicker(parent);
            CreateColorSaturationBrightnessThumb(colorPickerInstance);
#if UNITY_EDITOR
            Undo.CollapseUndoOperations(group);
#endif
            return colorPickerInstance;
        }
    }
}
#endif
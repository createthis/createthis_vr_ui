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
        public ColorPickerProfile colorPickerProfile;

        private GameObject colorPickerInstance;

        private GameObject CreateColorSaturationBrightnessThumb(GameObject parent) {
            GameObject thumb = EmptyChild(parent, "ColorSaturationBrightnessThumb");
            MeshFilter meshFilter = Undoable.AddComponent<MeshFilter>(thumb);
            ColorPickerProfile profile = Defaults.GetProfile(colorPickerProfile);
            meshFilter.mesh = profile.thumbBody.GetComponent<MeshFilter>().mesh;
            thumb.transform.localPosition = profile.thumbLocalPosition;
            thumb.transform.localScale = profile.thumbScale;
            MeshRenderer meshRenderer = Undoable.AddComponent<MeshRenderer>(thumb);
            meshRenderer.materials = new Material[] { profile.thumbMaterial };

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
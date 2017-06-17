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
        private GameObject colorHuePickerThumbInstance;
        private GameObject colorPickerContainerInstance;
        private GameObject colorHuePickerInstance;

        private GameObject CreateColorSaturationBrightnessThumb(GameObject parent) {
            GameObject thumb = EmptyChild(parent, "ColorSaturationBrightnessThumb");
            MeshFilter meshFilter = Undoable.AddComponent<MeshFilter>(thumb);
            ColorPickerProfile profile = Defaults.GetProfile(colorPickerProfile);
            meshFilter.mesh = profile.thumbBody.GetComponent<MeshFilter>().mesh;
            thumb.transform.localPosition = profile.sbThumbLocalPosition;
            thumb.transform.localScale = profile.sbThumbScale;
            MeshRenderer meshRenderer = Undoable.AddComponent<MeshRenderer>(thumb);
            meshRenderer.materials = new Material[] { profile.thumbMaterial };

            return thumb;
        }

        private GameObject CreateColorHuePickerThumb(GameObject parent) {
            GameObject thumb = EmptyChild(parent, "ColorHuePickerThumb");
            MeshFilter meshFilter = Undoable.AddComponent<MeshFilter>(thumb);
            ColorPickerProfile profile = Defaults.GetProfile(colorPickerProfile);
            meshFilter.mesh = profile.thumbBody.GetComponent<MeshFilter>().mesh;
            thumb.transform.localPosition = profile.hueThumbLocalPosition;
            thumb.transform.localScale = profile.hueThumbScale;
            MeshRenderer meshRenderer = Undoable.AddComponent<MeshRenderer>(thumb);
            meshRenderer.materials = new Material[] { profile.thumbMaterial };

            return thumb;
        }

        private GameObject CreateColorPickerContainer(GameObject parent) {
            GameObject container = EmptyChild(parent, "ColorPickerContainer");
            
            return container;
        }

        private GameObject CreateColorHuePicker(GameObject parent) {
            GameObject container = EmptyChild(parent, "ColorHuePicker");
            // FIXME: This needs to be Touchable
            MeshFilter meshFilter = Undoable.AddComponent<MeshFilter>(container);
            ColorPickerProfile profile = Defaults.GetProfile(colorPickerProfile);
            meshFilter.mesh = profile.thumbBody.GetComponent<MeshFilter>().mesh;
            container.transform.localPosition = profile.huePickerLocalPosition;
            container.transform.localScale = profile.huePickerScale;
            MeshRenderer meshRenderer = Undoable.AddComponent<MeshRenderer>(container);
            meshRenderer.materials = new Material[] { profile.colorHueMaterial };
            return container;
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
            colorHuePickerThumbInstance = CreateColorHuePickerThumb(colorPickerInstance);
            colorPickerContainerInstance = CreateColorPickerContainer(colorPickerInstance);
            colorHuePickerInstance = CreateColorHuePicker(colorPickerContainerInstance);
#if UNITY_EDITOR
            Undo.CollapseUndoOperations(group);
#endif
            return colorPickerInstance;
        }
    }
}
#endif
#if COLOR_PICKER
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using CreateThis.Unity;
using CreateThis.VR.UI;
#if COLOR_PICKER
using CreateThis.VR.UI.ColorPicker;
#endif

namespace CreateThis.Factory.VR.UI {
    public class ColorPickerFactory : BaseFactory {
        public GameObject parent;
        public ColorPickerProfile colorPickerProfile;

        private GameObject colorPickerInstance;
        private GameObject sbThumbInstance;
        private GameObject hueThumbInstance;
        private GameObject colorPickerContainerInstance;
        private GameObject colorSaturationBrightnessPickerInstance;

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
            ColorPickerProfile profile = Defaults.GetProfile(colorPickerProfile);
            container.transform.localPosition = profile.huePickerLocalPosition;
            container.transform.localScale = profile.huePickerScale;

            Undoable.AddComponent<ColorHuePicker>(container);

            ColorPickerThumbTouchable thumbTouchable = Undoable.AddComponent<ColorPickerThumbTouchable>(container);
            thumbTouchable.fixY = true;
            thumbTouchable.thumb = hueThumbInstance.transform;

            BoxCollider boxCollider = Undoable.AddComponent<BoxCollider>(container);
            boxCollider.isTrigger = true;

            // FIXME: This needs to be Touchable

            MeshFilter meshFilter = Undoable.AddComponent<MeshFilter>(container);
            meshFilter.mesh = profile.thumbBody.GetComponent<MeshFilter>().mesh;

            MeshRenderer meshRenderer = Undoable.AddComponent<MeshRenderer>(container);
            meshRenderer.materials = new Material[] { profile.colorHueMaterial };
            return container;
        }

        private GameObject CreateColorIndicator(GameObject parent) {
            GameObject container = EmptyChild(parent, "ColorIndicator");
            ColorPickerProfile profile = Defaults.GetProfile(colorPickerProfile);
            container.transform.localPosition = profile.colorIndicatorLocalPosition;
            container.transform.localScale = profile.colorIndicatorScale;

            Undoable.AddComponent<ColorIndicator>(container);

            MeshFilter meshFilter = Undoable.AddComponent<MeshFilter>(container);
            meshFilter.mesh = profile.thumbBody.GetComponent<MeshFilter>().mesh;
            
            MeshRenderer meshRenderer = Undoable.AddComponent<MeshRenderer>(container);
            meshRenderer.materials = new Material[] { profile.solidColorMaterial };
            return container;
        }

        private GameObject CreateColorSaturationBrightnessPicker(GameObject parent) {
            GameObject container = EmptyChild(parent, "ColorSaturationBrightnessPicker");
            ColorPickerProfile profile = Defaults.GetProfile(colorPickerProfile);
            container.transform.localPosition = profile.colorSBLocalPosition;
            container.transform.localScale = profile.colorSBScale;

            var script = Undoable.AddComponent<ColorSaturationBrightnessPicker>(container);
            script.backgroundMaterial = profile.colorSaturationBrightnessMaterial;

            ColorPickerThumbTouchable thumbTouchable = Undoable.AddComponent<ColorPickerThumbTouchable>(container);
            thumbTouchable.fixY = false;
            thumbTouchable.thumb = sbThumbInstance.transform;

            BoxCollider boxCollider = Undoable.AddComponent<BoxCollider>(container);
            boxCollider.isTrigger = true;

            // FIXME: This needs to be Touchable

            MeshFilter meshFilter = Undoable.AddComponent<MeshFilter>(container);
            meshFilter.mesh = profile.thumbBody.GetComponent<MeshFilter>().mesh;
            
            MeshRenderer meshRenderer = Undoable.AddComponent<MeshRenderer>(container);
            meshRenderer.materials = new Material[] { profile.colorSaturationBrightnessMaterial };
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
            sbThumbInstance = CreateColorSaturationBrightnessThumb(colorPickerInstance);
            hueThumbInstance = CreateColorHuePickerThumb(colorPickerInstance);
            colorPickerContainerInstance = CreateColorPickerContainer(colorPickerInstance);
            CreateColorHuePicker(colorPickerContainerInstance);
            CreateColorIndicator(colorPickerContainerInstance);
            CreateColorSaturationBrightnessPicker(colorPickerContainerInstance);
#if UNITY_EDITOR
            Undo.CollapseUndoOperations(group);
#endif
            return colorPickerInstance;
        }
    }
}
#endif
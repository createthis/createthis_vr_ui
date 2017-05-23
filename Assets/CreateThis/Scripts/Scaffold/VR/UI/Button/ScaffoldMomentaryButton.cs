using UnityEngine;
using CreateThis.VR.UI.Button;
using CreateThis.VR.UI.Interact;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CreateThis.Scaffold.VR.UI.Button {
    public class ScaffoldMomentaryButton : ScaffoldBase {
        public GameObject target;
        public string buttonText;
        public GameObject buttonBody;
        public Material material;
        public Material highlight;
        public Material outline;
        public AudioClip buttonClickDown;
        public AudioClip buttonClickUp;
        public TextAlignment alignment;
        public int fontSize;
        public Color fontColor;

        private GameObject buttonBodyInstance;
        private GameObject buttonTextLabelInstance;

        private T SafeAddComponent<T>(GameObject target) where T : Component {
#if UNITY_EDITOR
            return Undo.AddComponent<T>(target);
#else
            return target.AddComponent<T>();
#endif
        }

        private AudioSource AddAudioSource(GameObject target, AudioClip audioClip) {
            if (!audioClip) return null;
            AudioSource audioSource = SafeAddComponent<AudioSource>(target);
            audioSource.clip = buttonClickDown;
            audioSource.spatialBlend = 1f;
            return audioSource;
        }

        private void PopulateTarget() {
            if (!buttonBodyInstance) return;

#if UNITY_EDITOR
            Undo.RegisterCompleteObjectUndo(target, "Change name before");
#endif

            target.name = "Button_" + buttonText;

            AudioSource audioSourceDown = AddAudioSource(target, buttonClickDown);
            AudioSource audioSourceUp = AddAudioSource(target, buttonClickUp);

            MomentaryButton button = SafeAddComponent<MomentaryButton>(target);
            if (audioSourceDown) button.buttonClickDown = audioSourceDown;
            if (audioSourceUp) button.buttonClickUp = audioSourceUp ;
            button.buttonBody = buttonBodyInstance;
            button.buttonText = buttonTextLabelInstance;

            GrowButtonByTextMesh growButton = SafeAddComponent<GrowButtonByTextMesh>(target);
            growButton.buttonBody = buttonBodyInstance;
            growButton.textMesh = buttonTextLabelInstance.GetComponent<TextMesh>();
            growButton.alignment = alignment;

            SafeAddComponent<BoxCollider>(target);
            SafeAddComponent<Rigidbody>(target);

            Selectable selectable = SafeAddComponent<Selectable>(target);
            selectable.highlightMaterial = highlight;
            selectable.outlineMaterial = outline;
            selectable.textColor = fontColor;
            selectable.unselectedMaterials = new Material[] { material };
            selectable.recursive = true;
        }

        private void CreateButtonBody() {
            if (buttonBodyInstance) return;
            buttonBodyInstance = Instantiate(buttonBody);

#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(buttonBodyInstance, "Created ButtonBody");
#endif

            buttonBodyInstance.transform.parent = target.transform;
            buttonBodyInstance.name = "ButtonBody";

            Selectable selectable = buttonBodyInstance.AddComponent<Selectable>();
            selectable.highlightMaterial = highlight;
            selectable.outlineMaterial = outline;
            selectable.textColor = fontColor;
            selectable.unselectedMaterials = new Material[] { material };

            buttonBodyInstance.AddComponent<BoxCollider>();
        }

        private void CreateTextLabel() {
            if (buttonTextLabelInstance) return;
            buttonTextLabelInstance = new GameObject();

#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(buttonTextLabelInstance, "Created ButtonTextLabel");
#endif

            buttonTextLabelInstance.transform.parent = target.transform;
            buttonTextLabelInstance.name = "ButtonTextLabel";

            TextMesh textMesh = buttonTextLabelInstance.AddComponent<TextMesh>();
            textMesh.text = buttonText;
            textMesh.fontSize = fontSize;
            textMesh.color = fontColor;
        }

        public override void Generate() {
            base.Generate();
            
#if UNITY_EDITOR
            Undo.SetCurrentGroupName("ScaffoldMomentaryButton Generate");
            int group = Undo.GetCurrentGroup();

            Undo.RegisterCompleteObjectUndo(this, "ScaffoldMomentaryButton state");
#endif
            CreateButtonBody();
            CreateTextLabel();
            PopulateTarget();
#if UNITY_EDITOR
            Undo.CollapseUndoOperations(group);
#endif
        }
    }
}
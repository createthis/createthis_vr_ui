using UnityEngine;
#if VRTK
using CreateThis.VRTK;
#endif
using CreateThis.Unity;
using CreateThis.VR.UI;
using CreateThis.VR.UI.Button;
using CreateThis.VR.UI.Panel;
using CreateThis.VR.UI.Interact;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CreateThis.Factory.VR.UI.Button {
    public enum ButtonBehavior {
        Momentary,
        Toggle,
    }

    public abstract class ButtonBaseFactory : BaseFactory {
        public GameObject parent;
        public string buttonText;
        public ButtonProfile buttonProfile;
        public TextAlignment alignment;
        public PanelBase panel;

        protected abstract ButtonBehavior buttonBehavior { get; set; }
        protected GameObject buttonInstance;
        protected GameObject buttonBodyInstance;
        protected GameObject buttonTextLabelInstance;

        private AudioSource AddAudioSource(GameObject target, AudioClip audioClip) {
            if (!audioClip) return null;
            AudioSource audioSource = Undoable.AddComponent<AudioSource>(target);
            audioSource.clip = audioClip;
            audioSource.spatialBlend = 1f;
            audioSource.playOnAwake = false;
            return audioSource;
        }

        public virtual void PopulateButton(ButtonBase button, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            if (audioSourceDown) button.buttonClickDown = audioSourceDown;
            if (audioSourceUp) button.buttonClickUp = audioSourceUp;
            button.buttonBody = buttonBodyInstance;
            button.buttonText = buttonTextLabelInstance;
            if (panel) button.panel = panel;
        }

        protected virtual void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            ButtonBase button = Undoable.AddComponent<ButtonBase>(target);
            PopulateButton(button, audioSourceDown, audioSourceUp);
        }

        private ButtonProfile GetButtonProfile() {
            ButtonProfile profile = Defaults.GetMomentaryButtonProfile(buttonProfile);
            if (buttonBehavior == ButtonBehavior.Toggle) {
                profile = Defaults.GetToggleButtonProfile(buttonProfile);
            }
            return profile;
        }

        private void BuildButton(ButtonProfile profile) {
            if (!buttonBodyInstance) return;

#if UNITY_EDITOR
            Undo.RegisterCompleteObjectUndo(buttonInstance, "Change name before");
#endif
            if (buttonBehavior == ButtonBehavior.Momentary) {
            }

            buttonInstance.name = "Button_" + buttonText;

            AudioSource audioSourceDown = AddAudioSource(buttonInstance, profile.buttonClickDown);
            AudioSource audioSourceUp = AddAudioSource(buttonInstance, profile.buttonClickUp);

            BoxCollider boxCollider = Undoable.AddComponent<BoxCollider>(buttonInstance);
            boxCollider.size = profile.bodyScale;
            boxCollider.isTrigger = true;

            AddButton(buttonInstance, audioSourceDown, audioSourceUp);

            GrowButtonByTextMesh growButton = Undoable.AddComponent<GrowButtonByTextMesh>(buttonInstance);
            growButton.buttonBody = buttonBodyInstance;
            growButton.textMesh = buttonTextLabelInstance.GetComponent<TextMesh>();
            growButton.alignment = alignment;
            growButton.minWidth = profile.minWidth;
            growButton.padding = profile.padding;

            Rigidbody rigidBody = Undoable.AddComponent<Rigidbody>(buttonInstance);
            rigidBody.isKinematic = true;

#if VRTK
            Undoable.AddComponent<CreateThis_VRTK_Interactable>(buttonInstance);
#endif

            Selectable selectable = Undoable.AddComponent<Selectable>(buttonInstance);
            selectable.highlightMaterial = profile.highlight;
            selectable.outlineMaterial = profile.outline;
            selectable.textColor = profile.fontColor;
            selectable.unselectedMaterials = new Material[] { profile.material };
            selectable.recursive = true;

            growButton.Resize();
        }

        private void CreateButton() {
            if (buttonInstance) return;
            buttonInstance = EmptyChild(parent, "Button");
        }

        private void CreateButtonBody(ButtonProfile profile) {
            if (buttonBodyInstance) return;
            buttonBodyInstance = Instantiate(profile.buttonBody);

#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(buttonBodyInstance, "Created ButtonBody");
#endif
            buttonBodyInstance.SetActive(true);
            buttonBodyInstance.transform.localScale = profile.bodyScale;
            buttonBodyInstance.transform.parent = buttonInstance.transform;
            buttonBodyInstance.transform.localPosition = Vector3.zero;
            buttonBodyInstance.transform.localRotation = Quaternion.identity;
            buttonBodyInstance.name = "ButtonBody";

            MeshRenderer meshRenderer = buttonBodyInstance.GetComponent<MeshRenderer>();
            meshRenderer.materials = new Material[1] { profile.material };

            Selectable selectable = buttonBodyInstance.AddComponent<Selectable>();
            selectable.highlightMaterial = profile.highlight;
            selectable.outlineMaterial = profile.outline;
            selectable.textColor = profile.fontColor;
            selectable.unselectedMaterials = new Material[] { profile.material };

            if (!buttonBodyInstance.GetComponent<BoxCollider>()) {
                buttonBodyInstance.AddComponent<BoxCollider>();
            }
        }

        private void CreateTextLabel(ButtonProfile profile) {
            if (buttonTextLabelInstance) return;
            buttonTextLabelInstance = EmptyChild(buttonInstance, "ButtonTextLabel", profile.labelScale);
            buttonTextLabelInstance.transform.localPosition = new Vector3(0, 0, profile.labelZ);

            TextMesh textMesh = buttonTextLabelInstance.AddComponent<TextMesh>();
            textMesh.text = buttonText;
            textMesh.fontSize = profile.fontSize;
            textMesh.color = profile.fontColor;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Left;
            textMesh.characterSize = profile.characterSize;
        }

        public override GameObject Generate() {
            base.Generate();
            
#if UNITY_EDITOR
            Undo.SetCurrentGroupName("ButtonFactory Generate");
            int group = Undo.GetCurrentGroup();

            Undo.RegisterCompleteObjectUndo(this, "ButtonFactory state");
#endif
            ButtonProfile profile = GetButtonProfile();
            CreateButton();
            CreateButtonBody(profile);
            CreateTextLabel(profile);
            BuildButton(profile);
#if UNITY_EDITOR
            Undo.CollapseUndoOperations(group);
#endif
            return buttonInstance;
        }
    }
}
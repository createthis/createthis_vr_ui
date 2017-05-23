using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CreateThis.VR.UI.Container {
    [ExecuteInEditMode]
    public class UpdateBoxColliderFromTextMesh : MonoBehaviour {
        public BoxCollider boxCollider;
        public TextMesh textMesh;
        public bool log;

        private string lastText;
        private bool resettingCollider;
        private RowContainer rowContainer;
        private bool hasInitialized;

        public void OnEnable() {
            resettingCollider = false;
        }

        public IEnumerator ResetCollider() {
            resettingCollider = true;
#if UNITY_EDITOR
            if (EditorApplication.isPlaying) {
#endif
                boxCollider = gameObject.GetComponent<BoxCollider>();
                BoxCollider newBoxCollider = gameObject.AddComponent<BoxCollider>();
                Destroy(boxCollider);
                yield return 0;
                boxCollider = newBoxCollider;
                rowContainer.MoveChildren();
#if UNITY_EDITOR
            } else {
                boxCollider = gameObject.GetComponent<BoxCollider>();
                BoxCollider newBoxCollider = gameObject.AddComponent<BoxCollider>();
                DestroyImmediate(boxCollider);
                boxCollider = newBoxCollider;
                rowContainer.MoveChildren();
            }
#endif
            resettingCollider = false;
        }

        public void Initialize() {
            if (hasInitialized) return;
            rowContainer = transform.parent.gameObject.GetComponent<RowContainer>();
            hasInitialized = true;
        }

        // Use this for initialization
        void Start() {
            Initialize();
        }

        // Update is called once per frame
        void Update() {
            Initialize();
            if (gameObject.activeSelf && !resettingCollider && lastText != textMesh.text) {
                StartCoroutine(ResetCollider());
                lastText = textMesh.text;
            }
        }
    }
}
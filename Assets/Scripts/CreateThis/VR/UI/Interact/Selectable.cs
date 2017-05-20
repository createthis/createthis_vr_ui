using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreateThis.Unity;

namespace CreateThis.VR.UI.Interact {
    public class Selectable : MonoBehaviour {
        public Material highlightMaterial;
        public Material outlineMaterial;
        public Mesh outlineMesh;
        public Color textColor;
        public Material unselectedMaterial;  // Backward compatibility - do not use for future work.
        public Material[] unselectedMaterials;
        public Material stickySelectedMaterial;
        private Material[] blendedStickySelectedMaterials;
        public bool stickySelected;

        public bool selected;
        public bool recursive;

        public GameObject outlineGameObject;

        public bool renderMesh;
        public bool renderWireframe;
        public bool renderNormals;
        public int textureCacheId;

        private Material[] materials;
        private Material selectedMaterial;
        private Material[] selectedMaterials;
        private MeshRenderer meshRenderer;
        private LineRenderer lineRenderer;
        private TextMesh textMesh;
        private bool hasInitialized;

        private void UpdateMaterialMeshRenderer(GameObject target, Selectable selectable, bool selected) {
            if (!selectable.meshRenderer) return;

            if (selectable.materials == null) {
                selectable.materials = selectable.meshRenderer.materials;
            }
            if (selected) {
                //Debug.Log("UpdateMaterialMeshRenderer Selected");
                selectable.materials = selectedMaterials;
            } else {
                if (stickySelected) {
                    selectable.materials = blendedStickySelectedMaterials;
                } else {
                    //Debug.Log("UpdateMaterialMeshRenderer Unselected");
                    selectable.materials = unselectedMaterials;
                }
            }
            if (selectable.textMesh) {
                //Debug.Log("selectableController.materials.Length="+ selectableController.materials.Length);
                //Debug.Log("unselectedMaterials.Length=" + unselectedMaterials.Length);
                if (textColor != Color.clear) {
                    selectable.textMesh.color = textColor; // FIXME: this is crap
                } else {
                    selectable.textMesh.color = selectable.materials[0].color;
                }
            } else {
                selectable.meshRenderer.materials = selectable.materials;
            }
        }

        private void UpdateMaterialMeshRendererNoSelectableController(GameObject target, bool selected) {
            if (!target.GetComponent<MeshRenderer>()) return;
            Material[] mats = target.GetComponent<MeshRenderer>().materials;
            if (selected) {
                //Debug.Log("UpdateMaterialMeshRendererNoSelectableController Selected");
                mats = selectedMaterials;
            } else {
                //Debug.Log("UpdateMaterialMeshRendererNoSelectableController Unselected");
                mats = unselectedMaterials;
            }
            if (target.GetComponent<TextMesh>()) {
                if (textColor != Color.clear) {
                    target.GetComponent<TextMesh>().color = textColor;
                } else {
                    target.GetComponent<TextMesh>().color = mats[0].color;
                }
            } else {
                target.GetComponent<MeshRenderer>().materials = mats;
            }
        }

        private void UpdateMaterialLineRenderer(GameObject target, Selectable selectable, bool selected) {
            if (!selectable.lineRenderer) return;

            Material[] mats = selectable.lineRenderer.materials;
            if (selected) {
                //Debug.Log("UpdateMaterialLineRenderer Selected");
                mats = selectedMaterials;
            } else {
                //Debug.Log("UpdateMaterialLineRenderer Unselected");
                mats = unselectedMaterials;
            }
            selectable.lineRenderer.materials = mats;
        }

        private void UpdateMaterialLineRendererNoSelectableController(GameObject target, bool selected) {
            if (!target.GetComponent<LineRenderer>()) return;

            Material[] mats = target.GetComponent<LineRenderer>().materials;
            if (selected) {
                //Debug.Log("UpdateMaterialLineRendererNoSelectableController Selected");
                mats = selectedMaterials;
            } else {
                //Debug.Log("UpdateMaterialLineRendererNoSelectableController Unselected");
                mats = unselectedMaterials;
            }
            target.GetComponent<LineRenderer>().materials = mats;
        }

        private void UpdateMaterialNoSelectableController(GameObject target, bool selected) {
            UpdateMaterialMeshRendererNoSelectableController(target, selected);
        }

        private void UpdateMaterial(GameObject target, Selectable selectable, bool selected) {
            UpdateMaterialMeshRenderer(target, selectable, selected);
            UpdateMaterialLineRenderer(target, selectable, selected);
        }

        public void UpdateOutlineGameObjectSelected() {
            if (!outlineGameObject) return;
            outlineGameObject.GetComponent<MeshRenderer>().enabled = selected;
        }


        public Material[] GetMaterials() {
            if (stickySelected) {
                return blendedStickySelectedMaterials;
            } else {
                return unselectedMaterials;
            }
        }

        public void SyncMaterials() {
            UpdateStickySelectedMaterials();
        }

        public void UpdateStickySelectedMaterials() {
            blendedStickySelectedMaterials = new Material[unselectedMaterials.Length];
            for (int i = 0; i < unselectedMaterials.Length; i++) {
                if (stickySelectedMaterial) {
                    Color color = unselectedMaterials[i].color * stickySelectedMaterial.color;
                    color.a = 1.0f;
                    blendedStickySelectedMaterials[i] = MaterialCache.MaterialByColor(color, renderMesh, renderWireframe, renderNormals, textureCacheId);
                } else {
                    Color color = unselectedMaterials[i].color * Color.red;
                    color.a = 1.0f;
                    blendedStickySelectedMaterials[i] = MaterialCache.MaterialByColor(color, renderMesh, renderWireframe, renderNormals, textureCacheId);
                }
            }
        }

        public void SetStickySelected(bool value) {
            if (!hasInitialized) Initialize();
            stickySelected = value;
            SyncMaterials();
            SetSelected(selected);

            if (recursive) {
                foreach (Transform child in transform) {
                    Selectable childSelectable = child.GetComponent<Selectable>();
                    if (childSelectable && child.gameObject.activeSelf) {
                        childSelectable.SetStickySelected(stickySelected);
                    } else {
                        UpdateMaterialNoSelectableController(child.gameObject, selected);
                    }
                }
            }
        }

        public void SetSelected(bool value) {
            selected = value;

            UpdateMaterial(gameObject, this, selected);
            UpdateOutlineGameObjectSelected();

            if (recursive) {
                foreach (Transform child in transform) {
                    Selectable childSelectable = child.GetComponent<Selectable>();
                    if (childSelectable && child.gameObject.activeSelf) {
                        UpdateMaterial(child.gameObject, childSelectable, selected);
                        childSelectable.SetSelected(selected);
                    } else {
                        UpdateMaterialNoSelectableController(child.gameObject, selected);
                    }
                }
            }
        }

        public void UpdateSelectedMaterials() {
            if (outlineMesh) SyncOutlineGameObject();
            UpdateOutlineGameObjectSelected();
            int materialLength = outlineGameObject ? unselectedMaterials.Length : unselectedMaterials.Length * 2;
            if (selectedMaterials == null || materialLength != selectedMaterials.Length) { // avoid GC alloc on subsequent calls
                selectedMaterials = new Material[materialLength];
            }

            for (int i = 0; i < unselectedMaterials.Length; i++) {
                Color color;
                if (unselectedMaterials[i].HasProperty("_Color")) {
                    color = unselectedMaterials[i].color + highlightMaterial.GetColor("_TintColor") / 3;
                } else {
                    color = unselectedMaterials[i].GetColor("_TintColor") + highlightMaterial.GetColor("_TintColor") / 3;
                }
                color.a = 1.0f;
                Material material = MaterialCache.MaterialByColor(color, renderMesh, renderWireframe, renderNormals, textureCacheId);

                selectedMaterials[i] = material;
                if (!outlineGameObject) {
                    selectedMaterials[i + unselectedMaterials.Length] = outlineMaterial;
                }
            }
        }

        public void SyncOutlineGameObject() {
            MeshFilter meshFilter;
            if (outlineGameObject) {
                meshFilter = outlineGameObject.GetComponent<MeshFilter>();
                meshFilter.mesh = outlineMesh;
                return;
            }
            outlineGameObject = new GameObject();
            outlineGameObject.name = "Outline";
            outlineGameObject.transform.parent = transform;
            outlineGameObject.transform.localPosition = Vector3.zero;
            outlineGameObject.transform.localRotation = Quaternion.identity;
            outlineGameObject.transform.localScale = Vector3.one;
            MeshRenderer meshRenderer = outlineGameObject.AddComponent<MeshRenderer>();
            meshFilter = outlineGameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = outlineMesh;
            meshRenderer.materials = new Material[1] { outlineMaterial };
        }

        // Use this for initialization
        public void Initialize() {
            selected = false;
            stickySelected = false;
            if (!hasInitialized) { // avoid GC alloc on subsequent calls
                if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
                if (textMesh == null) textMesh = GetComponent<TextMesh>();
                if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
                if (meshRenderer) materials = meshRenderer.materials;
                renderMesh = true;
                renderWireframe = false;
                renderNormals = false;
                textureCacheId = -1;
            }

            hasInitialized = true;
            if (unselectedMaterial != null) { // Backward compatibility - do not use for future work.
                unselectedMaterials = new Material[] { unselectedMaterial };
            }
            UpdateSelectedMaterials();
        }

        void Start() {
            if (!hasInitialized) Initialize();
        }
    }
}

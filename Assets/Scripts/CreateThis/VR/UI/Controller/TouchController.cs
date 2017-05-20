using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour {
    public SettingsController settingsController;
    public MeshController meshController;
    public Material unselected;
    public Material controllerMaterial;
    public GameObject pointerCone;
    public GameObject snapCrosshairPrefab;
    public GameObject alignmentXPrefab;
    public GameObject alignmentYPrefab;
    public GameObject alignmentZPrefab;
    public GameObject alignment3dPrefab;
    public GameObject primitivePlanePrefab;
    public GameObject primitiveBoxPrefab;
    public GameObject primitiveCirclePrefab;
    public GameObject primitiveCylinderPrefab;
    public GameObject primitiveSpherePrefab;
    public GameObject boxSelectionPrefab;
    public string hardware;

    private Valve.VR.EVRButtonId touchPadButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;
    public GameObject pickup;
    public bool dragging;
    public bool triggerDown;
    public bool deferredTriggerExit;
    public bool noTriggerDownNext;
    public bool airGrab;
    private string lastMode;
    private GameObject snapCrosshairInstance;
    private GameObject alignmentXInstance;
    private GameObject alignmentYInstance;
    private GameObject alignmentZInstance;
    private GameObject alignment3dInstance;
    private GameObject primitivePlaneInstance;
    private GameObject primitiveBoxInstance;
    private GameObject primitiveCircleInstance;
    private GameObject primitiveCylinderInstance;
    private GameObject primitiveSphereInstance;
    private GameObject boxSelectionInstance;
    private List<string> snappableModes = new List<string>(new string[] { "vertex", "primitive_plane", "primitive_box", "primitive_circle", "primitive_cylinder", "primitive_sphere" });
    private bool lastScannerMode;
    private GameObject pointerConeInstance;
    private GameObject spawnPoint;
    private List<string> modesIgnoringTouchableControllerForVertices;
    private List<string> primitiveModes = new List<string>(new string[] {
        "primitive_plane",
        "primitive_box",
        "primitive_circle",
        "primitive_cylinder",
        "primitive_sphere",
    });
    private Vector3 validationPoint;
    private Color lastFillColor;


    // Use this for initialization
    void Start() {
        snapCrosshairInstance = null;
        alignmentXInstance = null;
        alignmentYInstance = null;
        alignmentZInstance = null;
        alignment3dInstance = null;
        primitiveSphereInstance = null;
        primitiveCylinderInstance = null;
        primitiveCircleInstance = null;
        primitivePlaneInstance = null;
        primitiveBoxInstance = null;
        boxSelectionInstance = null;
        deferredTriggerExit = false;
        airGrab = false;
        lastMode = meshController.modeManager.mode;
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        Debug.Log("TouchController[" + trackedObj.index + "] start");
        DetectVRHardware();
        spawnPoint = new GameObject();
        spawnPoint.name = "SpawnPoint";
        spawnPoint.transform.parent = this.transform;
        pointerConeInstance = Instantiate(pointerCone, this.transform.position, this.transform.rotation);
        pointerConeInstance.transform.parent = this.transform;
        UpdatePointerCone(true);
        modesIgnoringTouchableControllerForVertices = new List<string>(new string[] {
            "primitive_circle",
            "primitive_cylinder",
            "primitive_sphere",
            "primitive_plane",
            "primitive_box",
            "box_select",
            "select_triangles"
            });

        SteamVR_Events.RenderModelLoaded.Listen(OnRenderModelLoaded);
    }

    private void UpdatePointerConeFromFillColor() {
        if (settingsController.fillColor == lastFillColor) return;
        lastFillColor = settingsController.fillColor;
        Renderer renderer = pointerConeInstance.GetComponent<Renderer>();
        Material[] materials = renderer.materials;
        Color pointerConeColor = settingsController.fillColor;
        pointerConeColor.a = materials[0].color.a;
        materials[0].color = pointerConeColor;
        renderer.materials = materials;
    }

    public void DetectVRHardware() {
        string model = UnityEngine.VR.VRDevice.model != null ? UnityEngine.VR.VRDevice.model : "";
        if (model.IndexOf("Rift") >= 0) {
            hardware = "oculus_touch";
        } else {
            hardware = "htc_vive";
        }
        Debug.Log("hardware=" + hardware);
    }

    public Quaternion SpawnLocalRotation() {
        if (settingsController.tracingMode) {
            return Quaternion.Euler(90, 0, 0) * pointerConeInstance.transform.localRotation;
        } else {
            return Quaternion.identity;
        }
    }

    public Vector3 SpawnLocalPosition() {
        if (settingsController.tracingMode) {
            return new Vector3(-0.005f, 0, -0.030f) + pointerConeInstance.transform.localPosition;
        } else {
            return Vector3.zero;
        }
    }

    public bool PrimitiveInProgress() {
        bool value = primitivePlaneInstance || primitiveBoxInstance || primitiveCircleInstance || primitiveCylinderInstance || primitiveSphereInstance;
        //Debug.Log("PrimitiveInProgress " + value);
        return value;
    }

    public void UpdateSpawnPoint() {
        spawnPoint.transform.localRotation = SpawnLocalRotation();
        spawnPoint.transform.localPosition = SpawnLocalPosition();

        BoxCollider collider = GetComponent<BoxCollider>();
        Vector3 defaultCenter = new Vector3(0, 0, -0.02f);
        if (settingsController.tracingMode) {
            collider.center = defaultCenter + new Vector3(-0.001f,0,0.035f) + spawnPoint.transform.localPosition;
        } else {
            collider.center = defaultCenter;
        }
    }

    public void UpdatePointerCone(bool first = true) {
        if (!first && lastScannerMode == settingsController.tracingMode) return;
        lastScannerMode = settingsController.tracingMode;

        if (settingsController.tracingMode || settingsController.customPointerLocation) {
            if (!settingsController.customPointerLocation) {
                if (hardware == "htc_vive") {
                    pointerConeInstance.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                    pointerConeInstance.transform.localPosition = new Vector3(0.006f, -0.013f, -0.144f);
                } else {
                    pointerConeInstance.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                    pointerConeInstance.transform.localPosition = new Vector3(0.011f, -0.007f, -0.109f);
                }
            } else {
                Vector3 position = settingsController.customPointerPosition;
                Vector3 rotation = settingsController.customPointerRotation;
                pointerConeInstance.transform.localRotation = Quaternion.Euler(rotation);
                pointerConeInstance.transform.localPosition = position;
            }
        } else {
            pointerConeInstance.transform.localRotation = Quaternion.Euler(90, 0, 0);
            pointerConeInstance.transform.localPosition = new Vector3(0.007f, 0, -0.035f);
        }
        UpdateSpawnPoint();
    }

    private void OnRenderModelLoaded(SteamVR_RenderModel model, bool connected) {
        Renderer[] renderers = model.gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (Renderer renderer in renderers) {
            renderer.material = controllerMaterial;
            //renderer.enabled = false;
        }
    }

    void TriggerDownModeVertex() {
        Vector3 position = (settingsController.SnapEnabled() && snapCrosshairInstance != null) ? meshController.snapManager.SnappedWorldPosition(spawnPoint.transform.position) : spawnPoint.transform.position;
        meshController.verticesManager.CreateAndAddVertexInstanceByWorldPosition(position);
    }

    void TriggerDownModeCirclePrimitive() {
        if (primitiveCircleInstance == null) {
            primitiveCircleInstance = Instantiate(primitiveCirclePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            primitiveCircleInstance.transform.parent = meshController.gameObject.transform;
            primitiveCircleInstance.GetComponent<PrimitiveCircleController>().meshController = meshController;
            TriggerPlacementController triggerPlacementController = primitiveCircleInstance.GetComponent<TriggerPlacementController>();
            triggerPlacementController.onFirstTrigger.Invoke(spawnPoint.transform, (int)trackedObj.index);
        } else {
            TriggerPlacementController triggerPlacementController = primitiveCircleInstance.GetComponent<TriggerPlacementController>();
            triggerPlacementController.onSecondTrigger.Invoke(spawnPoint.transform, (int)trackedObj.index);
            Destroy(primitiveCircleInstance);
        }
    }

    void TriggerDownModeCylinderPrimitive() {
        if (primitiveCylinderInstance == null) {
            primitiveCylinderInstance = Instantiate(primitiveCylinderPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            primitiveCylinderInstance.transform.parent = meshController.gameObject.transform;
            primitiveCylinderInstance.GetComponent<PrimitiveCylinderController>().meshController = meshController;
            TriggerPlacementController triggerPlacementController = primitiveCylinderInstance.GetComponent<TriggerPlacementController>();
            triggerPlacementController.onFirstTrigger.Invoke(spawnPoint.transform, (int)trackedObj.index);
        } else {
            TriggerPlacementController triggerPlacementController = primitiveCylinderInstance.GetComponent<TriggerPlacementController>();
            triggerPlacementController.onSecondTrigger.Invoke(spawnPoint.transform, (int)trackedObj.index);
        }
    }

    void TriggerDownModeSpherePrimitive() {
        if (primitiveSphereInstance == null) {
            primitiveSphereInstance = Instantiate(primitiveSpherePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            primitiveSphereInstance.transform.parent = meshController.gameObject.transform;
            primitiveSphereInstance.GetComponent<PrimitiveSphereController>().meshController = meshController;
            TriggerPlacementController triggerPlacementController = primitiveSphereInstance.GetComponent<TriggerPlacementController>();
            triggerPlacementController.onFirstTrigger.Invoke(spawnPoint.transform, (int)trackedObj.index);
        } else {
            TriggerPlacementController triggerPlacementController = primitiveSphereInstance.GetComponent<TriggerPlacementController>();
            triggerPlacementController.onSecondTrigger.Invoke(spawnPoint.transform, (int)trackedObj.index);
            Destroy(primitiveSphereInstance);
        }
    }

    void TriggerDownModePlanePrimitive() {
        if (primitivePlaneInstance == null) {
            primitivePlaneInstance = Instantiate(primitivePlanePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            primitivePlaneInstance.transform.parent = meshController.gameObject.transform;
            primitivePlaneInstance.GetComponent<PrimitivePlaneController>().meshController = meshController;
            TriggerPlacementController triggerPlacementController = primitivePlaneInstance.GetComponent<TriggerPlacementController>();
            triggerPlacementController.onFirstTrigger.Invoke(spawnPoint.transform, (int)trackedObj.index);
        } else {
            TriggerPlacementController triggerPlacementController = primitivePlaneInstance.GetComponent<TriggerPlacementController>();
            triggerPlacementController.onSecondTrigger.Invoke(spawnPoint.transform, (int)trackedObj.index);
            Destroy(primitivePlaneInstance);
        }
    }

    void TriggerDownModeBoxPrimitive() {
        if (primitiveBoxInstance == null) {
            primitiveBoxInstance = Instantiate(primitiveBoxPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            primitiveBoxInstance.transform.parent = meshController.gameObject.transform;
            primitiveBoxInstance.GetComponent<PrimitiveBoxController>().meshController = meshController;
            TriggerPlacementController triggerPlacementController = primitiveBoxInstance.GetComponent<TriggerPlacementController>();
            triggerPlacementController.onFirstTrigger.Invoke(spawnPoint.transform, (int)trackedObj.index);
        } else {
            TriggerPlacementController triggerPlacementController = primitiveBoxInstance.GetComponent<TriggerPlacementController>();
            triggerPlacementController.onSecondTrigger.Invoke(spawnPoint.transform, (int)trackedObj.index);
        }
    }

    void TriggerDownModeBoxSelect() {
        if (boxSelectionInstance == null) {
            boxSelectionInstance = Instantiate(boxSelectionPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            boxSelectionInstance.GetComponent<BoxSelectionController>().realMeshController = meshController;
            boxSelectionInstance.GetComponent<BoxSelectionController>().CreateMesh();
            TriggerPlacementController triggerPlacementController = boxSelectionInstance.GetComponent<TriggerPlacementController>();
            triggerPlacementController.onFirstTrigger.Invoke(spawnPoint.transform, (int)trackedObj.index);
        } else {
            TriggerPlacementController triggerPlacementController = boxSelectionInstance.GetComponent<TriggerPlacementController>();
            triggerPlacementController.onSecondTrigger.Invoke(spawnPoint.transform, (int)trackedObj.index);
        }
    }

    void TriggerDownModeAlignment(ref GameObject alignmentPrefab) {
        GameObject alignmentInstance = Instantiate(alignmentPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        alignmentInstance.transform.parent = meshController.gameObject.transform;
        Vector3 position = settingsController.SnapEnabled() ? meshController.snapManager.SnappedWorldPosition(spawnPoint.transform.position) : spawnPoint.transform.position;
        alignmentInstance.transform.localPosition = alignmentInstance.transform.parent.transform.InverseTransformPoint(position);
        alignmentInstance.transform.rotation = alignmentInstance.transform.parent.transform.rotation;
        meshController.alignmentToolsManager.alignmentTools.Add(alignmentInstance);
        alignmentInstance.GetComponent<AlignmentToolController>().meshController = meshController;
    }

    private void MoveAlignmentTool(ref GameObject alignmentToolInstance) {
        if (alignmentToolInstance != null) {
            if (settingsController.SnapEnabled()) {
                alignmentToolInstance.transform.position = meshController.snapManager.SnappedWorldPosition(spawnPoint.transform.position);
            } else {
                alignmentToolInstance.transform.position = spawnPoint.transform.position;
            }
            alignmentToolInstance.transform.rotation = meshController.transform.rotation;
        }
    }

    void LateUpdate() {
        if (settingsController.SnapEnabled() && snapCrosshairInstance != null) {
            snapCrosshairInstance.transform.position = meshController.snapManager.SnappedWorldPosition(spawnPoint.transform.position);
            snapCrosshairInstance.transform.rotation = meshController.transform.rotation;
        }

        MoveAlignmentTool(ref alignmentXInstance);
        MoveAlignmentTool(ref alignmentYInstance);
        MoveAlignmentTool(ref alignmentZInstance);
        MoveAlignmentTool(ref alignment3dInstance);
    }

    private void CreateOrDestroySnapCrosshair() {
        if (settingsController.SnapEnabled() && snapCrosshairInstance == null && snappableModes.Contains(meshController.modeManager.mode)) {
            snapCrosshairInstance = Instantiate(snapCrosshairPrefab, spawnPoint.transform.position, Quaternion.identity);
        }

        if (snapCrosshairInstance != null && !settingsController.SnapEnabled() || !snappableModes.Contains(meshController.modeManager.mode)) {
            Destroy(snapCrosshairInstance);
        }
    }

    private void CreateOrDestroyAlignmentCrosshair(GameObject alignmentPrefab, ref GameObject alignmentCrosshairInstance, string mode) {
        if (alignmentCrosshairInstance == null && meshController.modeManager.mode == mode) {
            alignmentCrosshairInstance = Instantiate(alignmentPrefab, spawnPoint.transform.position, Quaternion.identity);
            BoxCollider[] boxColliders = alignmentCrosshairInstance.GetComponents<BoxCollider>();
            foreach (BoxCollider boxCollider in boxColliders) {
                boxCollider.enabled = false;
            }
        }

        if (alignmentCrosshairInstance != null && meshController.modeManager.mode != mode) {
            Destroy(alignmentCrosshairInstance);
        }
    }

    public void ClearPickup() { // null pickup when user clicks a button on the menu
        pickup = null;
        noTriggerDownNext = true;
    }

    // Update is called once per frame
    void Update() {
        UpdatePointerCone();
        UpdatePointerConeFromFillColor();
        if (controller == null) {
            Debug.Log("[" + trackedObj.index + "] Controller not initialized");
            return;
        }
        BoxCollider collider = GetComponent<BoxCollider>();
        validationPoint = transform.TransformPoint(collider.center) + transform.up * 10.0f; // some point outside the mesh

        CreateOrDestroySnapCrosshair();
        CreateOrDestroyAlignmentCrosshair(alignmentXPrefab, ref alignmentXInstance, "alignment_x");
        CreateOrDestroyAlignmentCrosshair(alignmentYPrefab, ref alignmentYInstance, "alignment_y");
        CreateOrDestroyAlignmentCrosshair(alignmentZPrefab, ref alignmentZInstance, "alignment_z");
        CreateOrDestroyAlignmentCrosshair(alignment3dPrefab, ref alignment3dInstance, "alignment_3d");

        // Deselect on mode change
        if (lastMode != meshController.modeManager.mode && pickup != null) {
            if (pickup.GetComponent<TouchableController>()) {
                pickup.GetComponent<TouchableController>().onUnSelected.Invoke();
            }
            if (dragging) {
                if (pickup.GetComponent<GrabbableController>()) {
                    pickup.GetComponent<GrabbableController>().onGrabEnd.Invoke(spawnPoint.transform, (int)trackedObj.index);
                }
            }
            pickup = null;
            dragging = false;
            triggerDown = false;
        }

        // Handle Trigger Down
        if (!dragging && controller.GetPressDown(triggerButton) && !controller.GetPress(touchPadButton)) {
            //Debug.Log("[" + trackedObj.index + "] trigger pressed at " + this.transform.position.ToString());
            if (pickup != null && pickup.GetComponent<TouchableController>() && !(modesIgnoringTouchableControllerForVertices.Contains(meshController.modeManager.mode) && pickup.tag == "Vertex")) {
                pickup.GetComponent<TouchableController>().onTriggerDown.Invoke(spawnPoint.transform, (int)trackedObj.index);
                if (noTriggerDownNext) {
                    noTriggerDownNext = false;
                } else {
                    triggerDown = true;
                }
            } else {
                switch (meshController.modeManager.mode) {
                    case "vertex":
                        TriggerDownModeVertex();
                        break;
                    case "primitive_plane":
                        TriggerDownModePlanePrimitive();
                        break;
                    case "primitive_box":
                        TriggerDownModeBoxPrimitive();
                        break;
                    case "primitive_circle":
                        TriggerDownModeCirclePrimitive();
                        break;
                    case "primitive_cylinder":
                        TriggerDownModeCylinderPrimitive();
                        break;
                    case "primitive_sphere":
                        TriggerDownModeSpherePrimitive();
                        break;
                    case "box_select":
                        TriggerDownModeBoxSelect();
                        break;
                    case "alignment_x":
                        TriggerDownModeAlignment(ref alignmentXPrefab);
                        break;
                    case "alignment_y":
                        TriggerDownModeAlignment(ref alignmentYPrefab);
                        break;
                    case "alignment_z":
                        TriggerDownModeAlignment(ref alignmentZPrefab);
                        break;
                    case "alignment_3d":
                        TriggerDownModeAlignment(ref alignment3dPrefab);
                        break;
                }
            }
        }

        if (controller.GetPressUp(triggerButton)) {
            if (pickup != null && pickup.GetComponent<TouchableController>() && pickup.GetComponent<TouchableController>().onTriggerUp != null) {
                pickup.GetComponent<TouchableController>().onTriggerUp.Invoke(spawnPoint.transform, (int)trackedObj.index);
                triggerDown = false;
                if (deferredTriggerExit) {
                    pickup = null; // this happens when holding the trigger while moving away from a collider, then releasing outside the collider.
                    deferredTriggerExit = false;
                }
            }
            if (pickup == null && meshController.modeManager.mode == "alignment_delete") {
                triggerDown = false;
            }
        }

        if (controller.GetPressDown(gripButton) && pickup != null && !(primitiveModes.Contains(meshController.modeManager.mode) && PrimitiveInProgress())) {
            Debug.Log("[" + trackedObj.index + "] grip pressed");            
            if (pickup.GetComponent<GrabbableController>()) {
                pickup.GetComponent<GrabbableController>().onGrabStart.Invoke(spawnPoint.transform, (int)trackedObj.index);
                dragging = true;
                pickup.GetComponent<Rigidbody>().isKinematic = true;
            }
            Debug.Log("[" + trackedObj.index + "] Dragging true");
        }

        if (controller.GetPressDown(gripButton) && pickup == null && !PrimitiveInProgress()) { // Drag on empty space moves the mesh
            pickup = meshController.gameObject;
            meshController.GetComponent<GrabbableController>().onGrabStart.Invoke(spawnPoint.transform, (int)trackedObj.index);
            dragging = true;
            meshController.GetComponent<Rigidbody>().isKinematic = true;
            airGrab = true;
        }

        if (dragging && pickup.GetComponent<GrabbableController>()) {
            pickup.GetComponent<GrabbableController>().onGrabUpdate.Invoke(spawnPoint.transform, (int)trackedObj.index);
        }

        if (triggerDown && pickup && pickup.GetComponent<TouchableController>()) {
            if (pickup.GetComponent<TouchableController>().onTriggerUpdate != null)
                pickup.GetComponent<TouchableController>().onTriggerUpdate.Invoke(spawnPoint.transform, (int)trackedObj.index);
        }

        if (primitiveCircleInstance != null) {
            if (meshController.modeManager.mode != "primitive_circle") {
                Destroy(primitiveCircleInstance);
            } else {
                TriggerPlacementController triggerPlacementController = primitiveCircleInstance.GetComponent<TriggerPlacementController>();
                triggerPlacementController.onMoveUpdate.Invoke(spawnPoint.transform, (int)trackedObj.index);
            }
        }

        if (primitiveCylinderInstance != null) {
            if (meshController.modeManager.mode != "primitive_cylinder") {
                Destroy(primitiveCylinderInstance);
            } else {
                TriggerPlacementController triggerPlacementController = primitiveCylinderInstance.GetComponent<TriggerPlacementController>();
                triggerPlacementController.onMoveUpdate.Invoke(spawnPoint.transform, (int)trackedObj.index);
            }
        }

        if (primitiveSphereInstance != null) {
            if (meshController.modeManager.mode != "primitive_sphere") {
                Destroy(primitiveSphereInstance);
            } else {
                TriggerPlacementController triggerPlacementController = primitiveSphereInstance.GetComponent<TriggerPlacementController>();
                triggerPlacementController.onMoveUpdate.Invoke(spawnPoint.transform, (int)trackedObj.index);
            }
        }

        if (primitivePlaneInstance != null) {
            if (meshController.modeManager.mode != "primitive_plane") {
                Destroy(primitivePlaneInstance);
            } else {
                TriggerPlacementController triggerPlacementController = primitivePlaneInstance.GetComponent<TriggerPlacementController>();
                triggerPlacementController.onMoveUpdate.Invoke(spawnPoint.transform, (int)trackedObj.index);
            }
        }

        if (primitiveBoxInstance != null) {
            if (meshController.modeManager.mode != "primitive_box") {
                Destroy(primitiveBoxInstance);
            } else {
                TriggerPlacementController triggerPlacementController = primitiveBoxInstance.GetComponent<TriggerPlacementController>();
                triggerPlacementController.onMoveUpdate.Invoke(spawnPoint.transform, (int)trackedObj.index);
            }
        }

        if (boxSelectionInstance != null) {
            if (meshController.modeManager.mode != "box_select") {
                boxSelectionInstance.GetComponent<BoxSelectionController>().Remove();
            } else {
                TriggerPlacementController triggerPlacementController = boxSelectionInstance.GetComponent<TriggerPlacementController>();
                triggerPlacementController.onMoveUpdate.Invoke(spawnPoint.transform, (int)trackedObj.index);
            }
        }

        if (controller.GetPressUp(gripButton) && pickup != null && dragging == true) {
            Debug.Log("[" + trackedObj.index + "] Grip release, pickup.tag=" + pickup.tag);
            if (pickup.GetComponent<GrabbableController>()) {
                pickup.GetComponent<GrabbableController>().onGrabEnd.Invoke(spawnPoint.transform, (int)trackedObj.index);
                dragging = false;
                if (pickup && pickup.tag == "Vertex") pickup = null;
                if (airGrab) {
                    pickup = null;
                    airGrab = false;
                }
            }
            Debug.Log("[" + trackedObj.index + "] Dragging false");
        }
        lastMode = meshController.modeManager.mode;
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.GetComponent<TouchableController>() && !(modesIgnoringTouchableControllerForVertices.Contains(meshController.modeManager.mode) && collider.tag == "Vertex")) {
            collider.GetComponent<TouchableController>().onSelected.Invoke();
            collider.GetComponent<TouchableController>().onSelectedEnter.Invoke(spawnPoint.transform, (int)trackedObj.index);
        }
        if (!dragging && !triggerDown && !PrimitiveInProgress()) {
            pickup = collider.gameObject;
        }
    }

    private void OnTriggerStay(Collider collider) {
        if (collider.GetComponent<TouchableController>() && !(modesIgnoringTouchableControllerForVertices.Contains(meshController.modeManager.mode) && collider.tag == "Vertex")) {
            collider.GetComponent<TouchableController>().onSelectedUpdate.Invoke(spawnPoint.transform, (int)trackedObj.index);
        }
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.GetComponent<TouchableController>() && !(modesIgnoringTouchableControllerForVertices.Contains(meshController.modeManager.mode) && collider.tag == "Vertex")) {
            collider.GetComponent<TouchableController>().onUnSelected.Invoke();
            collider.GetComponent<TouchableController>().onSelectedExit.Invoke(spawnPoint.transform, (int)trackedObj.index);
        }
        if (!dragging && !triggerDown) {
            if (pickup != null && collider != null && pickup != collider.gameObject) {
                // Warm hand off - do nothing.
            } else {
                //Debug.Log("OnTriggerExit");
                
                if (pickup && !StillInside(collider.transform.position)) {
                    pickup = null;
                }
            }
        }
        if (triggerDown) deferredTriggerExit = true;
    }

    private bool StillInside(Vector3 target) {
        RaycastHit[] hits;

        Vector3 dir = target - validationPoint;
        float dist = dir.magnitude;
        //Debug.Log("Validating if the contact has really exited... Raycast with distance: " + dist);

        hits = Physics.RaycastAll(validationPoint, dir.normalized, dist);
        //Debug.DrawRay(validationPoint, dir.normalized * dist, Color.white, 40.0f);
        foreach (RaycastHit hit in hits) {
            //Debug.Log("hit.collider.gameobject.name=" + hit.collider.gameObject.name);
            //Debug.Break();
            if (hit.collider.gameObject == gameObject) {
                //Debug.Log("The contact seems to still be inside.");
                return true;
            }
        }

        //Debug.Log("The contact seems to have left");
        return false;
    }
}

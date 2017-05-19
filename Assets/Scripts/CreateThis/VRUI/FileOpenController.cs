using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class FileOpenController : MonoBehaviour {
    public KineticScroller kineticScroller;
    public GameObject kineticScrollItemPrefab;
    public float height;
    public MeshController meshController;
    public GameObject folderPrefab;
    public GameObject currentPathLabel;

    private string currentPath;
    private List<GameObject> list;
    private string openPath;

    private GameObject InstantiatePrefabUsingGameObject(GameObject myGameObject) {
        GameObject instance = Instantiate(kineticScrollItemPrefab);
        instance.transform.localRotation = Quaternion.identity;
        instance.GetComponent<KineticScrollerItem>().kineticScroller = kineticScroller;
        MeshFilter meshFilter = instance.GetComponent<MeshFilter>();
        meshFilter.mesh = myGameObject.GetComponent<MeshFilter>().mesh;
        MeshRenderer meshRenderer = instance.GetComponent<MeshRenderer>();
        meshRenderer.materials = myGameObject.GetComponent<MeshRenderer>().materials;
        SelectableController selectableController = instance.GetComponent<SelectableController>();
        selectableController.unselectedMaterials = meshRenderer.materials;
        BoxCollider otherBoxCollider = myGameObject.GetComponent<BoxCollider>();
        if (otherBoxCollider) {
            BoxCollider boxCollider = instance.GetComponent<BoxCollider>();
            boxCollider.center = otherBoxCollider.center;
            boxCollider.size = otherBoxCollider.size;
        }

        CapsuleCollider otherCapsuleCollider = myGameObject.GetComponent<CapsuleCollider>();
        if (otherCapsuleCollider) {
            Destroy(instance.GetComponent<BoxCollider>());
            CapsuleCollider capsuleCollider = instance.AddComponent<CapsuleCollider>();
            capsuleCollider.radius = otherCapsuleCollider.radius;
            capsuleCollider.height = otherCapsuleCollider.height;
            capsuleCollider.direction = otherCapsuleCollider.direction;
        }
        
        return instance;
    }

    private string[] FilesInCurrentPath() {
        return Directory.GetFiles(currentPath, "*.obj");
    }

    private string[] FoldersInCurrentPath() {
        return new DirectoryInfo(currentPath).GetDirectories().Where(x => (x.Attributes & FileAttributes.Hidden) == 0).Select(f => f.FullName).ToArray();
    }

    private string GetParentPath() {
        if (Directory.GetParent(currentPath) != null) {
            return Directory.GetParent(currentPath).FullName;
        }
        return currentPath;
    }

    private void Clear() {
        foreach (GameObject fileObject in list) {
            Destroy(fileObject);
        }
        list.Clear();
    }

    private GameObject BuildFileObject(string path, bool isFolder) {
        GameObject meshObject = isFolder ? Instantiate(folderPrefab) : GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        GameObject fileObject = InstantiatePrefabUsingGameObject(meshObject);
        Destroy(meshObject);

        FileObjectController fileObjectController = fileObject.AddComponent<FileObjectController>();
        fileObjectController.path = path;
        fileObjectController.isFolder = isFolder;

        GameObject text = new GameObject();
        TextMesh textMesh = text.AddComponent<TextMesh>();
        text.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        textMesh.text = Path.GetFileName(path);
        text.name = "fileNameLabel_" + textMesh.text;
        Bounds textBounds = textMesh.GetComponent<Renderer>().bounds;
        text.transform.position = fileObject.transform.position + new Vector3(-textBounds.size.x / 2, -height * .75f, 0);
        text.transform.parent = fileObject.transform;
        return fileObject;
    }

    private void ListDirectory() {
        if (Directory.GetParent(currentPath) != null) { // If not root directory
            list.Add(BuildFileObject("parent", true));
        }

        string[] filePaths = FilesInCurrentPath();

        for (int i = 0; i < filePaths.Length; i++) {
            list.Add(BuildFileObject(filePaths[i], false));
        }

        string[] dirPaths = FoldersInCurrentPath();

        for (int i = 0; i < dirPaths.Length; i++) {
            list.Add(BuildFileObject(dirPaths[i], true));
        }
        kineticScroller.SetHeight(height);
        kineticScroller.SetList(list);
    }

    public string SpecialDirectoryNameToPath(string path) {
        switch (path) {
            case "documents":
                return KnownFolders.GetPath(KnownFolder.Documents);
            case "downloads":
                return KnownFolders.GetPath(KnownFolder.Downloads);
            case "desktop":
                return KnownFolders.GetPath(KnownFolder.Desktop);
            default:
                return null;
        }
    }

    public void ChangeToSpecialDirectory(string specialDirectoryName) {
        string path = SpecialDirectoryNameToPath(specialDirectoryName);
        ChangeDirectory(path);
    }

    public void ChangeDirectory(string path) {
        currentPath = path;
        UpdateCurrentPathLabel();
        Clear();
        ListDirectory();
    }

    public void SaveAndOpen(string path = null) {
        if (path == null || path == "") path = openPath;
        meshController.unsavedPanelController.SetVisible(false);
        meshController.Save();
        Open(path);
    }

    public void Open(string path = null) {
        if (path == null || path == "") path = openPath;
        meshController.unsavedPanelController.SetVisible(false);
        meshController.selectionManager.ClearSelectedVertices();
        meshController.verticesManager.DeleteVertexInstances();
        meshController.trianglesManager.DeleteTriangleInstances();
        meshController.persistenceManager.Load(path);
        meshController.fileOpenPanelController.SetVisible(false);
        meshController.SetMode(meshController.modeManager.mode);
    }

    public void HandleClick(GameObject fileObject) {
        FileObjectController fileObjectController = fileObject.GetComponent<FileObjectController>();
        string path = fileObjectController.path;
        if (fileObjectController.isFolder) {
            if (fileObjectController.path == "parent") {
                path = GetParentPath();
            }
            ChangeDirectory(path);
        } else {
            if (meshController.persistenceManager.changedSinceLastSave) {
                openPath = path;
                meshController.fileOpenPanelController.SetVisible(false);
                meshController.unsavedPanelController.SetVisible(true);
            } else {
                Open(path);
            }
        }
    }

    private void UpdateCurrentPathLabel() {
        TextMesh textMesh = currentPathLabel.GetComponent<TextMesh>();
        textMesh.text = currentPath;
    }

    void OnEnable() {
        KineticScroller.OnClicked += HandleClick;
    }

    void OnDisable() {
        KineticScroller.OnClicked -= HandleClick;
    }

    // Use this for initialization
    void Start () {
        currentPath = Directory.GetCurrentDirectory();
        UpdateCurrentPathLabel();
        list = new List<GameObject>();
        ListDirectory();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

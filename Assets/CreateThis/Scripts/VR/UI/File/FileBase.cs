using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using CreateThis.System;
using CreateThis.VR.UI.Interact;
using CreateThis.VR.UI.Scroller;
using CreateThis.VR.UI.Panel;

namespace CreateThis.VR.UI.File {
    public abstract class FileBase : StandardPanel {
        public KineticScroller kineticScroller;
        public GameObject kineticScrollItemPrefab;
        public float height;
        public GameObject folderPrefab;
        public GameObject currentPathLabel;
        public string searchPattern;

        protected string currentPath;
        private List<GameObject> list;

        private GameObject InstantiatePrefabUsingGameObject(GameObject myGameObject) {
            GameObject instance = Instantiate(kineticScrollItemPrefab);
            instance.SetActive(true);
            instance.transform.localRotation = Quaternion.identity;
            instance.GetComponent<KineticScrollerItem>().kineticScroller = kineticScroller;
            MeshFilter meshFilter = instance.GetComponent<MeshFilter>();
            meshFilter.mesh = myGameObject.GetComponent<MeshFilter>().mesh;
            MeshRenderer meshRenderer = instance.GetComponent<MeshRenderer>();
            meshRenderer.materials = myGameObject.GetComponent<MeshRenderer>().materials;
            Selectable selectable = instance.GetComponent<Selectable>();
            selectable.unselectedMaterials = meshRenderer.materials;
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
            return Directory.GetFiles(currentPath, searchPattern);
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

            FileData fileData = fileObject.AddComponent<FileData>();
            fileData.path = path;
            fileData.isFolder = isFolder;

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

        public void ChangeDirectoryToKnownFolder(KnownFolder knownFolder) {
            string path = KnownFolders.GetPath(knownFolder);
            ChangeDirectory(path);
        }

        public void ChangeDirectory(string path) {
            currentPath = path;
            UpdateCurrentPathLabel();
            Clear();
            ListDirectory();
        }

        protected void ClickedFile(string path) {

        }

        public void HandleClick(GameObject fileObject) {
            FileData fileData = fileObject.GetComponent<FileData>();
            string path = fileData.path;
            if (fileData.isFolder) {
                if (fileData.path == "parent") {
                    path = GetParentPath();
                }
                ChangeDirectory(path);
            } else {
                ClickedFile(path);
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
        void Start() {
            currentPath = Directory.GetCurrentDirectory();
            UpdateCurrentPathLabel();
            list = new List<GameObject>();
            ListDirectory();
        }
    }
}
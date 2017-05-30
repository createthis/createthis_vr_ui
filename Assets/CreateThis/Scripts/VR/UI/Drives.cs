using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using CreateThis.VR.UI.Button;

namespace CreateThis.VR.UI {
    public class Drives : MonoBehaviour {
        public GameObject driveButtonPrefab;
        public Vector3 position;

        private List<GameObject> driveButtons;
        private string[] drives;


        private void InstantiateDriveButton(string path) {
            GameObject driveButton = Instantiate(driveButtonPrefab);
            driveButton.transform.parent = driveButtonPrefab.transform.parent;
            driveButton.transform.localRotation = Quaternion.identity;
            driveButton.transform.localScale = driveButtonPrefab.transform.localScale;
            driveButton.transform.localPosition = driveButtonPrefab.transform.localPosition + -driveButton.transform.right * DriveButtonRowLocalWidth();
            driveButton.GetComponent<GrowButtonByTextMesh>().Initialize();
            driveButton.GetComponent<DriveButton>().SetPath(path);
            driveButtons.Add(driveButton);
        }

        private float DriveButtonLocalWidth() {
            bool oldActive = driveButtonPrefab.activeSelf;
            driveButtonPrefab.SetActive(true);

            Bounds bounds = driveButtonPrefab.GetComponent<BoxCollider>().bounds;
            float width = bounds.size.x * driveButtonPrefab.transform.localScale.x;
            driveButtonPrefab.SetActive(oldActive);
            return width;
        }

        private float DriveButtonRowLocalWidth() {
            return DriveButtonLocalWidth() * driveButtons.Count;
        }

        private float DriveButtonWidth() {
            bool oldActive = driveButtonPrefab.activeSelf;
            driveButtonPrefab.SetActive(true);

            Bounds bounds = driveButtonPrefab.GetComponent<BoxCollider>().bounds;
            float width = bounds.size.x;
            driveButtonPrefab.SetActive(oldActive);
            return width;
        }

        private float DriveButtonRowWidth() {
            return DriveButtonWidth() * driveButtons.Count;
        }

        private void InstantiateDriveButtons(string[] myDrives) {
            foreach (string drive in myDrives) {
                InstantiateDriveButton(drive);
            }
            driveButtonPrefab.SetActive(false);
        }

        private void Clear() {
            foreach (GameObject driveButton in driveButtons) {
                Destroy(driveButton);
            }
            driveButtons.Clear();
        }

        private void RefreshDrives() {
            string[] newDrives = Directory.GetLogicalDrives();
            if (drives.SequenceEqual(newDrives)) return;
            Clear();
            driveButtonPrefab.SetActive(true);
            InstantiateDriveButtons(newDrives);
            drives = newDrives;
        }

        // Use this for initialization
        void Start() {
            driveButtons = new List<GameObject>();
            drives = Directory.GetLogicalDrives();
            InstantiateDriveButtons(drives);
            InvokeRepeating("RefreshDrives", 2.0f, 2.0f);
        }

        // Update is called once per frame
        void Update() {
        }
    }
}
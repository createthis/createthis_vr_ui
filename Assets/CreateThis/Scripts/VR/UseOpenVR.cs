using System.Collections;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.SceneManagement;

namespace CreateThis.VR.UI {
    public class UseOpenVR : MonoBehaviour {
        IEnumerator LoadDevice(string newDevice) {
            VRSettings.LoadDeviceByName(newDevice);
            yield return null;
            VRSettings.enabled = true;
            SteamVR.enabled = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        // Use this for initialization
        void Start() {
            if (VRSettings.loadedDeviceName != "OpenVR") {
                Debug.Log(string.Join(",", VRSettings.supportedDevices));
                StartCoroutine(LoadDevice("OpenVR"));
            }
        }

        // Update is called once per frame
        void Update() {

        }
    }
}
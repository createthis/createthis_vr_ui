using System.Collections;
using UnityEngine;

namespace CreateThis.VR {
    public static class Haptic {
        //length is how long the vibration should go for
        //strength is vibration strength from 0-1
        public static IEnumerator LongVibration(int controllerIndex, float length, float strength) {
            for (float i = 0; i < length; i += Time.deltaTime) {
                SteamVR_Controller.Input(controllerIndex).TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
                yield return null;
            }
        }

        //vibrationCount is how many vibrations
        //vibrationLength is how long each vibration should go for
        //gapLength is how long to wait between vibrations
        //strength is vibration strength from 0-1
        public static IEnumerator LongVibration(MonoBehaviour monoBehaviour, int controllerIndex, int vibrationCount, float vibrationLength, float gapLength, float strength) {
            strength = Mathf.Clamp01(strength);
            for (int i = 0; i < vibrationCount; i++) {
                if (i != 0) yield return new WaitForSeconds(gapLength);
                yield return monoBehaviour.StartCoroutine(LongVibration(controllerIndex, vibrationLength, strength));
            }
        }
    }
}

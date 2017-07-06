using UnityEngine;
using UnityEngine.Events;

namespace CreateThis.VR.UI.UnityEvent {
    [global::System.Serializable]
    public class FilePathEvent : UnityEvent<string, Transform, int> {
    }
}
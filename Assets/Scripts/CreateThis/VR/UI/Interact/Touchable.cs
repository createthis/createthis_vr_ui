using UnityEngine;
using CreateThis.VR.UI.UnityEvent;

namespace CreateThis.VR.UI.Interact {
    public class Touchable : MonoBehaviour {
        public UnityEngine.Events.UnityEvent onSelected;
        public UnityEngine.Events.UnityEvent onUnSelected;

        public GrabEvent onSelectedEnter;
        public GrabEvent onSelectedUpdate;
        public GrabEvent onSelectedExit;

        public GrabEvent onTriggerDown;
        public GrabEvent onTriggerUpdate;
        public GrabEvent onTriggerUp;


        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}
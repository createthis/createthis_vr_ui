using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticScrollerItem : MonoBehaviour {
    public KineticScroller kineticScroller;

    public void OnTriggerDown(Transform controller, int controllerIndex) {
        kineticScroller.fileObjectGrabbed = gameObject;
        kineticScroller.OnTriggerDown(controller, controllerIndex);
    }

    public void OnTriggerUpdate(Transform controller, int controllerIndex) {
        kineticScroller.OnTriggerUpdate(controller, controllerIndex);
    }

    public void OnTriggerUp(Transform controller, int controllerIndex) {
        kineticScroller.OnTriggerUp(controller, controllerIndex);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

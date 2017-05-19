using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[System.Serializable]
public class MyGrabEvent : UnityEvent<Transform, int> {
}

public class GrabbableController : MonoBehaviour {
    public MyGrabEvent onGrabStart;
    public MyGrabEvent onGrabUpdate;
    public MyGrabEvent onGrabEnd;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

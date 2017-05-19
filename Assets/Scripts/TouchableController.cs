using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TouchableController : MonoBehaviour {
    public UnityEvent onSelected;
    public UnityEvent onUnSelected;

    public MyGrabEvent onSelectedEnter;
    public MyGrabEvent onSelectedUpdate;
    public MyGrabEvent onSelectedExit;

    public MyGrabEvent onTriggerDown;
    public MyGrabEvent onTriggerUpdate;
    public MyGrabEvent onTriggerUp;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletController : MonoBehaviour {
    private int notSelectableCount;
    private BoxCollider boxCollider;
    private SelectableController selectableController;
    private bool hasInitialized = false;

    public void ZeroNotSelectableCount() {
        Initialize();

        notSelectableCount = 0;
        boxCollider.enabled = true;
    }

    public void SetSelectable(bool value) {
        Initialize();

        if (value) {
            if (notSelectableCount > 0) notSelectableCount--;
        } else {
            notSelectableCount++;
        }

        if (notSelectableCount == 0) {
            boxCollider.enabled = true;
        } else {
            boxCollider.enabled = false;
            selectableController.SetSelected(false);
        }
    }

    public void Initialize() {
        if (hasInitialized) return;
        boxCollider = GetComponent<BoxCollider>();
        selectableController = GetComponent<SelectableController>();
        notSelectableCount = 0;
        hasInitialized = true;
    }

    // Use this for initialization
    void Start () {
        Initialize();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

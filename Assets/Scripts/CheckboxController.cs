using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[System.Serializable]
public class MyBoolEvent : UnityEvent<bool> {
}

public class CheckboxController : MonoBehaviour {
    public MyBoolEvent onClicked;
    public bool isChecked;
    public GameObject checkBox;
    public GameObject checkMark;
    public GameObject label;

    public void SetChecked(bool value) {
        isChecked = value;
        Changed();
    }

    public void Changed() {
        if (isChecked) {
            checkBox.SetActive(false);
            checkMark.SetActive(true);
        } else {
            checkBox.SetActive(true);
            checkMark.SetActive(false);
        }
    }

    // Use this for initialization
    void Start () {
        isChecked = false;
	}

    public void Clicked() {
        isChecked = !isChecked;
        onClicked.Invoke(isChecked);

        Changed();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveButtonController : MonoBehaviour {
    public FileOpenController fileOpenController;
    public TextMesh textMeshLabel;

    private string path;

    public void Click() {
        fileOpenController.ChangeDirectory(path);
    }

    public void SetPath(string value) {
        path = value;
        textMeshLabel.text = value;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

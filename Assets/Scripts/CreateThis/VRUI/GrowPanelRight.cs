using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowPanelRight : MonoBehaviour {
    public GameObject target;
    public float minWidth;

    private List<Func<float>> callbacks;
    private bool hasInitialized = false;

    public void Resize() {
        float maxWidth = MaxWidth();
        float leftPadding = PanelLib.CalculateLeftPaddingOfChildTextLabel(gameObject, target);

        Transform[] children = DetachReattachLib.DetachChildren(gameObject);

        PanelLib.PanelResizeWidth panelResizeWidth = PanelLib.ResizeWidth(gameObject, maxWidth, leftPadding);
        transform.localScale = new Vector3(panelResizeWidth.xScale, transform.localScale.y, transform.localScale.z);
        transform.position = transform.position + transform.right * panelResizeWidth.xOffset;

        DetachReattachLib.ReattachChildren(children, gameObject);
    }

    public void AddWidthCallback(Func<float> callback) {
        Initialize();
        callbacks.Add(callback);
    }

    public void RemoveWidthCallback(Func<float> callback) {
        Initialize();
        callbacks.Remove(callback);
    }

    private float MaxWidth() {
        Initialize();
        float maxWidth = 0;
        foreach (Func<float> callback in callbacks) {
            float width = callback();
            if (width > maxWidth) maxWidth = width;
        }
        if (maxWidth < minWidth) maxWidth = minWidth;
        return maxWidth;
    }

    private float TargetWidth() {
        Bounds textBounds = target.GetComponent<Renderer>().bounds;
        return textBounds.size.x;
    }

    private void AddTargetWidthCallback() {
        if (target == null) return;
        AddWidthCallback(TargetWidth);
    }

    public void Initialize() {
        if (hasInitialized) return;
        callbacks = new List<Func<float>>();
        hasInitialized = true;
        AddTargetWidthCallback();
    }

    // Use this for initialization
    void Start() {
        Initialize();
    }

    // Update is called once per frame
    void Update() {
    }
}

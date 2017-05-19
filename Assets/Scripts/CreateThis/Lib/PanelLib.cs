using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PanelLib {
    public class PanelResizeWidth {
        public float xScale;
        public float xOffset;
    }

    public class PanelResizeHeight {
        public float yScale;
        public float yOffset;
    }

    public static Vector3 Position(Camera sceneCamera, Transform controller, Vector3 offset, float minDistance) {
        float dotProduct = Vector3.Dot(sceneCamera.transform.forward, controller.position - sceneCamera.transform.position);
        if (dotProduct < minDistance) dotProduct = minDistance;
        return sceneCamera.transform.position + sceneCamera.transform.forward * (dotProduct + offset.z) + sceneCamera.transform.right * offset.x + sceneCamera.transform.up * offset.y;
    }

    private static Vector3 CalculateNormal(Vector3 a, Vector3 b, Vector3 c) {
        Vector3 planeNormal = Vector3.Cross(c - a, b - a).normalized;
        return planeNormal;
    }

    public static Quaternion Rotation(Camera sceneCamera, Vector3 position) {
        Vector3 newCameraPosition = sceneCamera.transform.position + sceneCamera.transform.up * 0.1f;
        float distance = Vector3.Distance(position, newCameraPosition);
        Vector3 up;
        if ((sceneCamera.transform.eulerAngles.x > 60 && sceneCamera.transform.eulerAngles.x < 90) || (sceneCamera.transform.eulerAngles.x > 270 && sceneCamera.transform.eulerAngles.x < 313)) {
            up = CalculateNormal(position, newCameraPosition, newCameraPosition + sceneCamera.transform.right * distance);
        } else {
            up = Vector3.up;
        }
        return Quaternion.LookRotation((position - newCameraPosition).normalized, up);
    }

    public static Bounds GetUnrotatedRendererBounds(GameObject target) {
        Transform oldParent = target.transform.parent;
        Quaternion oldRotation = target.transform.rotation;
        Transform[] children = null;

        if (oldParent) {
            children = DetachReattachLib.DetachChildren(oldParent.gameObject);
        }

        target.transform.rotation = Quaternion.identity;
        if (!target.GetComponent<BoxCollider>()) { 
}
        Bounds bounds = target.GetComponent<Renderer>().bounds;
        target.transform.rotation = oldRotation;

        if (oldParent) {
            DetachReattachLib.ReattachChildren(children, oldParent.gameObject);
        }

        return bounds;
    }

    public static Bounds GetWorldBounds(GameObject target) {
        Bounds bounds;

        if (target.GetComponent<BoxCollider>()) {
            Vector3 localSize = target.GetComponent<BoxCollider>().size;
            Vector3 worldSize = LocalVectorToWorldVectorWithoutRotation(localSize, target);
            bounds = new Bounds();
            bounds.size = worldSize;
        } else if(target.GetComponent<Renderer>()) {
            bounds = GetUnrotatedRendererBounds(target);
        } else if (target.GetComponent<RowContainer>()) {
            bounds = target.GetComponent<RowContainer>().bounds;
        } else {
            if (target.GetComponent<ColumnContainer>()) {
                bounds = target.GetComponent<ColumnContainer>().bounds;
            } else {
                Debug.Log("PanelLib GetWorldBounds " + target.name + " has no way to determine bounds. Add a BoxCollider.");
                bounds = new Bounds();
            }
        }

        return bounds;
    }

    public static float CalculateLeftPaddingOfChildTextLabel(GameObject panel, GameObject textLabel) {
        Bounds bounds = GetWorldBounds(panel);
        Quaternion oldPanelRotation = panel.transform.rotation;
        Transform oldParent = panel.transform.parent;
        Transform[] children = DetachReattachLib.DetachChildren(oldParent.gameObject);

        panel.transform.rotation = Quaternion.identity;

        float worldXOffset = Vector3.Distance(new Vector3(textLabel.transform.position.x, 0, 0), new Vector3(panel.transform.position.x, 0, 0));

        panel.transform.rotation = oldPanelRotation;
        DetachReattachLib.ReattachChildren(children, oldParent.gameObject);

        float leftPadding = bounds.size.x / 2 - worldXOffset;
        //if (panel.name == "SnapSpacingNumbersButton") Debug.Log("bounds.size.x=" + bounds.size.x + ",worldXOffset=" + worldXOffset);
        return leftPadding;
    }

    public static float SumWithSpacing(List<float> values, float spacing) {
        float value = 0;
        for (int i = 0; i < values.Count; i++) {
            value += values[i];
            if (i != values.Count - 1) value += spacing;
        }
        return value;
    }

    public static float GetWorldWidth(GameObject target) {
        Bounds bounds = GetWorldBounds(target);
        return bounds.size.x;
    }

    public static float GetWorldHeight(GameObject target) {
        Bounds bounds = GetWorldBounds(target);
        return bounds.size.y;
    }

    public static PanelResizeWidth ResizeWidth(GameObject panel, float width, float padding, bool useRatio = false) {
        PanelResizeWidth panelResizeWidth = new PanelResizeWidth();
        Bounds bounds = GetWorldBounds(panel);

        float oldScale = panel.transform.localScale.x;
        float oldWidth = bounds.size.x;
        float newWidth = width + padding * 2.15f;
        //if (panel.name == "SnapSpacingNumbersButton") Debug.Log("oldWidth=" + oldWidth + ",newWidth=" + newWidth + ",width=" + width + ",padding=" + padding);

        // oldWidth   newWidth
        // -------- = --------
        // oldScale   newScale
        if (useRatio) { // used for button resizing
            panelResizeWidth.xScale = RatioLib.SolveForD(oldWidth, oldScale, newWidth);
        } else {
            panelResizeWidth.xScale = newWidth;
        }
        panelResizeWidth.xOffset = (newWidth - oldWidth) / 2;

        return panelResizeWidth;
    }

    public static PanelResizeHeight ResizeHeight(GameObject panel, float height, float padding) {
        PanelResizeHeight panelResizeHeight = new PanelResizeHeight();
        Bounds bounds = GetWorldBounds(panel);

        float oldHeight = bounds.size.y;
        float newHeight = height + padding * 2.15f;

        // oldWidth   newWidth
        // -------- = --------
        // oldScale   newScale
        panelResizeHeight.yScale = newHeight;
        panelResizeHeight.yOffset = (newHeight - oldHeight) / 2;

        return panelResizeHeight;
    }

    public static Vector3 LocalVectorToWorldVectorWithoutRotation(Vector3 localVector, GameObject target) {
        Quaternion oldRotation = target.transform.rotation;
        Transform oldParent = target.transform.parent;
        Transform[] children = DetachReattachLib.DetachChildren(oldParent.gameObject);
        target.transform.rotation = Quaternion.identity;
        Vector3 worldPoint = target.transform.TransformVector(localVector);
        target.transform.rotation = oldRotation;
        DetachReattachLib.ReattachChildren(children, oldParent.gameObject);

        return worldPoint;
    }

    public static Vector3 WorldVectorToLocalVectorWithoutRotation(Vector3 worldVector, GameObject target) {
        Quaternion oldRotation = target.transform.rotation;
        Transform oldParent = target.transform.parent;
        Transform[] children = null;
        if (oldParent) {
            children = DetachReattachLib.DetachChildren(oldParent.gameObject);
        }
        target.transform.rotation = Quaternion.identity;
        Vector3 localPoint = target.transform.InverseTransformVector(worldVector);
        target.transform.rotation = oldRotation;
        if (oldParent) {
            DetachReattachLib.ReattachChildren(children, oldParent.gameObject);
        }

        return localPoint;
    }

    public static float WorldDistanceToLocalDistanceY(float worldDistance, GameObject target) {
        Vector3 localPoint = WorldVectorToLocalVectorWithoutRotation(new Vector3(0, worldDistance, 0), target);
        return localPoint.y;
    }

    public static float WorldDistanceToLocalDistance(float worldDistance, GameObject target) {
        Vector3 localPoint = WorldVectorToLocalVectorWithoutRotation(new Vector3(worldDistance, 0, 0), target);
        return localPoint.x;
    }
}

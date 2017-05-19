using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class GrowButtonByTextMesh : MonoBehaviour {
    public float minWidth = 0.025f;
    public float padding = 0.002f;
    public GameObject buttonBody;
    public TextMesh textMesh;
    public TextAlignment alignment;
    public bool log = false;

    private string lastText;

    private bool hasInitialized = false;

    public void Resize() {
#if UNITY_EDITOR
        Undo.SetCurrentGroupName("GrowButtonByTextMesh Resize");
        int group = Undo.GetCurrentGroup();
#endif

        float rowWidth = RowWidth();

#if UNITY_EDITOR
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            Undo.RecordObject(gameObject.transform.GetChild(i), "Resize Child");
        }
#endif

        textMesh.gameObject.transform.SetParent(null, true);

#if UNITY_EDITOR
        Undo.RecordObject(transform, "Scale Main Object");
#endif

        PanelLib.PanelResizeWidth panelResizeWidth = PanelLib.ResizeWidth(gameObject, rowWidth, 0, true);
        transform.localScale = new Vector3(panelResizeWidth.xScale, transform.localScale.y, transform.localScale.z);
        textMesh.gameObject.transform.SetParent(gameObject.transform, true);
        if (alignment == TextAlignment.Left) {
            // FIXME: this can be optimized by batching all WorldDistanceToLocalDistance calls for this object
            float localTextMeshWidth = PanelLib.WorldDistanceToLocalDistance(TextMeshWidth(), textMesh.gameObject);
            float localParentContainerWidth = PanelLib.WorldDistanceToLocalDistance(rowWidth, textMesh.gameObject);
            float localPadding = PanelLib.WorldDistanceToLocalDistance(padding, textMesh.gameObject);
            float xOffset = localTextMeshWidth / 2 + localPadding - localParentContainerWidth / 2;
            textMesh.gameObject.transform.localPosition = new Vector3(
                xOffset * textMesh.gameObject.transform.localScale.x,
                textMesh.gameObject.transform.localPosition.y,
                textMesh.gameObject.transform.localPosition.z
                );
        } else {
            textMesh.gameObject.transform.localPosition = new Vector3(0, textMesh.gameObject.transform.localPosition.y, textMesh.gameObject.transform.localPosition.z);
        }

#if UNITY_EDITOR
        Undo.CollapseUndoOperations(group);
#endif

        ExecuteEvents.Execute<IChild3dWidgetResized>(transform.parent.gameObject, null, (x, y) => x.Child3dWidgetResized());
    }

    private float RowWidth() {
        Initialize();
        float rowWidth = padding + TextMeshWidth() + padding;
        if (rowWidth < minWidth) rowWidth = minWidth;
        return rowWidth;
    }

    private float TextMeshWidth() {
        Bounds textBounds = PanelLib.GetWorldBounds(textMesh.gameObject);
        return textBounds.size.x;
    }

    public void Initialize() {
        if (hasInitialized) return;
        lastText = textMesh.text;
        hasInitialized = true;
    }

    // Use this for initialization
    void Start() {
        Initialize();
    }

    // Update is called once per frame
    void Update() {
        if (textMesh.text != lastText) {
            Resize();
            lastText = textMesh.text;
        }
    }
}

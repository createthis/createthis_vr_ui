using System.Collections.Generic;
using UnityEngine;
using CreateThis.Math;
using CreateThis.Unity;
using CreateThis.VR.UI.Container;

namespace CreateThis.VR.UI.Panel {
    /* 
     * Utility methods for positioning and resizing panels
     */
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

        public static Quaternion Rotation(Camera sceneCamera, Vector3 position) {
            Vector3 newCameraPosition = sceneCamera.transform.position + sceneCamera.transform.up * 0.1f;
            float distance = Vector3.Distance(position, newCameraPosition);
            Vector3 up;
            if ((sceneCamera.transform.eulerAngles.x > 60 && sceneCamera.transform.eulerAngles.x < 90) || (sceneCamera.transform.eulerAngles.x > 270 && sceneCamera.transform.eulerAngles.x < 313)) {
                up = Triangle.CalculateNormal(position, newCameraPosition, newCameraPosition + sceneCamera.transform.right * distance);
            } else {
                up = Vector3.up;
            }
            return Quaternion.LookRotation((position - newCameraPosition).normalized, up);
        }

        public static float SumWithSpacing(List<float> values, float spacing) {
            float value = 0;
            for (int i = 0; i < values.Count; i++) {
                value += values[i];
                if (i != values.Count - 1) value += spacing;
            }
            return value;
        }

        public static PanelResizeWidth ResizeWidth(GameObject panel, float width, float padding, bool useRatio = false) {
            PanelResizeWidth panelResizeWidth = new PanelResizeWidth();
            Bounds bounds = ObjectBounds.ToWorld(panel);

            float oldScale = panel.transform.localScale.x;
            float oldWidth = bounds.size.x;
            float newWidth = width + padding * 2.15f;
            //if (panel.name == "SnapSpacingNumbersButton") Debug.Log("oldWidth=" + oldWidth + ",newWidth=" + newWidth + ",width=" + width + ",padding=" + padding);

            // oldWidth   newWidth
            // -------- = --------
            // oldScale   newScale
            if (useRatio) { // used for button resizing
                panelResizeWidth.xScale = Ratio.SolveForD(oldWidth, oldScale, newWidth);
            } else {
                panelResizeWidth.xScale = newWidth;
            }
            panelResizeWidth.xOffset = (newWidth - oldWidth) / 2;

            return panelResizeWidth;
        }

        public static PanelResizeHeight ResizeHeight(GameObject panel, float height, float padding) {
            PanelResizeHeight panelResizeHeight = new PanelResizeHeight();
            Bounds bounds = ObjectBounds.ToWorld(panel);

            float oldHeight = bounds.size.y;
            float newHeight = height + padding * 2.15f;

            // oldWidth   newWidth
            // -------- = --------
            // oldScale   newScale
            panelResizeHeight.yScale = newHeight;
            panelResizeHeight.yOffset = (newHeight - oldHeight) / 2;

            return panelResizeHeight;
        }
    }
}

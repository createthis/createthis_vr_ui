using UnityEngine;
using CreateThis.VR.UI.Container;

namespace CreateThis.Unity {
    /* Rather than an Axis Aligned Bounding Box (AABB), this delivers an
     * Object Oriented Bounding Box (OOBB).
     */
    public static class ObjectBounds {
        private static Bounds GetUnrotatedRendererBounds(GameObject target) {
            Transform oldParent = target.transform.parent;
            Quaternion oldRotation = target.transform.rotation;
            Transform[] children = null;

            if (oldParent) {
                children = DetachReattach.DetachChildren(oldParent.gameObject);
            }

            target.transform.rotation = Quaternion.identity;
            if (!target.GetComponent<BoxCollider>()) {
            }
            Bounds bounds = target.GetComponent<Renderer>().bounds;
            target.transform.rotation = oldRotation;

            if (oldParent) {
                DetachReattach.ReattachChildren(children, oldParent.gameObject);
            }

            return bounds;
        }

        // An OBB projected into world space, but still rotated with the target object.
        public static Bounds ToWorld(GameObject target) {
            Bounds bounds;

            if (target.GetComponent<BoxCollider>()) {
                Vector3 localSize = target.GetComponent<BoxCollider>().size;
                Vector3 worldSize = TransformWithoutRotation.LocalVectorToWorldVector(localSize, target);
                bounds = new Bounds();
                bounds.size = worldSize;
            } else if (target.GetComponent<Renderer>()) {
                bounds = GetUnrotatedRendererBounds(target);
            } else if (target.GetComponent<RowContainer>()) {
                bounds = target.GetComponent<RowContainer>().bounds;
            } else {
                if (target.GetComponent<ColumnContainer>()) {
                    bounds = target.GetComponent<ColumnContainer>().bounds;
                } else {
                    Debug.Log("ObjectBounds.ToWorld " + target.name + " has no way to determine bounds. Add a BoxCollider.");
                    bounds = new Bounds();
                }
            }

            return bounds;
        }

        public static float WorldWidth(GameObject target) {
            Bounds bounds = ToWorld(target);
            return bounds.size.x;
        }

        public static float WorldHeight(GameObject target) {
            Bounds bounds = ToWorld(target);
            return bounds.size.y;
        }
    }
}

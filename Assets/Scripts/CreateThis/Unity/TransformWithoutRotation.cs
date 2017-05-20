using UnityEngine;

namespace CreateThis.Unity {
    public static class TransformWithoutRotation {
        public static Vector3 LocalVectorToWorldVector(Vector3 localVector, GameObject target) {
            Quaternion oldRotation = target.transform.rotation;
            Transform oldParent = target.transform.parent;
            Transform[] children = DetachReattach.DetachChildren(oldParent.gameObject);
            target.transform.rotation = Quaternion.identity;
            Vector3 worldPoint = target.transform.TransformVector(localVector);
            target.transform.rotation = oldRotation;
            DetachReattach.ReattachChildren(children, oldParent.gameObject);

            return worldPoint;
        }

        public static Vector3 WorldVectorToLocalVector(Vector3 worldVector, GameObject target) {
            Quaternion oldRotation = target.transform.rotation;
            Transform oldParent = target.transform.parent;
            Transform[] children = null;
            if (oldParent) {
                children = DetachReattach.DetachChildren(oldParent.gameObject);
            }
            target.transform.rotation = Quaternion.identity;
            Vector3 localPoint = target.transform.InverseTransformVector(worldVector);
            target.transform.rotation = oldRotation;
            if (oldParent) {
                DetachReattach.ReattachChildren(children, oldParent.gameObject);
            }

            return localPoint;
        }

        public static float WorldDistanceToLocalDistanceY(float worldDistance, GameObject target) {
            Vector3 localPoint = WorldVectorToLocalVector(new Vector3(0, worldDistance, 0), target);
            return localPoint.y;
        }

        public static float WorldDistanceToLocalDistance(float worldDistance, GameObject target) {
            Vector3 localPoint = WorldVectorToLocalVector(new Vector3(worldDistance, 0, 0), target);
            return localPoint.x;
        }
    }
}
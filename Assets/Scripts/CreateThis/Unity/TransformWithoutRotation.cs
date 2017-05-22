using UnityEngine;

namespace CreateThis.Unity {
    public static class TransformWithoutRotation {
        public class State {
            public GameObject target;
            public Quaternion rotation;
            public Transform parent;
            public Transform[] children;
        }

        public static State DetachChildrenAndZeroRotation(GameObject target) {
            State state = new State();
            state.target = target;
            state.rotation = target.transform.rotation;
            state.parent = target.transform.parent;
            state.children = null;
            if (state.parent) {
                state.children = DetachReattach.DetachChildren(state.parent.gameObject);
            }
            target.transform.rotation = Quaternion.identity;
            return state;
        }

        public static void ReattachChildrenAndRestoreRotation(GameObject target, State state) {
            target.transform.rotation = state.rotation;
            if (state.parent) {
                DetachReattach.ReattachChildren(state.children, state.parent.gameObject);
            }
        }

        public static Vector3 LocalVectorToWorldVector(Vector3 localVector, GameObject target) {
            State state = DetachChildrenAndZeroRotation(target);
            Vector3 worldPoint = target.transform.TransformVector(localVector);
            ReattachChildrenAndRestoreRotation(target, state);

            return worldPoint;
        }

        public static Vector3 WorldVectorToLocalVector(Vector3 worldVector, GameObject target) {
            State state = DetachChildrenAndZeroRotation(target);
            target.transform.rotation = Quaternion.identity;
            Vector3 localPoint = target.transform.InverseTransformVector(worldVector);
            ReattachChildrenAndRestoreRotation(target, state);

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
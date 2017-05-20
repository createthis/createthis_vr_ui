using UnityEngine;

namespace CreateThis.Unity {
    public static class Triangle {
        public static Vector3 CalculateNormal(Vector3 a, Vector3 b, Vector3 c) {
            Vector3 planeNormal = Vector3.Cross(c - a, b - a).normalized;
            return planeNormal;
        }
    }
}

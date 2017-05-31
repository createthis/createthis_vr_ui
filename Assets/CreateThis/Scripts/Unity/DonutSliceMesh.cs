using System.Collections.Generic;
using System;
using UnityEngine;

namespace CreateThis.Unity {
    public class DonutSliceMesh {
        public class Arc {
            public Vector3[] verts;
            public Vector2[] uvs;
        }
        public int numVerts = 46; // number of vertices per 360 degrees

        private string VerticesToString(Vector3[] verts) {
            List<string> result = new List<string>();

            foreach (Vector3 vertex in verts) {
                result.Add(vertex.ToString());
            }
            return string.Join(",", result.ToArray());
        }

        public float NumVertsOfDegrees(float degrees) {
            return degrees * numVerts / 360.0f;
        }

        public Mesh Build(float innerRadius, float outerRadius, float degrees) {
            int numVertsOfDegrees = (int)NumVertsOfDegrees(degrees);

            Arc innerArc = BuildArc(innerRadius, degrees);
            Arc outerArc = BuildArc(outerRadius, degrees);

            Mesh plane = new Mesh();
            Vector3[] verts = new Vector3[innerArc.verts.Length + outerArc.verts.Length];
            Array.Copy(innerArc.verts, verts, innerArc.verts.Length);
            Array.Copy(outerArc.verts, 0, verts, innerArc.verts.Length, outerArc.verts.Length);
            plane.vertices = verts;

            Vector2[] uvs = new Vector2[innerArc.uvs.Length + outerArc.uvs.Length];
            Array.Copy(innerArc.uvs, uvs, innerArc.uvs.Length);
            Array.Copy(outerArc.uvs, 0, uvs, innerArc.uvs.Length, outerArc.uvs.Length);
            plane.uv = uvs;

            int[] tris = new int[(numVertsOfDegrees * 2 /* two arcs */ * 3 /* three verts per triangle */)];
            // Here we create all of our triangles. We walk each vertex in the inner arc. 
            // Since this is a flat donut, each vertex on the inner arc has two triangles.
            // The first triangle will start with a vertex in the inner arc and connect to 2 on the outer arc.
            // The second triangle will start with the same vertex, connect to the next vertex on the outer arc and the next vertex on the inner arc.
            for (int i = 0; i + 1 < numVertsOfDegrees; ++i) {
                int index = i * 6;
                // First triangle
                tris[index + 0] = i;
                tris[index + 1] = i + numVertsOfDegrees + 0;
                tris[index + 2] = i + numVertsOfDegrees + 1;

                // Second triangle
                tris[index + 3] = i;
                tris[index + 4] = i + numVertsOfDegrees + 1;
                tris[index + 5] = i + 1;
            }

            plane.triangles = tris;
            return plane;
        }

        public Arc BuildArc(float radius, float degrees) {
            int numVertsOfDegrees = (int)NumVertsOfDegrees(degrees);

            Vector3[] verts = new Vector3[numVertsOfDegrees];
            Vector2[] uvs = new Vector2[numVertsOfDegrees];

            // In the beginning we set up for everything we’ll need later. We get an array of Vector3(3 floats) to use for every point as well as arrays for uv coordinates and triangles.
            // The first vert is in the center of the triangle  
            verts[0] = Vector3.zero;
            uvs[0] = new Vector2(0.5f, 0.5f);

            // The number of pieces of pie (triangle) is equal to the number of vertices - 1.
            float angle = degrees / (float)(numVertsOfDegrees - 1);

            // create all of the verts in the outer circle by casting a ray out from the center
            for (int i = 0; i < numVertsOfDegrees; ++i) {
                Vector3 upVector = new Vector3(0, radius, 0);
                verts[i] = Quaternion.AngleAxis(angle * (float)(i - 1), Vector3.back) * upVector;
                float normedHorizontal = (verts[i].x + 1.0f) * 0.5f;
                float normedVertical = normedHorizontal;
                uvs[i] = new Vector2(normedHorizontal, normedVertical);
            }

            Arc arc = new Arc();
            arc.verts = verts;
            arc.uvs = uvs;
            return arc;
        }
    }
}
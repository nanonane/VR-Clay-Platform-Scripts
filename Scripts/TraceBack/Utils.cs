using System.Collections;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;

namespace Utils
{

    class MeshImporter
    {
        private static string ObjDir = "F:\\UnityProjects\\HoloLensClient\\Assets\\ClayModels\\Rabbit\\";
        public Mesh importModel(string objName)
        {
            string objData = File.ReadAllText(ObjDir + objName + ".obj");
            Mesh mesh = new Mesh();
            mesh.name = "ImportedObjMesh";

            // Lists to store vertices, uvs, and normals data
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            // Split the OBJ data by lines
            string[] lines = objData.Split('\n');

            foreach (string line in lines)
            {
                // Split each line by whitespace
                string[] elements = line.Trim().Split(' ');

                if (elements.Length == 0)
                    continue;

                // Parse vertex positions
                if (elements[0] == "v" && elements.Length >= 4)
                {
                    float x = float.Parse(elements[1]);
                    float y = float.Parse(elements[2]);
                    float z = float.Parse(elements[3]);
                    vertices.Add(new Vector3(x, y, z));
                }
                // Parse face data
                else if (elements[0] == "f" && elements.Length >= 4)
                {
                    // Each face can be a triangle or a polygon
                    // We assume all faces are triangles in this example
                    // For more complex models, you may need to handle polygons
                    for (int i = 1; i <= 3; i++)
                    {
                        string indexStr = "";
                        for (int j = 0; j < elements[i].Length; j++)
                        {
                            if (elements[i][j] == '/')
                                break;
                            indexStr += elements[i][j];
                        }
                        int vertexIndex = int.Parse(indexStr) - 1;
                        triangles.Add(vertexIndex);
                    }
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();

            // 法向
            Vector3[] vs = mesh.vertices;
            int[] ts = mesh.triangles;

            Vector3[] normals = new Vector3[vs.Length];
            for (int i = 0; i < ts.Length; i += 3)
            {
                int index1 = triangles[i];
                int index2 = triangles[i + 1];
                int index3 = triangles[i + 2];

                Vector3 side1 = vertices[index2] - vertices[index1];
                Vector3 side2 = vertices[index3] - vertices[index1];
                Vector3 normal = Vector3.Cross(side1, side2).normalized;

                normals[index1] += normal;
                normals[index2] += normal;
                normals[index3] += normal;
            }

            for (int i = 0; i < normals.Length; i++)
            {
                normals[i].Normalize();
            }

            mesh.normals = normals;
            // 法向生成完毕

            return mesh;
        }

    }

}

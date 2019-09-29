using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace V.GrassSystem
{
    [CustomEditor(typeof(TerrainGrassPainter))]
    public class TerrainGrassPainterEditor : Editor
    {

        TerrainGrassPainter painter;

        Material grassEditorMat;
        Mesh terrainEditorMesh;
        GameObject projectionObj;
        

        private void OnEnable()
        {
            //Check If We can find the GrassEditor Shader......
            Shader grassEditorShader = Shader.Find("V/GrassEditor");
            if (grassEditorShader)
            {
                grassEditorMat = new Material(grassEditorShader);
            }
            else
            {
                return;
            }




            painter = target as TerrainGrassPainter;
            terrainEditorMesh = PlaneMash(65);
            terrainEditorMesh.hideFlags = HideFlags.HideAndDontSave;

            projectionObj = new GameObject();
            projectionObj.hideFlags = HideFlags.HideAndDontSave;
            MeshRenderer mr = projectionObj.AddComponent<MeshRenderer>();
            MeshFilter mf = projectionObj.AddComponent<MeshFilter>();
            mf.sharedMesh = terrainEditorMesh;

       

            
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
        }

        public void OnSceneGUI()
        {
            if (painter)
            {
                if (painter.mode == TerrainGrassPainter.Mode.Editing)
                {
                    
                }
            }
        }

        public static Mesh PlaneMash(int numberOfRows)
        {

            float botLeftX = (numberOfRows - 1) / -2f;
            float botLeftZ = (numberOfRows - 1) / -2f;

            Mesh meshyMcMeshFace = new Mesh();
            Vector3[] verts = new Vector3[numberOfRows * numberOfRows];
            Vector2[] uvs = new Vector2[numberOfRows * numberOfRows];

            int numSuqares = numberOfRows - 1;
            int[] tris = new int[numSuqares * numSuqares * 2 * 3];//trianges

            int i = 0;
            int t = 0;

            for (float x = 0; x < numberOfRows; ++x)
            {
                for (float z = 0; z < numberOfRows; ++z)
                {                                         // unity plane default vertices is 11 vertices. 10 squre
                    verts[i].x = botLeftX + x;
                    verts[i].y = 0;
                    verts[i].z = botLeftZ + z; //Based on the percentgy

                    uvs[i].x = (float)x / (numberOfRows - 1);
                    uvs[i].y = (float)z / (numberOfRows - 1);


                    if (x == numberOfRows - 1 || z == numberOfRows - 1)
                    {
                        ++i;
                        continue;
                    }

                    tris[t] = i;
                    tris[t + 1] = i + 1;
                    tris[t + 2] = i + numberOfRows + 1;

                    tris[t + 3] = i;
                    tris[t + 4] = i + numberOfRows + 1;
                    tris[t + 5] = i + numberOfRows;

                    t += 6;
                    ++i;
                }
            }
            meshyMcMeshFace.name = "The Generated M.D.";
            meshyMcMeshFace.vertices = verts;
            meshyMcMeshFace.uv = uvs;
            meshyMcMeshFace.triangles = tris;
            meshyMcMeshFace.RecalculateBounds();
            meshyMcMeshFace.RecalculateNormals();
            return meshyMcMeshFace;
        }

    }
}

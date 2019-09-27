using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace V.GrassSystem
{
    [CustomEditor(typeof(GrassManager))]
    public class GrassManagerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GrassManager grassManager = (GrassManager)target;
            if (GUILayout.Button("Create Tile"))
            {
                grassManager.CreateTile();
            }
            if (GUILayout.Button("Update Mesh"))
            {
                grassManager.UpdateMesh();
            }
            if (GUILayout.Button("Update Material"))
            {
                grassManager.UpdateMaterials();
            }


            GUILayout.Space(50);


            if (GUILayout.Button("CombineMesh"))
            {
                grassManager.CreateCombinedMesh();
            }

            if (GUILayout.Button("Save Mesh"))
            {
                string path = EditorUtility.SaveFolderPanel("Save Mesh", "", "");
                path = ToUnity(path);
                SaveMesh(grassManager.combinedGrassMesh, path + "/" + "Grass_lod0.asset");
                //SaveMesh(grassManager.combinedMesh_LOD1, path + "/" + "Grass_lod1.asset");
                //SaveMesh(grassManager.combinedMesh_LOD2, path + "/" + "Grass_lod2.asset");
                AssetDatabase.Refresh();
            }
        }

        void SaveMesh(Mesh mesh, string path)
        {
            if (mesh == null)
            {
                Debug.Log("Combined Mesh is Null");
                return;
            }
            AssetDatabase.CreateAsset(mesh, path);
            AssetDatabase.SaveAssets();
        }

        public static string ToUnity(string path)
        {
            path = path.Replace('\\', '/');
            string workSpace = System.IO.Path.GetFullPath("Assets").Replace('\\', '/');
            if (path.Contains(workSpace))
                return "Assets" + path.Substring(workSpace.Length);
            else
                return null;
        }



    }
}

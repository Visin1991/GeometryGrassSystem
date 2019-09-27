using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V.GrassSystem
{
    public static class GrassMeshGenerator
    {
        public static Mesh CreateGrassMesh(Mesh sourceMeshe_LOD0, int instanceCound,int chunkSize,float size_min,float size_max,float height_min,float height_max)
        {
            if (sourceMeshe_LOD0 == null) { return null; }
            List<Mesh> meshList_LOD0 = new List<Mesh>();

            List<Vector3> positions = new List<Vector3>();
            List<Quaternion> rotations = new List<Quaternion>();
            List<Vector3> scales = new List<Vector3>();

            for (int i = 0; i < instanceCound; i++)
            {
                Mesh newMesh_LOD0 = new Mesh();
                newMesh_LOD0.vertices = sourceMeshe_LOD0.vertices;
                newMesh_LOD0.triangles = sourceMeshe_LOD0.triangles;

                //Rand TRS
                Vector3 pos = new Vector3(Random.Range(-(float)chunkSize * 0.5f, (float)chunkSize * 0.5f), 0, Random.Range(-(float)chunkSize * 0.5f, (float)chunkSize * 0.5f));
                Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                float rangeSize = Random.Range(size_min, size_max);
                float rangeHeight = Random.Range(height_min, height_max);
                Vector3 scale = new Vector3(rangeSize, rangeHeight, rangeSize);

                positions.Add(pos);
                rotations.Add(rotation);
                scales.Add(scale);

                //UV stuff............
                float randSeed = Random.Range(-180, 180f);
                List<Vector4> newUVS0 = new List<Vector4>();

                for (int k = 0; k < sourceMeshe_LOD0.uv.Length; k++)
                {
                    Vector4 newUV = new Vector4(randSeed, sourceMeshe_LOD0.uv[k].y, pos.x, pos.z);
                    newUVS0.Add(newUV);
                }

                //Set UV
                newMesh_LOD0.SetUVs(0, newUVS0);

                //Add To List
                meshList_LOD0.Add(newMesh_LOD0);
            }

            Mesh combinedMesh_LOD0 = CombineMeshes(meshList_LOD0,scales,rotations,positions, 1, 1);
            return combinedMesh_LOD0;
        }

        private static Mesh CombineMeshes(List<Mesh> meshes,List<Vector3> scales,List<Quaternion> rotations,List<Vector3> positions,int step = 1, float _xzScale = 1)
        {
            var combine = new CombineInstance[meshes.Count];
            for (int i = 0; i < meshes.Count; i += step)
            {
                combine[i].mesh = meshes[i];
                Vector3 scale = new Vector3(scales[i].x * _xzScale, scales[i].y, scales[i].z * _xzScale);
                combine[i].transform = Matrix4x4.TRS(positions[i], rotations[i], scale);
            }
            var mesh = new Mesh();
            mesh.CombineMeshes(combine);
            mesh.name = "Combined Mesh";
            return mesh;
        }
    }
}

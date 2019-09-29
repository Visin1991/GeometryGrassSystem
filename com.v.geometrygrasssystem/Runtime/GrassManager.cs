using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V.GrassSystem
{
    public class GrassManager : MonoBehaviour
    {
        [Header("---------------------------RunTime----------------")]
        public Terrain terrain;

        public float terrinSize = 512;
        public float terrainHeight = 1200;
        public Transform followTarget;
        public Material grassMat;
        public Mesh combinedGrassMesh;

        public bool showGrid = false;
        public Vector3 terrainPosition = Vector3.zero;

        Vector3 prePos;


        MaterialPropertyBlock m_materialPropertyBlock;
        MaterialPropertyBlock MaterialPropertyBlock
        {
            get {
                if (m_materialPropertyBlock == null)
                {
                    m_materialPropertyBlock = new MaterialPropertyBlock();
                }
                return m_materialPropertyBlock;
            }
        }

        
        public enum ChunkSize
        {
            _4x4 = 2 << 1,
            _8x8 = 2 << 2,
            _16x16 = 2 << 3,
            _32x32 = 2 << 4,
            _64x64 = 2 << 5
        }


        [Header("--------------------------Design----------------")]
        public int lod0Tile_size = 3;
        public int instanceCount = 500;
        public ChunkSize chunkSize = ChunkSize._8x8;
        public float size_min = 0.8f;
        public float size_max = 1.3f;
        public float height_min = 0.8f;
        public float height_max = 1.3f;
        public Mesh sourceMesh;   

        float updateTime = 1;

        private void Start()
        {
            if (terrain)
            {
                TerrainData data = terrain.terrainData;
 
                terrainHeight = data.size.y;

                terrinSize = data.size.x;
                terrainPosition = terrain.transform.position;

                if (grassMat)
                {
                    grassMat.SetFloat("_HeightMultiplier", terrainHeight);
                }

                SetHeightMap();
            }

            Update();
        }

        private void Update()
        {
            updateTime -= Time.deltaTime;
            if (updateTime >= 0) { return; }
            updateTime = 1;

            if (followTarget == null)
            {
                return;
            }

            Vector3 targetPosition = transform.position;


            if (followTarget)
            {
                targetPosition = followTarget.position;
            }

            Vector3 reletivePos = targetPosition - terrainPosition;

            float xID = Mathf.CeilToInt(reletivePos.x/ (float)chunkSize);
            float zID = Mathf.CeilToInt(reletivePos.z/ (float)chunkSize);
            float xPos = (xID - 0.5f) * (float)chunkSize;
            float zPos = (zID - 0.5f) * (float)chunkSize;

            transform.position = new Vector3(xPos, transform.position.y, zPos) + terrainPosition;

            if (prePos != transform.position)
            {
                prePos = transform.position;

                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    Vector3 pos = child.position;
                    pos -= terrainPosition;

                    float tileIDX = Mathf.FloorToInt(pos.x / (float)chunkSize);
                    float tileIDZ = Mathf.FloorToInt(pos.z / (float)chunkSize);

                    float scale = (float)chunkSize / terrinSize;
                    float xOffset = scale * tileIDX;
                    float yOffset = scale * tileIDZ;

                    //This is for Debug...........
#if UNITY_EDITOR
                    GrassTile tile = child.GetComponent<GrassTile>();
                    tile.scaleOffset.z = tile.scaleOffset.w = scale;
                    tile.scaleOffset.x = xOffset;
                    tile.scaleOffset.y = yOffset;

                    tile.tileID.x = tileIDX;
                    tile.tileID.y = tileIDZ;
#endif
                    //......................................

                    MeshRenderer mr = child.GetComponent<MeshRenderer>();
                    if (mr)
                    {
                        MaterialPropertyBlock.SetVector("_UVOffset", new Vector4(xOffset, yOffset, scale, scale));
                        mr.SetPropertyBlock(MaterialPropertyBlock);
                    }
                }
            }
        }
        public void CreateTile()
        {  
            for (int x = 0; x < lod0Tile_size; x++)
            {
                for (int z = 0; z < lod0Tile_size; z++)
                {
                    GameObject tile0 = new GameObject("X_" + x + "__|__Z__" + z);

                    float posX = x * 8;
                    float posZ = z * 8;

                    posX -= Mathf.Floor(lod0Tile_size * 0.5f) * 8;
                    posZ -= Mathf.Floor(lod0Tile_size * 0.5f) * 8;

                    tile0.AddComponent<GrassTile>();

                    tile0.transform.SetParent(transform);


                    tile0.transform.localPosition = new Vector3(posX, 0, posZ);
                    MeshRenderer mr =  tile0.AddComponent<MeshRenderer>();
                    mr.sharedMaterial = grassMat;

                    MeshFilter mf =  tile0.AddComponent<MeshFilter>();
                    mf.sharedMesh = combinedGrassMesh;

                   
                }
            }
        }

        [ContextMenu("Create Combied Mesh")]
        public void CreateCombinedMesh()
        {
            combinedGrassMesh = GrassMeshGenerator.CreateGrassMesh(sourceMesh, instanceCount, (int)chunkSize , size_min, size_max, height_min, height_max);
        }

        public void UpdateMesh()
        {        
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                 MeshFilter mf = transform.GetChild(i).GetComponent<MeshFilter>();
                if (mf)
                {
                    mf.sharedMesh = combinedGrassMesh;
                }            
            }
        }

        public void UpdateMaterials()
        {
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                MeshRenderer mr = transform.GetChild(i).GetComponent<MeshRenderer>();
                if (mr)
                {
                    mr.sharedMaterial = grassMat;
                }
            }
        }

        public void SetHeightMap(RenderTexture renderTexture)
        {
            if (grassMat)
            {
                grassMat.SetTexture("_HeightMap", renderTexture);
            }
        }

        private void OnDrawGizmos()
        {
            if (!showGrid) return;

            Color s_Color = Gizmos.color;
            Gizmos.color = Color.red;
            int childCount = transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Vector3 pos = transform.GetChild(i).transform.position;
                if (followTarget) { pos.y = followTarget.position.y; }
                Gizmos.DrawWireCube(pos, new Vector3((float)chunkSize, 800.0f, (float)chunkSize));
            }
            Gizmos.color = s_Color;
        }

        public void SetHeightMap()
        {
            if (terrain)
            {
                TerrainData terrainData = terrain.terrainData;
                if (terrainData.heightmapTexture)
                {
                    SetHeightMap(terrainData.heightmapTexture);
                }
            }
        }

    }

}
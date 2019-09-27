using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTile : MonoBehaviour
{
    public Vector2 tileID =Vector2.one;
    public Vector4 scaleOffset = Vector4.one;
    private void OnEnable()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf)
        {
            Bounds bs = mf.sharedMesh.bounds;
            bs.size = new Vector3(bs.size.x, 2000.0f, bs.size.z);
            mf.sharedMesh.bounds = bs;
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Color s_Color = Gizmos.color;
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(transform.position, new Vector3(8, 100, 8));
    //    Gizmos.color = s_Color;
    //}
}

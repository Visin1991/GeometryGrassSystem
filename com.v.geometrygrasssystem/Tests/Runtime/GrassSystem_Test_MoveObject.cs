using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSystem_Test_MoveObject : MonoBehaviour
{
    // Update is called once per frame
    Vector3 pos;

    private void Start()
    {
        pos = transform.position;
    }

    void Update()
    {
        
        float x = Mathf.Sin(Time.time * 1/(10.0f)) * 128;
        float z = Mathf.Cos(Time.time * 1 / (8.0f)) * 88;
        transform.position = pos + new Vector3(x,0,z);
    }
}

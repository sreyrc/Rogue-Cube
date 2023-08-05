using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class LookWorldForward : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Quaternion rotation = new Quaternion();
        rotation.SetLookRotation(Vector3.forward, Vector3.up);
        transform.rotation = rotation;
    }
}

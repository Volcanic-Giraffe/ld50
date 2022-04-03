using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legs : MonoBehaviour
{
    private Rigidbody _rig;
    
    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rig.velocity = Vector3.zero;
        _rig.angularVelocity = Vector3.zero;
        
        transform.rotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
    }
}

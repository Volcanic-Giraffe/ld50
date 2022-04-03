using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg : MonoBehaviour
{
    private Rigidbody _rig;

    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var vel = _rig.velocity;

        _rig.velocity = new Vector3(Mathf.Min(vel.x, 10f), Mathf.Min(vel.y, 10f), Mathf.Min(vel.z, 10f));
        
        
        var rot = _rig.angularVelocity;

        _rig.angularVelocity = new Vector3(Mathf.Min(rot.x, 10f), Mathf.Min(rot.y, 10f), Mathf.Min(rot.z, 10f));
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Pan : MonoBehaviour
{
    private void Update()
    {

        if (Input.GetButtonDown("Jump"))
        {
            var rig = GetComponent<Rigidbody>();
            
            rig.AddForce(Vector3.up*1000f, ForceMode.Impulse);
        }
    }
}

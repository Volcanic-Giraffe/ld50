using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BpongRacket : MonoBehaviour
{
    [SerializeField] private float inputForce;
    
    private Rigidbody _rig;

    // private float rotY = 0.0f; // rotation around the up/y axis
    // private float rotX = 0.0f; // rotation around the right/x axis
    
    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        // Debug.Log("### mxy: " + mouseX + " " + mouseY);
        if (Input.GetMouseButtonDown(0))
        {
            _rig.AddTorque(new Vector3(0,0,inputForce), ForceMode.Impulse);
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            
            _rig.AddTorque(new Vector3(0,0,-inputForce), ForceMode.Impulse);
        }
    }
}

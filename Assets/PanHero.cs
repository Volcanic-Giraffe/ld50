using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanHero : MonoBehaviour
{
    public float Speed = 5f;
    public float JumpHeight = 2f;
    public float GroundDistance = 0.2f;
    public float DashDistance = 5f;
    
    private Rigidbody _body;
    private Vector3 _inputs = Vector3.zero;
    private bool _isGrounded = true;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _inputs = Vector3.zero;
        _inputs.x = Input.GetAxis("Horizontal");
        _inputs.z = Input.GetAxis("Vertical");
        if (_inputs != Vector3.zero)
            transform.forward = _inputs;

       
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            // TODO move to fixedUpdate
            _body.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            // TODO move to fixedUpdate
            Vector3 dashVelocity = Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime)));
            _body.AddForce(dashVelocity, ForceMode.VelocityChange);
        }

    }

  

    void FixedUpdate()
    {
        // Cast 4 rays, get 4 hits, calculate normal from plane
        //RaycastHit lr, rr, lf, rf;
        //Physics.Raycast(_body.position + Vector3.up - Vector3.right, Vector3.down, out lr);
        //Physics.Raycast(_body.position + Vector3.up + Vector3.right, Vector3.down, out rr);
        //Physics.Raycast(_body.position + Vector3.up - Vector3.forward, Vector3.down, out lf);
        //Physics.Raycast(_body.position + Vector3.up + Vector3.forward, Vector3.down, out rf);
        //var upDir = (Vector3.Cross(rr.point - Vector3.up, lr.point - Vector3.up) +
        //         Vector3.Cross(lr.point - Vector3.up, lf.point - Vector3.up) +
        //         Vector3.Cross(lf.point - Vector3.up, rf.point - Vector3.up) +
        //         Vector3.Cross(rf.point - Vector3.up, rr.point - Vector3.up)
        //        ).normalized;

        //transform.up = upDir;

        var ray = new Ray(transform.position, -transform.up); // cast ray downwards
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            _isGrounded = hit.distance <= GroundDistance;
            transform.up = hit.normal;
        }
        // TODO does not work
        var r = _body.rotation;
        _body.MovePosition(_body.position + _inputs * Speed * Time.fixedDeltaTime);
        _body.rotation = r;

    }
}
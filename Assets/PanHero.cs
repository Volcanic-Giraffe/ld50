using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanHero : MonoBehaviour
{
    [SerializeField] private Transform handAnchor;
    
    public float Speed = 5f;
    public float JumpHeight = 2f;
    public float GroundDistance = 0.2f;
    public float DashDistance = 5f;
    
    private Rigidbody _body;
    private bool _isGrounded = true;

    private Weapon _weapon;

    private void Awake()
    {
        _weapon = GetComponentInChildren<Weapon>();
    }

    void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    public void LookAt(Vector3 target)
    {
        if (target != Vector3.zero) transform.forward = target;
    }

    public void Jump()
    {
        if (_isGrounded)
        {
            // TODO move to fixedUpdate
            _body.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }
    }
    
    public void Dash()
    {
        
        // TODO move to fixedUpdate
        Vector3 dashVelocity = Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime)));
        _body.AddForce(dashVelocity, ForceMode.VelocityChange);
    }

    public void HoldTrigger()
    {
        if (_weapon != null)
        {
            _weapon.HoldTrigger();
        }
    }

    public void ReleaseTrigger()
    {
        if (_weapon != null)
        {
            _weapon.ReleaseTrigger();
        }
    }

    public void AimAt(Vector3 target)
    {
        var dir = target - handAnchor.transform.position;

        handAnchor.transform.right = dir;
            
        _weapon.AimAt(target);
    }
    
    void Update()
    {


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

    }

    public void FixedMove(Vector3 inputs)
    {
        // TODO does not work
        var r = _body.rotation;
        _body.MovePosition(_body.position + inputs * (Speed * Time.fixedDeltaTime));
        _body.rotation = r;
    }
}
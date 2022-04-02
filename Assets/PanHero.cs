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
        //if (target != Vector3.zero) transform.forward = target;
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
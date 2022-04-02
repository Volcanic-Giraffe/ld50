using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float downceleration;
    private Rigidbody _rig;
    private float _movX;
    private bool _jump;
    private bool _floating;

    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
    }

    void Start()
    {
    }

    void Update()
    {
        RaycastHit hit;
        _floating = !Physics.Raycast(transform.position, -Vector3.up, out hit, 0.55f, ~LayerMask.GetMask("Hero"));
        _movX = Input.GetAxisRaw("Horizontal");
        if (!_floating)
        {
            _jump = Input.GetButton("Jump");
        }
    }

    private void FixedUpdate()
    {
        _rig.AddForce(_movX * speed * Time.fixedDeltaTime, 0, 0, ForceMode.Impulse);
        if (_floating)
        {
            _rig.AddForce(-transform.up * downceleration * (_rig.velocity.y > 0 ? 0.5f : 1f), ForceMode.Acceleration);
        }
        if (_jump)
        {
            _rig.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            _jump = false;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BpongHero : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody _rig;
    
    private float _movX;

    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
         _movX = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        _rig.AddForce(_movX * speed * Time.fixedDeltaTime, 0, 0, ForceMode.Impulse);
    }
}

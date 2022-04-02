using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BpongHeroHitter : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float smackForce;
    [SerializeField] private float downceleration;
    [SerializeField] GameObject SmackEffect;
    private Rigidbody _rig;
    private float _movX;
    private bool _jump;
    private bool _floating;
    private bool _smack;
    private float _smackCD;
    private float _smackRadius;

    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _smackRadius = SmackEffect.GetComponent<SphereCollider>().radius;
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
        if (_smackCD > 0) _smackCD -= Time.deltaTime;
        if(_smackCD <=0 && Input.GetButton("Fire1"))
        {
            _smack = true;
            _smackCD = 0.5f;
        }
        SmackEffect.SetActive(_smack);
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
        if(_smack)
        {
            var set = new HashSet<Rigidbody>();
            var hits = Physics.SphereCastAll(transform.position, 2f, transform.up, 0.001f, LayerMask.GetMask("Objects"));
            foreach(var rcHit in hits)
            {
                if (!set.Contains(rcHit.rigidbody)) {
                    set.Add(rcHit.rigidbody);
                    rcHit.rigidbody.AddForce(smackForce * (rcHit.rigidbody.position - transform.position), ForceMode.Impulse);
                }
            }
            Debug.Log(hits.Length);
            _smack = false;
        }
    }
}

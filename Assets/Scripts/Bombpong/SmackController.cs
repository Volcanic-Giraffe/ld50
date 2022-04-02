using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmackController : MonoBehaviour
{
    [SerializeField] private float smackForce;
    [SerializeField] GameObject SmackEffect;
    private Rigidbody _rig;
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
            _smack = false;
        }
    }
}

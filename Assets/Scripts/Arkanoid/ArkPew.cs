using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArkPew : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody _rig;

    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ArkBlock"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    public void Shoot(Vector3 aimerPos)
    {
        _rig.AddForce((aimerPos - transform.position) * speed);
    }
}

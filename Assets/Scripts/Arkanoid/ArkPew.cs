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
            var block = other.gameObject.GetComponent<ArkBlock>();

            if (block != null && !block.IsSolid)
            {
                Destroy(other.gameObject);
            }
            
            Destroy(gameObject);
        }
    }

    public void Shoot(Vector3 aimerPos, float chargeTime)
    {
        _rig.AddForce((aimerPos - transform.position).normalized * (speed * chargeTime));
    }
}

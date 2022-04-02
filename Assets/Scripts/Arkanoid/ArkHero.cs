using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArkHero : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private GameObject ArkPewGO;
    
    private Rigidbody _rig;
    
    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
    }

    public void PewJump(Vector3 aimerPos)
    {
        _rig.AddForce((transform.position - aimerPos) * jumpForce);

        var pew = Instantiate(ArkPewGO);

        pew.transform.localPosition = transform.localPosition;

        var pewComp = pew.GetComponent<ArkPew>();
        pewComp.Shoot(aimerPos);
    }
}

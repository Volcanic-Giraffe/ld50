using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Pan : MonoBehaviour
{
    [SerializeField] private PanHeat heat;

    public PanHeat PanHeat => heat;
    private float intensity = 0.5f;
    public void IncreaseHeat()
    {
        PanHeat.SetRadius(PanHeat.Radius + 2);
        PanHeat.SetGlow(intensity);
        intensity += 0.5f;
        PanHeat.transform.DOMoveY(PanHeat.transform.position.y + 1, 2f);
    }
    
    private void Update()
    {

        if (Input.GetButtonDown("Jump"))
        {
            var rig = GetComponent<Rigidbody>();
            
            rig.AddForce(Vector3.up*1000f, ForceMode.Impulse);
        }
    }
}

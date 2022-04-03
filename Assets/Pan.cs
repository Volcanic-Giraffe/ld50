using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Pan : MonoBehaviour
{
    [SerializeField] private PanHeat heat;
    private float _originY;

    public PanHeat PanHeat => heat;
    private float intensity = 0.5f;
    
    private Rigidbody _rig;


    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
        _originY = transform.position.y;
    }
    
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
            PanFlip();
        }
    }

    public void PanFlip()
    {
        _rig.DOMoveY(_originY+ 3f, 1.4f).SetEase(Ease.InBack).OnComplete(
            () =>
            {
                _rig.DOMoveY(_originY, 0.7f).SetEase(Ease.OutBack);
            });
    }
}

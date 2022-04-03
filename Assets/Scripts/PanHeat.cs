using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class PanHeat : MonoBehaviour
{
    [SerializeField] private List<Transform> wobbles;

    [SerializeField] private Transform parentCylinder;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private MeshRenderer pan;
    public float Radius => _targetRadius;

    public float RadiusMax => 29f / 2; // matches pan size

    private float _targetRadius;
    
    private void Start()
    {
        foreach (var wobble in wobbles)
        {
            wobble.transform.rotation = Quaternion.Euler(-3,0,0);
            wobble.transform.DORotate(new Vector3(3,0,0), Random.Range(5f, 7f)).SetLoops(-1, LoopType.Yoyo);
        }
        
        _targetRadius = parentCylinder.transform.localScale.x / 2f;
    }

    public void SetRadius(float radius)
    {
        if (radius > RadiusMax) radius = RadiusMax;
        
        var psShape = particles.shape;
        psShape.radius = radius * 0.6f;

        parentCylinder.transform.DOKill();
        parentCylinder.transform.DOScale(new Vector3(radius * 2f, 0.1f, radius * 2f),3f);

        _targetRadius = radius;
    }

    public void SetGlow(float intensity)
    {
        var color = pan.material.GetColor("_EmissionColor");
        pan.material.SetColor("_EmissionColor", color * intensity);
    }
}

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

    [SerializeField] private GameObject heatEffectGO;
    [SerializeField] private Transform effectsContainer;
    
    public float Radius => _targetRadius;

    public float RadiusMax => 29f / 2; // matches pan size

    private float _targetRadius;

    private Dictionary<int, HeatEffect> _effects; // effect by target object ID

    private void Awake()
    {
        _effects = new Dictionary<int, HeatEffect>();
    }

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

    private void OnTriggerEnter(Collider other)
    {
        var id = other.gameObject.GetInstanceID();

        var bounds = other.bounds.size;

        if (bounds.x + bounds.y + bounds.z < 2f)
        {
            //too small
            return;
        }
        
        if (!_effects.ContainsKey(id))
        {
            var effectObj = Instantiate(heatEffectGO, effectsContainer);
            var effect = effectObj.GetComponent<HeatEffect>();
            effect.Setup(other.gameObject.transform, effectsContainer.transform, other.bounds.size);
            
            _effects.Add(id, effect);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        var id = other.gameObject.GetInstanceID();

        if (_effects.ContainsKey(id))
        {
            var effect = _effects[id];
            effect.Stop();
            
            _effects.Remove(id);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particles;

    private Transform _target;
    private Transform _transform;

    private Transform _surface;
    
    private bool _stopped;

    public void Setup(Transform target, Transform surface, Vector3 size)
    {
        _target = target;
        _surface = surface;

        if (size.x > 2 || size.y > 2 && size.z > 2)
        {
            var pShape = _particles.shape;
            pShape.radius *= 2f;
            
            // var pEm = _particles.emission;
            // var pRate = pEm.rateOverTime;
            // pRate.constant *= 0.5f;
        } 
    }
    
    
    private void Awake()
    {
        _transform = transform;
    }

    void Update()
    {
        if (_stopped) return;
        
        if (_target == null)
        {
            Stop();
        }
        else
        {
            var tPos = _target.position;
            
            var offsetY = _surface.position.y - tPos.y;
            
            var y = Mathf.Min(_surface.transform.position.y, tPos.y + offsetY); // in case target sinks
            
            _transform.position = new Vector3(tPos.x, y, tPos.z);
        }
    }

    public void Stop()
    {
        if (_stopped) return;
        
        _stopped = true;

        _particles.Stop();
        
        Destroy(gameObject, 2f);
    }
}

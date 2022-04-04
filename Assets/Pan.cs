using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pan : MonoBehaviour
{
    public static float Radius = 16f;
    
    [SerializeField] private PanHeat heat;
    [SerializeField] private float heatIncrementY;
    [SerializeField] private float heatMaxLocalY;
    
    public PanHeat PanHeat => heat;
    private float intensity = 1.0f;
    
    private Rigidbody _rig;

    private float _originPanY;
    private float _originHeatLocalY;
    private float _targetHeatLocalY;
    
    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
        _originPanY = transform.position.y;
        _originHeatLocalY = heat.transform.localPosition.y;

        _targetHeatLocalY = _originHeatLocalY;
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.U))
        // {
        //     PanFlip();
        // }
        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     IncreaseHeat();
        // }

    }
    
    public void IncreaseHeat()
    {
        if (_targetHeatLocalY >= heatMaxLocalY) return;

        _targetHeatLocalY += heatIncrementY;
        
        PanHeat.SetRadius(PanHeat.Radius + 2);
        intensity += 0.2f;
        PanHeat.SetGlow(intensity);
        PanHeat.transform.DOLocalMoveY(_targetHeatLocalY, 2f);
    }

    public void PanFlip()
    {
        _rig.DOMoveY(_originPanY+ 3f, 1.4f).SetEase(Ease.InBack).OnComplete(
            () =>
            {
                FlipProps();
                
                _rig.DOMoveY(_originPanY, 0.7f).SetEase(Ease.OutBack);
            });
    }

    private void FlipProps()
    {
        var props =  PanLevel.Instance.Props;

        var force = 400f;

        foreach (var prop in props)
        {
            if (prop == null) continue;
            var rig = prop.GetComponent<Rigidbody>();

            if (rig != null)
            {
                rig.AddTorque(new Vector3(Random.Range(-force, force), Random.Range(-force, force), Random.Range(-force, force)));
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArkAimer : MonoBehaviour
{
    [SerializeField] private Transform chargeIndicator;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeMax;
    
    public Camera Camera;
    public ArkHero ArkHero;

    private bool _beginCharge;
    private bool _releaseCharge;

    private float _chargeTime;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _beginCharge = true;
            _chargeTime = 0;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _releaseCharge = true;
        }

        if (_beginCharge && !_releaseCharge)
        {
            _chargeTime += Time.deltaTime * chargeSpeed;
            _chargeTime = Mathf.Min(_chargeTime, chargeMax);
        }
        var scale = Mathf.Max(((_chargeTime * 3f) / chargeMax), 1f);
        chargeIndicator.localScale = Vector3.one * scale;
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

        var mask = new List<string>()
        {
            "ArkInputPane"
        };

        var raycast = Physics.Raycast(ray, out hit, 250, LayerMask.GetMask(mask.ToArray()));

        if (raycast)
        {
            transform.position = hit.point;
        }

        if (_releaseCharge)
        {
            ArkHero.ReleaseCharge(transform.position, _chargeTime);
            
            _beginCharge = false;
            _releaseCharge = false;
            _chargeTime = 0;
        }
    }
}

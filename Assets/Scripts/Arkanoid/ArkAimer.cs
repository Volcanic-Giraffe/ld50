using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArkAimer : MonoBehaviour
{
    public Camera Camera;
    public ArkHero ArkHero;

    private bool _doJump;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _doJump = true;
        }
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

        if (_doJump)
        {
            _doJump = false;
            ArkHero.PewJump(transform.position);
        }
    }
}

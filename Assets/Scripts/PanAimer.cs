using System.Collections.Generic;
using UnityEngine;

public class PanAimer : MonoBehaviour
{
    public Camera Camera;
    private int _groundMask;

    private void Awake()
    {
        if (Camera == null)
        {
            Camera = Camera.main;
        }
        _groundMask = LayerMask.GetMask(new [ ] {"Ground", "Characters"});
    }

    public Vector3 AimPoint()
    {
        return transform.position + Vector3.up * 1.5f;
    }
    private void FixedUpdate()
    {
        var ray = Camera.ScreenPointToRay(Input.mousePosition);

        var raycast = Physics.Raycast(ray, out var hit, 250, _groundMask);

        if (raycast)
        {
            transform.position = hit.point;
        }
    }
}
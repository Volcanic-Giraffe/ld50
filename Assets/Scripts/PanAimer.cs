using System.Collections.Generic;
using UnityEngine;

public class PanAimer : MonoBehaviour
{
    public Camera Camera;

    private void Awake()
    {
        if (Camera == null)
        {
            Camera = Camera.main;
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

        var mask = new List<string>()
        {
            "Ground"
        };

        var raycast = Physics.Raycast(ray, out hit, 250, LayerMask.GetMask(mask.ToArray()));

        if (raycast)
        {
            transform.position = hit.point;
        }
    }
}
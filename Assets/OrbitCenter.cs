using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCenter : MonoBehaviour
{
    [SerializeField] GameObject Target;
    void Start()
    {
        //_offset = transform.position -  
    }

    void Update()
    {
        if (Target != null)
        {
            var angle = Target.transform.position.AngleOffAroundAxis(transform.position, Vector3.up);
            if(Mathf.Abs(angle) > 20f) transform.RotateAround(Vector3.zero, Vector3.up, Mathf.Lerp(0, angle,Time.deltaTime));
        }
    }
}

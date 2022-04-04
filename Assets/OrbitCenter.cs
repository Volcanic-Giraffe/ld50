using DG.Tweening;
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
        if (Target != null && PanLevel.Instance.Started)
        {
            var angle = Target.transform.position.AngleOffAroundAxis(transform.position, Vector3.up);
            if(Mathf.Abs(angle) > 20f) transform.RotateAround(Vector3.zero, Vector3.up, Mathf.Lerp(0, angle,Time.deltaTime));
        }
    }
}

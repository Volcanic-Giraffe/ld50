using System;
using UnityEngine;

public class ArkDeathRay : MonoBehaviour
{
    [SerializeField] private float speed;

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        transform.localPosition += Vector3.down *( speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}

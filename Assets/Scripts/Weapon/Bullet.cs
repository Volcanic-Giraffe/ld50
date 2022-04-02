using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Altitude;
    
    public BulletConfig Config { get; private set; }

    private Rigidbody _rig;

    private float _speed;

    private int _floorMask;
    
    private void Awake()
    {
        _floorMask = LayerMask.GetMask("Ground");

        _rig = GetComponent<Rigidbody>();
    }

    public void Setup(BulletConfig config)
    {
        Config = config;

        _speed = config.Speed;
    }

    private void FixedUpdate()
    {
        var newPos = transform.position + (transform.forward * (Time.fixedDeltaTime * _speed));

        var raycast = Physics.Raycast(newPos, -Vector3.up, out var hit, 10f, _floorMask);

        if (raycast)
        {
            _rig.MovePosition(hit.point + Vector3.up * Altitude);
            
            // Debug.DrawLine (newPos, hit.point, Color.red);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var dmg = other.gameObject.GetComponent<Damageable>();

        if (dmg == null)
        {
            dmg = other.gameObject.GetComponentInParent<Damageable>();
        }
        
        if (dmg != null)
        {
            dmg.Hit(gameObject, Config.Damage);
        }
        
        Destroy(gameObject);
    }
}

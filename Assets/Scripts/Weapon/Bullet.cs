using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletConfig Config { get; private set; }

    private Rigidbody _rig;

    private float _speed;
    
    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
    }

    public void Setup(BulletConfig config)
    {
        Config = config;

        _speed = config.Speed;
    }

    private void FixedUpdate()
    {
        _rig.velocity = transform.forward * (_speed * Time.fixedDeltaTime);
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

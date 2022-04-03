using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Altitude;
    public GameObject ImpactGO;
    
    public BulletConfig Config { get; private set; }
    
    public int Team { get; set; }

    private Rigidbody _rig;

    private float _speed;

    private int _floorMask;
    
    private void Awake()
    {
        _floorMask = LayerMask.GetMask("Ground");

        _rig = GetComponent<Rigidbody>();
    }

    public void Setup(BulletConfig config, int team)
    {
        Team = team;

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
        
        if (dmg != null && CanAttack(Team, dmg.Team))
        {
            dmg.Hit(gameObject, Config.Damage);

            if (ImpactGO != null)
            {
                Instantiate(ImpactGO, transform.position, Quaternion.identity);
            }
        }
        
        Destroy(gameObject);
    }

    private bool CanAttack(int teamA, int teamB)
    {
        return teamB == 0 || teamA != teamB;
    }
}

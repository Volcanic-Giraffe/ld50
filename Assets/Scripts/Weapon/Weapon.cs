using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private WeaponConfig config;
    
    private bool _triggerPressed;

    private float _fireTimer;
    private float _reloadTimer;

    private int _bulletsInClip;

    public event Action<int, int> OnClipUpdated;

    public WeaponConfig Config => config;
    public bool IsFullClip => _bulletsInClip == config.ClipSize;
    public bool IsReloading => _reloadTimer > 0;
    
    public int BulletsInClip => _bulletsInClip;

    private void Start()
    {
        SetBulletsInClip(config.ClipSize);
    }

    private void Update()
    {
        if (_reloadTimer > 0)
        {
            _reloadTimer -= Time.deltaTime;
            if (_reloadTimer <= 0)
            {
                SetBulletsInClip(config.ClipSize);
            }
        }
        
        UpdateFiring();
    }

    private void UpdateFiring()
    {
        if (_fireTimer > 0)
        {
            _fireTimer -= Time.deltaTime;
        }

        if (_fireTimer <= 0 && _triggerPressed && !IsReloading)
        {
            _fireTimer += config.FireDelay;

            FireOnce();
        }
    }

    private void FireOnce()
    {
        if (_bulletsInClip <= 0) return;
        
        for (int i = 0; i < config.Pellets; i++)
        {
            var pos = muzzle.transform.position;
            var rot = muzzle.transform.rotation * Quaternion.Euler(0, 0, Random.Range(-config.Spread, config.Spread));

            PanLevel.Instance.SpawnBullet(config.BulletConfig, pos, rot);
        }
        
        SetBulletsInClip(_bulletsInClip - 1);
    }

    public void HoldTrigger()
    {
        _triggerPressed = true;
    }

    public void ReleaseTrigger()
    {
        _triggerPressed = false;
    }

    public void StartReloading()
    {
        if (IsFullClip) return;
        if (IsReloading) return;

        _reloadTimer = config.ReloadTime;
    }

    private void SetBulletsInClip(int bullets)
    {
        if (bullets > config.ClipSize) bullets = config.ClipSize;
        
        _bulletsInClip = bullets;

        if (bullets == 0) StartReloading();
        
        OnClipUpdated?.Invoke(bullets, config.ClipSize);
    }
}

[Serializable]
public class WeaponConfig
{
    public string Title;
    public float FireDelay;
    public float Spread;
    public int Pellets;

    public float ReloadTime;
    public int ClipSize;

    public BulletConfig BulletConfig;
}

[Serializable]
public class BulletConfig
{
    public GameObject Prefab;
    
    public float Damage;
    public float Speed;

    public override string ToString()
    {
        return $"Bullet: {Damage} {Speed}";
    }
}
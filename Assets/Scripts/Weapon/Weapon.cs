using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private WeaponConfig config;
    public SpriteRenderer muzzleFlash;

    private bool _triggerPressed;

    private float _fireTimer;
    private float _reloadTimer;

    private int _bulletsInClip;
    private float _muzzleFlashTimer;

    public event Action<int, int> OnClipUpdated;

    public Transform Muzzle => muzzle;
    public WeaponConfig Config => config;
    public bool IsFullClip => _bulletsInClip == config.ClipSize;
    public bool IsReloading => _reloadTimer > 0;
    
    public int BulletsInClip => _bulletsInClip;

    public int Team { get; set; }
    
    private void Start()
    {
        SetBulletsInClip(config.ClipSize);
        var sprc = GetComponentInChildren<WeaponSpriteController>();
        if(sprc != null)
        {
            sprc.SetParent(GetComponentInParent<PanHero>().gameObject);
        }
        muzzleFlash.enabled = false;
    }

    private void Update()
    {
        if (!PanLevel.Instance.Started) return;

        if (_reloadTimer > 0)
        {
            _reloadTimer -= Time.deltaTime;
            if (_reloadTimer <= 0)
            {
                Sounds.Instance.PlayRandom(config.SoundReload);
                SetBulletsInClip(config.ClipSize);
            }
        }
        
        UpdateFiring();
        if(_muzzleFlashTimer>0)
        {
            _muzzleFlashTimer -= Time.deltaTime;
            if(_muzzleFlashTimer<=0)
            {
                muzzleFlash.enabled = false;
            }
        }
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
        muzzleFlash.enabled = true;
        _muzzleFlashTimer = 0.1f;
        
        for (int i = 0; i < config.Pellets; i++)
        {
            var pos = muzzle.transform.position;
            var rot = muzzle.transform.rotation * Quaternion.Euler(0, Random.Range(-config.Spread, config.Spread), 0);

            PanLevel.Instance.SpawnBullet(config.BulletConfig, pos, rot, Team);
            if (!Config.Auto) ReleaseTrigger();
        }
        
        Sounds.Instance.PlayRandom(config.SoundShot);
        
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

    public void AimAt(Vector3 target)
    {
        muzzle.LookAt(target);
    }

    public void RandomizeInitialDelay()
    {
        SetBulletsInClip(0);
        _reloadTimer = Random.Range(0, config.ReloadTime);
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

    public bool Auto;

    public string SoundShot;
    public string SoundReload;
    
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
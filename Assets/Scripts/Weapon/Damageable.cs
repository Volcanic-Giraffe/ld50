using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;

    public bool Died => _died;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => health;
    public float Percent  => health / maxHealth;

    private bool _died;

    public event Action<HitInfo> OnHit;
    public event Action OnDie;
    
    public int Team { get; set; }

    public void Hit(float damage, bool heatDamage)
    {
        if (_died || Invulnerable) return;

        health -= damage;

        OnHit?.Invoke(new HitInfo()
        {
            HeatDamage = heatDamage
        });
        
        if (health <= 0)
        {
            Die();
        }
    }

    public bool Invulnerable { get; internal set; }

    private void Die()
    {
        if (_died) return;

        _died = true;

        OnHit = null;
        
        if (OnDie != null)
        {
            OnDie?.Invoke();
            OnDie = null;
        }
        
        Destroy(gameObject);
    }
}


public class HitInfo
{
    public bool HeatDamage;
}
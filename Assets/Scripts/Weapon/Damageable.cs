using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;

    [SerializeField] private int team;

    public bool Died => _died;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => health;

    private bool _died;

    public event Action OnHit;
    public event Action OnDie;

    public void Hit(GameObject source, float damage)
    {
        if (_died) return;

        health -= damage;

        OnHit?.Invoke();
        
        if (health <= 0)
        {
            Die();
        }
    }

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
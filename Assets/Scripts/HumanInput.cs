using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanInput : MonoBehaviour
{
    private PanHero _hero;

    private Vector3 _inputs = Vector3.zero;
    
    public bool Deactivated { get; set; }

    private void Awake()
    {
        Deactivated = true;
        _hero = GetComponent<PanHero>();
    }

    void Start()
    {
        if (PanLevel.Instance.Started)
        {
            Setup();
        }
        else
        {
            PanLevel.Instance.OnLevelStarted += OnLevelStarted;
        }
        
    }

    private void OnLevelStarted()
    {
        PanLevel.Instance.OnLevelStarted -= OnLevelStarted;
        Setup();
    }

    public void Setup()
    {
        Deactivated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Deactivated) return;
        if (PanLevel.Instance.Failed) return;
        
        if (_hero != null)
        {
            _inputs = Vector3.zero;
            _inputs.x = Input.GetAxis("Horizontal");
            _inputs.z = Input.GetAxis("Vertical");
            _inputs = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y, Vector3.up) * _inputs;
            
            _hero.LookAt(_inputs);
        }
        
         
        // if (Input.GetButtonDown("Jump"))
        // {
        //     _hero.Jump();
        // }
        if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire2"))
        {
            _hero.Dash(_inputs);
        }
        
        if (Input.GetButtonDown("Fire1"))
        {
            _hero.HoldTrigger();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            _hero.ReleaseTrigger();
        }
        
        if (Input.GetButtonUp("Reload"))
        {
            _hero.Weapon.StartReloading();
        }
        
        if (PanLevel.Instance.Aimer != null)
        {
            var target = PanLevel.Instance.Aimer.AimPoint();
            
            _hero.AimAt(target);
        }
    }

    private void FixedUpdate()
    {
        if (Deactivated) return;
        if (PanLevel.Instance.Failed) return;
        
        _hero.FixedMove(_inputs);
    }
}

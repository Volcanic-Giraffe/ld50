using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanInput : MonoBehaviour
{
    private PanHero _hero;

    private Vector3 _inputs = Vector3.zero;
    
    private void Awake()
    {
        _hero = GetComponent<PanHero>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_hero != null)
        {
            _inputs = Vector3.zero;
            _inputs.x = Input.GetAxis("Horizontal");
            _inputs.z = Input.GetAxis("Vertical");
            _inputs = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y, Vector3.up) * _inputs;
            
            _hero.LookAt(_inputs);
        }
        
         
        if (Input.GetButtonDown("Jump"))
        {
            _hero.Jump();
        }
        if (Input.GetButtonDown("Fire2"))
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
        _hero.FixedMove(_inputs);
    }
}

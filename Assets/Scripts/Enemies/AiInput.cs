using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiInput : MonoBehaviour
{
    private PanHero _character;

    private PanHero _target;
    
    private void Awake()
    {
        _character = GetComponent<PanHero>();
    }

    void Update()
    {
        if (_target == null)
        {
            _target = PanLevel.Instance.Player;
            _character.ReleaseTrigger();
        }
        else
        {
            _character.LookAt(_target.transform.position);
            _character.AimAt(_target.transform.position);

            _character.HoldTrigger();
        }
  
    }
}

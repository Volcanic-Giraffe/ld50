using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class VegetableBody : MonoBehaviour
{
    [SerializeField] BodyDataSO bodyData;
    [SerializeField] SpriteRenderer BodySprite;
    [SerializeField] SpriteRenderer EyeLeft;
    [SerializeField] SpriteRenderer EyeRight;
    [SerializeField] SpriteRenderer PupilLeft;
    [SerializeField] SpriteRenderer PupilRight;
    [SerializeField] SpriteRenderer Mouth;
    private Damageable _dmg;
    private Rigidbody _rb;
    private float bodySwitchTimer = 0;
    private bool lookFront;

    void Start()
    {
        _dmg = GetComponent<Damageable>();
        _rb = GetComponent<Rigidbody>();
        UpdateBody();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBody();
        UpdatePupils();
        AnimateMouth();
    }

    private void AnimateMouth()
    {
    }

    private void UpdateBody()
    {
        if (_rb.velocity.magnitude < 0.01 || Vector3.Dot(_rb.velocity, Camera.main.transform.position-transform.position)>=0)
        {
            if (!lookFront)
            {
                // Delay to avoid constant flickering
                bodySwitchTimer += Time.deltaTime;
                if (bodySwitchTimer > 0.5)
                {
                    SetLookFront(true);
                }
            }

        } else
        {
            if (lookFront)
            {
                bodySwitchTimer -= Time.deltaTime;
                if (bodySwitchTimer < -0.5)
                {
                    SetLookFront(false);
                }
            }
        }

        if (_dmg.Percent > 0.6)
        {
            BodySprite.sprite = lookFront ? bodyData.Front1 : bodyData.Back1;
        }
        else if (_dmg.Percent > 0.3)
        {
            BodySprite.sprite = lookFront ? bodyData.Front2 : bodyData.Back2;
        }
        else
        {
            BodySprite.sprite = lookFront ? bodyData.Front3 : bodyData.Back3;
        }
    }

    private void SetLookFront(bool v)
    {
        bodySwitchTimer = 0;
        lookFront = v;
        EyeLeft.gameObject.SetActive(v);
        EyeRight.gameObject.SetActive(v);
        Mouth.gameObject.SetActive(v);
    }

    private void UpdatePupils()
    {
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Damageable))]
public class VegetableBody : MonoBehaviour
{
    [SerializeField] public BodyDataSO bodyData;
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

    public bool IsRolling;

    void Start()
    {
        _dmg = GetComponent<Damageable>();
        _dmg.OnHit += _dmg_OnHit;
        _dmg.OnDie += _dmg_OnDie;
        _rb = GetComponent<Rigidbody>();
        UpdateBody();
    }

    private void _dmg_OnDie()
    {
        StopAllCoroutines();
        BodySprite.DOKill();
    }

    private void _dmg_OnHit()
    {
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        BodySprite.DOKill();
        yield return BodySprite.DOColor(Color.black, 0.05f).WaitForCompletion();
        yield return BodySprite.DOColor(Color.white, 0.1f).WaitForCompletion();
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

        if(IsRolling)
        {
            BodySprite.sprite = bodyData.Rot1;
        }
        else if (_dmg.Percent > 0.6)
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
        Debug.Log($"{v}");
        EyeLeft.gameObject.SetActive(v);
        EyeRight.gameObject.SetActive(v);
        Mouth.gameObject.SetActive(v);
    }

    private void UpdatePupils()
    {
    }
}

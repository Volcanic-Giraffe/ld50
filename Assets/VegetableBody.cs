using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PanHero))]
public class VegetableBody : MonoBehaviour
{
    [SerializeField] BodyDataSO bodyData;
    [SerializeField] SpriteRenderer BodySprite;
    [SerializeField] SpriteRenderer EyeLeft;
    [SerializeField] SpriteRenderer EyeRight;
    [SerializeField] SpriteRenderer PupilLeft;
    [SerializeField] SpriteRenderer PupilRight;
    [SerializeField] SpriteRenderer Mouth;
    private PanHero _hero;
    private Rigidbody _rb;
    private float bodySwitchTimer = 0;
    private bool lookFront;

    void Start()
    {
        _hero = GetComponent<PanHero>();
        _rb = GetComponent<Rigidbody>();
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
        // Works weird when on opposite side =(
        if(_rb.velocity.magnitude < 0.01 || Vector3.Dot(_rb.velocity, Camera.main.transform.position-transform.position)>=0)
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
    }

    private void SetLookFront(bool v)
    {
        bodySwitchTimer = 0;
        lookFront = v;
        // TODO switch dmg percent
        BodySprite.sprite = v ? bodyData.Front1 : bodyData.Back1;
        EyeLeft.gameObject.SetActive(v);
        EyeRight.gameObject.SetActive(v);
        Mouth.gameObject.SetActive(v);
    }

    private void UpdatePupils()
    {
    }
}

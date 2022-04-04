using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    [SerializeField] GameObject PiecePrefab;
    [SerializeField] Animator mouthAnim;
    private Sounds _sounds;
    private Damageable _dmg;
    private Rigidbody _rb;
    private float bodySwitchTimer = 0;
    private bool lookFront;
    private string[] RandomAnims = new string[] { "Scream1", "Scream2", "Scream3" };

    public bool IsRolling;
    private PickRandomSprite _prs;
    private float PupilLimit = 0.16f;
    private float _mouthTimer;
    private Coroutine _mouthCR;

    public BodyDataSO BodyData {
        get => bodyData; 
        set { 
            bodyData = value;
            EyeLeft.transform.localPosition += bodyData.EyeOffset;
            EyeRight.transform.localPosition += new Vector3(-bodyData.EyeOffset.x, bodyData.EyeOffset.y, bodyData.EyeOffset.z);
        } 
    }

    void Start()
    {
        _sounds = GetComponentInChildren<Sounds>() ?? _sounds;
        _dmg = GetComponent<Damageable>();
        _dmg.OnHit += _dmg_OnHit;
        _dmg.OnDie += _dmg_OnDie;
        _rb = GetComponent<Rigidbody>();
        mouthAnim.enabled = false;
        _prs = Mouth.gameObject.GetComponent<PickRandomSprite>();
        UpdateBody();
        EyeLeft.transform.localPosition += bodyData.EyeOffset;
        EyeRight.transform.localPosition += new Vector3(-bodyData.EyeOffset.x, bodyData.EyeOffset.y, bodyData.EyeOffset.z);
    }


    private void _dmg_OnDie()
    {
        var body = Instantiate(BodyData.BodyPrefab);
        body.transform.position = transform.position;
        StopAllCoroutines();
        BodySprite.DOKill();
        
        PupilLeft.transform.DOKill();
        PupilRight.transform.DOKill();
    }

    private void _dmg_OnHit(HitInfo hitInfo)
    {
        if (_mouthCR != null) StopCoroutine(_mouthCR);
        _prs.Pick();
        if (Random.value > 0.8) {
            var rnd = Random.Range(0.5f, 3f);
            PupilLeft.transform.localScale = new Vector3(rnd, rnd, rnd);
            PupilRight.transform.localScale = new Vector3(rnd, rnd, rnd);
        }

        if (Random.value > 0.92)
        {
            _sounds.PlayRandom("scream");
        } else if(Random.value > 0.7)
        {
            _sounds.PlayRandom("grunt");
        }

        StartCoroutine(Flash(hitInfo));
    }

    private void MovePupilsRandomly()
    {
        if (PupilLeft == null || PupilRight == null) return;
        
        PupilLeft.transform.DOKill();
        PupilRight.transform.DOKill();
        var target = new Vector3(Random.Range(PupilLimit, -PupilLimit), Random.Range(PupilLimit, -PupilLimit),PupilLeft.transform.localPosition.z);
        PupilLeft.transform.DOLocalMove(target, Random.Range(0.1f, 0.9f));
        if(Random.value > 0.8f) {target = new Vector3(Random.Range(PupilLimit, -PupilLimit), Random.Range(PupilLimit, -PupilLimit), PupilLeft.transform.localPosition.z); }
        PupilRight.transform.DOLocalMove(target, Random.Range(0.1f, 0.9f));
    }

    IEnumerator Flash(HitInfo hitInfo)
    {
        if (!hitInfo.HeatDamage)
        {
            var amnt = Random.Range(1, 3);
            for (int i = 0; i < amnt; i++)
            {
                var p = Instantiate(PiecePrefab);
                p.transform.position = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.2f, 0.5f), Random.Range(-0.5f, 0.5f));
                p.GetComponentInChildren<SpriteRenderer>().color = bodyData.PieceTint;
                p.GetComponent<Rigidbody>().AddExplosionForce(10, transform.position, 1);
            }
        }

        var hitColor = hitInfo.HeatDamage ? Color.red : Color.black;
        
        BodySprite.DOKill();
        yield return BodySprite.DOColor(hitColor, 0.05f).WaitForCompletion();
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
        if (_mouthTimer > 0) _mouthTimer -= Time.deltaTime;
        if (_mouthTimer <= 0)
        {
            if(_mouthCR != null) StopCoroutine(_mouthCR);
            _mouthTimer = Random.Range(3f, 8f);
            _mouthCR = StartCoroutine(AnimateMouthCR());
        }
    }

    private IEnumerator AnimateMouthCR()
    {
        _prs.Pick();
        yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
        _prs.Pick();
        yield return new WaitForSeconds(Random.Range(0.1f, 0.7f));
        _prs.Pick();
        if(Random.value > 0.5f) yield return new WaitForSeconds(Random.Range(0.1f, 2f));
        _prs.Pick();
        if (Random.value > 0.5f) yield return new WaitForSeconds(Random.Range(0.1f, 0.6f));
        _prs.Pick();
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
        EyeLeft.gameObject.SetActive(!IsRolling && v);
        EyeRight.gameObject.SetActive(!IsRolling && v);
        Mouth.gameObject.SetActive(!IsRolling && v);
    }

    private void UpdatePupils()
    {
        if (_dmg.Died) return;
        
        if (_mouthTimer > 0) _mouthTimer -= Time.deltaTime;
        if(_mouthTimer <= 0)
        {
            _mouthTimer = Random.Range(1f, 5f);
            MovePupilsRandomly();
        }
    }
}

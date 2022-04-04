using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PanHero : MonoBehaviour
{
    [SerializeField] private Transform handAnchor;
    [SerializeField] private Transform weaponHand;
    [SerializeField] private float downceleration;
    
    public float Speed = 5f;
    public float JumpHeight = 2f;
    public float GroundDistance = 0.2f;
    public float DashForce = 5f;
    public float DashCD = 2f;

    private Rigidbody _body;
    private Animator _anim;
    private VegetableBody _vgBody;
    private bool _isGrounded = true;
    private float _dashTimer;
    private float _invTimer;
    private int _floorMask;

    public bool IntroDone { get; set; }
    
    public Damageable Damageable { get; private set; }
    public Weapon Weapon { get; private set; }

    public int Team => gameObject.GetInstanceID();

    public event Action<Weapon, int> OnWeaponSwitched;

    
    private void Awake()
    {
        _floorMask = LayerMask.GetMask("Ground");
        
        Weapon = GetComponentInChildren<Weapon>();

        if (Weapon != null)
        {
            Weapon.Team = Team;
        }
        
        Damageable = GetComponent<Damageable>();

        if (Damageable != null)
        {
            Damageable.Team = Team;
        }
    }

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _vgBody = GetComponentInChildren<VegetableBody>();
    }

    public void LookAt(Vector3 target)
    {
        
    }

    public void Jump()
    {
        if (_isGrounded)
        {
            // TODO move to fixedUpdate
            _body.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }
    }
    
    public void Dash(Vector3 direction)
    {
        if (_dashTimer > 0) return;
        _vgBody.IsRolling = true;
        if (direction.x < 0)
        {
            _anim.Play("Roll");
        }
        else
        {
            _anim.Play("RollLeft");
        }
        _dashTimer = DashCD;
        _invTimer = 0.5f;
        Damageable.Invulnerable = true;
        //Vector3 dashVelocity = Vector3.Scale(direction, DashForce * new Vector3((Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime)));
        _body.AddForce(direction.normalized * DashForce, ForceMode.VelocityChange);
    }


    public void HoldTrigger()
    {
        if (Weapon != null)
        {
            Weapon.HoldTrigger();
        }
    }

    public void ReleaseTrigger()
    {
        if (Weapon != null)
        {
            Weapon.ReleaseTrigger();
        }
    }

    public void AimAt(Vector3 target)
    {
        if (!IntroDone) return;
        
        var dir = target - handAnchor.transform.position;

        handAnchor.transform.right = dir;
            
        Weapon.AimAt(target);
    }
    
    void Update()
    {
        if (!IntroDone) return;
        
        //transform.forward = Vector3.zero;
        if (_dashTimer > 0) _dashTimer -= Time.deltaTime;
        if (_invTimer > 0)
        {
            _invTimer -= Time.deltaTime;
            if (_invTimer <= 0)
            {
                Damageable.Invulnerable = false;
                _vgBody.IsRolling = false;
            }
        }
        
    }


    void FixedUpdate()
    {
        if (!IntroDone) return;
        
        var ray = new Ray(transform.position, -transform.up); // cast ray downwards
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            _isGrounded = hit.distance <= GroundDistance;
            // transform.up = hit.normal;
        }
        
        var floating = !Physics.Raycast(transform.position, -Vector3.up, out hit, 0.55f,_floorMask);

        if (floating)
        {
            _body.AddForce(-transform.up * downceleration * (_body.velocity.y > 0 ? 0.5f : 1f), ForceMode.Acceleration);
        }
        
        FixedTeleportToZero();
    }

    private void FixedTeleportToZero()
    {
        var mPos = transform.position;

        var center = new Vector3(0, mPos.y, 0);

        var maxRadius = Pan.Radius + 1f;
        
        var escapedPan = Vector3.Distance(mPos, center) > maxRadius;
        var fellDown = transform.position.y < -maxRadius;
        
        if (escapedPan || fellDown)
        {
            var tele = new Vector3(0, 10f, 0);
            
            _body.MovePosition(tele);
            _body.velocity = Vector3.zero;
            _body.angularVelocity = Vector3.zero;
        }
    }

    public void FixedMove(Vector3 inputs)
    {
        if (!IntroDone) return;
        
        // TODO does not work
        var r = _body.rotation;
        _body.MovePosition(_body.position + inputs * (Speed * Time.fixedDeltaTime));
        _body.rotation = r;
    }
    
    public void ChangeBody(BodyDataSO data)
    {
        GetComponent<VegetableBody>().BodyData = data;
    }

    public void ChangeWeapon(GameObject weaponGO)
    {
        Destroy(Weapon);

        var weaponObj = Instantiate(weaponGO, weaponHand);
        weaponObj.transform.localPosition = Vector3.zero;
        weaponObj.transform.localRotation = Quaternion.identity;

        Weapon = weaponObj.GetComponent<Weapon>();
        if (Weapon != null)
        {
            Weapon.Team = Team;
        }
        
        OnWeaponSwitched?.Invoke(Weapon, Weapon.BulletsInClip);
    }

    public void Intro()
    {
        var mPos = transform.position;
        
        _body.MovePosition(mPos + Vector3.up * 10f);
        _body.DOMove(mPos, 1f).SetDelay(2f).OnComplete(() =>
        {
            IntroDone = true;
        });
    }
}
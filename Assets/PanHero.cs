using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PanHero : MonoBehaviour
{
    [SerializeField] private Transform handAnchor;
    [SerializeField] private Transform weaponHand;
    
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

    public Damageable Damageable { get; private set; }
    public Weapon Weapon { get; private set; }

    public int Team => gameObject.GetInstanceID();

    public event Action<Weapon> OnWeaponSwitched;
    
    private void Awake()
    {
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
        var dir = target - handAnchor.transform.position;

        handAnchor.transform.right = dir;
            
        Weapon.AimAt(target);
    }
    
    void Update()
    {
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
        var ray = new Ray(transform.position, -transform.up); // cast ray downwards
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            _isGrounded = hit.distance <= GroundDistance;
            transform.up = hit.normal;
        }

    }

    public void FixedMove(Vector3 inputs)
    {
        // TODO does not work
        var r = _body.rotation;
        _body.MovePosition(_body.position + inputs * (Speed * Time.fixedDeltaTime));
        _body.rotation = r;
    }
    
    public void ChangeBody(BodyDataSO data)
    {
        GetComponent<VegetableBody>().bodyData = data;
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
        
        OnWeaponSwitched?.Invoke(Weapon);
    }
}
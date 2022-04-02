using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGunShooterController : MonoBehaviour
{
    [SerializeField] private float shootForce;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float cooldown= 0.2f;

    private Rigidbody _rig;
    private bool _fire;
    private float _cooldown;

    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_cooldown > 0) _cooldown -= Time.deltaTime;
        if (_cooldown <= 0 && Input.GetButton("Fire1"))
        {
            _fire = true;
            _cooldown = cooldown;
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var raycast = Physics.Raycast(ray, out hit, 250, LayerMask.GetMask("ArkInputPane"));

        if (_fire)
        {
            _fire = false;
            if(raycast) Shoot(hit.point);
        }
    }

    public void Shoot(Vector3 aimerPos)
    {
        var force = Vector3.Normalize(aimerPos - transform.position) * shootForce;
        var pr = Instantiate(projectile);
        pr.transform.localPosition = transform.localPosition;

        pr.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        _rig.AddForce(-force, ForceMode.Impulse);
    }
}

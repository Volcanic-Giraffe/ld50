using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiInput : MonoBehaviour
{
    [SerializeField] private float holdPositionTime;
    [SerializeField] private float holdPositionTimeSpread;
    [SerializeField] private float newPositionRadius;

    private const int NewPositionAttempts = 5;
    private const float PositionReachRadius = 1.5f;
    
    private PanHero _character;

    private PanHero _target;

    private float holdPositionTimer;

    private Vector3 _targetPos;
    
    private void Awake()
    {
        _targetPos = transform.position;
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

            // TODO raycast visibility, bursts, etc
            if(Random.value > 0.7f) _character.HoldTrigger();
        }

        holdPositionTimer -= Time.deltaTime;

        if (holdPositionTimer <= 0)
        {
            holdPositionTimer = holdPositionTime + Random.Range(-holdPositionTimeSpread, holdPositionTimeSpread);

            FindNewPosition();
        }
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, _targetPos) > PositionReachRadius)
        {
            _character.FixedMove((_targetPos - transform.position).normalized);
        }
    }

    private void FindNewPosition()
    {
        var mask = LayerMask.GetMask("Ground");

        for (int i = 0; i < NewPositionAttempts; i++)
        {
            var angle = Random.value * Mathf.PI * 2f;
            var x = Mathf.Cos(angle) * newPositionRadius;
            var z = Mathf.Sin(angle) * newPositionRadius;

            var candidate = transform.position + new Vector3(x, 5f, z); // +5 so always above the pan
            
            var raycast = Physics.Raycast(candidate, -Vector3.up, out var hit, 10f, mask);

            if (raycast)
            {
                _targetPos = hit.point;
                // Debug.DrawLine (candidate, hit.point, Color.red, 2f);
                break;
            }
        }
    }
}

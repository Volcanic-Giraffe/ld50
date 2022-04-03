using UnityEngine;
using Random = UnityEngine.Random;

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

    private int _obstacleMask;
    private int _groundMask;
    
    private void Awake()
    {
        _targetPos = transform.position;
        _character = GetComponent<PanHero>();
        
        _obstacleMask = LayerMask.GetMask(new [] {"Obstacles"});
        _groundMask = LayerMask.GetMask("Ground");
    }

    private void Start()
    {
        // so all enemies does not shoot at the same time at level start
        _character.Weapon.RandomizeInitialDelay();
    }

    void Update()
    {
        UpdateShooting();

        holdPositionTimer -= Time.deltaTime;

        if (holdPositionTimer <= 0)
        {
            holdPositionTimer = holdPositionTime + Random.Range(-holdPositionTimeSpread, holdPositionTimeSpread);

            FindNewPosition();
        }
    }

    private void UpdateShooting()
    {
        if (_target == null)
        {
            _target = PanLevel.Instance.Player;
            _character.ReleaseTrigger();
        }
        else
        {
            var sPos = _character.Weapon.Muzzle.position;
            var tPos = _target.transform.position;
            
            var hitObstacle = Physics.Raycast(sPos, (tPos - sPos).normalized, out var hit, Vector3.Distance(sPos, tPos), _obstacleMask);

            if (hitObstacle)
            {
                Debug.DrawLine (sPos, hit.point, Color.red);
                _character.ReleaseTrigger();
            }
            else
            {
                Debug.DrawLine (sPos, tPos, Color.green);
                
                _character.LookAt(tPos);
                _character.AimAt(tPos);

                _character.HoldTrigger();
            }
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
        for (int i = 0; i < NewPositionAttempts; i++)
        {
            var angle = Random.value * Mathf.PI * 2f;
            var x = Mathf.Cos(angle) * newPositionRadius;
            var z = Mathf.Sin(angle) * newPositionRadius;

            var candidate = transform.position + new Vector3(x, 5f, z); // +5 so always above the pan
            
            var raycast = Physics.Raycast(candidate, -Vector3.up, out var hit, 10f, _groundMask);

            if (raycast)
            {
                _targetPos = hit.point;
                // Debug.DrawLine (candidate, hit.point, Color.red, 2f);
                break;
            }
        }
    }
}

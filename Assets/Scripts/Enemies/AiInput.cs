using UnityEngine;
using Random = UnityEngine.Random;

public class AiInput : MonoBehaviour
{
    [SerializeField] private float holdPositionTime;
    [SerializeField] private float holdPositionTimeSpread;
    [SerializeField] private float newPositionRadius;

    [SerializeField] private float gainAgroTime;

    private const int NewPositionAttempts = 5;
    private const float PositionReachRadius = 1.5f;
    
    private PanHero _character;

    private PanHero _target;

    private float holdPositionTimer;

    private Vector3 _targetPos;

    private int _obstacleMask;
    private int _groundMask;
    private AiState _state;

    public Damageable Damageable => _character.Damageable;

    public float GainAgroTime => gainAgroTime;
    
    private void Awake()
    {
        _targetPos = transform.position;
        _character = GetComponent<PanHero>();
        
        _obstacleMask = LayerMask.GetMask(new [] {"Obstacles"});
        _groundMask = LayerMask.GetMask("Ground");
    }

    private void Start()
    {
        PanLevel.Instance.OnLevelStarted += Setup;
    }

    private void Setup()
    {
        PanLevel.Instance.OnLevelStarted -= Setup;
        
        // so all enemies does not shoot at the same time at level start
        _character.Weapon.RandomizeInitialDelay();

        SetState(new AiStateIdle(this));

        _character.Damageable.OnHit += () =>
        {
            _state.GotHit();
        };
    }

    void Update()
    {
        _state?.Update();
    }
    
    private void FixedUpdate()
    {
        _state?.FixedUpdate();
    }

    public void UpdateAimAtTarget()
    {
        if (_target != null)
        {
            _character.LookAt(_target.transform.position);
            _character.AimAt(_target.transform.position);
        }
    }
    
    public void UpdateShootTarget()
    {
        if (CanSeeTarget())
        {
            _character.HoldTrigger();
        }
        else
        {
            _character.ReleaseTrigger();
        }
    }

    public bool CanSeeTarget()
    {
        if (_target == null)
        {
            _target = PanLevel.Instance.Player;
            return false;
        }
        else
        {
            var sPos = _character.Weapon.Muzzle.position;
            var tPos = _target.transform.position;
            
            var hitObstacle = Physics.Raycast(sPos, (tPos - sPos).normalized, out var hit, Vector3.Distance(sPos, tPos), _obstacleMask);

            return !hitObstacle;
        }
    }
    
    public void UpdateMoveToTarget()
    {
        holdPositionTimer -= Time.deltaTime;

        if (holdPositionTimer <= 0)
        {
            holdPositionTimer = holdPositionTime + Random.Range(-holdPositionTimeSpread, holdPositionTimeSpread);

            FindNewPosition();
        }
    }

    public void FixedMoveToTarget()
    {
        if (Vector3.Distance(transform.position, _targetPos) > PositionReachRadius)
        {
            _character.FixedMove((_targetPos - transform.position).normalized);
        }
    }
    
    public void FindNewPosition()
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

    public void SetState(AiState state)
    {
        _state = state;
    }
}

public class AiState
{
    protected AiInput _owner;

    public AiState(AiInput owner)
    {
        _owner = owner;
    }
    
    public virtual void Update()
    {
        
    }
    
    public virtual void FixedUpdate()
    {
        
    }

    public virtual void GotHit()
    {
    }
}

public class AiStateIdle : AiState
{

    private float _idleTimer;
    
    public AiStateIdle(AiInput owner) : base(owner)
    {
        _idleTimer = Random.Range(2f, 6f);
    }

    public override void Update()
    {
        _idleTimer -= Time.deltaTime;

        if (_idleTimer <= 0)
        {
            _owner.SetState(new AiStateSeek(_owner));
        }
        _owner.UpdateMoveToTarget();
    }
    
    public override void FixedUpdate()
    {
        _owner.FixedMoveToTarget();
    }
    
    public override void GotHit()
    {
        _owner.SetState(new AiStateAgro(_owner));
    }
}

public class AiStatePanic : AiState{
    public AiStatePanic(AiInput owner) : base(owner)
    {
    }

    public override void Update()
    {
        _owner.UpdateMoveToTarget();
    }
    
    public override void FixedUpdate()
    {
        _owner.FixedMoveToTarget();
    }
}

public class AiStateSeek : AiState
{
    private float _gainAgroTimer;
    
    public AiStateSeek(AiInput owner) : base(owner)
    {
        _gainAgroTimer = owner.GainAgroTime;
    }

    public override void Update()
    {
        _owner.UpdateMoveToTarget();

        if (_owner.CanSeeTarget())
        {
            _gainAgroTimer -= Time.deltaTime;
        }

        if (_gainAgroTimer <= 0)
        {
            _owner.SetState(new AiStateAgro(_owner));
        }
    }
    
    public override void FixedUpdate()
    {
        _owner.FixedMoveToTarget();
    }
    
    public override void GotHit()
    {
        _owner.SetState(new AiStateAgro(_owner));
    }
}

public class AiStateAgro : AiState
{
    private float _deagroTimer;

    private float _warningTimer;
    
    public AiStateAgro(AiInput owner) : base(owner)
    {
        _deagroTimer = Random.Range(1f, 3f);
        _warningTimer = 1.2f;
    }

    public override void Update()
    {
        _owner.UpdateAimAtTarget();
        _owner.UpdateMoveToTarget();
        
        _warningTimer -= Time.deltaTime;

        if (_warningTimer <= 0)
        {
            _owner.UpdateShootTarget();
            
            _deagroTimer -= Time.deltaTime;

            if (_deagroTimer <= 0)
            {
                _owner.SetState(new AiStateIdle(_owner));
            }
        }
        
        if (_owner.Damageable.Percent < 0.2f)
        {
            _owner.SetState(new AiStatePanic(_owner));
            return;
        }

    }

    public override void FixedUpdate()
    {
        _owner.FixedMoveToTarget();
    }
}

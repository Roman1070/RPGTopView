using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyNPCView : NPCViewBase, IDamagable
{
    [Inject] private UpdateProvider _updateProvider;
    [Inject] private PlayerView _player;

    protected override NPCType NPCType => NPCType.Enemy;
    private Dictionary<NPCState, bool> _states = new Dictionary<NPCState, bool>()
    {
        {NPCState.Chasing, false },
        {NPCState.Patroling,true },
        {NPCState.Attacking,false }
    };

    [SerializeField]
    private NavMeshAgent _navMeshAgent;
    [SerializeField]
    private AgentLinkMover _agentLinkMover;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private EnemyNPCConfig _config;
    [SerializeField]
    private Rigidbody _rb;

    private Vector3 _startPoint;

    private float _distanceToPlayer;
    private Vector3 _destinationPoint;
    private float _health;

    private void Start()
    {
        _updateProvider.Updates.Add(LocalUpdate);
        _startPoint = transform.position;
        _destinationPoint = GetRandomDestinationPoint();
        _navMeshAgent.SetDestination(_destinationPoint);
        _agentLinkMover.OnLinkStart += HandleLinkStart;
    }

    private void HandleLinkStart()
    {
        _animator.SetTrigger("Jump");
    }

    private void LocalUpdate()
    {
        UpdateState();
    }

    private void UpdateState()
    {
        _distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

        if (_distanceToPlayer <= _config.AttackRange && !_states[NPCState.Attacking])
        {
            Attack();
            _animator.SetLayerWeightSmooth(this, "MovementLayer", false, 4);
            _animator.SetLayerWeightSmooth(this, "CombatLayer", true, 4);
        }
        else if (_distanceToPlayer <= _config.DetectionDistance)
        {
            Chase();
        }
        else if (_distanceToPlayer > _config.ChaseDistance)
        {
            Patrol();
        }
    }

    private void Attack()
    {
        _states[NPCState.Attacking] = true;
        var attack = _config.Attacks.Random();
        _animator.SetTrigger(attack.Id);

        DOVirtual.DelayedCall(attack.Duration, () =>
        {
            if (_distanceToPlayer <= _config.AttackRange)
            {
                Attack();
            }
            else
            {
                _states[NPCState.Attacking] = false;
                _animator.SetTrigger("exit combat");
                Chase();
                _animator.SetLayerWeightSmooth(this, "MovementLayer", true, 8);
                _animator.SetLayerWeightSmooth(this, "CombatLayer", false, 8);
            }
        });
    }

    private void Chase()
    {
        _states[NPCState.Patroling] = false;
        _states[NPCState.Chasing] = true;
        _animator.SetFloat("Speed", 2, 0.15f, Time.deltaTime);
        _destinationPoint = _player.transform.position;

        if (Vector3.Distance(transform.position, _destinationPoint) < _navMeshAgent.stoppingDistance) 
        { 
            _navMeshAgent.isStopped = true;
            _navMeshAgent.velocity = Vector3.zero;
        }
        else if (Vector3.Distance(transform.position, _destinationPoint) > _navMeshAgent.stoppingDistance+0.5f)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_destinationPoint);
            _navMeshAgent.speed = _config.ChaseSpeed;
            if(Physics.Raycast(transform.position+Vector3.up,_player.transform.position- transform.position, out var hit, 10))
            {
                if (hit.collider.TryGetComponent<PlayerView>(out var player))
                    _navMeshAgent.velocity = (player.transform.position-transform.position).normalized * _config.ChaseSpeed;
            }
        }
        transform.LookAt(_player.transform);

    }
    private void Patrol()
    {
        _states[NPCState.Patroling] = true;
        _states[NPCState.Chasing] = false;
        _animator.SetFloat("Speed", 1, 0.15f, Time.deltaTime);
        _navMeshAgent.isStopped = false;
        _navMeshAgent.speed = _config.PatrolSpeed;
        if ((transform.position - _destinationPoint).magnitude < 1)
        {
            _destinationPoint = GetRandomDestinationPoint();
            _navMeshAgent.SetDestination(_destinationPoint);
        }
    }

    private Vector3 GetRandomDestinationPoint()
    {
        float z = Random.Range(_startPoint.z, _startPoint.z + _config.PatrolingDistance);
        float x = Random.Range(_startPoint.x, _startPoint.x + _config.PatrolingDistance);

        return new Vector3(x, transform.position.y, z);
    }

    public void TakeDamage(int damage, float pushbackForce)
    {
        _health -= damage;
       // transform.DOMove(transform.TransformDirection(0, 0, -1) * pushbackForce, 0.2f);
    }
}

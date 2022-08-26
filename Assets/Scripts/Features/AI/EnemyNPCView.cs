using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyNPCView : NPCViewBase
{
    [Inject] private UpdateProvider _updateProvider;
    [Inject] private PlayerView _player;

    protected override NPCType NPCType => NPCType.Enemy;
    private NPCState _state;

    [SerializeField]
    private NavMeshAgent _navMeshAgent;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private float _detectingDistance;
    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private float _patrolRange;

    private Vector3 _startPoint;

    private float _distanceToPlayer;
    private float _health;
    private Vector3 _destinationPoint;

    private void Start()
    {
        _updateProvider.Updates.Add(LocalUpdate);
        _startPoint = transform.position;
        _destinationPoint = GetRandomDestinationPoint();
        _navMeshAgent.SetDestination(_destinationPoint);
    }

    private void LocalUpdate()
    {
        UpdateState();
    }

    private void UpdateState()
    {
        _distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

        if (_distanceToPlayer <= _attackRange)
            _state = NPCState.Attacking;
        else if (_distanceToPlayer <= _detectingDistance)
        {
            _state = NPCState.Chasing;
            _animator.SetFloat("Speed", 2, 0.15f, Time.deltaTime);
            Chase();
        }
        else 
        {
            _state = NPCState.Patroling;
            _animator.SetFloat("Speed", 1, 0.15f,Time.deltaTime);
            Patrol();
        }
    }
    private void Chase()
    {
        _destinationPoint = _player.transform.position;
        _navMeshAgent.SetDestination(_destinationPoint);
        _navMeshAgent.speed = 6;
        transform.LookAt(_player.transform);
    }
    private void Patrol()
    {
        if ((transform.position - _destinationPoint).magnitude < 0.1f)
        {
            _destinationPoint = GetRandomDestinationPoint();
            _navMeshAgent.SetDestination(_destinationPoint);
        }
    }

    private Vector3 GetRandomDestinationPoint()
    {
        float z = Random.Range(_startPoint.z, _startPoint.z + _patrolRange);
        float x = Random.Range(_startPoint.x, _startPoint.x + _patrolRange);

        return new Vector3(x, transform.position.y, z);
    }
}

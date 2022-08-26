using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyNPCView : NPCViewBase
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

        if (_distanceToPlayer <= _config.AttackRange &&!_states[NPCState.Attacking])
        {
            Attack();
        }
        else if (_distanceToPlayer <= _config.DetectionDistance)
        {
            _states[NPCState.Chasing]=true;
            _animator.SetFloat("Speed", 2, 0.15f, Time.deltaTime);
            Chase();
        }
        else if (_distanceToPlayer > _config.ChaseDistance)
        {
            _states[NPCState.Patroling] = true;
            _animator.SetFloat("Speed", 1, 0.15f,Time.deltaTime);
            Patrol();
        }
    }

    private void Attack()
    {
        _states[NPCState.Attacking] = true;
        var attack = _config.Attacks.Random();
        _animator.SetLayerWeightSmooth(this, "MovementLayer", false, 4);
        _animator.SetLayerWeightSmooth(this, "CombatLayer", true, 4);
        _animator.SetTrigger(attack.Id);

        var pushVector = transform.TransformDirection(new Vector3(0, 0, 1)) * attack.PushForce.z + transform.TransformDirection(new Vector3(0, 1, 0)) * attack.PushForce.y;
        GetPushed(pushVector, attack.PushCurve);
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
                _animator.SetFloat("Speed", 2, 0.15f, Time.deltaTime);
                Chase();
                _animator.SetLayerWeightSmooth(this, "MovementLayer", true, 4);
                _animator.SetLayerWeightSmooth(this, "CombatLayer", false, 4);
            }
        });
    }

    private void Chase()
    {
        _destinationPoint = _player.transform.position;
        _navMeshAgent.SetDestination(_destinationPoint);
        _navMeshAgent.speed = _config.ChaseSpeed;
        transform.LookAt(_player.transform);
    }
    private void Patrol()
    {
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

    public void GetPushed(Vector3 direction, AnimationCurve curve)
    {
     //   GetComponent<Rigidbody>().MovePosition(direction);
    }
}

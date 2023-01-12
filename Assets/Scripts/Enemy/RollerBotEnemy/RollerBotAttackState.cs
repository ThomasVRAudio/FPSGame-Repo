using System.Collections;
using UnityEngine;

public class RollerBotAttackState : MonoBehaviour, IEnemyState, IRollerMovement
{
    [SerializeField] private GameObject head;
    [SerializeField] private FireWeapon[] fireWeapon;
    private RollerBotMovement _rollerBotMovement;
    private RollerBotHeadLookAt _RollerBotHeadLookAt;

    private IEnumerator _projectileRoutine;

    private Vector3 _curPosition;
    private Vector3 _direction;
    private Enemy _enemy;

    private bool _isInAttackState = false;
    private readonly float _speed = 7f;

    private int _projectileNumber = 0;
    private int?[] _poolNumber = null;

    public void OnEnterState(Enemy enemy)
    {
        _poolNumber = enemy.PoolNumber;
        _isInAttackState = true;
        enemy.OnExitHandler += OnExitState;

        _enemy = enemy;
        _curPosition = transform.position;
        _direction = (enemy.Target.transform.position - _curPosition).normalized * 10;

        _rollerBotMovement = GetComponent<RollerBotMovement>();
        _rollerBotMovement.MoveTowards(_curPosition, _curPosition + new Vector3(_direction.x, 0, _direction.z), this, _speed);

        _RollerBotHeadLookAt = GetComponent<RollerBotHeadLookAt>();


    }
    public void OnUpdateState(Enemy enemy)
    {
        if(_enemy.Target != null)
            _RollerBotHeadLookAt.LookAtTarget(_enemy.Target.transform.position);

    }

    public void OnExitState()
    {
        _enemy.OnExitHandler -= OnExitState;
        _isInAttackState = false;

        if(_projectileRoutine != null)
            StopCoroutine(_projectileRoutine);
    }

    public void OnMovedTowards()
    {
        if (!_isInAttackState) return;

        _curPosition = transform.position;

        _projectileRoutine = FireProjectile();
        StartCoroutine(_projectileRoutine);
    }

    private IEnumerator FireProjectile()
    {
        yield return new WaitForSeconds(0.12f);

        if (_projectileNumber % 2 == 0)
        {
            fireWeapon[0].Shoot(fireWeapon[0].transform.position + fireWeapon[0].transform.rotation * Vector3.forward * 10, _enemy);

        } else
        {
            fireWeapon[1].Shoot(fireWeapon[1].transform.position + fireWeapon[1].transform.rotation * Vector3.forward * 10, _enemy);
        }

        _projectileNumber++;
        _projectileRoutine = FireProjectile();
        StartCoroutine(_projectileRoutine);
    }
}
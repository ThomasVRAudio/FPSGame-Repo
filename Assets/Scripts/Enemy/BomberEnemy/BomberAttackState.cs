using System.Collections;
using UnityEngine;

public class BomberAttackState : MonoBehaviour, IEnemyState
{
    [SerializeField] private AnimationCurve movementCurve;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float overShoot = 20;
    private Vector3 _curPosition;
    private Vector3 _direction;
    private Quaternion _curRotation;
    private Enemy _enemy;

    private const float SPEED = 10;
    private const float MAX_ROTATE_TIME = 1f;

    private float _time;
    private float _rotateTime;
    private float _distance;
    private int?[] _poolNumber;

    private IEnumerator _bombsRoutine;

    private delegate void OnUpdate();
    private OnUpdate onUpdate;


    public void OnEnterState(Enemy enemy)
    {
        _poolNumber = enemy.PoolNumber;
        enemy.OnExitHandler += OnExitState;
        _enemy = enemy;
        _curPosition = transform.position;
        _curRotation = transform.rotation;
        _direction = (enemy.Target.transform.position - _curPosition).normalized * overShoot;
        _time = 0;
        _rotateTime = 0;
        _distance = Distance();
        onUpdate = RotateTowardsTarget;
    }

    public void OnUpdateState(Enemy enemy)
    {
        onUpdate();
    }

    private void OnExitState()
    {
        _enemy.OnExitHandler -= OnExitState;
        StopCoroutine(_bombsRoutine);
    }

    private void RotateTowardsTarget()
    {
        if (_rotateTime < MAX_ROTATE_TIME)
        {
            _rotateTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(_curRotation, Quaternion.LookRotation(_direction.normalized), movementCurve.Evaluate(_rotateTime / MAX_ROTATE_TIME));
            return;
        }

        _curRotation = Quaternion.LookRotation(_direction.normalized);
        _bombsRoutine = DropBomb();
        StartCoroutine(_bombsRoutine);
        _rotateTime = 0;

        onUpdate = MoveTowardsTarget;
    }

    private void MoveTowardsTarget()
    {
        if (_time < _distance / SPEED)
        {
            _time += Time.deltaTime;

            transform.position = Vector3.Lerp(_curPosition, _curPosition + new Vector3(_direction.x, 0, _direction.z), movementCurve.Evaluate(_time / (_distance / SPEED)));

            if (_time > (_distance / SPEED) * 0.9f && _bombsRoutine != null)
                StopCoroutine(_bombsRoutine);

        }
        else
        {
            transform.LookAt(_curPosition + new Vector3(_direction.x, 0, _direction.z));

            transform.position = _curPosition + new Vector3(_direction.x, 0, _direction.z);
            _curPosition = transform.position;
            
            _direction = (_enemy.Target.transform.position - _curPosition).normalized * overShoot;
            _distance = Distance();

            _time = 0;

            onUpdate = RotateTowardsTarget;
        }        
    }

    private float Distance() => _direction.magnitude;

    private IEnumerator DropBomb()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject bomb = ObjectPool.SharedInstance.GetPooledObject(_poolNumber[0]);
        if(bomb != null)
        {
            bomb.transform.SetPositionAndRotation(transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
            bomb.SetActive(true);
            bomb.GetComponent<Projectile>().Fire(null, _enemy, explosionPool: _poolNumber[1]);
        }

        _bombsRoutine = DropBomb();
        StartCoroutine(_bombsRoutine);
    }
}

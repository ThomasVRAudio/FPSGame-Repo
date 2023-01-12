using UnityEngine;

public class BomberRoamingState : MonoBehaviour, IEnemyState
{
    [SerializeField] private GameObject[] roamingSpots;
    [SerializeField] private AnimationCurve movementCurve;
    private Vector3 _curPosition;
    private int _spotNumber = 0;

    private Quaternion _curRotation;
    private float _rotateTime;
    private float _maxRotateTime = 1f;

    private readonly float _speed = 3;
    private float _time;

    private float _distance;

    private delegate void OnUpdate();
    private OnUpdate onUpdate;


    public void OnEnterState(Enemy enemy)
    {
        _curPosition = transform.position;
        _curRotation = transform.rotation;
        _time = 0;
        _rotateTime = 0;
        _distance = Distance();
        onUpdate = RotateTowardsTarget;
    }

    public void OnUpdateState(Enemy enemy)
    {
        onUpdate();
    }

    private void RotateTowardsTarget()
    {
        if (_rotateTime < _maxRotateTime)
        {
            _rotateTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(_curRotation, 
                                    Quaternion.LookRotation((roamingSpots[_spotNumber].transform.position - _curPosition).normalized), 
                                    movementCurve.Evaluate(_rotateTime / _maxRotateTime));
            return;
        }

        _curRotation = Quaternion.LookRotation((roamingSpots[_spotNumber].transform.position - _curPosition).normalized);
        _rotateTime = 0;

        onUpdate = MoveTowardsTarget;
    }

    private void MoveTowardsTarget()
    {

        if ( _time < _distance / _speed)
        {
            _time += Time.deltaTime;
            transform.position = Vector3.Lerp(_curPosition, roamingSpots[_spotNumber].transform.position, movementCurve.Evaluate(_time / (_distance / _speed)));
        } else
        {
            transform.position = roamingSpots[_spotNumber].transform.position;
            _curPosition = transform.position;

            _spotNumber = _spotNumber == roamingSpots.Length - 1 ? 0 : _spotNumber + 1;
            _distance = Distance();

            onUpdate = RotateTowardsTarget;
            _time = 0;
        }
    }

    private float Distance()
    {
        int prevSpot;
        prevSpot = _spotNumber == 0 ? roamingSpots.Length - 1 : _spotNumber - 1;

        return (roamingSpots[_spotNumber].transform.position - roamingSpots[prevSpot].transform.position).magnitude;
    }
}

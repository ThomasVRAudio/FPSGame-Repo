using UnityEngine;
using UnityEngine.Animations;

public class RollerBotMovement : MonoBehaviour
{
    [SerializeField] private AnimationCurve movementCurve;
    [SerializeField] private AnimationCurve rollBodyCurve;

    [SerializeField] private GameObject meshParent;
    [SerializeField] private GameObject sphereBody;
    [SerializeField] private GameObject fullBody;

    private Vector3 _curPosition;
    private Quaternion _curRotation;
    private Quaternion _fullBodyRotation;

    private float _rotateTime;
    private float _time;
    private float _distance;
    private float _rotateBodySpeed = 0;

    private float _speed = 5;
    private const float MAX_ROTATE_TIME = 1f;
    private const float MAX_ROTATE_BODY_SPEED = 200;

    private Vector3 _targetPos;
    private Vector3 _startPos;

    private IRollerMovement _rollerBot;


    private delegate void OnUpdate();
    private OnUpdate onUpdate;

    private void Awake()
    {
        onUpdate = NotMoving;
    }

    public void MoveTowards(Vector3 startPos, Vector3 targetPos, IRollerMovement rollerBot, float speed = 5)
    {
        _rollerBot = rollerBot;
        _targetPos = targetPos;
        _startPos = startPos;
        _speed = speed;

        _curPosition = transform.position;
        _curRotation = transform.rotation;

        _time = 0;
        _rotateTime = 0;

        _distance = (targetPos - startPos).magnitude;
        _fullBodyRotation = fullBody.transform.localRotation;

        onUpdate = RotateTowardsTarget;
    }

    private void Update()
    {
        onUpdate();
    }

    private void NotMoving() { }

    private void RotateTowardsTarget()
    {
        if (_rotateTime < MAX_ROTATE_TIME)
        {
            _rotateTime += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(_curRotation,
                                    Quaternion.LookRotation((_targetPos - _startPos).normalized),
                                    movementCurve.Evaluate(_rotateTime / MAX_ROTATE_TIME));
            return;
        }

        _curRotation = Quaternion.LookRotation((_targetPos - _startPos).normalized);
        _rotateTime = 0;
        _curPosition = transform.position;
        onUpdate = MoveTowardsTarget;
    }

    private void MoveTowardsTarget()
    {
        RollSphere();

        if ( _time < _distance / _speed)
        {
            _time += Time.deltaTime;
            
            transform.position = Vector3.Lerp(_curPosition, new Vector3(_targetPos.x, _curPosition.y , _targetPos.z), movementCurve.Evaluate(_time / (_distance / _speed)));
            _rotateBodySpeed = Mathf.Lerp(0, MAX_ROTATE_BODY_SPEED, rollBodyCurve.Evaluate(_time / (_distance / _speed)));

            fullBody.transform.localRotation = Quaternion.Lerp(_fullBodyRotation, Quaternion.Euler(0, 0, 17), rollBodyCurve.Evaluate(_time / (_distance / _speed)));
        } else
        {
            onUpdate = NotMoving;
            _rollerBot.OnMovedTowards();
        }
    }

    public void Interrupt() => onUpdate = NotMoving;
    private void RollSphere() => sphereBody.transform.localEulerAngles += new Vector3(0, 0, _rotateBodySpeed * Time.deltaTime);

}

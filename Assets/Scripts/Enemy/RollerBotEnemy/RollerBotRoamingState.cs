using UnityEngine;

public class RollerBotRoamingState : MonoBehaviour, IEnemyState, IRollerMovement
{
    [SerializeField] private GameObject[] roamingSpots;

    private RollerBotMovement _rollerBotMovement;
    private RollerBotHeadLookAt _rollerBotHeadLookAt;

    private Vector3 _curPosition;

    private Enemy _enemy;

    private int _spotNumber = 0;

    private bool _isInCurrentState = false;


    public void OnEnterState(Enemy enemy)
    {
        _isInCurrentState = true;

        enemy.OnExitHandler += OnExitState;
        
        _enemy = enemy;
        _curPosition = transform.position;

        _rollerBotMovement = GetComponent<RollerBotMovement>();
        _rollerBotMovement.MoveTowards(_curPosition, roamingSpots[_spotNumber].transform.position, this);
        _rollerBotHeadLookAt = GetComponent<RollerBotHeadLookAt>();

    }

    public void OnUpdateState(Enemy enemy) 
    {
            Vector3 pos = roamingSpots[_spotNumber].transform.position;
            _rollerBotHeadLookAt.LookAtTarget(new Vector3(pos.x, 2, pos.z));
    }

    public void OnExitState()
    {
        _enemy.OnExitHandler -= OnExitState;
        _isInCurrentState = false;
    }

    public void OnMovedTowards()
    {
        if (!_isInCurrentState) return;

        _curPosition = transform.position;
        _spotNumber = _spotNumber == roamingSpots.Length - 1 ? 0 : _spotNumber + 1;

        _rollerBotMovement.MoveTowards(_curPosition, roamingSpots[_spotNumber].transform.position, this);

    }
}

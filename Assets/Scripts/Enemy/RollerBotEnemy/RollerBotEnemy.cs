using System;
using UnityEngine;

[RequireComponent(typeof(RollerBotRoamingState), typeof(RollerBotAttackState), typeof(RollerBotDieState))]
public class RollerBotEnemy : Enemy, IDetect
{

    private float _health = 100f;
    public override event Action OnExitHandler;

    #region States
    public override IEnemyState BaseState => GetComponent<RollerBotRoamingState>();
    public override IEnemyState PlayerInRangeState => GetComponent<RollerBotAttackState>();
    public override IEnemyState DieState => GetComponent<RollerBotDieState>();
    #endregion

    public override float Health { get { return _health; } protected set { _health = value; } }

    #region Enter & Exit TriggerZone
    public void OnEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() == null) return;
        
        Target = other.gameObject;
        ChangeState(PlayerInRangeState);
    }

    public void OnExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() == null) return;
        Target = null;
        OnExitHandler?.Invoke();
        ChangeState(BaseState);
    }
    #endregion
}

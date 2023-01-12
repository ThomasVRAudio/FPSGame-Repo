using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable, IOwner
{
    public IEnemyState CurrentState { get; protected set; }
    public abstract float Health { get; protected set; }
    public abstract event Action OnExitHandler;

    public abstract IEnemyState BaseState { get; }
    public abstract IEnemyState PlayerInRangeState { get; }
    public abstract IEnemyState DieState { get; }

    public int?[] PoolNumber = null;
    public GameObject Target { get; set; }

    void Start() => OnInitialize();

    void Update() => CurrentState.OnUpdateState(this);

    protected virtual void OnInitialize()
    {
        CurrentState = BaseState;
        CurrentState.OnEnterState(this);
    }

    public void ChangeState(IEnemyState state)
    {
        CurrentState = state;
        CurrentState.OnEnterState(this);
    }

    public virtual void TakeDamage(float amount)
    {
        Health -= amount;

        if (Health <= 0)
            ChangeState(DieState);
    }
}

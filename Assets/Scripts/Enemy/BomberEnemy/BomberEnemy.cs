using System;
using UnityEngine;

[RequireComponent(typeof(BomberRoamingState), typeof(BomberAttackState), typeof(BomberDieState))]
public class BomberEnemy : Enemy, IDetect
{
    [Header("Projectile Pool Settings")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private int amount = 20;
    [SerializeField] private GameObject explosion;
    [SerializeField] private int explosionAmount = 10;

    private float _health = 100f;
    public override event Action OnExitHandler;

    #region States
    public override IEnemyState BaseState => GetComponent<BomberRoamingState>();
    public override IEnemyState PlayerInRangeState => GetComponent<BomberAttackState>();
    public override IEnemyState DieState => GetComponent<BomberDieState>();
    #endregion

    public override float Health { get { return _health; } protected set { _health = value; } }

    protected override void OnInitialize()
    {
        CurrentState = BaseState;
        CurrentState.OnEnterState(this);

        PoolNumber = new int?[2];

        ObjectPool.SharedInstance.AddObjectPool(projectile, amount);
        PoolNumber[0] = ObjectPool.SharedInstance.GetObjectPoolNumber(projectile);

        ObjectPool.SharedInstance.AddObjectPool(explosion, explosionAmount);
        PoolNumber[1] = ObjectPool.SharedInstance.GetObjectPoolNumber(explosion);
    }

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.layer == 6)
            ChangeState(DieState);

    }
}

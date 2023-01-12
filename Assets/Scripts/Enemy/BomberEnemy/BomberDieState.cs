using UnityEngine;

public class BomberDieState : MonoBehaviour, IEnemyState
{
    public void OnEnterState(Enemy enemy)
    {
        Destroy(gameObject);
    }

    public void OnUpdateState(Enemy enemy)
    {
        
    }
}

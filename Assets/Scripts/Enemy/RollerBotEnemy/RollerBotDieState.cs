using UnityEngine;

public class RollerBotDieState : MonoBehaviour, IEnemyState
{
    public void OnEnterState(Enemy enemy)
    {
        Destroy(gameObject);
    }

    public void OnUpdateState(Enemy enemy)
    {
        
    }
}

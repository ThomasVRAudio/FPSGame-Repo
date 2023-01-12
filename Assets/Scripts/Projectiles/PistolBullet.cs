using UnityEngine;

public class PistolBullet : Projectile
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float damage = 10f;
    protected override float Speed => speed;
    protected override float Damage => damage;
}

using UnityEngine;

public class ElectricProjectile : Projectile
{
    [SerializeField] private float speed = 30f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject explosion;
    protected override float Speed => speed;
    protected override float Damage => damage;

    protected override void OnDestroyProjectile(int? explosionPool)
    {
        GameObject Explosion = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(Explosion, 2f);
        Destroy(gameObject, 0.1f);
    }
}

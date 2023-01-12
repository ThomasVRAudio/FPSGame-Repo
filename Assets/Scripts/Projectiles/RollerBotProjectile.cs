using System;
using UnityEngine;

public class RollerBotProjectile : Projectile
{
    [SerializeField] private float speed = 30f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject explosion;
    protected override float Speed => speed;
    protected override float Damage => damage;

    protected override void SetDestroyTime() { }
    //protected override void OnDestroyProjectile(int? explosionPool)
    //{
    //    GameObject Explosion = Instantiate(explosion, transform.position, transform.rotation);
    //    Destroy(Explosion, 2f);
    //    Invoke(nameof(SetInactive), 0.1f);
    //}

    //private void SetInactive()
    //{
    //    gameObject.SetActive(false);
    //}
}

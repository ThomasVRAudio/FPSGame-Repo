using System.Collections;
using UnityEngine;

public class BomberProjectile : Projectile
{
    [SerializeField] private float speed = 30f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject explosion;
    protected override float Speed => speed;
    protected override float Damage => damage;

    protected override void Movement() { }
    protected override void SetDestroyTime() { }

    protected override void ResetRBSpeed() => GetComponent<Rigidbody>().velocity = Vector3.zero;
}
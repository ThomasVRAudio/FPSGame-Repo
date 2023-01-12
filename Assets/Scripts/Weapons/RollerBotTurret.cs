using System;
using System.ComponentModel;
using UnityEngine;

public class RollerBotTurret : FireWeapon
{
    [Header("Weapon Locations")]
    [SerializeField] private Transform aimLocation;
    [SerializeField] private Transform defaultLocation;
    [SerializeField] private GameObject barrel;

    [Header("Models")]
    [SerializeField] private GameObject weaponModel;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject projectileModel;
    [SerializeField] private GameObject muzzleFlash;

    [Header("Recoil")]
    [SerializeField] private float recoilAmount = 0.01f;
    [SerializeField] private float recoilTime = 0.1f;

    [Header("Stats")]
    [SerializeField] private float weaponRange = 100f;

    [Header("Pool Amount")]
    [SerializeField] private int projectileAmount = 20;
    [SerializeField] private int explosionAmount = 10;

    protected override float RecoilAmount => recoilAmount;
    protected override float RecoilTime => recoilTime;
    protected override float WeaponRange => weaponRange;
    protected override GameObject MuzzleFlash => muzzleFlash;
    protected override GameObject ProjectileModel => projectileModel;
    protected override GameObject Explosion => explosion;
    protected override GameObject Barrel => barrel;
    public override Transform AimLocation => transform;
    public override Transform DefaultLocation => transform;
    public override GameObject WeaponModel => weaponModel;


    private void Start()
    {
        PoolNumber = new int?[2];
        PoolAmount = new int?[2];

        PoolAmount[0] = projectileAmount;
        PoolAmount[1] = explosionAmount;

        SetProjectilePool();
        SetExplosionPool();
    }
}

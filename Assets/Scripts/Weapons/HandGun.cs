using UnityEngine;

public class HandGun : FireWeapon
{
    [Header("Weapon Locations")]
    [SerializeField] private Transform aimLocation;
    [SerializeField] private Transform defaultLocation;
    [SerializeField] private GameObject barrel;

    [Header("Models")]
    [SerializeField] private GameObject weaponModel;
    [SerializeField] private GameObject projectileModel;
    [SerializeField] private GameObject muzzleFlash;

    [Header("Recoil")]
    [SerializeField] private float recoilAmount = 0.01f;
    [SerializeField] private float recoilTime = 0.1f;

    [Header("Stats")]
    [SerializeField] private float weaponRange = 100f;
    [SerializeField] private float weaponRayDamage = 0f;

    [Header("Camera Shake")]
    [SerializeField] private float camShakeMagnitude = 2f;
    [SerializeField] private float camShakeLength = 1f;

    protected override float RayDamage => weaponRayDamage;
    protected override float RecoilAmount => recoilAmount;
    protected override float RecoilTime => recoilTime;
    protected override float WeaponRange => weaponRange;
    protected override GameObject MuzzleFlash => muzzleFlash;
    protected override GameObject ProjectileModel => projectileModel;
    protected override GameObject Barrel => barrel;
    protected override float CamShakeMagnitude => camShakeMagnitude;
    protected override float CamShakeLength => camShakeLength;
    public override Transform AimLocation => aimLocation;
    public override Transform DefaultLocation => defaultLocation;
    public override GameObject WeaponModel => weaponModel;



}

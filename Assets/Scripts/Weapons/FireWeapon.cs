using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.VFX;

public abstract class FireWeapon : MonoBehaviour
{
    public float Range => WeaponRange;
    public abstract Transform AimLocation { get; }
    public abstract Transform DefaultLocation { get;}
    public abstract GameObject WeaponModel { get; }
    protected abstract GameObject Barrel { get; }
    protected virtual GameObject ProjectileModel => null;
    protected virtual GameObject Explosion => null;
    protected virtual float RayDamage => 20f;
    protected virtual float RecoilTime => 0.01f;
    protected virtual float RecoilAmount => 0.01f;
    protected virtual float WeaponRange => 100f;
    protected virtual float CamShakeMagnitude => 0f;
    protected virtual float CamShakeLength => 0f;
    protected virtual GameObject MuzzleFlash => null;

    protected int?[] PoolNumber = null;
    protected int?[] PoolAmount = null;

    protected LayerMask _gunIgnoreMask;

    private IOwner _owner;

    private void Start()
    {
        _gunIgnoreMask = GunManager.Instance.GunIgnoreMask;
    }

    public virtual void Shoot(Camera cam, IOwner owner) 
    {
        _owner = owner;
        Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, WeaponRange, ~_gunIgnoreMask);

        if (MuzzleFlash != null)
            MuzzleFlash.GetComponent<VisualEffect>().Play();

        StartCoroutine(cam.GetComponent<CameraShake>().Shake(CamShakeMagnitude, CamShakeLength));

        if (ProjectileModel == null) SetRayDamage(hit);
        else FireProjectile(hit.point);

        StartCoroutine(Recoil());
    }

    public virtual void Shoot(Vector3 dir, IOwner owner, Camera cam = null)
    {
        _owner = owner;

        if (MuzzleFlash != null)
            MuzzleFlash.GetComponent<VisualEffect>().Play();

        if (cam != null)
            StartCoroutine(cam.GetComponent<CameraShake>().Shake(CamShakeMagnitude, CamShakeLength));
        
        FireProjectile(dir);

        StartCoroutine(Recoil());
    }

    public virtual void Reload() { }

    protected virtual void FireProjectile(Vector3 dir)
    {
        if (PoolNumber != null)
        {
            FireFromPool(dir);
            return;
        }

        GameObject projectile = Instantiate(ProjectileModel, Barrel.transform.position, Barrel.transform.rotation);
        projectile.GetComponent<Projectile>().Fire(dir, _owner);

    }

    private void SetRayDamage(RaycastHit hit)
    {
        if (hit.transform.GetComponent<IDamageable>() != null)
            hit.transform.GetComponent<IDamageable>().TakeDamage(RayDamage);
    }

    protected virtual IEnumerator Recoil()
    {
        Vector3 startPos = WeaponModel.transform.localPosition;

        Vector3 RecoilPosition = new Vector3(WeaponModel.transform.localPosition.x, 
                                                WeaponModel.transform.localPosition.y, 
                                                WeaponModel.transform.localPosition.z - RecoilAmount);


        WeaponModel.transform.localPosition = RecoilPosition;
        float t = 0;

        while (t < RecoilTime)
        {
            WeaponModel.transform.localPosition = Vector3.Lerp(RecoilPosition, startPos, t / RecoilTime);

            t += Time.deltaTime;
            yield return null;
        }

        WeaponModel.transform.localPosition = startPos;
    }

    protected virtual void FireFromPool(Vector3 dir)
    {
        GameObject bullet = ObjectPool.SharedInstance.GetPooledObject(PoolNumber[0]);
        if (bullet != null)
        {
            bullet.transform.SetPositionAndRotation(Barrel.transform.position, Barrel.transform.rotation);
            bullet.SetActive(true);
            bullet.GetComponent<Projectile>().Fire(dir, _owner, explosionPool: PoolNumber[1]);
        }
    }

    protected virtual void SetProjectilePool()
    {
        if (PoolAmount == null) return;

        ObjectPool.SharedInstance.AddObjectPool(ProjectileModel, (int)PoolAmount[0]);
        PoolNumber[0] = ObjectPool.SharedInstance.GetObjectPoolNumber(ProjectileModel);
    }

    protected virtual void SetExplosionPool()
    {
        if (PoolAmount == null) return;

        ObjectPool.SharedInstance.AddObjectPool(Explosion, (int)PoolAmount[1]);
        PoolNumber[1] = ObjectPool.SharedInstance.GetObjectPoolNumber(Explosion);
    }

}

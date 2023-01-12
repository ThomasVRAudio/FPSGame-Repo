using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
	private Vector3 _direction;
	protected virtual float Damage => 10f;
	protected virtual float Speed => 2f;

	protected Vector3? Target;

	private IOwner _owner;

	private float _lifeTimer;

	private int? _explosionPool;
	private int? _projectilePool;

	public virtual void Fire(Vector3? target, IOwner owner = null, int? explosionPool = null, int? projectilePool = null)
	{
		_lifeTimer = 0;
		_owner = owner;
		Target = target;

		_explosionPool = explosionPool;
		_projectilePool = projectilePool;

		if (target != null)
			_direction = (Vector3)target - transform.position;	

		ResetRBSpeed();
        SetDestroyTime();
    }

	protected virtual void ResetRBSpeed() { }

	private void Update()
	{
		Movement();
		_lifeTimer += Time.deltaTime;

		if (_lifeTimer > 2)
			gameObject.SetActive(false);
	}

    private void OnTriggerEnter(Collider other) => OnEnter(other);

	protected virtual void Movement()
	{
		if (Target == null) return;

		transform.Translate(Speed * Time.deltaTime * _direction.normalized, Space.World);
		
	}

	protected virtual void SetDestroyTime() => Destroy(gameObject, 5f);

	protected virtual void OnEnter(Collider other)
	{
		if (other.GetComponent<IOwner>() == _owner) return;

		if (other.GetComponent<IDamageable>() != null)
			other.GetComponent<IDamageable>().TakeDamage(Damage);

		OnDestroyProjectile(_explosionPool);
	}

	protected virtual void OnDestroyProjectile(int? _explosionPool) 
	{
		if(_explosionPool == null)
			Destroy(gameObject, 0.1f);

		PoolExplosion();
	}

	protected virtual void PoolExplosion()
	{
        GameObject explosion = ObjectPool.SharedInstance.GetPooledObject(_explosionPool);

        if (explosion != null)
        {
            explosion.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            explosion.SetActive(true);

            SetObjectInactive.Instance.SetInActive(explosion, 0.3f);
        }

        SetObjectInactive.Instance.SetInActive(gameObject, 0.2f);
    }
}

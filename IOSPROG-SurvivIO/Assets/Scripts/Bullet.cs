using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float _lifetime = 3f;

    private float _damage = 10f;
    private GameObject _owner;

    private void Start()
    {
        Destroy(gameObject, _lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_owner != null && (other.gameObject == _owner || other.transform.IsChildOf(_owner.transform)))
        {
            return;
        }

        if (other.GetComponent<Bullet>() != null ||
            other.GetComponentInParent<WeaponPickup>() != null ||
            other.GetComponentInParent<BulletPickup>() != null ||
            other.GetComponentInParent<SpawnManager>() != null)
        {
            return;
        }

        Player hitPlayer = other.GetComponentInParent<Player>();
        if (hitPlayer != null)
        {
            hitPlayer.TakeDamage(_damage);
            Destroy(gameObject);
            return;
        }

        Enemy hitEnemy = other.GetComponentInParent<Enemy>();
        if (hitEnemy != null)
        {
            hitEnemy.TakeDamage(_damage);
            Destroy(gameObject);
            return;
        }

        if (other.isTrigger)
        {
            return;
        }

        Destroy(gameObject);
    }

    public void Setup(float damage, GameObject owner)
    {
        _damage = damage;
        _owner = owner;
    }
}
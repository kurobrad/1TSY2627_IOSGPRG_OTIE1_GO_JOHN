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
        if (other.GetComponent<Bullet>() != null ||
            other.GetComponentInParent<WeaponPickup>() != null ||
            other.GetComponentInParent<BulletPickup>() != null ||
            other.GetComponentInParent<SpawnManager>() != null)
        {
            return;
        }

        // damage players
        Player hitPlayer = other.GetComponentInParent<Player>();
        if (hitPlayer != null)
        {
            if (hitPlayer.gameObject == _owner) return;

            Debug.Log("player was hit");
            hitPlayer.TakeDamage(_damage);
            Destroy(gameObject);
            return;
        }

        // Destroy on environment/obstacles
        Destroy(gameObject);
    }

    public void Setup(float damage, GameObject owner)
    {
        _damage = damage;
        _owner = owner;
    }
}
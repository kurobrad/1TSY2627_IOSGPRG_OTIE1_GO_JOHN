using UnityEngine;

[RequireComponent(typeof(PlayerWeapon))]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [Header("Health Configs")]
    [SerializeField] private float _currentHealth = 100f;
    [SerializeField] private float _maxHealth = 100f;

    public float PlayerHP => _currentHealth;
    public float MaxHP => _maxHealth;
    public PlayerWeapon WeaponManager { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        WeaponManager = GetComponent<PlayerWeapon>();
    }

    public void TakeDamage(float amount)
    {
        _currentHealth = Mathf.Max(0f, _currentHealth - amount);
    }

    public void Heal(float amount)
    {
        _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
    }

    public void AddAmmo(BulletPickup.AmmoType type, int amount)
    {
        WeaponManager?.AddAmmo(type, amount);
    }
}
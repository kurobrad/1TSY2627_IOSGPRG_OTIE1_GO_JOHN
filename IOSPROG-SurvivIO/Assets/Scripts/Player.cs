using UnityEngine;

[RequireComponent(typeof(PlayerWeapon))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _maxHp = 100f;
    private float _currentHp;
    private PlayerWeapon _playerWeapon;

    public float MaxHP => _maxHp;
    public float PlayerHP => _currentHp;

    private void Awake()
    {
        _currentHp = _maxHp;
        _playerWeapon = GetComponent<PlayerWeapon>();
    }

    public void EquipWeaponFromPickup(WeaponPickup.WeaponType weaponType, int loadedAmmo)
    {
        if (_playerWeapon != null)
        {
            _playerWeapon.EquipWeaponFromPickup(weaponType, loadedAmmo);
        }
    }

    public void AddAmmo(BulletPickup.AmmoType ammoType, int amount)
    {
        if (_playerWeapon != null)
        {
            _playerWeapon.AddAmmo(ammoType, amount);
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHp = Mathf.Max(0f, _currentHp - damage);
        Debug.Log($"Player HP: {_currentHp}/{_maxHp}");
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerWeapon))]
[RequireComponent(typeof(PlayerControls))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _maxHp = 100f;
    private float _currentHp;
    private PlayerWeapon _playerWeapon;
    private PlayerControls _playerControls;
    private Rigidbody2D _rb;
    private bool _isDead;

    public float MaxHP => _maxHp;
    public float PlayerHP => _currentHp;

    private void Awake()
    {
        _currentHp = _maxHp;
        _playerWeapon = GetComponent<PlayerWeapon>();
        _playerControls = GetComponent<PlayerControls>();
        _rb = GetComponent<Rigidbody2D>();
        _isDead = false;
    }

    public void EquipWeaponFromPickup(WeaponPickup.WeaponType weaponType, int loadedAmmo)
    {
        if (_isDead) return;

        if (_playerWeapon != null)
        {
            _playerWeapon.EquipWeaponFromPickup(weaponType, loadedAmmo);
        }
    }

    public void AddAmmo(BulletPickup.AmmoType ammoType, int amount)
    {
        if (_isDead) return;

        if (_playerWeapon != null)
        {
            _playerWeapon.AddAmmo(ammoType, amount);
        }
    }

    public void TakeDamage(float damage)
    {
        if (_isDead) return;

        _currentHp = Mathf.Max(0f, _currentHp - damage);
        Debug.Log($"Player HP: {_currentHp}/{_maxHp}");

        if (_currentHp <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        _isDead = true;
        Debug.Log("Player has died!");

        if (_rb != null)
        {
            _rb.linearVelocity = Vector2.zero;
            _rb.simulated = false;
        }

        if (TryGetComponent(out SpriteRenderer sr)) sr.enabled = false;
        if (TryGetComponent(out Collider2D col)) col.enabled = false;

        if (_playerWeapon != null)
        {
            _playerWeapon.enabled = false;
        }

        if (_playerControls != null)
        {
            _playerControls.enabled = false;
        }

        Transform weaponHolder = transform.Find("WeaponHolder");
        if (weaponHolder != null)
        {
            weaponHolder.gameObject.SetActive(false);
        }

        StartCoroutine(CO_DeathDelayRoutine());
    }

    private IEnumerator CO_DeathDelayRoutine()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
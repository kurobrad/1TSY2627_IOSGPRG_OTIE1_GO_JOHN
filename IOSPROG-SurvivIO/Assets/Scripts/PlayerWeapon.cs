using System.Collections;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public enum ActiveSlot
    {
        None,
        Primary,
        Secondary
    }

    public ActiveSlot CurrentSlot { get; private set; } = ActiveSlot.None;
    public WeaponPickup.WeaponType PrimaryWeapon { get; private set; }
    public WeaponPickup.WeaponType SecondaryWeapon { get; private set; }

    public bool HasPrimaryWeapon => _hasPrimaryWeapon;
    public bool HasSecondaryWeapon => _hasSecondaryWeapon;
    public int PrimaryClip => _primaryClip;
    public int SecondaryClip => _secondaryClip;

    public int CurrentClipAmmo => CurrentSlot == ActiveSlot.Primary ? _primaryClip : (CurrentSlot == ActiveSlot.Secondary ? _secondaryClip : 0);
    public int Reserve9mm => _reserve9mm;
    public int Reserve12g => _reserve12g;
    public int Reserve556 => _reserve556;

    public int CurrentReserveAmmo
    {
        get
        {
            if (CurrentSlot == ActiveSlot.None) return 0;

            WeaponPickup.WeaponType activeWep = CurrentSlot == ActiveSlot.Primary ? PrimaryWeapon : SecondaryWeapon;

            if (activeWep == WeaponPickup.WeaponType.Pistol) return _reserve9mm;
            if (activeWep == WeaponPickup.WeaponType.Shotgun) return _reserve12g;
            if (activeWep == WeaponPickup.WeaponType.AutomaticRifle) return _reserve556;

            return 0;
        }
    }

    [Header("Clip Capacities")]
    [SerializeField] private int _clipSizePistol = 15;
    [SerializeField] private int _clipSizeShotgun = 2;
    [SerializeField] private int _clipSizeAuto = 30;

    [Header("Max Bullet Inventory Capacities")]
    [SerializeField] private int _maxCarry9mm = 90;
    [SerializeField] private int _maxCarry12g = 60;
    [SerializeField] private int _maxCarry556 = 120;

    [Header("Bullet Prefabs")]
    [SerializeField] private GameObject _bullet9mmPrefab;
    [SerializeField] private GameObject _bullet12gPrefab;
    [SerializeField] private GameObject _bullet556Prefab;

    [Header("Combat Settings")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _bulletSpeed = 20f;
    [SerializeField] private float _shotgunSpreadAngle = 30f;

    [Header("Weapon Visuals")]
    [SerializeField] private SpriteRenderer _weaponSpriteRenderer;
    [SerializeField] private Sprite _pistolSprite;
    [SerializeField] private Sprite _shotgunSprite;
    [SerializeField] private Sprite _autoSprite;

    private int _primaryClip;
    private int _secondaryClip;
    private int _reserve9mm;
    private int _reserve12g;
    private int _reserve556;
    private float _nextFireTime;

    private bool _hasPrimaryWeapon;
    private bool _hasSecondaryWeapon;
    private bool _isHoldingFireButton;
    private bool _isReloading;

    private void Start()
    {
        CurrentSlot = ActiveSlot.None;
        UpdateWeaponVisuals();
    }

    private void Update()
    {
        if (_isReloading || CurrentSlot == ActiveSlot.None) return;

        if (_isHoldingFireButton && Time.time >= _nextFireTime)
        {
            WeaponPickup.WeaponType activeWeapon = CurrentSlot == ActiveSlot.Primary ? PrimaryWeapon : SecondaryWeapon;

            // only auto rifle fires continously while holding button
            if (activeWeapon == WeaponPickup.WeaponType.AutomaticRifle)
            {
                AttemptFire();
            }
        }
    }

    public void AddAmmo(BulletPickup.AmmoType ammoType, int amount)
    {
        if (ammoType == BulletPickup.AmmoType.NineMM)
        {
            _reserve9mm = Mathf.Min(_reserve9mm + amount, _maxCarry9mm);
        }
        else if (ammoType == BulletPickup.AmmoType.TwelveGauge)
        {
            _reserve12g = Mathf.Min(_reserve12g + amount, _maxCarry12g);
        }
        else if (ammoType == BulletPickup.AmmoType.FiveFiveSix)
        {
            _reserve556 = Mathf.Min(_reserve556 + amount, _maxCarry556);
        }
    }

    public void EquipWeaponFromPickup(WeaponPickup.WeaponType newWeapon, int loadedAmmo)
    {
        if (newWeapon == WeaponPickup.WeaponType.Pistol)
        {
            SecondaryWeapon = newWeapon;
            _secondaryClip = GetMaxClip(newWeapon);
            _hasSecondaryWeapon = true;
            CurrentSlot = ActiveSlot.Secondary;
        }
        else
        {
            PrimaryWeapon = newWeapon;
            _primaryClip = GetMaxClip(newWeapon);
            _hasPrimaryWeapon = true;
            CurrentSlot = ActiveSlot.Primary;
        }

        _isReloading = false;
        UpdateWeaponVisuals();
    }

    public void BTN_SelectPrimaryWeapon()
    {
        if (_hasPrimaryWeapon) SwitchToSlot(ActiveSlot.Primary);
    }

    public void BTN_SelectSecondaryWeapon()
    {
        if (_hasSecondaryWeapon) SwitchToSlot(ActiveSlot.Secondary);
    }

    public void BTN_PointerDownFire()
    {
        _isHoldingFireButton = true;

        if (!_isReloading && Time.time >= _nextFireTime)
        {
            AttemptFire();
        }
    }

    public void BTN_PointerUpFire()
    {
        _isHoldingFireButton = false;
    }

    public void BTN_Reload()
    {
        if (!_isReloading && CurrentReserveAmmo > 0)
        {
            StartCoroutine(CO_ReloadRoutine());
        }
    }

    private void SwitchToSlot(ActiveSlot newSlot)
    {
        CurrentSlot = newSlot;
        _isReloading = false;
        UpdateWeaponVisuals();
    }

    private void AttemptFire()
    {
        if (CurrentSlot == ActiveSlot.None || _isReloading) return;

        WeaponPickup.WeaponType activeWeapon = CurrentSlot == ActiveSlot.Primary ? PrimaryWeapon : SecondaryWeapon;

        if (CurrentClipAmmo > 0)
        {
            if (CurrentSlot == ActiveSlot.Primary) _primaryClip--;
            if (CurrentSlot == ActiveSlot.Secondary) _secondaryClip--;

            SpawnProjectiles(activeWeapon);

            float fireRate = activeWeapon switch
            {
                WeaponPickup.WeaponType.Pistol => 0.25f,
                WeaponPickup.WeaponType.AutomaticRifle => 0.1f,
                WeaponPickup.WeaponType.Shotgun => 0.6f,
                _ => 0.3f
            };

            _nextFireTime = Time.time + fireRate;

            if (CurrentClipAmmo <= 0 && CurrentReserveAmmo > 0)
            {
                StartCoroutine(CO_ReloadRoutine());
            }
        }
        else if (CurrentReserveAmmo > 0)
        {
            StartCoroutine(CO_ReloadRoutine());
        }
    }

    private void SpawnProjectiles(WeaponPickup.WeaponType weaponType)
    {
        if (_firePoint == null) return;

        GameObject chosenPrefab = weaponType switch
        {
            WeaponPickup.WeaponType.Pistol => _bullet9mmPrefab,
            WeaponPickup.WeaponType.Shotgun => _bullet12gPrefab,
            WeaponPickup.WeaponType.AutomaticRifle => _bullet556Prefab,
            _ => null
        };

        if (chosenPrefab == null) return;

        float damage = weaponType == WeaponPickup.WeaponType.AutomaticRifle ? 15f : 10f;

        if (weaponType == WeaponPickup.WeaponType.Shotgun)
        {
            for (int i = 0; i < 6; i++)
            {
                float randomAngle = Random.Range(-_shotgunSpreadAngle / 2f, _shotgunSpreadAngle / 2f);
                Quaternion rotation = _firePoint.rotation * Quaternion.Euler(0, 0, randomAngle);
                CreateBullet(chosenPrefab, rotation, damage);
            }
        }
        else
        {
            CreateBullet(chosenPrefab, _firePoint.rotation, damage);
        }
    }

    private void CreateBullet(GameObject bulletPrefab, Quaternion rotation, float damage)
    {
        GameObject bullet = Instantiate(bulletPrefab, _firePoint.position, rotation);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Setup(damage, gameObject);
        }

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = bullet.transform.up * _bulletSpeed;
        }
    }

    private IEnumerator CO_ReloadRoutine()
    {
        if (CurrentReserveAmmo <= 0 || _isReloading || CurrentSlot == ActiveSlot.None) yield break;

        _isReloading = true;
        WeaponPickup.WeaponType activeWeapon = CurrentSlot == ActiveSlot.Primary ? PrimaryWeapon : SecondaryWeapon;

        float reloadTime = activeWeapon switch
        {
            WeaponPickup.WeaponType.Pistol => 1.5f,
            WeaponPickup.WeaponType.AutomaticRifle => 2.0f,
            WeaponPickup.WeaponType.Shotgun => 2.5f,
            _ => 1.5f
        };

        yield return new WaitForSeconds(reloadTime);

        if (CurrentSlot == ActiveSlot.Primary)
        {
            int maxClip = PrimaryWeapon == WeaponPickup.WeaponType.Shotgun ? _clipSizeShotgun : _clipSizeAuto;
            int bulletsNeeded = maxClip - _primaryClip;

            if (PrimaryWeapon == WeaponPickup.WeaponType.Shotgun && _reserve12g > 0)
            {
                int bulletsToLoad = Mathf.Min(bulletsNeeded, _reserve12g);
                _primaryClip += bulletsToLoad;
                _reserve12g -= bulletsToLoad;
            }
            else if (PrimaryWeapon == WeaponPickup.WeaponType.AutomaticRifle && _reserve556 > 0)
            {
                int bulletsToLoad = Mathf.Min(bulletsNeeded, _reserve556);
                _primaryClip += bulletsToLoad;
                _reserve556 -= bulletsToLoad;
            }
        }
        else if (CurrentSlot == ActiveSlot.Secondary)
        {
            int bulletsNeeded = _clipSizePistol - _secondaryClip;
            if (_reserve9mm > 0)
            {
                int bulletsToLoad = Mathf.Min(bulletsNeeded, _reserve9mm);
                _secondaryClip += bulletsToLoad;
                _reserve9mm -= bulletsToLoad;
            }
        }

        _isReloading = false;
    }

    private void UpdateWeaponVisuals()
    {
        if (_weaponSpriteRenderer == null) return;

        if (CurrentSlot == ActiveSlot.None)
        {
            _weaponSpriteRenderer.sprite = null;
            return;
        }

        WeaponPickup.WeaponType activeWeapon = CurrentSlot == ActiveSlot.Primary ? PrimaryWeapon : SecondaryWeapon;

        _weaponSpriteRenderer.sprite = activeWeapon switch
        {
            WeaponPickup.WeaponType.Pistol => _pistolSprite,
            WeaponPickup.WeaponType.Shotgun => _shotgunSprite,
            WeaponPickup.WeaponType.AutomaticRifle => _autoSprite,
            _ => null
        };
    }

    public int GetMaxClip(WeaponPickup.WeaponType type)
    {
        if (type == WeaponPickup.WeaponType.Pistol) return _clipSizePistol;
        if (type == WeaponPickup.WeaponType.Shotgun) return _clipSizeShotgun;
        return _clipSizeAuto;
    }
}
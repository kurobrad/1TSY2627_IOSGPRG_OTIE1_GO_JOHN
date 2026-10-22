using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WeaponPickup : MonoBehaviour
{
    public enum WeaponType
    {
        Pistol,
        Shotgun,
        AutomaticRifle
    }

    [Header("Weapon Settings")]
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private int _loadedAmmo = 8;

    public WeaponType Type => _weaponType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            player.EquipWeaponFromPickup(_weaponType, _loadedAmmo);

            if (SpawnManager.Instance != null)
            {
                SpawnManager.Instance.NotifyWeaponTaken(_weaponType);
            }

            Destroy(gameObject);
        }
    }
}
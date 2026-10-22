using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BulletPickup : MonoBehaviour
{
    public enum AmmoType
    {
        NineMM,
        TwelveGauge,
        FiveFiveSix
    }

    [Header("Ammo Settings")]
    [SerializeField] private AmmoType _ammoType;
    [SerializeField] private int _amount = 15;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            player.AddAmmo(_ammoType, _amount);

            if (SpawnManager.Instance != null)
            {
                SpawnManager.Instance.NotifyAmmoTaken();
            }

            Destroy(gameObject);
        }
    }
}
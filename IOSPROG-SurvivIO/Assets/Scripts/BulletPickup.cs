using UnityEngine;

public class BulletPickup : MonoBehaviour
{
    public enum AmmoType
    {
        NineMM,
        TwelveGauge,
        FiveFiveSix
    }

    [Header("Loot Configuration")]
    [SerializeField] private AmmoType _type;
    [SerializeField] private int _ammoAmount = 15;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent<Player>(out var player))
        {
            player.AddAmmo(_type, _ammoAmount);
            Destroy(gameObject);
        }
    }
}
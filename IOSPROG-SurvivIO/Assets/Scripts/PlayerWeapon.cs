using System;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public enum ActiveSlot
    {
        None,
        Primary,
        Secondary
    }

    [Header("Ammo Reserves")]
    [SerializeField] private int _reserve9mm = 0;
    [SerializeField] private int _reserve556 = 0;
    [SerializeField] private int _reserve12g = 0;

    public int Reserve9mm => _reserve9mm;
    public int Reserve556 => _reserve556;
    public int Reserve12g => _reserve12g;

    public ActiveSlot CurrentSlot { get; private set; } = ActiveSlot.None;

    public void AddAmmo(BulletPickup.AmmoType type, int amount)
    {
        switch (type)
        {
            case BulletPickup.AmmoType.NineMM:
                _reserve9mm += amount;
                break;
            case BulletPickup.AmmoType.FiveFiveSix:
                _reserve556 += amount;
                break;
            case BulletPickup.AmmoType.TwelveGauge:
                _reserve12g += amount;
                break;
        }
    }
}
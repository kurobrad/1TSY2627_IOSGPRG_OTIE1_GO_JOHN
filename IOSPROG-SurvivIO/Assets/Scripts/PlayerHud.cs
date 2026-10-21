using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [Header("Player Reference")]
    [SerializeField] private Player _player;
    [SerializeField] private PlayerWeapon _weapon;

    [Header("Health UI")]
    [SerializeField] private Slider _healthBarSlider;

    [Header("Ammo Capacity Placeholders")]
    [SerializeField] private TextMeshProUGUI _ammo9mmText;
    [SerializeField] private TextMeshProUGUI _ammo556Text;
    [SerializeField] private TextMeshProUGUI _ammo12gText;

    [Header("Weapon Slot Placeholders")]
    [SerializeField] private TextMeshProUGUI _primarySlotPlaceholder;
    [SerializeField] private TextMeshProUGUI _secondarySlotPlaceholder;

    private void Start()
    {
        if (_player != null && _healthBarSlider != null)
        {
            _healthBarSlider.maxValue = _player.MaxHP;
            _healthBarSlider.value = _player.PlayerHP;
        }
    }

    private void Update()
    {
        UpdateHealthBar();
        UpdateAmmoPlaceholders();
    }

    private void UpdateHealthBar()
    {
        if (_player != null && _healthBarSlider != null)
        {
            _healthBarSlider.value = _player.PlayerHP;
        }
    }

    private void UpdateAmmoPlaceholders()
    {
        if (_weapon == null) return;

        if (_ammo9mmText != null) _ammo9mmText.text = $"9mm: {_weapon.Reserve9mm}";
        if (_ammo556Text != null) _ammo556Text.text = $"5.56mm: {_weapon.Reserve556}";
        if (_ammo12gText != null) _ammo12gText.text = $"12G: {_weapon.Reserve12g}";
    }
}
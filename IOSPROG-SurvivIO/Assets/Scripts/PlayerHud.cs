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

        // auto assign weapon if missing
        if (_weapon == null && _player != null)
        {
            _weapon = _player.GetComponent<PlayerWeapon>();
        }
    }

    private void Update()
    {
        UpdateHealthBar();
        UpdateAmmoPlaceholders();
        UpdateSlotPlaceholders();
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

    private void UpdateSlotPlaceholders()
    {
        if (_weapon == null) return;

        // primary slot display
        if (_primarySlotPlaceholder != null)
        {
            if (_weapon.HasPrimaryWeapon)
            {
                _primarySlotPlaceholder.text = $"Primary: {_weapon.PrimaryWeapon} ({_weapon.PrimaryClip}/{_weapon.GetMaxClip(_weapon.PrimaryWeapon)})";
            }
            else
            {
                _primarySlotPlaceholder.text = "Primary: Empty";
            }
        }

        // secondary slot display
        if (_secondarySlotPlaceholder != null)
        {
            if (_weapon.HasSecondaryWeapon)
            {
                _secondarySlotPlaceholder.text = $"Secondary: {_weapon.SecondaryWeapon} ({_weapon.SecondaryClip}/{_weapon.GetMaxClip(_weapon.SecondaryWeapon)})";
            }
            else
            {
                _secondarySlotPlaceholder.text = "Secondary: Empty";
            }
        }
    }
}
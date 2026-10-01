using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance { get; private set; }

    [Header("Health Settings")]
    [SerializeField] private int _maxHealth = 3;

    [Header("UI References")]
    [Tooltip("Assign if using a UI Slider for Health")]
    [SerializeField] private Slider _healthSlider;

    [Tooltip("Assign if using a UI Filled Image for Health Bar")]
    [SerializeField] private Image _healthFillImage;

    private int _currentHealth;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);
        Debug.Log($"player took damage! current hp: {_currentHealth}/{_maxHealth}");

        UpdateHealthUI();

        if (_currentHealth <= 0 && GameManager.instance != null)
        {
            Debug.Log("game over!");
            GameManager.instance.CallGameOver();
        }
    }

    public void Heal(int amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
        Debug.Log($"player heals current HP: {_currentHealth}/{_maxHealth}");

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        float healthRatio = (float)_currentHealth / _maxHealth;

        if (_healthSlider != null)
        {
            _healthSlider.maxValue = _maxHealth;
            _healthSlider.value = _currentHealth;
        }

        if (_healthFillImage != null)
        {
            _healthFillImage.fillAmount = healthRatio;
        }
    }
}
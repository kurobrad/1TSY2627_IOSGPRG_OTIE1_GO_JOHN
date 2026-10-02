using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DashSystem : MonoBehaviour
{
    public static DashSystem instance { get; private set; }

    [Header("Dash Settings")]
    [SerializeField] private Image _dashFillImage;
    [SerializeField] private Button _dashButton;
    [SerializeField] private float _dashDuration = 3f;
    [SerializeField] private int _maxDashCharge = 10;
    [SerializeField] private int _dashGainOnKill = 1;

    private int _dashCharge = 0;
    private bool _isDashing = false;

    public bool IsDashing => _isDashing;

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
        _dashCharge = 0;
        UpdateDashUI();
    }

    private void Update()
    {
        if (_isDashing)
        {
            AutoSlashEnemies();
        }
    }

    public void OnEnemyKilled()
    {
        AddDashCharge(_dashGainOnKill);
    }

    public void AddDashCharge(int amount)
    {
        if (_isDashing) return;

        _dashCharge = Mathf.Clamp(_dashCharge + amount, 0, _maxDashCharge);
        UpdateDashUI();
    }

    public void BTN_ActivateDash()
    {
        Debug.Log($"current dash charge: {_dashCharge} / {_maxDashCharge}");

        if (_dashCharge < _maxDashCharge || _isDashing)
        {
            Debug.Log("dash failed: charge is not full");
            return;
        }

        StartCoroutine(CO_DashState());
    }

    private IEnumerator CO_DashState()
    {
        _isDashing = true;
        _dashCharge = 0;
        UpdateDashUI();

        yield return new WaitForSeconds(_dashDuration);

        _isDashing = false;
        UpdateDashUI();
    }

    private void AutoSlashEnemies()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
            {
                if (GameManager.instance != null)
                {
                    GameManager.instance.IncrementScore(100);
                }
                Destroy(enemy.gameObject);
            }
        }
    }

    private void UpdateDashUI()
    {
        if (_dashFillImage != null)
        {
            _dashFillImage.fillAmount = (float)_dashCharge / _maxDashCharge;
        }
    }

    public void SetDashGainOnKill(int amount)
    {
        _dashGainOnKill = amount;
    }
}
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float _maxHp = 50f;
    [SerializeField] private float _currentHp;

    public float CurrentHP => _currentHp;
    public float MaxHP => _maxHp;

    private void Awake()
    {
        _currentHp = _maxHp;
    }

    public void TakeDamage(float damage)
    {
        _currentHp -= damage;

        if (_currentHp < 1f)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
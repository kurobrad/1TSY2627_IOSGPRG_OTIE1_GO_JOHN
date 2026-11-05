using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    public enum AIState
    {
        Patrol,
        ChaseAndAttack
    }

    [Header("Current State")]
    [SerializeField] private AIState _currentState = AIState.Patrol;

    [Header("Movement & Radii")]
    [SerializeField] private float _moveSpeed = 3.5f;
    [SerializeField] private float _detectionRadius = 8f;
    [SerializeField] private float _attackRadius = 6f;
    [SerializeField] private float _patrolChangeInterval = 3f;

    [Header("Combat Settings")]
    [SerializeField] private WeaponPickup.WeaponType _assignedWeapon;
    [SerializeField] private GameObject _bullet9mmPrefab;
    [SerializeField] private GameObject _bullet12gPrefab;
    [SerializeField] private GameObject _bullet556Prefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _bulletSpeed = 15f;
    [SerializeField] private float _shotgunSpreadAngle = 30f;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer _weaponSpriteRenderer;
    [SerializeField] private Sprite _pistolSprite;
    [SerializeField] private Sprite _shotgunSprite;
    [SerializeField] private Sprite _autoSprite;

    private Rigidbody2D _rb;
    private Transform _playerTransform;
    private Vector2 _patrolDirection;
    private float _nextPatrolTime;
    private float _nextFireTime;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Player player = FindFirstObjectByType<Player>();
        if (player != null)
        {
            _playerTransform = player.transform;
        }

        AssignRandomGun();
        PickNewPatrolDirection();
    }

    private void Update()
    {
        DetectPlayer();

        switch (_currentState)
        {
            case AIState.Patrol:
                HandlePatrolTimer();
                break;

            case AIState.ChaseAndAttack:
                HandleAttackTiming();
                break;
        }
    }

    private void FixedUpdate()
    {
        if (_currentState == AIState.Patrol)
        {
            _rb.linearVelocity = _patrolDirection * _moveSpeed;
            RotateTowards(_rb.position + _patrolDirection);
        }
        else if (_currentState == AIState.ChaseAndAttack && _playerTransform != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);

            if (distanceToPlayer > _attackRadius * 0.5f)
            {
                Vector2 moveDir = (_playerTransform.position - transform.position).normalized;
                _rb.linearVelocity = moveDir * _moveSpeed;
            }
            else
            {
                _rb.linearVelocity = Vector2.zero;
            }

            RotateTowards(_playerTransform.position);
        }
    }

    private void AssignRandomGun()
    {
        // 0 = Pistol, 1 = Shotgun, 2 = AutomaticRifle
        int randomIndex = Random.Range(0, 3);
        _assignedWeapon = (WeaponPickup.WeaponType)randomIndex;

        if (_weaponSpriteRenderer != null)
        {
            _weaponSpriteRenderer.sprite = _assignedWeapon switch
            {
                WeaponPickup.WeaponType.Pistol => _pistolSprite,
                WeaponPickup.WeaponType.Shotgun => _shotgunSprite,
                WeaponPickup.WeaponType.AutomaticRifle => _autoSprite,
                _ => null
            };
        }
    }

    private void DetectPlayer()
    {
        if (_playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);

        if (distanceToPlayer <= _detectionRadius)
        {
            _currentState = AIState.ChaseAndAttack;
        }
        else if (distanceToPlayer > _detectionRadius * 1.3f)
        {
            _currentState = AIState.Patrol;
        }
    }

    private void HandlePatrolTimer()
    {
        if (Time.time >= _nextPatrolTime)
        {
            PickNewPatrolDirection();
        }
    }

    private void PickNewPatrolDirection()
    {
        _patrolDirection = Random.insideUnitCircle.normalized;
        _nextPatrolTime = Time.time + _patrolChangeInterval;
    }

    private void HandleAttackTiming()
    {
        if (_playerTransform == null) return;

        float distance = Vector2.Distance(transform.position, _playerTransform.position);

        if (distance <= _attackRadius && Time.time >= _nextFireTime)
        {
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        if (_firePoint == null) return;

        GameObject chosenBullet = _assignedWeapon switch
        {
            WeaponPickup.WeaponType.Pistol => _bullet9mmPrefab,
            WeaponPickup.WeaponType.Shotgun => _bullet12gPrefab,
            WeaponPickup.WeaponType.AutomaticRifle => _bullet556Prefab,
            _ => null
        };

        if (chosenBullet == null) return;

        if (_assignedWeapon == WeaponPickup.WeaponType.Shotgun)
        {
            for (int i = 0; i < 6; i++)
            {
                float randomAngle = Random.Range(-_shotgunSpreadAngle / 2f, _shotgunSpreadAngle / 2f);
                Quaternion rot = _firePoint.rotation * Quaternion.Euler(0, 0, randomAngle);
                SpawnBullet(chosenBullet, rot);
            }
            _nextFireTime = Time.time + 1.2f;
        }
        else if (_assignedWeapon == WeaponPickup.WeaponType.AutomaticRifle)
        {
            SpawnBullet(chosenBullet, _firePoint.rotation);
            _nextFireTime = Time.time + 0.18f;
        }
        else
        {
            SpawnBullet(chosenBullet, _firePoint.rotation);
            _nextFireTime = Time.time + 0.6f;
        }
    }

    private void SpawnBullet(GameObject prefab, Quaternion rotation)
    {
        GameObject bullet = Instantiate(prefab, _firePoint.position, rotation);

        if (bullet.TryGetComponent(out Bullet bulletScript))
        {
            bulletScript.Setup(10f, gameObject);
        }

        if (bullet.TryGetComponent(out Rigidbody2D rb))
        {
            rb.linearVelocity = bullet.transform.up * _bulletSpeed;
        }
    }

    private void RotateTowards(Vector3 targetPosition)
    {
        Vector2 dir = (targetPosition - transform.position).normalized;
        if (dir.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            _rb.rotation = angle;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRadius);
    }
}
using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [Header("Weapon Prefabs")]
    [SerializeField] private GameObject _pistolPrefab;
    [SerializeField] private GameObject _shotgunPrefab;
    [SerializeField] private GameObject _autoRiflePrefab;

    [Header("Ammo Prefabs")]
    [SerializeField] private GameObject[] _ammoPrefabs;

    [Header("Obstacle Prefabs & Settings")]
    [SerializeField] private GameObject[] _obstaclePrefabs;
    [SerializeField] private int _obstacleSpawnCount = 15;

    [Header("Map Boundaries for Random Spawning")]
    [SerializeField] private float _xMin = -90f;
    [SerializeField] private float _xMax = 90f;
    [SerializeField] private float _yMin = -45f;
    [SerializeField] private float _yMax = 45f;

    [Header("Spawn Settings")]
    [SerializeField] private bool _spawnOnStart = true;
    [SerializeField] private int _maxWeaponsPerType = 10;
    [SerializeField] private int _maxTotalAmmo = 30;
    [SerializeField] private float _respawnDelay = 2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (_spawnOnStart)
        {
            SpawnAllLoot();
            SpawnObstacles();
        }
    }

    public void SpawnAllLoot()
    {
        SpawnWeaponGroup(_pistolPrefab, _maxWeaponsPerType);
        SpawnWeaponGroup(_shotgunPrefab, _maxWeaponsPerType);
        SpawnWeaponGroup(_autoRiflePrefab, _maxWeaponsPerType);

        if (_ammoPrefabs != null && _ammoPrefabs.Length > 0)
        {
            for (int i = 0; i < _maxTotalAmmo; i++)
            {
                SpawnSingleRandomAmmo();
            }
        }
    }

    private void SpawnObstacles()
    {
        if (_obstaclePrefabs == null || _obstaclePrefabs.Length == 0) return;

        for (int i = 0; i < _obstacleSpawnCount; i++)
        {
            int randomObstacleIndex = Random.Range(0, _obstaclePrefabs.Length);
            GameObject chosenObstacle = _obstaclePrefabs[randomObstacleIndex];

            if (chosenObstacle != null)
            {
                Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
                Instantiate(chosenObstacle, GetRandomPosition(), randomRotation);
            }
        }
    }

    public void NotifyWeaponTaken(WeaponPickup.WeaponType weaponType)
    {
        StartCoroutine(CO_RespawnWeaponRoutine(weaponType));
    }

    public void NotifyAmmoTaken()
    {
        StartCoroutine(CO_RespawnAmmoRoutine());
    }

    private IEnumerator CO_RespawnWeaponRoutine(WeaponPickup.WeaponType type)
    {
        yield return new WaitForSeconds(_respawnDelay);

        GameObject prefabToSpawn = type switch
        {
            WeaponPickup.WeaponType.Pistol => _pistolPrefab,
            WeaponPickup.WeaponType.Shotgun => _shotgunPrefab,
            WeaponPickup.WeaponType.AutomaticRifle => _autoRiflePrefab,
            _ => null
        };

        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, GetRandomPosition(), Quaternion.identity);
        }
    }

    private IEnumerator CO_RespawnAmmoRoutine()
    {
        yield return new WaitForSeconds(_respawnDelay);
        SpawnSingleRandomAmmo();
    }

    private void SpawnSingleRandomAmmo()
    {
        if (_ammoPrefabs != null && _ammoPrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, _ammoPrefabs.Length);
            GameObject chosenAmmo = _ammoPrefabs[randomIndex];

            if (chosenAmmo != null)
            {
                Instantiate(chosenAmmo, GetRandomPosition(), Quaternion.identity);
            }
        }
    }

    private void SpawnWeaponGroup(GameObject weaponPrefab, int count)
    {
        if (weaponPrefab == null) return;

        for (int i = 0; i < count; i++)
        {
            Instantiate(weaponPrefab, GetRandomPosition(), Quaternion.identity);
        }
    }

    public Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(_xMin, _xMax);
        float randomY = Random.Range(_yMin, _yMax);

        return new Vector3(randomX, randomY, 0f);
    }
}
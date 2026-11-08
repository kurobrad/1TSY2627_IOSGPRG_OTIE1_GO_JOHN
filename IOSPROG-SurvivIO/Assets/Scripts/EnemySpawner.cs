using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private int _initialSpawnCount = 10;
    [SerializeField] private int _maxEnemies = 20;
    [SerializeField] private float _spawnInterval = 3.0f;

    [Header("Spawn Area Bounds")]
    [SerializeField] private float _minX = -90f;
    [SerializeField] private float _maxX = 90f;
    [SerializeField] private float _minY = -45f;
    [SerializeField] private float _maxY = 45f;

    private float _nextSpawnTime;

    private void Start()
    {
        for (int i = 0; i < _initialSpawnCount; i++)
        {
            SpawnEnemy();
        }
    }

    private void Update()
    {
        int currentEnemyCount = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude).Length;

        if (currentEnemyCount < _maxEnemies && Time.time >= _nextSpawnTime)
        {
            SpawnEnemy();
            _nextSpawnTime = Time.time + _spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        if (_enemyPrefab == null) return;

        Vector2 randomSpawnPos = new Vector2(
            Random.Range(_minX, _maxX),
            Random.Range(_minY, _maxY)
        );

        Instantiate(_enemyPrefab, randomSpawnPos, Quaternion.identity);
    }
}
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _powerupPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnInterval = 1.5f;

    [Header("Powerup Settings")]
    [SerializeField] private float _powerupSpawnInterval = 10f;

    private float _enemyTimer = 0f;
    private float _powerupTimer = 0f;

    private void Update()
    {
        if (GameManager.instance != null && (GameManager.instance.IsGameOver || GameManager.instance.IsGamePaused))
            return;

        _enemyTimer += Time.deltaTime;
        _powerupTimer += Time.deltaTime;

        if (_enemyTimer >= _spawnInterval)
        {
            SpawnEnemy();
            _enemyTimer = 0f;
        }

        if (_powerupTimer >= _powerupSpawnInterval)
        {
            SpawnPowerup();
            _powerupTimer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        if (_enemyPrefab != null && _spawnPoint != null)
        {
            Instantiate(_enemyPrefab, _spawnPoint.position, Quaternion.identity);
        }
    }

    private void SpawnPowerup()
    {
        if (_powerupPrefab != null && _spawnPoint != null)
        {
            Instantiate(_powerupPrefab, _spawnPoint.position, Quaternion.identity);
        }
    }
}
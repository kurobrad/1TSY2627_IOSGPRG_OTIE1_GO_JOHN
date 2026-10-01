using UnityEngine;

public class Powerup : MonoBehaviour
{
    public enum PowerupType
    {
        Heal,
        FillDashGauge,
        ScoreBonus
    }

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;

    [Header("Powerup Type & Values")]
    [SerializeField] private PowerupType _powerupType = PowerupType.Heal;
    [SerializeField] private int _healAmount = 1;
    [SerializeField] private int _scoreAmount = 100;

    private void Update()
    {
        // move straight down
        transform.position += Vector3.down * _moveSpeed * Time.deltaTime;

        // collect powerup when reach it reaches the player line
        if (transform.position.y < -2.82f)
        {
            Debug.Log("powerup collected!");
            ApplyPowerupEffect();
            Destroy(gameObject);
        }
    }

    private void ApplyPowerupEffect()
    {
        switch (_powerupType)
        {
            case PowerupType.Heal:
                if (PlayerStats.instance != null)
                {
                    PlayerStats.instance.Heal(_healAmount);
                }
                break;

            case PowerupType.ScoreBonus:
                if (GameManager.instance != null)
                {
                    GameManager.instance.IncrementScore(_scoreAmount);
                }
                break;
        }
    }
}
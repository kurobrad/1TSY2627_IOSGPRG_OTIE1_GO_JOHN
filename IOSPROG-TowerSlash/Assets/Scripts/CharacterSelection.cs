using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _playerSpriteRenderer;
    [SerializeField] private Sprite _tankSprite;
    [SerializeField] private Sprite _rogueSprite;
    [SerializeField] private Sprite _fighterSprite;

    private void Start()
    {
        // pause until a hero is picked
        Time.timeScale = 0f;
    }

    public void BTN_SelectTank()
    {
        ApplyHeroChoice(maxHp: 5, dashGainPerKill: 1, sprite: _tankSprite);
        Debug.Log("TANK (5 HP, +1 Dash/Kill)");
    }

    public void BTN_SelectRogue()
    {
        ApplyHeroChoice(maxHp: 2, dashGainPerKill: 2, sprite: _rogueSprite);
        Debug.Log("ROGUE (2 HP, +2 Dash/Kill)");
    }

    public void BTN_SelectFighter()
    {
        ApplyHeroChoice(maxHp: 3, dashGainPerKill: 1, sprite: _fighterSprite);
        Debug.Log("FIGHTER (3 HP, +1 Dash/Kill)");
    }

    private void ApplyHeroChoice(int maxHp, int dashGainPerKill, Sprite sprite)
    {
        // update max HP
        if (PlayerStats.instance != null)
        {
            PlayerStats.instance.SetMaxHealth(maxHp);
        }

        // update dash gain rate
        if (DashSystem.instance != null)
        {
            DashSystem.instance.SetDashGainOnKill(dashGainPerKill);
        }

        // update character sprite
        if (_playerSpriteRenderer != null && sprite != null)
        {
            _playerSpriteRenderer.sprite = sprite;
        }

        // hide panel and unpause game
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
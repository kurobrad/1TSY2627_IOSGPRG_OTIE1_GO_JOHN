using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _pausePanel;

    [Header("Score UI (Optional)")]
    [SerializeField] private TextMeshProUGUI _hudScoreText;
    [SerializeField] private TextMeshProUGUI _gameOverScoreText;

    private int _score = 0;
    private bool _isGameOver = false;
    private bool _isGamePaused = false;

    public bool IsGameOver => _isGameOver;
    public bool IsGamePaused => _isGamePaused;
    public int Score => _score;

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
        Time.timeScale = 1f;
        _isGameOver = false;
        _isGamePaused = false;
        _score = 0;

        if (_gameOverPanel != null) _gameOverPanel.SetActive(false);
        if (_pausePanel != null) _pausePanel.SetActive(false);

        UpdateScoreUI();
    }

    private void Update()
    {
        // Toggle pause via Escape or P key during gameplay
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) && !_isGameOver)
        {
            TogglePause();
        }
    }

    public void IncrementScore(int amount)
    {
        _score += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (_hudScoreText != null)
        {
            _hudScoreText.text = $"Score: {_score}";
        }
    }

    public void CallGameOver()
    {
        if (_isGameOver) return;

        Debug.Log("Game Over Triggered!");
        _isGameOver = true;
        Time.timeScale = 0f;

        if (_gameOverScoreText != null)
        {
            _gameOverScoreText.text = $"Final Score: {_score}";
        }

        if (_gameOverPanel != null)
        {
            _gameOverPanel.SetActive(true);
        }
    }

    public void TogglePause()
    {
        if (_isGameOver) return;

        _isGamePaused = !_isGamePaused;
        Time.timeScale = _isGamePaused ? 0f : 1f;

        if (_pausePanel != null)
        {
            _pausePanel.SetActive(_isGamePaused);
        }

        Debug.Log(_isGamePaused ? "Game Paused" : "Game Resumed");
    }

    public void BTN_Resume()
    {
        if (_isGamePaused)
        {
            TogglePause();
        }
    }

    public void BTN_Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _pausePanel;

    [Header("Score UI")]
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
        _isGameOver = false;
        _isGamePaused = false;
        _score = 0;

        AudioListener.pause = false;

        if (_gameOverPanel != null) _gameOverPanel.SetActive(false);
        if (_pausePanel != null) _pausePanel.SetActive(false);

        UpdateScoreUI();

        if (SoundManager.instance != null)
        {
            SoundManager.instance.ResumeBGM();
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

        if (SoundManager.instance != null)
        {
            SoundManager.instance.PauseBGM();
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

        if (SoundManager.instance != null)
        {
            if (_isGamePaused) SoundManager.instance.PauseBGM();
            else SoundManager.instance.ResumeBGM();
        }

        Debug.Log(_isGamePaused ? "game paused" : "game resumed");
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
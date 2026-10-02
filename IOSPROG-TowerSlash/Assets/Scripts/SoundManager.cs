using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }

    [Header("Audio References")]
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioClip _bgmClip;

    [Header("Settings")]
    [Range(0f, 1f)][SerializeField] private float _bgmVolume = 0.5f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        PlayBGM();
    }

    public void PlayBGM()
    {
        if (_bgmSource == null || _bgmClip == null) return;

        _bgmSource.clip = _bgmClip;
        _bgmSource.loop = true;
        _bgmSource.volume = _bgmVolume;
        _bgmSource.ignoreListenerPause = true;

        if (!_bgmSource.isPlaying)
        {
            _bgmSource.Play();
        }
    }

    public void PauseBGM()
    {
        if (_bgmSource != null && _bgmSource.isPlaying)
        {
            _bgmSource.Pause();
        }
    }

    public void ResumeBGM()
    {
        if (_bgmSource == null) return;

        if (!_bgmSource.isPlaying)
        {
            _bgmSource.UnPause();

            if (!_bgmSource.isPlaying)
            {
                PlayBGM();
            }
        }
    }
}
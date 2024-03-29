using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("AudioManager not initialized");
            return _instance;
        }
    }
    [SerializeField] AudioClip[] _bgm;
    [SerializeField] AudioClip _uiButtonClick;
    [SerializeField] AudioClip _packageDelivered;
    [SerializeField] AudioClip[] _crashFx;
    [SerializeField] AudioSource _bgmSource;
    [SerializeField] AudioSource _sfxSource;
    [SerializeField] AudioSource _carSource;
    int i;

    [SerializeField] AudioMixer _music, _sfx;

    public bool _crashing;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1;
        if (_instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        _instance = this;
    }
    private void Start()
    {
        _crashing = false;
        _carSource = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<AudioSource>();
        i = PlayerPrefs.GetInt("BGM", 0);
        _bgmSource.clip = _bgm[i];
        _bgmSource.Play();
        SetInitialVolume();
    }
    void SetInitialVolume()
    {
        _music.SetFloat("Music", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume", 0.1f)) *20);
        _sfx.SetFloat("SFX", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume", 0.1f)) *20);
    }
    private void Update()
    {
        if (UIManager.Instance.IsPaused())
        {
            if (_carSource)
                _carSource.pitch = Mathf.Lerp(_carSource.pitch, 0.5f, Time.unscaledDeltaTime);
            if (_bgmSource)
                _bgmSource.pitch = Mathf.Lerp(_bgmSource.pitch, 0.5f, Time.unscaledDeltaTime);
        }
        else
        {
            _bgmSource.pitch = Mathf.Lerp(_bgmSource.pitch, 1f, Time.unscaledDeltaTime);
        }
    }
    public int GetMusic()
    {
        return i;
    }
    public void SetMusic(int x)
    {
        i = x;
        _bgmSource.clip = _bgm[i];
        _bgmSource.Play();
    }
    public void nextMusic()
    {
        i++;
        if(i >= _bgm.Length)
        {
            i = 0;
        }
        _bgmSource.clip = _bgm[i];
        _bgmSource.Play();
        PlayerPrefs.SetInt("BGM", GetMusic());
    }

    public void PlaySoundEffect()
    {
        _sfxSource.clip = _uiButtonClick;
        _sfxSource.Play();
    }
    public void PackageDelivered()
    {
        _sfxSource.clip = _packageDelivered;
        _sfxSource.Play();
    }
    public void PlayCrashSound(float speed, float angle)
    {
        _crashing = true;
        float mildThreshold = 5f;
        float moderateThreshold = 20f;
        if (speed < mildThreshold)
        {
            _sfxSource.clip = _crashFx[0];
            _sfxSource.Play();
            Debug.Log("Mild Crash");
        }
        else if (speed >= mildThreshold && speed < moderateThreshold)
        {
            _sfxSource.clip = _crashFx[1];
            _sfxSource.Play(); 
            Debug.Log("Moderate Crash");
        }
        else
        {
            _sfxSource.clip = _crashFx[2];
            _sfxSource.Play();
            Debug.Log("Severe Crash");
        }
        Invoke(nameof(ResetVibrations), .2f);
    }
    public void ResetVibrations()
    {
        _crashing = false;
        Gamepad.current?.SetMotorSpeeds(0, 0);
    }
}
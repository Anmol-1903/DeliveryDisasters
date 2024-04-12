using TMPro;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] TMP_Dropdown _resolutionDropdown, _qualityDropdown;
    [SerializeField] Toggle _fullscreenToggle, _postProcessingBool,_vSyncBool,_fpsBool;
    [SerializeField] Slider _musicSlider, _sfxSlider, _sensitivitySlider;
    [SerializeField] AudioMixer _music, _sfx;
    Resolution[] _resolutions;
    [SerializeField] GameObject _fpsCounter;
    GameObject _postProcessing;
    private void Awake()
    {
        _postProcessing = GameObject.FindGameObjectWithTag("PostProcessVolume");
        #region Resolution
        _resolutions = Screen.resolutions;
        _resolutionDropdown.ClearOptions();
        List<string> _options = new List<string>();
        int _currentResolutionIndex = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string _option = _resolutions[i].width.ToString() + " x " + _resolutions[i].height.ToString();
            _options.Add(_option);
            if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
            {
                _currentResolutionIndex = i;
            }
        }
        _resolutionDropdown.AddOptions(_options);
        _resolutionDropdown.value = _currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();
        #endregion
        FetchData();
        ApplyLoadedSettings();
    }
    public void ClosePauseMenu()
    { 
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SaveData();
    }
    public void OpenPauseMenu()
    {
        Time.timeScale = 0f;
    }
    public void SetQuality(int _quality)
    {
        QualitySettings.SetQualityLevel(_quality);
    }
    public void SetMusic(int _music)
    {
        AudioManager.Instance.nextMusic(_music);
    }
    public void SetFullScreen(bool _fullScreen)
    {
        Screen.fullScreen = _fullScreen;
    }
    public void SetPostProcessing(bool _pp)
    {
        _postProcessing.SetActive(_pp);
    }
    public void SetResolution(int _resolutionIndex)
    {
        Resolution res = _resolutions[_resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
    public void SetSensitivity(float _sensi)
    {
        CinemachineFreeLook freelook = FindObjectOfType<CinemachineFreeLook>();
        if (freelook)
        {
            freelook.m_XAxis.m_MaxSpeed = _sensi * 100;
            freelook.m_YAxis.m_MaxSpeed = _sensi * 1;
        }
    }
    public void UnStuck()
    {
        GameObject _player = GameObject.FindGameObjectWithTag("Player");
        _player.transform.position += Vector3.up;
        _player.transform.eulerAngles = new Vector3(0, _player.transform.eulerAngles.y, 0);
    }
    public void MusicVolume(float _vol)
    {
        _music.SetFloat("Music", Mathf.Log10(_vol) * 20);
    }
    public void AudioVolume(float _vol)
    {
        _sfx.SetFloat("SFX", Mathf.Log10(_vol) * 20);
    }
    public void SetVSync(bool _vsync)
    {
        if (_vsync)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;
    }
    public void SetFPSCounter(bool _on)
    {
        if (_fpsCounter)
        {
            _fpsCounter.SetActive(_on);
        }
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void SaveData()
    {
        PlayerPrefs.SetInt("ResolutionIndex", _resolutionDropdown.value);
        PlayerPrefs.SetInt("QualityIndex", _qualityDropdown.value);
        PlayerPrefs.SetInt("FullScreenBool", (_fullscreenToggle.isOn) ? 1 : 0);
        PlayerPrefs.SetInt("PostProcessingBool", (_postProcessingBool.isOn) ? 1 : 0);
        PlayerPrefs.SetInt("VSyncBool", (_vSyncBool.isOn) ? 1 : 0);
        PlayerPrefs.SetInt("FPSBool", (_fpsBool.isOn) ? 1 : 0);
        PlayerPrefs.SetFloat("MusicVolume", _musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", _sfxSlider.value);
        if (_sensitivitySlider)
            PlayerPrefs.SetFloat("SensitivityValue", _sensitivitySlider.value);
    }
    public void ApplyLoadedSettings()
    {
        SetResolution(PlayerPrefs.GetInt("ResolutionIndex", _resolutionDropdown.options.Count - 1));
        SetQuality(PlayerPrefs.GetInt("QualityIndex", _qualityDropdown.options.Count - 1));
        SetFullScreen((PlayerPrefs.GetInt("FullScreenBool", 1) == 0) ? false : true);
        SetPostProcessing((PlayerPrefs.GetInt("PostProcessingBool", 1) == 0) ? false : true);
        SetVSync((PlayerPrefs.GetInt("VSyncBool", 1) == 0) ? false : true);
        SetFPSCounter((PlayerPrefs.GetInt("FPSBool", 1) == 0) ? false : true);
        MusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.1f));
        AudioVolume(PlayerPrefs.GetFloat("SFXVolume", 0.1f));
        SetSensitivity(PlayerPrefs.GetFloat("SensitivityValue", 3f));
    }
    public void FetchData()
    {
        _resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", _resolutionDropdown.options.Count-1);
        _qualityDropdown.value = PlayerPrefs.GetInt("QualityIndex", _qualityDropdown.options.Count-1);
        _fullscreenToggle.isOn = (PlayerPrefs.GetInt("FullScreenBool", 1) == 0) ? false : true;
        _postProcessingBool.isOn = (PlayerPrefs.GetInt("PostProcessingBool", 1) == 0) ? false : true;
        _vSyncBool.isOn = (PlayerPrefs.GetInt("VSyncBool", 1) == 0) ? false : true;
        _fpsBool.isOn = (PlayerPrefs.GetInt("FPSBool", 1) == 0) ? false : true;
        _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.1f);
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.1f);
        if(_sensitivitySlider)
            _sensitivitySlider.value = PlayerPrefs.GetFloat("SensitivityValue", 3f);
    }
    public void PlaySFX()
    {
        //AudioManager.Instance.PlaySoundEffect();
    }
}
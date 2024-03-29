using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Vector3 _offset;
    [SerializeField] int _totalCars;
    [SerializeField] int _totalTiles;
    [SerializeField] GameObject _settingsPanel;
    [SerializeField] GameObject _UI;
    [SerializeField] GameObject _customize;
    [SerializeField] Slider Hue, Sat, Bri, Met, Smo;
    Animator _anim;
    int _carNumber;
    int _tileNumber;

    [SerializeField] Material[] _carMainMaterial;
    [SerializeField] Image[] _bgs;


    /*
     
     REWRITE THIS ENTIRE CODE WHICH WILL MAKE IT SO THERE'S ONLY ONE CAR ACTIVE IN THE SCENE. THE CAMERA SHOULD STAY FIXED BITCH.
     
     */





    private void Start()
    {
        H = PlayerPrefs.GetFloat("HUE", 0);
        S = PlayerPrefs.GetFloat("SAT", 0);
        V = PlayerPrefs.GetFloat("BRI", 0);
        metalic = PlayerPrefs.GetFloat("METALLIC", 0);
        smoothness = PlayerPrefs.GetFloat("SMOOTHNESS", 0);
        SetMaterial();
        SetSliders();
        Time.timeScale = 1f;
    }

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _carNumber = PlayerPrefs.GetInt("Car", 0);
        _tileNumber = PlayerPrefs.GetInt("Tile", 0);
    }
    public void NextCar()
    {
        if (_carNumber < _totalCars - 1)
        {
            _carNumber++;
        }
        else
        {
            _carNumber = 0;
        }
        PlayerPrefs.SetInt("Car", _carNumber);
    }
    public void PreviousCar()
    {
        if (_carNumber > 0)
        {
            _carNumber--;
        }
        else
        {
            _carNumber = _totalCars - 1;
        }
        PlayerPrefs.SetInt("Car", _carNumber);
    }
    public void NextTile()
    {
        if (_tileNumber < _totalTiles - 1)
        {
            _tileNumber++;
        }
        else
        {
            _tileNumber = 0;
        }
        PlayerPrefs.SetInt("Tile", _tileNumber);
    }
    public void PreviousTile()
    {
        if (_tileNumber > 0)
        {
            _tileNumber--;
        }
        else
        {
            _tileNumber = _totalTiles - 1;
        }
        PlayerPrefs.SetInt("Tile", _tileNumber);
    }
    public void LoadLevel()
    {
        StartCoroutine(LoadingScreen("Game"));
    }
    IEnumerator LoadingScreen(string _sceneName)
    {
        _anim.SetTrigger("StartLoading");
        yield return new WaitForSecondsRealtime(1f);
        AsyncOperation _operation = SceneManager.LoadSceneAsync(_sceneName);
        while (!_operation.isDone)
        {
            yield return null;
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void SettingsMenu()
    {
        _settingsPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void CloseSettingsMenu()
    {
        _settingsPanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void OpenCustomizeMenu()
    {
        _customize.SetActive(true);
        _UI.SetActive(false);
    }
    public void CloseCustomizeMenu()
    {
        _customize.SetActive(false);
        _UI.SetActive(true);
    }
    float H, S, V, metalic, smoothness;
    public void SetMaterial()                //  Materials are updated here
    {
        foreach (Material mat in _carMainMaterial)
        {
            mat.color = Color.HSVToRGB(H, S, V);
            mat.SetFloat("_Metallic", metalic);
            mat.SetFloat("_Smoothness", smoothness);
        }
        foreach(Image bg in _bgs)
        {
            bg.color = Color.HSVToRGB(H, S, V);
        }
    }                  
    public void HueController(float _val)
    {
        H = _val;
        SetMaterial();
        PlayerPrefs.SetFloat("HUE", _val);
    }
    public void SaturationController(float _val)
    {
        S = _val;
        SetMaterial();
        PlayerPrefs.SetFloat("SAT", _val);
    }
    public void BrightnessController(float _val)
    {
        V = _val;
        SetMaterial();
        PlayerPrefs.SetFloat("BRI", _val);
    }
    public void MetallicController(float _val)
    {
        metalic = _val;
        SetMaterial();
        PlayerPrefs.SetFloat("METALLIC", _val);
    }
    public void SmoothnessController(float _val)
    {
        smoothness = _val;
        SetMaterial();
        PlayerPrefs.SetFloat("SMOOTHNESS", _val);
    }
    public void SetSliders()
    {
        Hue.value = PlayerPrefs.GetFloat("HUE", 0);
        Sat.value = PlayerPrefs.GetFloat("SAT", 0);
        Bri.value = PlayerPrefs.GetFloat("BRI", 0);
        Met.value = PlayerPrefs.GetFloat("METALLIC", 0);
        Smo.value = PlayerPrefs.GetFloat("SMOOTHNESS", 0);
    }
}
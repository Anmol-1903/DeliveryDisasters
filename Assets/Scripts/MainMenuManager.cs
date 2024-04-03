using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject Info;
    [SerializeField] GameObject Customize;
    [SerializeField] GameObject Settings;
    [SerializeField] GameObject PlayButton; 

    [Header("Other")]
    [SerializeField] GameObject[] _Cars;
    [SerializeField] Transform _spawnLocation;
    [SerializeField] int _totalTiles;

    int _carNumber;
    int _tileNumber;

    bool s, c;
    ToggleGroup _toggleGroup;
    Animator _anim;

    private void Awake()
    {
        _toggleGroup = GetComponentInChildren<ToggleGroup>();
        _anim = GetComponentInChildren<Animator>();
        _carNumber = PlayerPrefs.GetInt("Car", 0);
        _tileNumber = PlayerPrefs.GetInt("Tile", 0);
    }
    private void Start()
    {
        InstantiateNewCar();
        _anim.SetInteger("Panel", 0);
        Settings.SetActive(false);
        Customize.SetActive(false);
    }
    public void SelectUpdate()
    {
        EventSystem.current.SetSelectedGameObject(PlayButton);
    }
    public void NextCar()
    {
        _carNumber = (_carNumber + 1) % _Cars.Length;
        PlayerPrefs.SetInt("Car", _carNumber);
        InstantiateNewCar();
    }
    public void PreviousCar()
    {
        _carNumber = (_carNumber - 1) < 0 ? _Cars.Length - 1 : _carNumber - 1;
        PlayerPrefs.SetInt("Car", _carNumber);
        InstantiateNewCar();
    }
    void InstantiateNewCar()
    {
        for (int i = 0; i < _Cars.Length; i++)
        {
            _Cars[i].SetActive(false);
        }
        _Cars[_carNumber].SetActive(true);
        _Cars[_carNumber].transform.SetPositionAndRotation(_spawnLocation.position, Quaternion.identity);
    }
    public void NextTile()
    {
        _tileNumber = (_tileNumber - 1) < 0 ? 2 : _tileNumber - 1;
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
    public void OpenSettings(bool on)
    {
        if (on)
        {
            _anim.SetInteger("Panel", 1);
            Settings.SetActive(true);
            Invoke(nameof(DisableCustomize), 1);
            Invoke(nameof(DisableInfo), 1);
        }
    }
    public void OpenCustomize(bool on)
    {
        if (on)
        {
            _anim.SetInteger("Panel", 2);
            Customize.SetActive(true);
            Invoke(nameof(DisableInfo), 1);
            Invoke(nameof(DisableSettings), 1);
            //SetSliders();
        }
    }
    void DisableSettings()
    {
        Settings.SetActive(false);
    }
    void DisableCustomize()
    {
        Customize.SetActive(false);
    }
    void DisableInfo()
    {
        Info.SetActive(false);
    }
    public void SaveAndExit()
    {
        Info.SetActive(true);
        _anim.SetInteger("Panel", 0);
        _toggleGroup.SetAllTogglesOff();
        Invoke(nameof(DisableSettings), 1);
        Invoke(nameof(DisableCustomize), 1);

        SelectUpdate();
    }
    public void OpenInsta()
    {
        Application.OpenURL("https://www.instagram.com/gamedev_anmol/");
    }
    public void OpenItch()
    {
        Application.OpenURL("https://anmol-1903.itch.io/");
    }
    public void OpenDonation()
    {
        Application.OpenURL("https://tr.ee/Rdv7gzw2MQ");
    }
}

    /*[SerializeField] Vector3 _offset;
    [SerializeField] GameObject _settingsPanel;
    [SerializeField] GameObject _UI;
    [SerializeField] GameObject _customize;
    [SerializeField] Slider Hue, Sat, Bri, Met, Smo;
    Animator _anim;

    [SerializeField] Material[] _carMainMaterial;
    [SerializeField] Image[] _bgs;



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
    */

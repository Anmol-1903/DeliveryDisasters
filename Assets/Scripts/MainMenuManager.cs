using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[Serializable]
public class CarInfo
{
    public GameObject car;
    public string name;
    [TextArea(12,10)]
    public string info;
}
public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject Info;
    [SerializeField] GameObject Customize;
    [SerializeField] GameObject Settings;
    [SerializeField] GameObject Credits;
    [SerializeField] GameObject PlayButton;

    [Header("Toggles")]
    [SerializeField] Toggle r;
    [SerializeField] Toggle g,y;

    [Header("Other")]
    [SerializeField] GameObject[] _trafficLights;
    [SerializeField] CarInfo[] _Cars;
    [SerializeField] Transform _spawnLocation;
    [SerializeField] int _totalTiles;
    [SerializeField] TextMeshProUGUI _carName, _carInfo, _terrainName;

    int _carNumber;
    int _tileNumber;

    bool s, c;
    Animator _anim;

    private void Awake()
    {
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
        Credits.SetActive(false);
        _trafficLights[0].SetActive(false);
        _trafficLights[1].SetActive(false);
        _trafficLights[2].SetActive(false);
        UpdateTexts();
        UpdateTerrainTexts();
    }
    void UpdateTexts()
    {
        _carName.text = _Cars[_carNumber].name;
        _carInfo.text = _Cars[_carNumber].info;
    }
    void UpdateTerrainTexts()
    {
        switch (_tileNumber)
        {
            case 0:
                _terrainName.text = "Offroad";
                break;
            case 1:
                _terrainName.text = "Diagonals";
                break;
            case 2:
                _terrainName.text = "Up N Downs";
                break;
        }
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
        UpdateTexts();
    }
    public void PreviousCar()
    {
        _carNumber = (_carNumber - 1) < 0 ? _Cars.Length - 1 : _carNumber - 1;
        PlayerPrefs.SetInt("Car", _carNumber);
        InstantiateNewCar();
        UpdateTexts();
    }
    void InstantiateNewCar()
    {
        for (int i = 0; i < _Cars.Length; i++)
        {
            _Cars[i].car.SetActive(false);
        }
        _Cars[_carNumber].car.SetActive(true);
        _Cars[_carNumber].car.transform.SetPositionAndRotation(_spawnLocation.position, Quaternion.identity);
    }
    public void NextTile()
    {
        _tileNumber = (_tileNumber + 1) >= _totalTiles ? 0 : _tileNumber + 1;
        PlayerPrefs.SetInt("Tile", _tileNumber);
        UpdateTerrainTexts();
    }
    public void PreviousTile()
    {
        _tileNumber = (_tileNumber - 1) < 0 ? 2 : _tileNumber - 1;
        PlayerPrefs.SetInt("Tile", _tileNumber);
        UpdateTerrainTexts();
    }
    public void OpenSettings(bool on)
    {
        if (on)
        {
            _anim.SetInteger("Panel", 1);
            Settings.SetActive(true);
            Invoke(nameof(DisableCustomize), 1);
            Invoke(nameof(DisableInfo), 1);
            Invoke(nameof(DisableCredits), 1);
            _trafficLights[0].SetActive(true);
            _trafficLights[1].SetActive(false);
            _trafficLights[2].SetActive(false);
        }
        else
        {
            SaveAndExit();
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
            Invoke(nameof(DisableCredits), 1);
            _trafficLights[0].SetActive(false);
            _trafficLights[1].SetActive(true);
            _trafficLights[2].SetActive(false);
        }
        else
        {
            SaveAndExit();
        }
    }
    public void OpenCredits(bool on)
    {
        if (on)
        {
            _anim.SetInteger("Panel", 3);
            Credits.SetActive(true);
            Invoke(nameof(DisableInfo), 1);
            Invoke(nameof(DisableSettings), 1);
            Invoke(nameof(DisableCustomize), 1);
            _trafficLights[0].SetActive(false);
            _trafficLights[1].SetActive(false);
            _trafficLights[2].SetActive(true);
        }
        else
        {
            SaveAndExit();
        }
    }
    void DisableSettings()
    {
        EventSystem.current.SetSelectedGameObject(r.gameObject);
        Settings.SetActive(false);
    }
    void DisableCustomize()
    {
        EventSystem.current.SetSelectedGameObject(y.gameObject);
        Customize.SetActive(false);
    }
    void DisableCredits()
    {
        EventSystem.current.SetSelectedGameObject(g.gameObject);
        Credits.SetActive(false);
    }
    void DisableInfo()
    {
        EventSystem.current.SetSelectedGameObject(PlayButton.gameObject);
        Info.SetActive(false);
    }
    public void SaveAndExit()
    {
        if (r.isOn || g.isOn || y.isOn)
            return;
        Info.SetActive(true);
        _anim.SetInteger("Panel", 0);
        Invoke(nameof(DisableSettings), 1);
        Invoke(nameof(DisableCustomize), 1);
        Invoke(nameof(DisableCredits), 1);

        _trafficLights[0].SetActive(false);
        _trafficLights[1].SetActive(false);
        _trafficLights[2].SetActive(false);

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
        Application.OpenURL("https://tr.ee/0V2-YAGCvz");
    }
    public void PlayGame()
    {
        _trafficLights[0].SetActive(true);
        _trafficLights[1].SetActive(true);
        _trafficLights[2].SetActive(true);
        GetComponentInChildren<Animator>().SetTrigger("Start");
    }
}
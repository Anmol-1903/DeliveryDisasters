using UnityEngine;
public class TimeManager : MonoBehaviour
{
    [SerializeField] Material _skybox;
    [SerializeField] GameObject _windZone;
    [SerializeField] StreetLights[] _streetLights;
    [Header("Time")]
    [Tooltip("Day Length in Minutes")]
    [SerializeField]
    private float _targetDayLength = 0.5f; //length of day in minutes
    private void Awake()
    {
        _streetLights = FindObjectsOfType<StreetLights>();
    }
    public float targetDayLength
    {
        get
        {
            return _targetDayLength;
        }
    }
    [SerializeField]
    [Range(0f, 1f)]
    public float _timeOfDay;
    public float timeOfDay
    {
        get
        {
            return _timeOfDay;
        }
    }
    [SerializeField]
    private int _dayNumber = 0; //tracks the days passed
    public int dayNumber
    {
        get
        {
            return _dayNumber;
        }
    }
    [SerializeField]
    private int _yearNumber = 0;
    public int yearNumber
    {
        get
        {
            return _yearNumber;
        }
    }
    private float _timeScale = 100f;
    [SerializeField]
    private int _yearLength = 100;
    public float yearLength
    {
        get
        {
            return _yearLength;
        }
    }
    public bool pause = false;
    [SerializeField]
    private AnimationCurve timeCurve;
    [Header("Sun Light")]
    [SerializeField]
    private Transform dailyRotation;
    [SerializeField]
    private Light sun;
    private float intensity;
    [SerializeField]
    private float sunBaseIntensity = 1f;
    [SerializeField]
    private float sunVariation = 1.5f;
    [SerializeField]
    private Gradient sunColor;
    private void Start()
    {
        Time.timeScale = 1f;
        _skybox = RenderSettings.skybox;
    }
    private void Update()
    {
        if (!pause && UIManager.Instance.GetTime() > 0)
        {
            UpdateTimeScale();
            UpdateTime();
        }
        AdjustSunRotation();
        SunIntensity();
        AdjustSunColor();
        if (_timeOfDay > 0.3f && _timeOfDay <= 0.7f)
        {
            for(int i=0; i < _streetLights.Length; i++)
            {
                _streetLights[i].TurnOffStreetLights();
            }
        }
        else
        {
            for (int i = 0; i < _streetLights.Length; i++)
            {
                _streetLights[i].TurnOnStreetLights();
            }
        }
    }

    private void UpdateTimeScale()
    {
        _timeScale = 24 / (_targetDayLength / 60);
    }

    private void UpdateTime()
    {
        _timeOfDay += Time.deltaTime * _timeScale / 86400;
        if (_timeOfDay > 1) 
        {
            _dayNumber++;
            _timeOfDay -= 1;

            if (_dayNumber > _yearLength) 
            {
                _yearNumber++;
                _dayNumber = 0;
            }
        }
    }
    private void AdjustSunRotation()
    {
        float sunAngle = timeOfDay * 360f;
        dailyRotation.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, sunAngle));
        _windZone.transform.localRotation = Quaternion.Euler(new Vector3(0f,sunAngle ,0f));
    }
    private void SunIntensity()
    {
        intensity = Vector3.Dot(sun.transform.forward, Vector3.down);
        intensity = Mathf.Clamp01(intensity);

        sun.intensity = intensity * sunVariation + sunBaseIntensity;
    }
    private void AdjustSunColor()
    {
        sun.color = sunColor.Evaluate(intensity);
        _skybox.SetColor("_Tint", sunColor.Evaluate(intensity));
    }
}
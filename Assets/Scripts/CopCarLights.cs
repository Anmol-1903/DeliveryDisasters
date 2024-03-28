using UnityEngine;
public class CopCarLights : MonoBehaviour
{
    [SerializeField] GameObject _redLight;
    [SerializeField] GameObject _blueLight;
    float _switchTime = .5f, _counter;
    private void Awake()
    {
        _counter = _switchTime;
    }
    private void Update()
    {
        if (gameObject.activeInHierarchy && _counter <= 0f)
        {
            CopLightSwitch();
        }
        else
        {
            _counter -= Time.deltaTime;
        }
    }
    void CopLightSwitch()
    {
        _counter = _switchTime;
        _blueLight.SetActive(!_blueLight.activeInHierarchy);
        _redLight.SetActive(!_redLight.activeInHierarchy);
    }
}
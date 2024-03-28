using UnityEngine;
public class TrafficLights : MonoBehaviour
{
    [SerializeField] float _redTime, _yellowTime, _greenTime;
    [SerializeField] GameObject _redLight, _yellowLight, _greenLight;
    [SerializeField] float _counter;
    [SerializeField] int _phase;
    private void Update()
    {
        if(_counter <= 0)
        {
            _phase++;
            if(_phase >= 3)
            {
                _phase = 0;
            }
            if (_phase == 0)
            {
                _redLight.SetActive(true);
                _yellowLight.SetActive(false);
                _greenLight.SetActive(false);
                _counter = _redTime;
            }
            else if (_phase == 1)
            {
                _redLight.SetActive(false);
                _yellowLight.SetActive(false);
                _greenLight.SetActive(true);
                _counter = _greenTime;
            }
            else if (_phase == 2)
            {
                _redLight.SetActive(false);
                _yellowLight.SetActive(true);
                _greenLight.SetActive(false);
                _counter = _yellowTime;
            }
        }
        else
        {
            _counter -= Time.deltaTime;
        }
    }
}
using UnityEngine;
public class CarSounds : MonoBehaviour
{
    AudioSource _carSoundSource;
    Rigidbody rb;

    [SerializeField] float _minSpeed, _maxSpeed;
    [SerializeField] float _minPitch, _maxPitch;
    float _currentSpeed, _pitchFromCar;

    private void Awake()
    {
        _carSoundSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        CarEngineSound();
    }
    void CarEngineSound()
    {
        if (UIManager.Instance.IsPaused())
            return;
        _currentSpeed = rb.velocity.magnitude;
        _pitchFromCar = rb.velocity.magnitude / 50;
        if(_currentSpeed <= _minSpeed)
        {
            _carSoundSource.pitch = _minPitch;
        }
        else if(_currentSpeed >= _maxSpeed)
        {
            _carSoundSource.pitch = _maxPitch;
        }
        else
        {
            _carSoundSource.pitch = _minPitch + _pitchFromCar;
        }
    }
}
using UnityEngine;
public class CarSounds : MonoBehaviour
{
    AudioSource _carSoundSource;
    Rigidbody rb;

    [SerializeField] float _minSpeed, _maxSpeed;
    [SerializeField] float _minPitch, _maxPitch;
    float _currentSpeed;

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
        _carSoundSource.pitch = Mathf.Lerp(_minPitch, _maxPitch, _currentSpeed / _maxSpeed);
    }
}
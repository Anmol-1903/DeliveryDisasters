using System;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField] float _maxSpeed = 10f;
    [SerializeField] float _motorForce = 1500f;
    [SerializeField] float _steerAngle = 25f;
    [SerializeField] float _currentSteerAngle;

    [SerializeField] float _minFOV = 40f;
    [SerializeField] float _maxFOV = 80f;

    [SerializeField] float _brakeTorque = 3000f;

    [SerializeField] WheelCollider _fl, _fr, _rl, _rr;
    [SerializeField] Transform _flt, _frt, _rlt, _rrt;

    [SerializeField] GameObject _headLights;
    [SerializeField] GameObject _brakeLights;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] float vibrationThreshold = 0.20f;

    CinemachineFreeLook cinemachineVirtual;
    [SerializeField] CinemachineBasicMultiChannelPerlin noiseProfile;

    CarController carcontroller;

    public GameObject _package;
    public GameObject _boot;
    float _horizontal;
    float _vertical;

    Rigidbody rb;
    float dotProduct;

    public bool hasPackage;
    public bool isHeadlightOn;
    private bool isHandbrakeActivated;
    private bool gamepadConnected = false;

    private void Awake()
    {
        carcontroller = new CarController();
        cinemachineVirtual = FindObjectOfType<CinemachineFreeLook>();
        cinemachineVirtual.Follow = GameObject.FindGameObjectWithTag("Player").transform;
        cinemachineVirtual.LookAt = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0);
        noiseProfile = cinemachineVirtual.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        rb = GetComponent<Rigidbody>();

    }
    private void OnEnable()
    {
        carcontroller.Enable();

        carcontroller.Drive.Vertical.performed += Vertical_performed;
        carcontroller.Drive.Horizontal.performed += Horizontal_performed;
        carcontroller.Drive.Handbrake.performed += Handbrake_performed;
        carcontroller.Drive.Lights.performed += Lights_performed;
        carcontroller.Drive.Pause.performed += Pause_performed;

        InputSystem.onDeviceChange += CheckForGamepad;
        //CheckForGamepad();

        carcontroller.Drive.Vertical.canceled += Vertical_canceled;
        carcontroller.Drive.Horizontal.canceled += Horizontal_canceled;
        carcontroller.Drive.Handbrake.canceled += Handbrake_canceled;
    }

    private void CheckForGamepad(InputDevice device, InputDeviceChange change)
    {
        gamepadConnected = false;

        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad != null)
            {
                gamepadConnected = true;
                // Set vibration to 1 (assuming you have a method to set vibration)
                ApplyGamepadVibration(1);
                break;
            }
        }

        if (!gamepadConnected)
        {
            // No gamepad connected, do something else or log a message
            Debug.Log("No gamepad connected.");
        }
    }

    private void OnDisable()
    {
        carcontroller.Disable();

        carcontroller.Drive.Vertical.performed -= Vertical_performed;
        carcontroller.Drive.Horizontal.performed -= Horizontal_performed;
        carcontroller.Drive.Handbrake.performed -= Handbrake_performed;
        carcontroller.Drive.Lights.performed -= Lights_performed;
        carcontroller.Drive.Pause.performed -= Pause_performed;

        InputSystem.onDeviceChange -= CheckForGamepad;

        carcontroller.Drive.Vertical.canceled -= Vertical_canceled;
        carcontroller.Drive.Horizontal.canceled -= Horizontal_canceled;
        carcontroller.Drive.Handbrake.canceled -= Handbrake_canceled;
    }

    private void Handbrake_canceled(InputAction.CallbackContext obj)
    {
        isHandbrakeActivated = false;
    }
    private void Pause_performed(InputAction.CallbackContext obj)
    {
        UIManager.Instance.Pause();
    }
    private void Horizontal_canceled(InputAction.CallbackContext obj)
    {
        _horizontal = 0;
    }

    private void Vertical_canceled(InputAction.CallbackContext obj)
    {
        _vertical = 0;
    }

    private void Lights_performed(InputAction.CallbackContext obj)
    {
        isHeadlightOn = !isHeadlightOn;
        Headlights();
    }

    private void Handbrake_performed(InputAction.CallbackContext obj)
    {
        isHandbrakeActivated = true;
    }

    private void Horizontal_performed(InputAction.CallbackContext obj)
    {
        _horizontal = obj.ReadValue<float>();
    }

    private void Vertical_performed(InputAction.CallbackContext obj)
    {
        _vertical = obj.ReadValue<float>();
    }

    private void Start()
    {
        if (_package != null)
            _package.SetActive(false);
        if (_boot != null)
            _boot.SetActive(true);
        hasPackage = false;
        ApplyGamepadVibration(0f);
    }
    private void Update()
    {
        ControlVibrations();
        UpdateWheelPos(_rl, _rlt);
        UpdateWheelPos(_rr, _rrt);
        UpdateWheelPos(_fl, _flt);
        UpdateWheelPos(_fr, _frt);

        CameraFov();

        if (!UIManager.Instance.IsPaused())             //While the game is not paused
        {
            Movement();
            Handbrake();
        }
    }
    void CameraFov()
    {
        float currentSpeed = rb.velocity.magnitude;
        cinemachineVirtual.m_Lens.FieldOfView = Mathf.Lerp(_minFOV, _maxFOV, currentSpeed / _maxSpeed * Time.deltaTime);
        if (cinemachineVirtual != null)
        {
            if (noiseProfile != null)
            {
                noiseProfile.m_FrequencyGain = (rb.velocity.magnitude / _maxSpeed * 2.5f) + .2f;
            }
        }
    }
    void Handbrake()
    {
        if (isHandbrakeActivated)
        {
            // Apply the handbrake force to the rear wheels
            _rl.brakeTorque = Mathf.Infinity;
            _rr.brakeTorque = Mathf.Infinity;

            // Disable motor torque
            _rl.motorTorque = 0f;
            _rr.motorTorque = 0f;

            _brakeLights.SetActive(true);
        }
        else
        {
            // Release the handbrake
            _rl.brakeTorque = 0;
            _rr.brakeTorque = 0;
        }
    }
    void Movement()
    {
        Vector3 carVelocity = rb.velocity;
        dotProduct = Vector3.Dot(transform.forward, carVelocity);
        // S braking
        if (_vertical < 0 && dotProduct > 0)
        {
            _brakeLights.SetActive(true);
            _fl.brakeTorque = _brakeTorque;
            _fr.brakeTorque = _brakeTorque;
            _rl.brakeTorque = _brakeTorque;
            _rr.brakeTorque = _brakeTorque;
        }
        // W braking
        else if (_vertical > 0 && dotProduct < 0)
        {
            _brakeLights.SetActive(true);
            _fl.brakeTorque = _brakeTorque;
            _fr.brakeTorque = _brakeTorque;
            _rl.brakeTorque = _brakeTorque;
            _rr.brakeTorque = _brakeTorque;
        }
        else if(!isHandbrakeActivated)
        {
            _brakeLights.SetActive(false);
            _fl.brakeTorque = 0;
            _fr.brakeTorque = 0;
            _rl.brakeTorque = 0;
            _rr.brakeTorque = 0;
        }
        // Movement
        if (rb.velocity.magnitude < _maxSpeed && !isHandbrakeActivated)
        {
            _rl.motorTorque = _vertical * _motorForce;
            _rr.motorTorque = _vertical * _motorForce;
        }
        else
        {
            _rl.motorTorque = 0f;
            _rr.motorTorque = 0f;
        }
        _currentSteerAngle = _steerAngle * _horizontal;

        // steering
        _fl.steerAngle = _currentSteerAngle;
        _fr.steerAngle = _currentSteerAngle;
    }
    void UpdateWheelPos(WheelCollider col, Transform t)
    {
        Vector3 pos;
        Quaternion rot;

        col.GetWorldPose(out pos, out rot);

        t.position = pos;
        t.rotation = rot;
    }
    void Headlights()
    {
        _headLights.SetActive(isHeadlightOn);
    }
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("MAP"))
        {
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("MAP") || other.gameObject.layer == LayerMask.NameToLayer("OBSTACLE"))
        {
            float collisionSpeed = other.relativeVelocity.magnitude;
            float collisionAngle = Vector3.Angle(other.relativeVelocity, transform.forward);

            AudioManager.Instance.PlayCrashSound(collisionSpeed, collisionAngle);
        }
    }
    void ControlVibrations()
    {
        if (IsWheelOnGroundLayer(_fl, groundLayer) || IsWheelOnGroundLayer(_fr, groundLayer) || IsWheelOnGroundLayer(_rl, groundLayer) || IsWheelOnGroundLayer(_rr, groundLayer))
        {
            if (dotProduct / _maxSpeed > vibrationThreshold)
            {
                float vibrationStrength = Mathf.InverseLerp(0, 1, dotProduct);
                Mathf.Clamp(vibrationStrength, 0f, 1f);
                ApplyGamepadVibration(vibrationStrength);
            }
            else
            {
                ApplyGamepadVibration(0f);
            }
        }
        else
        {
            if (dotProduct / _maxSpeed > vibrationThreshold)
            {
                float vibrationStrength = Mathf.InverseLerp(0, 1, dotProduct);
                Mathf.Clamp(vibrationStrength, 0f, 1f);
                ApplyGamepadVibration(vibrationStrength / 2);
            }
            else
            {
                ApplyGamepadVibration(0f);
            }
        }
    }
    void ApplyGamepadVibration(float Strength)
    {
        
    }
    bool IsWheelOnGroundLayer(WheelCollider wheel, LayerMask layerMask)
    {
        WheelHit wheelHit;
        if (wheel.GetGroundHit(out wheelHit))
        {
            int hitLayer = wheelHit.collider.gameObject.layer;
            return layerMask == (layerMask | (1 << hitLayer));
        }
        return false;
    }
}
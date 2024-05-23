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

    [SerializeField] LayerMask groundLayer, roadLayer;
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

    private InputDevice currentInputDevice;

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

        InputSystem.onActionChange += OnActionChange;

        carcontroller.Drive.Vertical.canceled += Vertical_canceled;
        carcontroller.Drive.Horizontal.canceled += Horizontal_canceled;
        carcontroller.Drive.Handbrake.canceled += Handbrake_canceled;
    }


    private void OnDisable()
    {
        carcontroller.Disable();

        carcontroller.Drive.Vertical.performed -= Vertical_performed;
        carcontroller.Drive.Horizontal.performed -= Horizontal_performed;
        carcontroller.Drive.Handbrake.performed -= Handbrake_performed;
        carcontroller.Drive.Lights.performed -= Lights_performed;
        carcontroller.Drive.Pause.performed -= Pause_performed;

        InputSystem.onActionChange -= OnActionChange;

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

        roadLayer = LayerMask.GetMask("Default");

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
        bool isOnRoad = IsWheelOnGroundLayer(_fl, roadLayer) || IsWheelOnGroundLayer(_fr, roadLayer) || IsWheelOnGroundLayer(_rl, roadLayer) || IsWheelOnGroundLayer(_rr, roadLayer);
        bool isOffRoad = IsWheelOnGroundLayer(_fl, groundLayer) || IsWheelOnGroundLayer(_fr, groundLayer) || IsWheelOnGroundLayer(_rl, groundLayer) || IsWheelOnGroundLayer(_rr, groundLayer);

        float maxMotorSpeed = isOffRoad ? 0.5f : 0.25f;
        float adjustedThreshold = isOffRoad ? vibrationThreshold / 2 : vibrationThreshold;

        float speed = Mathf.Abs(dotProduct);
        float normalizedSpeed = Mathf.InverseLerp(_maxSpeed * adjustedThreshold, _maxSpeed, speed);
        float vibrationStrength = Mathf.Clamp(normalizedSpeed, 0f, 1f) * maxMotorSpeed;

        if (!isOnRoad && !isOffRoad)
        {
            Debug.Log("Airborn");
            ApplyGamepadVibration(0f);
        }
        else if (normalizedSpeed > adjustedThreshold)
        {
            Debug.Log("Perfect");
            ApplyGamepadVibration(vibrationStrength);
        }
        else
        {
            Debug.Log("Too Slow");
            ApplyGamepadVibration(0f);
        }
    }

    void ApplyGamepadVibration(float strength)
    {
        if (currentInputDevice is Gamepad)
        {
            Gamepad.current?.SetMotorSpeeds(strength, strength);
        }
        else
        {
            Gamepad.current?.SetMotorSpeeds(0, 0);
        }
    }
    private void OnActionChange(object obj, InputActionChange change)
    {
        if (change == InputActionChange.ActionPerformed)
        {
            var action = obj as InputAction;
            if (action != null)
            {
                currentInputDevice = action.activeControl.device;
            }
        }
    }
    bool IsWheelOnGroundLayer(WheelCollider wheel, LayerMask layerMask)
    {
        RaycastHit hit;
        if (Physics.Raycast(wheel.transform.position, -wheel.transform.up, out hit, wheel.radius + wheel.suspensionDistance))
        {
            return ((1 << hit.collider.gameObject.layer) & layerMask) != 0;
        }
        Debug.DrawRay(wheel.transform.position, -wheel.transform.up, Color.red, wheel.radius + wheel.suspensionDistance);
        return false;
    }
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Gamepad.current?.SetMotorSpeeds(0, 0);
        }
    }
    private void OnApplicationQuit()
    {
        Gamepad.current?.SetMotorSpeeds(0, 0);
    }
}
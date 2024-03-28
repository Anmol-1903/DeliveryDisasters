using Cinemachine;
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

    [SerializeField] CinemachineFreeLook cinemachineVirtual;

    [SerializeField] WheelCollider _fl, _fr, _rl, _rr;
    [SerializeField] Transform _flt, _frt, _rlt, _rrt;

    [SerializeField] GameObject _headLights;
    [SerializeField] GameObject _tailLights;
    [SerializeField] Color _tailLightOnColor;
    [SerializeField] GameObject _brakeLights;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] float vibrationThreshold = 20f;

    public GameObject _package;
    public GameObject _boot;
    float _horizontal;
    float _vertical;

    Rigidbody rb;

    public bool hasPackage;
    public bool isHeadlightOn;
    private bool isHandbrakeActivated;

    private void Awake()
    {
        cinemachineVirtual = FindObjectOfType<CinemachineFreeLook>();
        cinemachineVirtual.Follow = GameObject.FindGameObjectWithTag("Player").transform;
        cinemachineVirtual.LookAt = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0);
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        if (_package != null)
            _package.SetActive(false);
        if (_boot != null)
            _boot.SetActive(true);
        hasPackage = false;
        isHeadlightOn = false;
        isHandbrakeActivated = false;
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

        if (!UIManager.Instance.gameOver)
        {
            Movement();
            if (Input.GetButton("Jump"))
            {
                Handbrake();
            }
            if (Input.GetButtonUp("Jump"))
            {
                ReleaseHandbrake();
            }
            if (Input.GetButtonDown("Submit"))
            {
                Headlights();
            }
        }
    }
    void CameraFov()
    {
        float currentSpeed = rb.velocity.magnitude;
        cinemachineVirtual.m_Lens.FieldOfView = Mathf.Lerp(_minFOV, _maxFOV, currentSpeed / _maxSpeed);
    }
    void Handbrake()
    {
        if (!isHandbrakeActivated)
        {
            // Apply the handbrake force to the rear wheels
            _rl.brakeTorque = Mathf.Infinity;
            _rr.brakeTorque = Mathf.Infinity;

            // Disable motor torque
            _rl.motorTorque = 0f;
            _rr.motorTorque = 0f;

            isHandbrakeActivated = true;

            _brakeLights.SetActive(true);
        }
    }
    void ReleaseHandbrake()
    {
        if (isHandbrakeActivated)
        {
            // Apply the handbrake force to the rear wheels
            _rl.brakeTorque = 0;
            _rr.brakeTorque = 0;

            isHandbrakeActivated = false;

            _brakeLights.SetActive(false);
        }
    }
    void Movement()
    {
        if (UIManager.Instance.GetTime() <= 0)
        {
            UIManager.Instance.GameOverScreen();
        }
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        Vector3 carVelocity = rb.velocity;
        float dotProduct = Vector3.Dot(transform.forward, carVelocity);
        // S braking
        if (_vertical < 0 && dotProduct > 0)
        {
            _brakeLights.SetActive(true);
            isHandbrakeActivated = true;
            _fl.brakeTorque = _brakeTorque;
            _fr.brakeTorque = _brakeTorque;
            _rl.brakeTorque = _brakeTorque;
            _rr.brakeTorque = _brakeTorque;
        }
        // W braking
        else if (_vertical > 0 && dotProduct < 0)
        {
            _brakeLights.SetActive(true);
            isHandbrakeActivated = true;
            _fl.brakeTorque = _brakeTorque;
            _fr.brakeTorque = _brakeTorque;
            _rl.brakeTorque = _brakeTorque;
            _rr.brakeTorque = _brakeTorque;
        }
        else
        {
            _brakeLights.SetActive(false);
            isHandbrakeActivated = false;
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
        if (isHeadlightOn)
        {
            _headLights.SetActive(false);
            _tailLights.GetComponent<Renderer>().materials[0].SetColor("_EmissionColor", Color.black);
            _tailLights.GetComponent<Renderer>().materials[0].DisableKeyword("_EMISSION");
            isHeadlightOn = false;
        }
        else
        {
            _headLights.SetActive(true);
            _tailLights.GetComponent<Renderer>().materials[0].SetColor("_EmissionColor", _tailLightOnColor);
            _tailLights.GetComponent<Renderer>().materials[0].EnableKeyword("_EMISSION");
            isHeadlightOn = true;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("MAP"))
        {
            UIManager.Instance.AddTime(-other.relativeVelocity.magnitude);
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
        if (AudioManager.Instance._crashing)
            return;

        if (IsWheelOnGroundLayer(_fl, groundLayer) || IsWheelOnGroundLayer(_fr, groundLayer) || IsWheelOnGroundLayer(_rl, groundLayer) || IsWheelOnGroundLayer(_rr, groundLayer))
        {
            float currentSpeed = rb.velocity.magnitude;
            if (currentSpeed > vibrationThreshold)
            {
                float vibrationStrength = Mathf.InverseLerp(vibrationThreshold, _maxSpeed, currentSpeed);
                Mathf.Clamp(vibrationStrength, 0f, 1f);
                ApplyGamepadVibration(vibrationStrength / 5);
            }
            else
            {
                ApplyGamepadVibration(0f);
            }
        }
        else
        {
            float currentSpeed = rb.velocity.magnitude;
            if (currentSpeed > vibrationThreshold)
            {
                float vibrationStrength = Mathf.InverseLerp(vibrationThreshold, _maxSpeed, currentSpeed);
                Mathf.Clamp(vibrationStrength, 0f, 1f);
                ApplyGamepadVibration(vibrationStrength / 10);
            }
            else
            {
                ApplyGamepadVibration(0f);
            }
        }
    }
    void ApplyGamepadVibration(float Strength)
    {
        if(!UIManager.Instance.isPaused)
            GamepadHaptics.Instance.SetHaptics(Strength);
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
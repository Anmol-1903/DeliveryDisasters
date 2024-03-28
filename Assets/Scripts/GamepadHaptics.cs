using UnityEngine;
using UnityEngine.InputSystem;
public class GamepadHaptics : MonoBehaviour
{
    private static GamepadHaptics _instance;
    public static GamepadHaptics Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Gamepad not initialized");
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        SetHaptics(0f);
    }
    private void OnDisable()
    {
        SetHaptics(0f);
    }
    public void SetHaptics(float strength)
    {
        Gamepad.current?.SetMotorSpeeds(strength, strength);
    }
}
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
public class InputDetection : MonoBehaviour
{
    public GameObject virtualMouse;
    private bool usingController = false;
    private Vector2 lastMousePosition;
    private void Awake()
    {
        virtualMouse = FindObjectOfType<VirtualMouseInput>().gameObject;
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 || UIManager.Instance.isPaused)
        {
            bool isControllerActive = Gamepad.current?.leftStick.ReadValue() != Vector2.zero;
            Vector2 currentMousePosition = Mouse.current.position.ReadValue();
            if (isControllerActive)
            {
                lastMousePosition = currentMousePosition;
                usingController = true;
            }
            else if (currentMousePosition != lastMousePosition && !isControllerActive)
            {
                lastMousePosition = currentMousePosition;
                usingController = false;
            }
            if (virtualMouse)
            {
                virtualMouse.SetActive(usingController);
            }
            if (Gamepad.current != null && usingController)
            {
                Cursor.visible = false;
            }
            else
            {
                Cursor.visible = true;
            }
            if(Gamepad.current == null)
            {
                virtualMouse.SetActive(false);
            }
        }
        else if (!UIManager.Instance.isPaused)
        {
            virtualMouse.SetActive(false);
            Cursor.visible = false;
        }
    }
}
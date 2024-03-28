using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.LowLevel;

public class VirtualMouse : MonoBehaviour
{
    [SerializeField] RectTransform canvasRectTransform;
    private VirtualMouseInput virtualMouse;
    private void Awake()
    {
        canvasRectTransform = transform.parent.GetComponent<RectTransform>();
        virtualMouse = GetComponent<VirtualMouseInput>();
        DontDestroyOnLoad(transform.parent.gameObject);
    }
    private void Update()
    {
        transform.localScale = Vector3.one * 1 / canvasRectTransform.localScale.x;
        virtualMouse.transform.GetChild(0).localScale = Vector3.one * canvasRectTransform.localScale.x / 1.5f;
        virtualMouse.cursorSpeed = 1000f * canvasRectTransform.localScale.x;
        transform.SetAsLastSibling();
    }
    void LateUpdate()
    {
        Vector2 virtualPosition = virtualMouse.virtualMouse.position.value;
        virtualPosition.x = Mathf.Clamp(virtualPosition.x, 0, Screen.width);
        virtualPosition.y = Mathf.Clamp(virtualPosition.y, 0, Screen.height);
        InputState.Change(virtualMouse.virtualMouse.position, virtualPosition);

        HandleScrolling();
    }
    void HandleScrolling()
    {
        if (Gamepad.current != null)
        {
            float scrollDelta = Gamepad.current.rightStick.y.ReadValue();
            if (scrollDelta != 0f)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    GameObject hoveredObject = EventSystem.current.currentSelectedGameObject;
                    if (hoveredObject != null)
                    {
                        ScrollRect scrollRect = hoveredObject.GetComponentInParent<ScrollRect>();
                        if (scrollRect != null)
                        {
                            scrollRect.verticalNormalizedPosition += scrollDelta * 0.1f;
                        }
                    }
                }
            }
        }
    }
}
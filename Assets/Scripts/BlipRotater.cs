using UnityEngine;
public class BlipRotater : MonoBehaviour
{
    Transform cam;
    private void Start()
    {
        cam = FindObjectOfType<MinimapCam>().transform;
    }
    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(90f, cam.transform.eulerAngles.y, 0f);
    }
}
using UnityEngine;
public class MinimapCam : MonoBehaviour
{
    GameObject player;
    Vector3 offset;
    private void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        offset = new Vector3(0, 500, 0);
    }
    private void LateUpdate()
    {
        transform.SetPositionAndRotation(player.transform.position + offset, Quaternion.Euler(0f, player.transform.eulerAngles.y, 0f));
    }
}
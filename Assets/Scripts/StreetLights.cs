using UnityEngine;
public class StreetLights : MonoBehaviour
{
    public void TurnOffStreetLights()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
    public void TurnOnStreetLights()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
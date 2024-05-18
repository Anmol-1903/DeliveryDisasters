using UnityEngine;

public class StayInside : MonoBehaviour
{
    [SerializeField] float MinimapSize;
    Transform MinimapCam;
    Vector3 TempV3;

    private void Start()
    {
        MinimapCam = FindObjectOfType<MinimapCam>().transform;
    }

    void Update()
    {
        TempV3 = transform.parent.transform.position;
        TempV3.y = transform.position.y;
        transform.position = TempV3;
    }
    void LateUpdate()
    {
        Vector3 centerPosition = MinimapCam.transform.localPosition;
        centerPosition.y -= 100f;
        float Distance = Vector3.Distance(transform.position, centerPosition);
        if (Distance > MinimapSize)
        {
            Vector3 fromOriginToObject = transform.position - centerPosition;
            fromOriginToObject *= MinimapSize / Distance;
            transform.position = centerPosition + fromOriginToObject;
        }
    }
}

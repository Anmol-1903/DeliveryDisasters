using UnityEngine;
public class AnimationEvent : MonoBehaviour
{
    public float _speedUp;
    public void StartCar()
    {
        AutoDrive[] temp = FindObjectsOfType<AutoDrive>();
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i].canMove = true;
        }
    }
    private void Update()
    {
        AutoDrive[] temp = FindObjectsOfType<AutoDrive>();
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i]._speedUp = _speedUp;
        }
    }
}
using UnityEngine;
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] _cars;
    private void Awake()
    {
        Instantiate(_cars[PlayerPrefs.GetInt("Car", 0)], GetComponentInChildren<Transform>().position, Quaternion.identity);
    }
}
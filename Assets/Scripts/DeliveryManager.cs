using UnityEngine;
using System.Collections.Generic;
using System;
public class DeliveryManager : MonoBehaviour
{
    [SerializeField] GameObject _pickupPrefab;
    [SerializeField] GameObject _dropoffPrefab;
    [SerializeField] GameObject _container;

    [SerializeField] List<GameObject> _houses;
    [SerializeField] List<GameObject> _locations;
    [SerializeField] GameObject[] _allHouses;
    [SerializeField] GameObject[] _allLocations;
    [SerializeField] GameObject _selectedHouse;
    [SerializeField] GameObject _selectedLocation;


    Vector3 drop1, drop2;
    float dist;

    public delegate void TimeUpdateEventHandler(int pts);

    public static event TimeUpdateEventHandler OnTimeUpdate;
    private void Start()
    {
        _allHouses = GameObject.FindGameObjectsWithTag("House");
        _allLocations = GameObject.FindGameObjectsWithTag("Waypoints");
        drop1 = Vector3.zero;
        drop2 = Vector3.zero;
        var director = FindObjectOfType<ArrowDirector>();
        GeneratePackage();
        if (director != null)
        {
            director.ReInitializeTarget();
        }
    }
    public void GeneratePackage()
    {
        _houses.Clear();
        foreach (GameObject home in _allHouses)
        {
            _houses.Add(home);
        }
        _selectedHouse = _houses[UnityEngine.Random.Range(0, _houses.Count)];
        _houses.Remove(_selectedHouse);
        _locations.Clear();
        foreach (GameObject loc in _allLocations)
        {
            if (loc.transform.parent == _selectedHouse.transform)
            {
                _locations.Add(loc);
            }
        }
        _selectedLocation = _locations[UnityEngine.Random.Range(0, _locations.Count)];
        _houses.Remove(_selectedLocation);
        drop1 = drop2;
        drop2 = _selectedLocation.transform.position;
        dist = Vector3.Distance(drop1,drop2);
        if (drop1 == Vector3.zero)
        {
            dist = 0;
            UIManager.Instance.UpdateTime(120);
        }
        if (!FindObjectOfType<PlayerController>().hasPackage)
        {
            UIManager.Instance.UpdateTime(Mathf.FloorToInt(dist/2.5f));
            GameObject temp = Instantiate(_pickupPrefab, _selectedLocation.transform.position, Quaternion.identity);
            temp.transform.SetParent(_container.transform);
            if (OnTimeUpdate != null)
            {
                OnTimeUpdate((int)dist);
            }
        }
        else
        {
            GameObject temp = Instantiate(_dropoffPrefab, _selectedLocation.transform.position, Quaternion.identity);
            temp.transform.SetParent(_container.transform);
        }
        var director = FindObjectOfType<ArrowDirector>();
        if(director != null)
        {
            director.ReInitializeTarget();
        }
    }
}
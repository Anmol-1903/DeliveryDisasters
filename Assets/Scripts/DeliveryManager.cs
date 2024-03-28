using UnityEngine;
using System.Collections.Generic;
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
    private void Start()
    {
        _allHouses = GameObject.FindGameObjectsWithTag("House");
        _allLocations = GameObject.FindGameObjectsWithTag("Waypoints");
        GeneratePackage();
        var director = FindObjectOfType<ArrowDirector>();
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
        _selectedHouse = _houses[Random.Range(0, _houses.Count)];
        _houses.Remove(_selectedHouse);
        _locations.Clear();
        foreach (GameObject loc in _allLocations)
        {
            if (loc.transform.parent == _selectedHouse.transform)
            {
                _locations.Add(loc);
            }
        }
        _selectedLocation = _locations[Random.Range(0, _locations.Count)];
        _houses.Remove(_selectedLocation);
        if (!FindObjectOfType<PlayerController>().hasPackage)
        {
            GameObject temp = Instantiate(_pickupPrefab, _selectedLocation.transform.position, Quaternion.identity);
            temp.transform.SetParent(_container.transform);
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
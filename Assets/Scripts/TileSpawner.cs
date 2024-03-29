using UnityEngine;
public class TileSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] _tilePrefab;
    int _tile;

    GameObject temp;

    Transform _nextSpawnPosition;
    Transform _container;

    
    private void Start()
    {
        _tile = PlayerPrefs.GetInt("Tile", 0);
        _container = transform;
        SpawnNewTile();
        SpawnNewTile();
        SpawnNewTile();
    }
    public void SpawnNewTile()
    {
        _tile = PlayerPrefs.GetInt("Tile", 0);
        if (_nextSpawnPosition != null)
        {
            temp = Instantiate(_tilePrefab[_tile], _nextSpawnPosition.position, Quaternion.identity);
        }
        else
        {
            temp = Instantiate(_tilePrefab[_tile], transform.position, Quaternion.identity);
        }
        temp.transform.parent = _container;
        _nextSpawnPosition = temp.transform.GetChild(0);
    }
}
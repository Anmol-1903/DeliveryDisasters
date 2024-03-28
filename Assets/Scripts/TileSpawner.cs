using UnityEngine;
public class TileSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] _tilePrefab;
    [SerializeField] GameObject[] _obstacles;
    [SerializeField] GameObject[] _obstacles1;
    int _tile;

    #region Stuff
    GameObject temp;
    Transform _obstacle1_Min;
    Transform _obstacle1_Max;
    Transform _obstacle2_Min;
    Transform _obstacle2_Max;
    Transform _obstacle3_Min;
    Transform _obstacle3_Max;
    Transform _obstacle4_Min;
    Transform _obstacle4_Max;

    GameObject _obstacle1;
    GameObject _obstacle2;
    GameObject _obstacle3;
    GameObject _obstacle4;

    Vector3 _obstacle1_SpawnPosition;
    Vector3 _obstacle2_SpawnPosition;
    Vector3 _obstacle3_SpawnPosition;
    Vector3 _obstacle4_SpawnPosition;

    Transform _nextSpawnPosition;
    Transform _container;

    Terrain _terrain;
    #endregion
    
    private void Start()
    {
        _tile = PlayerPrefs.GetInt("Tile", 0);
        _container = transform;
        SpawnNewTile();
        SpawnNewTile();
        SpawnNewTile();
        if (_terrain != null)
        {
            GenerateRandomTerrain();
        }
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
        if (_tile == 1)
        {
            SpawnObstacles();
        }
        else if(_tile == 2)
        {
            SpawnObstacles2();
        }
        _terrain = temp.GetComponentInChildren<Terrain>();
    }
    void SpawnObstacles()
    {
        _obstacle1_Min = temp.transform.GetChild(2);
        _obstacle1_Max = temp.transform.GetChild(3);
        _obstacle2_Min = temp.transform.GetChild(4);
        _obstacle2_Max = temp.transform.GetChild(5);
        _obstacle3_Min = temp.transform.GetChild(6);
        _obstacle3_Max = temp.transform.GetChild(7);
        _obstacle4_Min = temp.transform.GetChild(8);
        _obstacle4_Max = temp.transform.GetChild(9);


        _obstacle1 = _obstacles[Random.Range(0, _obstacles.Length)];
        _obstacle2 = _obstacles[Random.Range(0, _obstacles.Length)];
        _obstacle3 = _obstacles[Random.Range(0, _obstacles.Length)];
        _obstacle4 = _obstacles[Random.Range(0, _obstacles.Length)];

        _obstacle1_SpawnPosition = new Vector3(Random.Range(_obstacle1_Min.position.x, _obstacle1_Max.position.x), 0, Random.Range(_obstacle1_Min.position.z, _obstacle1_Max.position.z));
        _obstacle2_SpawnPosition = new Vector3(Random.Range(_obstacle2_Min.position.x, _obstacle2_Max.position.x), 0, Random.Range(_obstacle2_Min.position.z, _obstacle2_Max.position.z));
        _obstacle3_SpawnPosition = new Vector3(Random.Range(_obstacle3_Min.position.x, _obstacle3_Max.position.x), 0, Random.Range(_obstacle3_Min.position.z, _obstacle3_Max.position.z));
        _obstacle4_SpawnPosition = new Vector3(Random.Range(_obstacle4_Min.position.x, _obstacle4_Max.position.x), 0, Random.Range(_obstacle4_Min.position.z, _obstacle4_Max.position.z));

        GameObject o1 = Instantiate(_obstacle1, _obstacle1_SpawnPosition, Quaternion.identity);
        GameObject o2 = Instantiate(_obstacle2, _obstacle2_SpawnPosition, Quaternion.identity);
        GameObject o3 = Instantiate(_obstacle3, _obstacle3_SpawnPosition, Quaternion.identity);
        GameObject o4 = Instantiate(_obstacle4, _obstacle4_SpawnPosition, Quaternion.identity);

        o1.transform.parent = temp.transform;
        o2.transform.parent = temp.transform;
        o3.transform.parent = temp.transform;
        o4.transform.parent = temp.transform;

        o1.isStatic = true;
        o2.isStatic = true;
        o3.isStatic = true;
        o4.isStatic = true;
    }
    void SpawnObstacles2()
    {
        _obstacle1_Min = temp.transform.GetChild(2);

        _obstacle1 = _obstacles1[Random.Range(0, _obstacles1.Length)];

        GameObject o1 = Instantiate(_obstacle1, _obstacle1_Min.position, Quaternion.identity);

        o1.transform.parent = temp.transform;
        o1.isStatic = true;
    }
    private void GenerateRandomTerrain()
    {
        Terrain terrain = temp.GetComponentInChildren<Terrain>();
        TerrainData terrainData = terrain.terrainData;
        int resolution = terrainData.heightmapResolution;
        float[,] heightmap = new float[resolution, resolution];
        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                float randomHeight = Random.Range(0.1f, 0.5f);
                heightmap[x, y] = randomHeight;

                if (x == 0 || x == resolution - 1 || y == 0 || y == resolution - 1)
                {
                    heightmap[x, y] = 0f;
                }
            }
        }
        terrainData.SetHeights(0, 0, heightmap);
    }
}
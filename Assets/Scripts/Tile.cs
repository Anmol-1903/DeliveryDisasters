using UnityEngine;
public class Tile : MonoBehaviour
{
    private void Update()
    {
        transform.Translate(0, 0, -5 * Time.deltaTime);
    }
    private void OnTriggerExit(Collider other)
    {
        GetComponentInParent<TileSpawner>().SpawnNewTile();
        Destroy(gameObject);
    }
}
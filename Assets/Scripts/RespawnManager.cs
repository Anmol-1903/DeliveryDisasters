using UnityEngine;
public class RespawnManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = new Vector3(-25f,82.5f,230f);
            if (other.GetComponent<Rigidbody>())
            {
                other.GetComponent<Rigidbody>().velocity = Vector3.zero;
                other.transform.rotation = Quaternion.identity;
            }
        }
    }
}
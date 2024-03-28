using UnityEngine;
public class Delivery : MonoBehaviour
{
    [SerializeField] bool isPackage;
    private void Start()
    {
        if (!isPackage)
        {
            UIManager.Instance.AddTime(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) / 3);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().hasPackage = isPackage;
            if (other.GetComponent<PlayerController>().hasPackage)
            {
                other.GetComponent<Rigidbody>().mass *= 1.5f;
            }
            else
            {
                other.GetComponent<Rigidbody>().mass /= 1.5f;
            }
            if (!isPackage)
            {
                UIManager.Instance.UpdateScore(15);                 //   Time based scoring?
                AudioManager.Instance.PackageDelivered();
            }

            if(other.GetComponent<PlayerController>()._package)
                other.GetComponent<PlayerController>()._package.SetActive(isPackage);

            if(other.GetComponent<PlayerController>()._boot)
                other.GetComponent<PlayerController>()._boot.SetActive(!isPackage);

            FindObjectOfType<DeliveryManager>().GeneratePackage();
            Destroy(gameObject);
        }
    }
}
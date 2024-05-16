using UnityEngine;
public class Delivery : MonoBehaviour
{
    [SerializeField] bool isPackage;
    GameObject package;
    GameObject boot;
    Rigidbody rb;
    private void Awake()
    {
        PlayerController controller = FindObjectOfType<PlayerController>();
        package = controller._package;
        boot = controller._boot;
        rb = controller.GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().hasPackage = isPackage;
            if (isPackage)
            {
                rb.mass += 1000;
            }
            else
            {
                rb.mass -= 1000;
                AudioManager.Instance?.PackageDelivered();
            }

            if(package)
                package.SetActive(isPackage);
            if(boot)
                boot.SetActive(!isPackage);

            FindObjectOfType<DeliveryManager>().GeneratePackage();
            Destroy(gameObject);
        }
    }
}
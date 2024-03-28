using UnityEngine;
public class ArrowDirector : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] Transform _target;
    public void ReInitializeTarget()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _target = FindObjectOfType<Delivery>().transform;
    }
    private void Update()
    {
        if (_player != null && _target != null)
        {
            Vector3 _dir = _player.position - _target.position;
            Quaternion targetRotation = Quaternion.LookRotation(_dir, Vector3.up);
            float yRotation = targetRotation.eulerAngles.y;
            transform.eulerAngles = new Vector3(0, yRotation, 0);
        }
    }
}
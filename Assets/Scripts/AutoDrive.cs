using UnityEngine;
public class AutoDrive : MonoBehaviour
{
    [SerializeField] WheelCollider _fl, _fr, _rl, _rr;
    [SerializeField] Transform _flt, _frt, _rlt, _rrt;
    [SerializeField] float _speed;
    public bool canMove;
    public float _speedUp;
    private void Start()
    {
        canMove = false;
    }
    private void Update()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        if (!canMove)
            transform.position = new Vector3(0, transform.position.y, 45);
        else
            transform.position = new Vector3(0, transform.position.y, Mathf.Lerp(transform.position.z, 75, Mathf.Lerp(0, 1, _speedUp * Time.deltaTime) * Time.deltaTime));

        _fl.rotationSpeed = _speed;
        _rl.rotationSpeed = _speed;
        _fr.rotationSpeed = _speed;
        _rr.rotationSpeed = _speed;

        UpdateWheelPos(_rl, _rlt);
        UpdateWheelPos(_rr, _rrt);
        UpdateWheelPos(_fl, _flt);
        UpdateWheelPos(_fr, _frt);
    }
    void UpdateWheelPos(WheelCollider col, Transform t)
    {
        Vector3 pos;
        Quaternion rot;

        col.GetWorldPose(out pos, out rot);

        t.position = pos;
        t.rotation = rot;
    }
}
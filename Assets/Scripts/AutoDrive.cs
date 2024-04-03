using UnityEngine;
public class AutoDrive : MonoBehaviour
{
    float _timer;
    [SerializeField] GameObject _headlights;

    [SerializeField] WheelCollider _fl, _fr, _rl, _rr;
    [SerializeField] Transform _flt, _frt, _rlt, _rrt;
    [SerializeField] float _speed;
    private void Update()
    {
        if(_timer == 0)
        {
            _headlights.SetActive(!_headlights.activeInHierarchy);
            _timer = 10;
        }
        else
        {
            _timer -= Time.deltaTime;
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        transform.position = new Vector3(0, transform.position.y, 45);

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
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.2f;

    private float _currentVelocity;
    private float _offset;

    private void Start() {
        _offset = transform.position.z - target.position.z;
    }
    
    protected void Update()
    {
        float newZ = Mathf.SmoothDamp(
            transform.position.z,
            target.position.z + _offset,
            ref _currentVelocity,
            smoothTime,
            Mathf.Infinity);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
    }
}

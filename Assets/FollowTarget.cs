using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float maxSpeed = 0.2f;

    private Vector3 _currentVelocity;
    private Vector3 _offset;

    private void Start() {
        _offset = transform.position - target.position;
    }
    
    protected void Update()
    {
        transform.position = Vector3.SmoothDamp(
            transform.position,
            target.position + _offset,
            ref _currentVelocity,
            maxSpeed);
    }
}

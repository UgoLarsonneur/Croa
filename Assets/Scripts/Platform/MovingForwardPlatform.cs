using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingForwardPlatform : Platform
{
    [SerializeField] float minMoveSpeed;
    [SerializeField] float maxMoveSpeed;

    float _moveSpeed;

    private void Awake() {
        _moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }

    void Update()
    {
        transform.position -= Vector3.forward * _moveSpeed * Time.deltaTime;
    }
}

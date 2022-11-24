using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLine : MonoBehaviour
{
    [SerializeField] private Transform point0;
    [SerializeField] private Transform point1;
    [SerializeField] private float scrollSpeed;

    private LineRenderer _lineRenderer;
    private bool _isCharging = false;

    private void Awake() {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
        _lineRenderer.positionCount = 2;
    }

    private void Start() {
        EventManager.StartListening("Charge", OnCharge);
        EventManager.StartListening("Jump", OnJump);
    }

    private void OnCharge()
    {
        _isCharging = true;
        _lineRenderer.enabled = true;
        UpdatePos();
    }

    private void OnJump()
    {
        _isCharging = false;
        _lineRenderer.enabled = false;
    }
    protected void LateUpdate()
    {
        if(_isCharging)
        {
            UpdatePos();
            _lineRenderer.material.mainTextureOffset -= new Vector2(scrollSpeed * Time.deltaTime, 0f);
        }
    }

    private void UpdatePos()
    {
        _lineRenderer.SetPosition(1, point1.position);
        _lineRenderer.SetPosition(0, point0.position);
    }
}

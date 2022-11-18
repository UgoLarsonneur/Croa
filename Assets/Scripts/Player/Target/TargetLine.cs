using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLine : TargetUI
{
    [SerializeField] private Transform point0;
    [SerializeField] private Transform point1;
    [SerializeField] private float scrollSpeed;
    
    private Material _material;

    public override void Awake() {
        base.Awake();
        LineRenderer lineRenderer = (LineRenderer)_renderer;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    protected override void OnShow()
    {
        LineRenderer lineRenderer = (LineRenderer)_renderer;
        lineRenderer.SetPosition(0, point0.position);
        lineRenderer.SetPosition(1, point1.position);
        lineRenderer.material.mainTextureOffset -= new Vector2(scrollSpeed * Time.deltaTime, 0f);
    }
}

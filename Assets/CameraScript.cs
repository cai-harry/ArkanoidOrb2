using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Vector3 positionAtMinZoom;
    public Vector3 positionAtMaxZoom;

    public float zoomLevel;
    public float zoomChangeSpeed;

    public GameObject stage;

    void Start()
    {
        UpdateTransformFromZoomLevel();
    }

    private void FixedUpdate()
    {
        var vertical = Input.GetAxis("Vertical");
        zoomLevel -= zoomChangeSpeed * vertical;
        zoomLevel = Mathf.Clamp(zoomLevel, 0f, 1f);
        UpdateTransformFromZoomLevel();
    }

    private void UpdateTransformFromZoomLevel()
    {
        transform.position = Vector3.Lerp(positionAtMinZoom, positionAtMaxZoom, zoomLevel);
        transform.LookAt(stage.transform);
    }
}
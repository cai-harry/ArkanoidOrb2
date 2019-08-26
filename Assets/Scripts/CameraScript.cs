using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Vector3 positionAtMinHeight;
    public Vector3 positionAtMaxHeight;

    public float heightLevel;
    public float heightChangeSpeed;

    public GameObject stage;

    void Start()
    {
        UpdateTransformFromZoomLevel();
    }

    private void FixedUpdate()
    {
        var vertical = Input.GetAxis("Vertical");
        heightLevel += heightChangeSpeed * vertical;
        heightLevel = Mathf.Clamp(heightLevel, 0f, 1f);
        UpdateTransformFromZoomLevel();
    }

    private void UpdateTransformFromZoomLevel()
    {
        transform.position = Vector3.Lerp(positionAtMinHeight, positionAtMaxHeight, heightLevel);
        transform.LookAt(stage.transform);
    }
}
using System;
using UnityEngine;

public class Paddles : MonoBehaviour
{
    public float rotateSpeed;

    public float finalPaddleLightIntensity;
    
    public Light leftPaddleLight;
    public Light rightPaddleLight;

    private float _deltaRotationY;

    public float DeltaRotationY => _deltaRotationY;

    void FixedUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");
        _deltaRotationY = horizontal * rotateSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, _deltaRotationY);
    }

    private void Update()
    {
        var mainScript = transform.parent.gameObject.GetComponent<MainScript>();
        var lightMultiplier = 1 - mainScript.GetDaylightIntensity();
        leftPaddleLight.intensity = lightMultiplier * finalPaddleLightIntensity;
        rightPaddleLight.intensity = lightMultiplier * finalPaddleLightIntensity;
    }
}
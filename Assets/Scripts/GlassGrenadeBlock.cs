using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassGrenadeBlock : MultiHitBlock
{
    public Light blockLight;

    public float baseLightIntensity;
    public float lightIntensityOnHit;
    public float lightIntensityDecreaseRate;

    protected override void Start()
    {
        base.Start();
        ResetLightIntensity();
    }

    protected override void OnNthBallCollisionExit(int n)
    {
        switch (n)
        {
            case 1:
                SetToLightIntensityOnHit();
                break;
            case 2:
                SetToLightIntensityOnHit();
                break;
            case 3:
                SetToLightIntensityOnHit();
                break;
            case 4:
                ResetLightIntensity();
                ChangeMaterial(secondMaterial);
                break;
            case 5:
                OnBlockDestroyed();
                break;
            default:
                Debug.LogError($"Invalid ball collision number on {gameObject.name}");
                break;
        }
    }

    private void SetToLightIntensityOnHit()
    {
        blockLight.intensity = lightIntensityOnHit;
    }

    protected override void Update()
    {
        base.Update();
        blockLight.intensity -= lightIntensityDecreaseRate * Time.deltaTime;
        if (blockLight.intensity < baseLightIntensity)
        {
            ResetLightIntensity();
        }
    }

    private void ResetLightIntensity()
    {
        blockLight.intensity = baseLightIntensity;
    }
}

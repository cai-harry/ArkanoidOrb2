using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassGrenadeBlock : RockBlock
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

    protected override void OnBallCollisionExit(Collision ball)
    {
        base.OnBallCollisionExit(ball);
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

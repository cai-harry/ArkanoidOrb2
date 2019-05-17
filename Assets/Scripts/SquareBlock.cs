using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SquareBlock : SpinningBlock
{
    public Light[] lights;

    protected override void Start()
    {
        base.Start();
        foreach (var light in lights)
        {
            light.color = Random.ColorHSV(
                0f, 1f, 0.8f, 1f, 1f, 1f);
        }
    }

    protected override void OnBlockDestroyed()
    {
        foreach (var light in lights)
        {
            light.enabled = false;
        }
        base.OnBlockDestroyed();
    }
}

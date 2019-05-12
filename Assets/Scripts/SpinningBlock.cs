using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpinningBlock : NormalBlock
{
    public float MinSpinSpeed;
    public float MaxSpinSpeed;
    public bool SpinningFromStart;

    private float _spinSpeed;
    private bool _spinning;

    protected override void Start()
    {
        base.Start();
        _spinSpeed = Random.Range(MinSpinSpeed, MaxSpinSpeed);
        _spinning = false;

        if (SpinningFromStart)
        {
            StartSpinning();
        }
    }

    protected void FixedUpdate()
    {
        if (_spinning)
        {
            transform.Rotate(Vector3.up, _spinSpeed);
        }
    }

    public void StartSpinning()
    {
        _spinning = true;
    }
}

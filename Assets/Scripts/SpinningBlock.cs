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
            Debug.Log($"Starting spinning with speed {_spinSpeed}");
            StartSpinning();
        }
    }

    protected override void OnBallCollisionExit(Collision ball)
    {
        base.OnBallCollisionExit(ball);
        var ballScript = ball.gameObject.GetComponent<Ball>();
        ballScript.Spin = _spinSpeed;
    }

    protected void FixedUpdate()
    {
        if (_spinning)
        {
            Debug.Log($"Spinning with speed {_spinSpeed}");
            transform.Rotate(Vector3.up, _spinSpeed);
        }
    }

    public void StartSpinning()
    {
        _spinning = true;
    }
}

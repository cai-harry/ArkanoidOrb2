using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBlock : MultiHitBlock
{
    public GameObject ball;

    public int numBallsReleased;

    private bool _ballsReleased;

    protected override void Start()
    {
        base.Start();
        _ballsReleased = false;
    }

    protected override void OnBlockDestroyed()
    {
        base.OnBlockDestroyed();
        if (!_ballsReleased)
        {
            for (int i = 0; i < numBallsReleased; i++)
            {
                Vector3 offset = i * ball.transform.localScale.y * Vector3.up;
                Instantiate(ball, transform.position + offset, Quaternion.identity);
            }
            _ballsReleased = true;
        }
    }
}
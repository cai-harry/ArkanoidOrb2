using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBlock : NormalBlock
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
            var newBall = Instantiate(ball, transform.position, Quaternion.identity);
            newBall.SendMessage("SetOnFire");
            _ballsReleased = true;
        }
    }
}
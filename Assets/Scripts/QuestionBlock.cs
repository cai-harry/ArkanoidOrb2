using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBlock : RockBlock
{
    protected override void OnBallCollisionExit(Collision ball)
    {
        base.OnBallCollisionExit(ball);

        DoRandomThing();
    }

    private void DoRandomThing()
    {
        Debug.Log("Need to decide what this does");
    }
}

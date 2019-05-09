using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBlock : NormalBlock
{
    protected override void OnBallCollisionExit(Collision ball)
    {
        SendMessageUpwards("InstantiateBlockRing", Vector3.zero);
        base.OnBallCollisionExit(ball);
    }

}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBlock : NormalBlock
{
    public GameObject normalBlock;

    protected override void OnBallCollisionExit(Collision ball)
    {
        SendMessageUpwards("InstantiateBlockRing", transform.position);
        base.OnBallCollisionExit(ball);
    }
}
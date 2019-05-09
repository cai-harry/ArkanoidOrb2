using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBlock : NormalBlock
{
    protected override void OnBallCollisionExit(Collision ball)
    {
        Debug.Log("Sending Message Upwards");
        SendMessageUpwards("InstantiateBlockRing", transform.position);
        base.OnBallCollisionExit(ball);
    }
}
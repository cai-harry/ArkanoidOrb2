using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBlock : NormalBlock
{
    protected override void OnBallCollisionExit(Collision ball)
    {
        var mainScript = transform.parent.gameObject.GetComponent<MainScript>();
        mainScript.InstantiateBlockRing(transform.position);
        base.OnBallCollisionExit(ball);
    }
}
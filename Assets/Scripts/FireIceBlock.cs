using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireIceBlock : NormalBlock
{
    protected override void OnBallCollisionExit(Collision ball)
    {
        if (onFire || frozen)
        {
            return;
        }
        OnBlockDestroyed();
    }
}

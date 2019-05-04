using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBlock : NormalBlock
{
    public int instantiateNumBlocks;
    public float instantiateBlocksRadius;
    public float instantiateBlocksSpaceRadius;

    public GameObject normalBlock;

    protected override void OnBallCollisionExit(Collision ball)
    {
        base.OnBallCollisionExit(ball);

        for (int i = 0; i < instantiateNumBlocks; i++)
        {
            var angle = 2 * Mathf.PI * i / instantiateNumBlocks;
            var offsetDirection = new Vector3(
                Mathf.Sin(angle),
                0,
                Mathf.Cos(angle)
            );
            var offset = offsetDirection * instantiateBlocksRadius;
            var position = transform.position + offset;

            // TODO: should refer to MainScript.blockSpaceRadius
            if (position.magnitude < instantiateBlocksSpaceRadius)
            {
                Instantiate(normalBlock, position, Quaternion.identity);
            }
        }
    }
}
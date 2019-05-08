using UnityEngine;

public class BallBlock : NormalBlock
{
    public float delayBeforeDelete;

    public GameObject ball;

    protected override void DestroySelf()
    {
        Instantiate(ball, transform.position, Quaternion.identity);
        base.DestroySelf();
    }
}
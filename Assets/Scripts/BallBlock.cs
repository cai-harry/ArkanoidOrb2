using UnityEngine;

public class BallBlock : NormalBlock
{
    public float delayBeforeDelete;

    public GameObject ball;

    private bool _ballReleased;

    protected override void Start()
    {
        base.Start();
        _ballReleased = false;
    }

    protected override void OnBlockDestroyed()
    {
        base.OnBlockDestroyed();
        if (!_ballReleased)
        {
            Instantiate(ball, transform.position, Quaternion.identity);
            _ballReleased = true;
        }
    }
}
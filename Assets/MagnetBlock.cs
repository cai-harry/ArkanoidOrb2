using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class MagnetBlock : NormalBlock
{
    public float minRepulsion;
    public float maxRepulsion;
    public float magneticRadius;

    public ParticleSystem lightning;

    private float _repulsionForce;

    protected override void Start()
    {
        base.Start();
        _repulsionForce = Random.Range(minRepulsion, maxRepulsion);
    }

    private void FixedUpdate()
    {
        foreach (var ball in GameObject.FindGameObjectsWithTag("Ball"))
        {
            var ballRigidBody = ball.gameObject.GetComponent<Rigidbody>();
            ballRigidBody.AddExplosionForce(_repulsionForce, transform.position, magneticRadius);
            var lightningVelocity = lightning.velocityOverLifetime;
            var positionDeltaToBall = ball.transform.position - transform.position;
            lightningVelocity.x = positionDeltaToBall.x / ball.transform.localScale.x;
            lightningVelocity.y = positionDeltaToBall.y / ball.transform.localScale.y;
            lightningVelocity.z = positionDeltaToBall.z / ball.transform.localScale.z;
        }
    }
}
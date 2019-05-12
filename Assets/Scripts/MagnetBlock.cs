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
    public Vector3 lightningVelocityMultiplier;

    private float _repulsionForce;

    protected override void Start()
    {
        base.Start();
        _repulsionForce = Random.Range(minRepulsion, maxRepulsion);
    }

    protected override void Update()
    {
        base.Update();
        List<GameObject> ballsInRange = new List<GameObject>();
        foreach (var ball in GameObject.FindGameObjectsWithTag("Ball"))
        {
            var positionDeltaToBall = ball.transform.position - transform.position;
            if (positionDeltaToBall.magnitude < magneticRadius)
            {
                ballsInRange.Add(ball);
                var ballScript = ball.GetComponent<Ball>();
                ballScript.DisplayElectric(-positionDeltaToBall);
            }
        }

        var lightningVelocity = lightning.velocityOverLifetime;
        if (ballsInRange.Count > 0)
        {
            var ball = ChooseRandomFrom(ballsInRange);
            var positionDeltaToBall = ball.transform.position - transform.position;
            lightningVelocity.x =
                lightningVelocityMultiplier.x * positionDeltaToBall.x / ball.transform.localScale.x;
            lightningVelocity.y =
                lightningVelocityMultiplier.y * positionDeltaToBall.y / ball.transform.localScale.y;
            lightningVelocity.z =
                lightningVelocityMultiplier.z * positionDeltaToBall.z / ball.transform.localScale.z;
        }
        else
        {
            lightningVelocity.x = 0;
            lightningVelocity.y = 0;
            lightningVelocity.z = 0;
        }
    }

    private static GameObject ChooseRandomFrom(List<GameObject> list)
    {
        var randomIndex = Random.Range(0, list.Count);
        return list[randomIndex];
    }

    private void FixedUpdate()
    {
        foreach (var ball in GameObject.FindGameObjectsWithTag("Ball"))
        {
            var ballRigidBody = ball.gameObject.GetComponent<Rigidbody>();
            ballRigidBody.AddExplosionForce(_repulsionForce, transform.position, magneticRadius);
        }
    }
}
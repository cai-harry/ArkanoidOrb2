using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetBlock : NormalBlock
{
    public float minRepulsion;
    public float maxRepulsion;
    public float magneticRadius;

    private float _repulsionForce;

    void Start()
    {
        _repulsionForce = Random.Range(minRepulsion, maxRepulsion);
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
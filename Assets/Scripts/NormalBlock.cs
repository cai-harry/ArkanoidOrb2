using System;
using UnityEngine;

public class NormalBlock : FlammableFreezable
{
    public float explosionForce;

    public Animator anim;

    protected override void OnCollisionExit(Collision other)
    {
        base.OnCollisionExit(other);
        if (other.gameObject.CompareTag("Ball"))
        {
            OnBallCollisionExit(other);
        }
    }

    protected virtual void OnBallCollisionExit(Collision ball)
    {
        var ballRigidBody = ball.gameObject.GetComponent<Rigidbody>();
        ballRigidBody.AddExplosionForce(explosionForce, transform.position, 1f);
        OnBlockDestroyed();
    }

    protected virtual void OnBlockDestroyed()
    {
        anim.Play("BlockDestroy");
        // Animation triggers DestroySelf() on finish
    }

    protected virtual void DestroySelf()
    {
        Destroy(gameObject);
    }
}
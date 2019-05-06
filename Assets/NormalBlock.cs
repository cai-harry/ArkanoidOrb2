using System;
using UnityEngine;

public class NormalBlock : FlammableFreezable
{
    public float explosionForce;

    public Animator anim;

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            OnBallCollisionExit(other);
        }
    }

    protected virtual void OnBallCollisionExit(Collision ball)
    {
        var ballRigidBody = ball.gameObject.GetComponent<Rigidbody>();
        ballRigidBody.AddExplosionForce(explosionForce, transform.position, 1f);
        PlayDestroyAnimation();
    }

    protected virtual void PlayDestroyAnimation()
    {
        anim.Play("BlockDestroy");
        // Animation triggers DestroySelf() on finish
    }

    protected virtual void DestroySelf()
    {
        Destroy(gameObject);
    }
}
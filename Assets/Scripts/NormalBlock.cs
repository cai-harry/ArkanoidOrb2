using System;
using UnityEngine;

public class NormalBlock : FlammableFreezable
{
    public float explosionForce;

    public Animator anim;

    private Collider _collider;

    protected override void Start()
    {
        base.Start();
        _collider = gameObject.GetComponent<Collider>();
    }

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
        // _collider.enabled = false;
        anim.Play("BlockDestroy");
        // Animation triggers DestroySelf() on finish
    }

    protected virtual void DestroySelf()
    {
        var mainScript = transform.parent.gameObject.GetComponent<MainScript>();
        mainScript.RemoveFromBlocksInPlay(gameObject);
    }
}
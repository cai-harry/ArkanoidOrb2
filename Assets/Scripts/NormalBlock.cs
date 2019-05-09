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
        OnBlockDestroyed();
    }

    protected virtual void OnBlockDestroyed()
    {
        // TODO: add force field of strength ExplosionForce
        anim.Play("BlockDestroy");
        // Animation triggers DestroySelf() on finish
    }

    protected virtual void DestroySelf()
    {
        Destroy(gameObject);
    }
}
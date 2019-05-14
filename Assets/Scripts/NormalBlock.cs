using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class NormalBlock : FlammableFreezable
{
    public Animator anim;

    protected override void Start()
    {
        base.Start();
        transform.Rotate(Vector3.up, Random.Range(0f, 360f));
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
        OnBlockDestroyed();
    }

    protected virtual void OnBlockDestroyed()
    {
        DisableCollider();
        anim.Play("BlockDestroy");
        // Animation triggers DestroySelf() on finish
    }

    protected virtual void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void DisableCollider()
    {
        var collider = gameObject.GetComponent<Collider>();
        collider.enabled = false;
    }
}
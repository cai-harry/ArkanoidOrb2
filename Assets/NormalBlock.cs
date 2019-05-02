using System;
using UnityEngine;

public class NormalBlock : MonoBehaviour
{
    public float explosionForce;

    public Animator anim;


    protected virtual void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            var ballRigidBody = other.gameObject.GetComponent<Rigidbody>();
            ballRigidBody.AddExplosionForce(explosionForce, transform.position, 1f);
            PlayDestroyAnimation();
        }
    }

    protected void PlayDestroyAnimation()
    {
        anim.Play("BlockDestroy");
        // Animation triggers DestroySelf() on finish
    }

    protected virtual void DestroySelf()
    {
        Destroy(gameObject);
    }
}

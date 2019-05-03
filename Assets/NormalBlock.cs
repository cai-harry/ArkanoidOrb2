using System;
using UnityEngine;

public class NormalBlock : MonoBehaviour
{
    public float explosionForce;

    public Animator anim;

    public GameObject popupText;


    protected virtual void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            var go = Instantiate(
                popupText,
                transform.position,
                Quaternion.LookRotation(Camera.main.transform.position - transform.position),
                transform  // child of this block
            );
            go.GetComponent<TextMesh>().text = "x1";
            Destroy(go, 2f); // destroy after 2 seconds
            // TODO: refactor above

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
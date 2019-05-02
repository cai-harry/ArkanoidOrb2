using UnityEngine;

public class BallBlock : NormalBlock
{
    public float delayBeforeDelete;

    public GameObject ball;

    protected override void OnCollisionExit(Collision other)
    {
        
        if (other.gameObject.CompareTag("Ball"))
        {
            var ballRigidBody = other.gameObject.GetComponent<Rigidbody>();
            ballRigidBody.AddExplosionForce(explosionForce, transform.position, 1f);
            PlayDestroyAnimation();
            DestroySelf();
        }
    }

    protected override void DestroySelf()
    {
        Instantiate(ball, transform.position, Quaternion.identity);
        base.DestroySelf();
    }
}

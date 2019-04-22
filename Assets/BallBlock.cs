using UnityEngine;

public class BallBlock : MonoBehaviour
{
    public float delayBeforeDelete;
    public float explosionForce;

    public GameObject ball;

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            var ballRigidBody = other.gameObject.GetComponent<Rigidbody>();
            ballRigidBody.AddExplosionForce(explosionForce, transform.position, 1f);
            Instantiate(ball, transform.position, Quaternion.identity);
            Invoke("RemoveSelf", delayBeforeDelete);
        }
    }

    private void RemoveSelf()
    {
        Destroy(gameObject);
    }
}

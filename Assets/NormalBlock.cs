using UnityEngine;

public class NormalBlock : MonoBehaviour
{
    public float delayBeforeDelete;
    public float explosionForce;
    
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            var ballRigidBody = other.gameObject.GetComponent<Rigidbody>();
            ballRigidBody.AddExplosionForce(explosionForce, transform.position, 1f);
            Invoke("RemoveSelf", delayBeforeDelete);
        }
    }

    private void RemoveSelf()
    {
        Destroy(gameObject);
    }
}

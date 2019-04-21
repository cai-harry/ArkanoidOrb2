using UnityEngine;

public class BallBlock : MonoBehaviour
{
    public float delayBeforeDelete;

    public GameObject ball;
    
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            Instantiate(ball, transform.position, Quaternion.identity);
            Invoke("RemoveSelf", delayBeforeDelete);
        }
    }

    private void RemoveSelf()
    {
        Destroy(gameObject);
    }
}

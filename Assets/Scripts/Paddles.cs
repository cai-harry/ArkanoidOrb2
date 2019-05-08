using UnityEngine;

public class Paddles : MonoBehaviour
{
    public float rotateSpeed;
    
    void FixedUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var deltaRotationY = horizontal * rotateSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, deltaRotationY);
    }
}

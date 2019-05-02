using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float startSpeed;
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(startSpeed * Vector3.left);
    }
    
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
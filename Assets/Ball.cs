using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float startSpeed;

    private int _currentCombo;
    
    void Start()
    {
        _currentCombo = 0;
        
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(startSpeed * Vector3.left);
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Block"))
        {
            _currentCombo++;
        }

        if (other.gameObject.CompareTag("Paddle"))
        {
            _currentCombo = 0;
        }
    }

    public int GetCurrentCombo()
    {
        return _currentCombo;
    }
    
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
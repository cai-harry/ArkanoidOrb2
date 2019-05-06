using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : FlammableFreezable
{
    public Rigidbody rigidBody;
    public float startSpeed;

    public AudioSource bounceSound;
    public float playBounceSoundFrom;

    public GameObject popupText;

    private int _currentCombo;

    protected override void Start()
    {
        base.Start();
        
        _currentCombo = 0;

        rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(startSpeed * Vector3.left);
    }

    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        bounceSound.time = playBounceSoundFrom;
        bounceSound.Play();
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Block"))
        {
            _currentCombo++;
            ShowComboPopup();
        }

        if (other.gameObject.CompareTag("Paddle"))
        {
            _currentCombo = 0;
        }
    }

    protected void ShowComboPopup()
    {
        var go = Instantiate(
            popupText,
            transform.position,
            Quaternion.Euler(0, -90, 0) // TODO: hacky
        );
        go.GetComponent<TextMesh>().text = $"x{_currentCombo}";
        Destroy(go, 2f); // destroy after 2 seconds
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
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
    public ParticleSystem lightning;
    public Vector3 lightningVelocityMultiplier;

    private ParticleSystem.VelocityOverLifetimeModule _lightningVelocity;
    private int _currentCombo;

    protected override void Start()
    {
        base.Start();

        _currentCombo = 0;

        rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(startSpeed * Vector3.left);

        _lightningVelocity = lightning.velocityOverLifetime;
    }

    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        bounceSound.time = playBounceSoundFrom;
        bounceSound.Play();
    }

    protected override void OnCollisionExit(Collision other)
    {
        base.OnCollisionExit(other);
        if (other.gameObject.CompareTag("Block"))
        {
            OnBlockCollisionExit();
        }

        if (other.gameObject.CompareTag("Paddle"))
        {
            OnPaddleCollisionExit();
        }
    }

    private void DisplayElectric(Vector3 positionDeltaToBlock)
    {
        lightning.Play();
        _lightningVelocity.x = lightningVelocityMultiplier.x * positionDeltaToBlock.x;
        _lightningVelocity.y = lightningVelocityMultiplier.y * positionDeltaToBlock.y;
        _lightningVelocity.z = lightningVelocityMultiplier.z * positionDeltaToBlock.z;
    }

    private void OnBlockCollisionExit()
    {
        if (onFire)
        {
            _currentCombo += 2;
        }
        else if (!frozen)
        {
            _currentCombo++;
        }

        ShowComboPopup();
    }

    private void OnPaddleCollisionExit()
    {
        if (frozen)
        {
            ShowComboPopup();
            return;
        }

        _currentCombo = 0;
    }

    private void ShowComboPopup()
    {
        var comboPopup = Instantiate(
            popupText,
            transform.position,
            Quaternion.Euler(0, -90, 0) // TODO: hacky
        );
        var comboTextBox = comboPopup.GetComponent<TextMesh>();
        comboTextBox.text = $"x{_currentCombo}";

        if (onFire)
        {
            comboTextBox.color = Color.red;
        }

        if (frozen)
        {
            comboTextBox.color = Color.blue;
        }

        Destroy(comboPopup, 2f); // destroy after 2 seconds
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
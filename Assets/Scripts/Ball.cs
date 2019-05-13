using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : FlammableFreezable
{
    public Rigidbody rigidBody;
    public float startSpeed;

    public AudioSource bounceSound;
    public float playBounceSoundFrom;

    public GameObject popupText;
    public ParticleSystem lightning;
    public Vector3 lightningVelocityMultiplier;
    public ParticleSystem sparks;
    public float sparksEmissionRate;

    private MainScript _mainScript;
    
    private int _currentCombo;

    protected override void Start()
    {
        base.Start();

        _mainScript = GameObject.FindObjectOfType<MainScript>();

        _currentCombo = 0;

        rigidBody = GetComponent<Rigidbody>();

        var initialDirection = Random.insideUnitSphere;

        rigidBody.AddForce(startSpeed * initialDirection);
    }

    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        bounceSound.time = playBounceSoundFrom;
        bounceSound.Play();
        PlaySparks();
    }

    private void PlaySparks()
    {
        var sparksMain = sparks.main;
        sparksMain.startSpeed = rigidBody.velocity.magnitude;
        var sparksEmission = sparks.emission;
        sparksEmission.rateOverTime = sparksEmissionRate * rigidBody.velocity.magnitude;
        sparks.Play();
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

    public void SpeedUp(float toSpeed)
    {
        if (rigidBody.velocity.magnitude >= toSpeed)
        {
            return;
        }

        ChangeSpeed(toSpeed);
    }

    public void SlowDown(float toSpeed)
    {
        if (rigidBody.velocity.magnitude <= toSpeed)
        {
            return;
        }

        ChangeSpeed(toSpeed);
    }

    public void DisplayElectric(Vector3 positionDeltaToBlock)
    {
        var lightningVelocity = lightning.velocityOverLifetime;
        lightningVelocity.x = lightningVelocityMultiplier.x * positionDeltaToBlock.x;
        lightningVelocity.y = lightningVelocityMultiplier.y * positionDeltaToBlock.y;
        lightningVelocity.z = lightningVelocityMultiplier.z * positionDeltaToBlock.z;
        lightning.Play();
    }

    private void ChangeSpeed(float toSpeed)
    {
        PlaySparks();
        rigidBody.velocity = toSpeed * rigidBody.velocity.normalized;
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

        _mainScript.IncreaseScore(1, _currentCombo);
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
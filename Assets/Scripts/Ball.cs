﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : FlammableFreezable
{
    public Rigidbody rigidBody;
    public float startSpeed;

    public float minSpeed;
    public float addSpeedAfterCollision;
    public float maxSpeedAfterCollision;

    public float minY;

    public float magnusEffectStrength;
    public float spinDecay;

    public AudioSource bounceSound;
    public float playBounceSoundFrom;

    public bool setRandomColor;

    public TrailRenderer[] trailRenderers;
    public GameObject popupText;
    public ParticleSystem lightning;
    public Vector3 lightningVelocityMultiplier;
    public ParticleSystem sparks;
    public float sparksEmissionRate;

    private MainScript _mainScript;

    private float _spin;

    public float Spin
    {
        get { return _spin; }
        set { _spin = value; }
    }

    private Color _color;

    private int _currentCombo;

    private float _lastCollisionSpeed;

    protected override void Start()
    {
        base.Start();

        _mainScript = GameObject.FindObjectOfType<MainScript>();

        _currentCombo = 0;

        _spin = 0;

        rigidBody = GetComponent<Rigidbody>();

        if (setRandomColor)
        {
            SetColor(Random.ColorHSV());
        }

        var initialDirection = Random.insideUnitSphere;

        rigidBody.AddForce(startSpeed * initialDirection);
    }

    private void FixedUpdate()
    {
        // enforce minSpeed
        if (rigidBody.velocity.magnitude < minSpeed)
        {
            rigidBody.velocity = rigidBody.velocity.normalized * minSpeed;
        }

        if (transform.position.y < minY)
        {
            DestroySelf();
        }

        AddMagnusForce();
        _spin *= Mathf.Pow(spinDecay, Time.deltaTime);
    }

    protected override void Update()
    {
        base.Update();
        var trails = transform.Find("Trails");
        trails.Rotate(Vector3.up, 100 * _spin * Time.deltaTime);
    }

    private void AddMagnusForce()
    {
        var magnusForce = Quaternion.Euler(0, 90, 0) * rigidBody.velocity * _spin * magnusEffectStrength;
        rigidBody.AddForce(magnusForce);
    }

    private void SetColor(Color color)
    {
        _color = color;

        var renderer = gameObject.GetComponent<Renderer>();
        renderer.material.color = _color;

        foreach (var trailRenderer in trailRenderers)
        {
            trailRenderer.startColor = _color;
        }

        var main = sparks.main;
        main.startColor = _color;
        var sparksTrails = sparks.trails;
        sparksTrails.colorOverLifetime = _color;
    }

    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        PlayCollisionEffects();
        _lastCollisionSpeed = rigidBody.velocity.magnitude;
    }

    private void PlayCollisionEffects()
    {
        bounceSound.time = playBounceSoundFrom;
        bounceSound.Play();

        var sparksMain = sparks.main;
        sparksMain.startSpeed = rigidBody.velocity.magnitude;
        var sparksEmission = sparks.emission;
        sparksEmission.rateOverTime = sparksEmissionRate * rigidBody.velocity.magnitude;
        sparks.Play();
    }

    protected override void OnCollisionExit(Collision other)
    {
        base.OnCollisionExit(other);

        ChangeSpeed(
            Mathf.Min(
                addSpeedAfterCollision + other.relativeVelocity.magnitude,
                maxSpeedAfterCollision
            )
        );

        if (other.gameObject.CompareTag("Block"))
        {
            OnBlockCollisionExit();
        }

        if (other.gameObject.CompareTag("Paddle"))
        {
            OnPaddleCollisionExit(other);
        }
    }

    public void SpeedUp(float toSpeed)
    {
        if (rigidBody.velocity.magnitude >= toSpeed)
        {
            return;
        }

        PlayCollisionEffects();
        ChangeSpeed(toSpeed);
    }

    public void SlowDown(float toSpeed)
    {
        if (rigidBody.velocity.magnitude <= toSpeed)
        {
            return;
        }

        PlayCollisionEffects();
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

    private void OnPaddleCollisionExit(Collision paddleCollision)
    {
        if (frozen)
        {
            ShowComboPopup();
            return;
        }

        _currentCombo = 0;

        var paddleScript = paddleCollision.gameObject.transform.parent.GetComponent<Paddles>();
        _spin = paddleScript.DeltaRotationY;
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

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
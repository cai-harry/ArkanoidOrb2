using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class FlammableFreezable : MonoBehaviour
{
    public bool startOnFire;
    public bool startFrozen;

    public float maxTimeOnFire;
    public float maxTimeFrozen;

    protected bool onFire;
    protected bool frozen;

    private float _lastTimeSetOnFire;
    private float _lastTimeFrozen;

    private ParticleSystem _fireParticleSystem;
    private ParticleSystem _iceParticleSystem;

    private delegate void Del();

    private Del _collisionExitQueuedCommand;

    protected virtual void Start()
    {
        onFire = false;
        frozen = false;
        _fireParticleSystem = transform.Find("Fire").GetComponent<ParticleSystem>();
        _iceParticleSystem = transform.Find("Ice").GetComponent<ParticleSystem>();

        if (startOnFire)
        {
            SetOnFire();
        }

        if (startFrozen)
        {
            Freeze();
        }

        _collisionExitQueuedCommand = () => { };
    }

    protected virtual void Update()
    {
        if (onFire && Time.time - _lastTimeSetOnFire > maxTimeOnFire)
        {
            Extinguish();
        }

        if (frozen && Time.time - _lastTimeFrozen > maxTimeFrozen)
        {
            Unfreeze();
        }
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        var otherFlammableFreezableScript = other.gameObject.GetComponent<FlammableFreezable>();
        if (!otherFlammableFreezableScript) return;

        if (onFire)
        {
            otherFlammableFreezableScript.OnCollisionWithFire();
        }

        if (frozen)
        {
            otherFlammableFreezableScript.OnCollisionWithIce();
        }
    }

    protected virtual void OnCollisionExit(Collision other)
    {
        _collisionExitQueuedCommand();
        _collisionExitQueuedCommand = () => { };
    }

    public void OnCollisionWithFire()
    {
        if (frozen)
        {
            _collisionExitQueuedCommand = Unfreeze;
            return;
        }

        _collisionExitQueuedCommand = SetOnFire;
    }

    private void SetOnFire()
    {
        _fireParticleSystem.Play();
        onFire = true;
        _lastTimeSetOnFire = Time.time;
    }

    private void Extinguish()
    {
        _fireParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        onFire = false;
    }

    private void OnCollisionWithIce()
    {
        if (onFire)
        {
            _collisionExitQueuedCommand = Extinguish;
            return;
        }

        _collisionExitQueuedCommand = Freeze;
    }

    private void Freeze()
    {
        _iceParticleSystem.Play();
        frozen = true;
        _lastTimeFrozen = Time.time;
    }

    private void Unfreeze()
    {
        _iceParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        frozen = false;
    }
}
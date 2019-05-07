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

    private bool _onFire;
    private bool _frozen;

    private float _lastTimeSetOnFire;
    private float _lastTimeFrozen;

    private ParticleSystem _fireParticleSystem;
    private ParticleSystem _iceParticleSystem;
    
    private delegate void Del();

    private Del _collisionExitQueuedCommand;

    protected virtual void Start()
    {
        _onFire = false;
        _frozen = false;
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
        if (_onFire && Time.time - _lastTimeSetOnFire > maxTimeOnFire)
        {
            Extinguish();
        }

        if (_frozen && Time.time - _lastTimeFrozen > maxTimeFrozen)
        {
            Unfreeze();
        }
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if (_onFire)
        {
            other.gameObject.SendMessage("OnCollisionWithFire", SendMessageOptions.DontRequireReceiver);
        }

        if (_frozen)
        {
            other.gameObject.SendMessage("OnCollisionWithIce", SendMessageOptions.DontRequireReceiver);
        }
    }

    protected virtual void OnCollisionExit(Collision other)
    {
        _collisionExitQueuedCommand();
        _collisionExitQueuedCommand = () => { };
    }

    public void OnCollisionWithFire()
    {
        if (_frozen)
        {
            _collisionExitQueuedCommand = Unfreeze;
            return;
        }

        _collisionExitQueuedCommand = SetOnFire;
    }

    private void SetOnFire()
    {
        Debug.Log($"Setting {gameObject.name} on fire");
        _fireParticleSystem.Play();
        _onFire = true;
        _lastTimeSetOnFire = Time.time;
    }

    private void Extinguish()
    {
        Debug.Log($"Extinguishing {gameObject.name}");
        _fireParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        _onFire = false;
    }

    private void OnCollisionWithIce()
    {
        if (_onFire)
        {
            _collisionExitQueuedCommand = Extinguish;
            return;
        }

        _collisionExitQueuedCommand = Freeze;
    }

    private void Freeze()
    {
        Debug.Log($"Freezing {gameObject.name}");
        _iceParticleSystem.Play();
        _frozen = true;
        _lastTimeFrozen = Time.time;
    }

    private void Unfreeze()
    {
        Debug.Log($"Unfreezing {gameObject.name}");
        _iceParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        _frozen = false;
    }
}
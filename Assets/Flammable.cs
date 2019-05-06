using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Flammable : MonoBehaviour
{
    public bool startOnFire;

    private bool _onFire;
    private ParticleSystem _fireParticleSystem;

    protected virtual void Start()
    {
        _onFire = false;
        _fireParticleSystem = transform.Find("Fire").GetComponent<ParticleSystem>();

        if (!_fireParticleSystem)
        {
            throw new Exception("_fireParticleSystem not defined");
        }

        if (startOnFire)
        {
            SetOnFire();
        }
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        other.gameObject.SendMessage("SetOnFire", SendMessageOptions.DontRequireReceiver);
    }

    public void SetOnFire()
    {
        if (_onFire)
        {
            return;
        }

        _fireParticleSystem.Play();
        _onFire = true;
    }

    public void Extinguish()
    {
        if (!_onFire)
        {
            return;
        }

        _fireParticleSystem.Stop();
        _onFire = false;
    }
}
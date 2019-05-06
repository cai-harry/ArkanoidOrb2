using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class FlammableFreezable : MonoBehaviour
{
    public bool startOnFire;
    public bool startFrozen;

    private bool _onFire;
    private bool _frozen;

    private ParticleSystem _fireParticleSystem;
    private ParticleSystem _iceParticleSystem;

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
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if (_onFire)
        {
            other.gameObject.SendMessage("SetOnFire", SendMessageOptions.DontRequireReceiver);
        }

        if (_frozen)
        {
            other.gameObject.SendMessage("Freeze", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void SetOnFire()
    {
        if (_onFire)
        {
            return;
        }
        

        _fireParticleSystem.Play();
        _onFire = true;
        
        Unfreeze();
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

    private void Freeze()
    {
        if (_frozen)
        {
            return;
        }

        _iceParticleSystem.Play();
        _frozen = true;
        
        Extinguish();
    }

    public void Unfreeze()
    {
        if (!_frozen)
        {
            return;
        }

        _iceParticleSystem.Stop();
        _frozen = false;
    }
}
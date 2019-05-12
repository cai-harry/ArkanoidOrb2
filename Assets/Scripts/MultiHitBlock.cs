﻿using UnityEngine;
using UnityEngine.Serialization;

public class MultiHitBlock : NormalBlock
{
    public Material secondMaterial;

    private int _numHits = 0;

    protected override void OnBallCollisionExit(Collision ball)
    {
        _numHits++;
        OnNthBallCollisionExit(_numHits);
    }

    protected virtual void OnNthBallCollisionExit(int n)
    {
        switch (n)
        {
            case 1:
                ChangeMaterial(secondMaterial);
                break;
            case 2:
                OnBlockDestroyed();
                break;
            default:
                Debug.LogError($"Invalid ball collision number on {gameObject.name}");
                break;
        }
    }

    protected void ChangeMaterial(Material newMaterial)
    {
        var renderer = gameObject.GetComponent<Renderer>();
        renderer.material = newMaterial;
    }
}
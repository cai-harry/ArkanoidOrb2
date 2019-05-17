using UnityEngine;
using UnityEngine.Serialization;

public class MultiHitBlock : NormalBlock
{
    public Material[] materialsAfterFirstHit;
    public Material[] materialsAfterSecondHit;

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
                ChangeMaterial(materialsAfterFirstHit);
                break;
            case 2:
                ChangeMaterial(materialsAfterSecondHit);
                break;
            case 3:
                OnBlockDestroyed();
                break;
            default:
                LogInvalidBallCollisionNumber(n);
                break;
        }
    }

    protected virtual void ChangeMaterial(Material[] newMaterials)
    {
        var renderer = gameObject.GetComponent<Renderer>();
        renderer.materials = newMaterials;
    }

    protected void LogInvalidBallCollisionNumber(int n)
    {
        Debug.LogError($"Invalid ball collision number {n} on {gameObject.name}");
    }
}
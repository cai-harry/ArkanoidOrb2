using UnityEngine;

public class RockBlock : NormalBlock
{
    
    const int HitsToDestroy = 2;
    
    public Material crackedRockMaterial;
    
    private int _numHits = 0;

    protected override void OnBallCollisionExit(Collision ball)
    {
        ShowComboPopup();
        
        _numHits++;
        if (_numHits == 1)
        {
            var renderer = gameObject.GetComponent<Renderer>();
            renderer.material = crackedRockMaterial;
        }
        if (_numHits >= HitsToDestroy)
        {
            var ballRigidBody = ball.gameObject.GetComponent<Rigidbody>();
            ballRigidBody.AddExplosionForce(explosionForce, transform.position, 1f);
            PlayDestroyAnimation();
        }
    }

}
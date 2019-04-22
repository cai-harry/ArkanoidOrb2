using UnityEngine;

public class RockBlock : MonoBehaviour
{
    
    const int HitsToDestroy = 2;

    public float delayBeforeDelete;

    public Material crackedRockMaterial;
    
    private int _numHits = 0;

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            _numHits++;
            if (_numHits == 1)
            {
                var renderer = gameObject.GetComponent<Renderer>();
                renderer.material = crackedRockMaterial;
            }
            if (_numHits >= HitsToDestroy)
            {
                Invoke("RemoveSelf", delayBeforeDelete);
            }
        }
    }

    private void RemoveSelf()
    {
        Destroy(gameObject);
    }
}
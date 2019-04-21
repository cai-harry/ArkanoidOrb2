﻿using UnityEngine;

public class NormalBlock : MonoBehaviour
{
    public float delayBeforeDelete;
    
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            Invoke("RemoveSelf", delayBeforeDelete);
        }
    }

    private void RemoveSelf()
    {
        Destroy(gameObject);
    }
}

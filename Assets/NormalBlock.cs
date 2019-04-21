using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBlock : MonoBehaviour
{
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Ball")
        {
            Destroy(gameObject);
        }
    }
}

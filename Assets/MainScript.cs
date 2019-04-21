using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public float firstBlockSeconds;
    public float addBlockRate;
    public float blockSpaceRadius;
    
    public GameObject normalBlock;

    void Start()
    {
        InvokeRepeating("AddNormalBlock", firstBlockSeconds, addBlockRate);
    }
    
    private void AddNormalBlock()
    {
        // TODO: don't instantiate on top of other blocks or balls
        var randomUnitCirclePosition = Random.insideUnitCircle;
        var startPosition = new Vector3(
            blockSpaceRadius * randomUnitCirclePosition.x,
            normalBlock.transform.position.y,
            blockSpaceRadius * randomUnitCirclePosition.y
        );
        Instantiate(normalBlock, startPosition, Quaternion.identity);
    }

    
}
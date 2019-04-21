using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public float firstBlockSeconds;
    public float addBlockRate;
    public float blockSpaceRadius;
    
    public GameObject normalBlock;
    public GameObject rockBlock;

    void Start()
    {
        InvokeRepeating("AddNormalBlock", firstBlockSeconds, addBlockRate);
        InvokeRepeating("AddRockBlock", firstBlockSeconds, addBlockRate);
    }
    
    private void AddNormalBlock()
    {
        InstantiateBlock(normalBlock);
    }

    private void AddRockBlock()
    {
        InstantiateBlock(rockBlock);
    }

    private void InstantiateBlock(GameObject block)
    {
        // TODO: don't instantiate on top of other blocks or balls
        var randomUnitCirclePosition = Random.insideUnitCircle;
        var startPosition = new Vector3(
            blockSpaceRadius * randomUnitCirclePosition.x,
            block.transform.position.y,
            blockSpaceRadius * randomUnitCirclePosition.y
        );
        Instantiate(block, startPosition, Quaternion.identity);
    }
    
}
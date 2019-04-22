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
    public GameObject ballBlock;

    void Start()
    {
        InvokeRepeating("AddRandomBlock", firstBlockSeconds, addBlockRate);
    }

    private void AddRandomBlock()
    {
        var randFloat = Random.Range(0f, 1f);
        if (randFloat < 0.33f)
        {
            InstantiateBlock(ballBlock);
        }
        else if (randFloat < 0.67f)
        {
            InstantiateBlock(rockBlock);
        }
        else
        {
            InstantiateBlock(normalBlock);
        }
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
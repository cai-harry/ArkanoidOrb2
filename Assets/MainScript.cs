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
        InvokeRepeating("AddRandomBlock", firstBlockSeconds, addBlockRate);
    }

    private void AddRandomBlock()
    {
        if (Random.Range(0f, 1f) < 0.5f)
        {
            InstantiateBlock(normalBlock);
        }
        else
        {
            InstantiateBlock(rockBlock);
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
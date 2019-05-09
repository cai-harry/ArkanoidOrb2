using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBlock : NormalBlock
{
    protected override void OnBallCollisionExit(Collision ball)
    {
        var mainScript = transform.parent.gameObject.GetComponent<MainScript>();
        mainScript.ShuffleBlockTypes();
        base.OnBallCollisionExit(ball);
    }

}
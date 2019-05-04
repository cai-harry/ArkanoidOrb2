using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetBlock : NormalBlock
{
    public float minRepulsion;
    public float maxRepulsion;
    public float magneticRadius;


    public GameObject popupText;

    private float _repulsionForce;

    void Start()
    {
        _repulsionForce = Random.Range(minRepulsion, maxRepulsion);

        InvokeRepeating("ShowRepulsionForce", 0f, 2f);
    }

    private void FixedUpdate()
    {
        foreach (var ball in GameObject.FindGameObjectsWithTag("Ball"))
        {
            var ballRigidBody = ball.gameObject.GetComponent<Rigidbody>();
            ballRigidBody.AddExplosionForce(_repulsionForce, transform.position, magneticRadius);
        }
    }

    private void ShowRepulsionForce()
    {
        var go = Instantiate(
            popupText,
            transform.position,
            Quaternion.Euler(0, -90, 0) // TODO: hacky
        );
        var textMesh = go.GetComponent<TextMesh>();
        if (_repulsionForce > 0)
        {
            textMesh.color = Color.blue;
        }

        if (_repulsionForce < 0)
        {
            textMesh.color = Color.red;
        }

        textMesh.text = $"{_repulsionForce,3:F3}";
        Destroy(go, 2f); // destroy after 2 seconds
    }
}
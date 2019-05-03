using System;
using UnityEngine;

public class NormalBlock : MonoBehaviour
{
    public float explosionForce;

    public Animator anim;

    public GameObject popupText;


    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            OnBallCollisionExit(other);

            var ballScript = other.gameObject.GetComponent<Ball>();
            ShowComboPopup(ballScript);
        }
    }

    protected virtual void OnBallCollisionExit(Collision ball)
    {
        var ballRigidBody = ball.gameObject.GetComponent<Rigidbody>();
        ballRigidBody.AddExplosionForce(explosionForce, transform.position, 1f);
        PlayDestroyAnimation();
    }

    protected void ShowComboPopup(Ball ballScript)
    {
        var combo = ballScript.GetCurrentCombo();
        var go = Instantiate(
            popupText,
            transform.position,
            Quaternion.Euler(0, -90, 0) // TODO: hacky
        );
        go.GetComponent<TextMesh>().text = $"x{combo}";
        Destroy(go, 2f); // destroy after 2 seconds
    }

    protected virtual void PlayDestroyAnimation()
    {
        anim.Play("BlockDestroy");
        // Animation triggers DestroySelf() on finish
    }

    protected virtual void DestroySelf()
    {
        Destroy(gameObject);
    }
}
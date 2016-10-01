using UnityEngine;
using System.Collections;

public class enemyPatrol : MonoBehaviour {

    public float speed = 0.9f;
    public float maxSpeed = 5.0f;
    public float moveForce = 365.0f;
    public float maxDistance = 10.0f;
    private bool facingRight = true;
    private float moveVal = 1.0f;

    private Vector3 originalPosition; 



    void Awake()
    {
        originalPosition = transform.position;
    }


    void Update()
    {

    }


    void FixedUpdate()
    {

        // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
        if (speed * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
            // ... add a force to the player.
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * speed * moveForce);

        // If the player's horizontal velocity is greater than the maxSpeed...
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
            // ... set the player's velocity to the maxSpeed in the x axis.
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

        // If the input is moving the player right and the player is facing left...
        if (speed > 0 && !facingRight)
            // ... flip the player.
            Flip();
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (speed < 0 && facingRight)
            // ... flip the player.
            Flip();
    }


    void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.collider.gameObject.tag != "ground")
            speed *= -1;
    }
}

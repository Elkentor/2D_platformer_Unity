using UnityEngine;

public class Life : Pickup
{
    private Rigidbody2D rb;
    private int xVel = -4;

    public override void OnPickup(GameObject player)
    {
        GameManager.Instance.AddLife(1); // ? Use GameManager to add life
        Debug.Log("Life pickup collected!");
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(xVel, 4); // ? Use velocity instead of linearVelocity
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(xVel, rb.linearVelocity.y); // ? Corrected typo
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.CompareTag("Wall"))
        {
            xVel *= -1; // Reverse direction on wall hit
        }
    }
}

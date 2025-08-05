using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private bool isGrounded = false;
    private LayerMask groundLayer;


    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Collider2D col;
    [SerializeField] private Animator anim;
    [SerializeField] private float grounCheckRadius = 0.02f;
    [SerializeField] private float attackCooldown = 0.5f;
    private float lastAttackTime;


    [SerializeField] private int maxJumpCount = 2;
    private int jumpCount = 1;

    private Vector2 groundCheckPos => new Vector2(col.bounds.min.x + col.bounds.extents.x, col.bounds.min.y);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        groundLayer = LayerMask.GetMask("Ground");

        //check if the ground layer is set correctly
        if (groundLayer == 0)
            Debug.LogError("Ground layer is not set correctly. Please set the ground layer in the inspector.");
    }

    // Update is called once per frame
    void Update()
    {
        float hValue = Input.GetAxisRaw("Horizontal");
        SpriteFlip(hValue);

        rb.linearVelocityX = hValue * 5f;
        isGrounded = Physics2D.OverlapCircle(groundCheckPos, grounCheckRadius, groundLayer);

        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
        {
            rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
            jumpCount++;

        }

        if (isGrounded)
        {
            jumpCount = 1; // Reset jump count when grounded
        }

        float vValue = rb.linearVelocity.y;

        if (!isGrounded)
        {
            if (vValue > 0.1f)
            {
                anim.SetTrigger("Jump");
            }
            else if (vValue < -0.1f)
            {
                anim.SetTrigger("Fall");
            }
        }




        anim.SetFloat("hValue", Mathf.Abs(hValue));
        anim.SetBool("isGrounded", isGrounded);

        if (Input.GetKeyDown(KeyCode.LeftControl) && Time.time > lastAttackTime + attackCooldown)
        {
            anim.SetTrigger("Attack");
            lastAttackTime = Time.time;
        }

    }


    void SpriteFlip(float hValue)
    {
        if (hValue != 0) sr.flipX = (hValue < 0);
    }
}

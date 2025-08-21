//using UnityEngine;
//using System.Collections;

//[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Collider2D))]
//[RequireComponent(typeof(Animator))]
//public class PlayerController : MonoBehaviour
//{
//    // this transform is used to check if the player is grounded only needed if the ground check position is generated and a seperate object
//    //private Transform groundCheckPos;

//    private int _score = 0;
//    private int _lives = 3;
//    private int jumpForce = 6;
//    private bool isDead = false;
//    [SerializeField] private Transform respawnPoint;



//    private Coroutine jumpForceChange = null;

//    public void ActivateJumpForceChange()
//    {
//        //start a coroutine that changes the jump force for x seconds
//        if (jumpForceChange != null)
//        {
//            StopCoroutine(jumpForceChange);
//            jumpForceChange = null;
//            jumpForce = 6; // Reset to default jump force
//        }

//        jumpForceChange = StartCoroutine(ChangeJumpForce()); // Change jump force to 12 for 3 seconds
//    }
//    private IEnumerator ChangeJumpForce()
//    {
//        jumpForce = 12; // Set new jump force
//        Debug.Log($"Jump force change to {jumpForce} at {Time.time}");
//        yield return new WaitForSeconds(5f); // Wait for 5 seconds
//        jumpForce = 6; // Reset to default jump force
//        Debug.Log($"Jump force change to {jumpForce} at {Time.time}");
//        jumpForceChange = null; // Clear the coroutine reference
//    }
//    public int score
//    {
//        get => _score;
//        set
//        {
//            if (value < 0)
//                _score = 0;
//            else
//                _score = value;
//        }
//    }
//    public int lives
//    {
//        get => _lives;
//        set
//        {
//            if (value < 0)
//            {
//                //gameover goes here
//                Debug.Log("Game Over! You have no lives left.");
//                _lives = 0;
//            }
//            else if (value > maxLives)
//            {
//                _lives = maxLives;
//            }
//            else
//            {
//                _lives = value;
//            }
//        }
//    }

//    public int maxLives = 9;

//    //[SerializeField] private bool isGrounded = false;
//    private LayerMask groundLayer;

//    private Rigidbody2D rb;
//    private SpriteRenderer sr;
//    private Collider2D col;
//    private Animator anim;
//    private GroundCheck groundCheck;

//    [SerializeField] private int maxJumpCount = 2; // Maximum number of jumps allowed (e.g., double jump)
//    private int jumpCount = 1;

//    [SerializeField] private float groundCheckRadius = 0.02f; // Radius for ground check, adjust as necessary
//    private float initialGroundCheckRadius;

//    private IEnumerator HandleDeath()
//    {
//        rb.linearVelocity = Vector2.zero;
//        rb.bodyType = RigidbodyType2D.Static; // Prevent movement
//        col.enabled = false; // Disable collisions
//        sr.enabled = true; // Keep sprite visible during animation

//        yield return new WaitForSeconds(1.5f); // Wait for death animation

//        Destroy(gameObject); // Destroy player

//        yield return new WaitForSeconds(0.5f); // Optional delay before respawn

//        RespawnPlayer(); // Call respawn logic
//    }

//    private void RespawnPlayer()
//    {
//        Instantiate(gameObject, respawnPoint.position, Quaternion.identity);
//    }


//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        sr = GetComponent<SpriteRenderer>();
//        col = GetComponent<Collider2D>();
//        anim = GetComponent<Animator>();

//        groundLayer = LayerMask.GetMask("Ground");

//        if (groundLayer == 0)
//        {
//            Debug.LogWarning("Ground layer not set. Please set the Ground layer in the LayerMask. Groundcheck not created");
//            return;
//        }
//        groundCheck = new GroundCheck(col, groundLayer, groundCheckRadius);
//        initialGroundCheckRadius = groundCheckRadius;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        float hValue = Input.GetAxisRaw("Horizontal");
//        float vValue = Input.GetAxisRaw("Vertical");
//        AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
//        SpriteFlip(hValue);
//        //Debug.Log("Ground Check Position: " + groundCheckPos);

//        rb.linearVelocityX = hValue * 5f; // Adjust speed as necessary
//        groundCheck.CheckIsGrounded();

//        if (!currentState.IsName("Fire") && Input.GetButtonDown("Fire1"))
//        {
//            anim.SetTrigger("Fire");
//        }
//        else if (currentState.IsName("Fire"))
//        {
//            rb.linearVelocity = Vector2.zero;
//        }
//        else if (currentState.IsName("Jump") && (Input.GetButton("Fire2") && vValue > 0))
//        {
//            anim.SetTrigger("JumpAtk");
//        }

//        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
//        {
//            rb.linearVelocityY = 0;
//            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Adjust jump force as necessary
//            jumpCount++;
//            //Debug.Log("Jump Count: " + jumpCount);
//        }

//        if (groundCheck.IsGrounded)
//            jumpCount = 1; // Reset jump count when grounded

//        // Update animator parameters
//        anim.SetFloat("hValue", Mathf.Abs(hValue));
//        anim.SetFloat("vValue", rb.linearVelocity.y);
//        anim.SetBool("isGrounded", groundCheck.IsGrounded);
//        //Debug.Log($"Ground Check Radius from Player object: {groundCheckRadius}");
//        if (initialGroundCheckRadius != groundCheckRadius)
//            groundCheck.UpdateGroundCheckRadius(groundCheckRadius);
//    }
//    void SpriteFlip(float hValue)
//    {
//        if (hValue != 0) sr.flipX = (hValue < 0);
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (isDead) return;

//        if (collision.CompareTag("DeadCollider"))
//        {
//            isDead = true;
//            lives--; // This will trigger your lives setter logic
//            anim.SetTrigger("Dead"); // Make sure you have a "Dead" trigger in Animator
//            StartCoroutine(HandleDeath());
//        }
//        else if (collision.CompareTag("Squish") && rb.linearVelocityY < 0)
//        {
//            collision.GetComponentInParent<Enemy>().TakeDamage(0, DamageType.JumpedOn);
//            rb.linearVelocity = Vector2.zero;
//            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
//        }
//    }


//}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint; // Assign in Inspector
    [SerializeField] private int maxLives = 9;
    [SerializeField] private int jumpForce = 6;
    [SerializeField] private int maxJumpCount = 2;
    [SerializeField] private float groundCheckRadius = 0.02f;

    private int _score = 0;
    private int _Lives = 3;
    private int jumpCount = 1;
    private float initialGroundCheckRadius;
    private bool isDead = false;

    private Coroutine jumpForceChange = null;

    private LayerMask groundLayer;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Collider2D col;
    private Animator anim;
    private GroundCheck groundCheck;

    public int Score
    {
        get => _score;
        set => _score = Mathf.Max(0, value);
    }

    public int Lives
    {
        get => _Lives;
        set
        {
            if (value < 0)
            {
                Debug.Log("Game Over! You have no lives left.");
                _Lives = 0;
            }
            else if (value > maxLives)
            {
                _Lives = maxLives;
            }
            else
            {
                _Lives = value;
            }
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        groundLayer = LayerMask.GetMask("Ground");

        if (groundLayer == 0)
        {
            Debug.LogWarning("Ground layer not set. Please set the Ground layer in the LayerMask.");
            return;
        }

        groundCheck = new GroundCheck(col, groundLayer, groundCheckRadius);
        initialGroundCheckRadius = groundCheckRadius;
    }

    void Update()
    {
        if (isDead) return;

        float hValue = Input.GetAxisRaw("Horizontal");
        float vValue = Input.GetAxisRaw("Vertical");
        AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);

        SpriteFlip(hValue);
        rb.linearVelocity = new Vector2(hValue * 5f, rb.linearVelocity.y);

        groundCheck.CheckIsGrounded();

        if (!currentState.IsName("Fire") && Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Fire");
        }
        else if (currentState.IsName("Fire"))
        {
            rb.linearVelocity = Vector2.zero;
        }
        else if (currentState.IsName("Jump") && Input.GetButton("Fire2") && vValue > 0)
        {
            anim.SetTrigger("JumpAtk");
        }

        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
        }

        if (groundCheck.IsGrounded)
            jumpCount = 1;

        anim.SetFloat("hValue", Mathf.Abs(hValue));
        anim.SetFloat("vValue", rb.linearVelocity.y);
        anim.SetBool("isGrounded", groundCheck.IsGrounded);

        if (initialGroundCheckRadius != groundCheckRadius)
            groundCheck.UpdateGroundCheckRadius(groundCheckRadius);
    }

    void SpriteFlip(float hValue)
    {
        if (hValue != 0) sr.flipX = hValue < 0;
    }

    public void ActivateJumpForceChange()
    {
        if (jumpForceChange != null)
        {
            StopCoroutine(jumpForceChange);
            jumpForceChange = null;
            jumpForce = 6;
        }

        jumpForceChange = StartCoroutine(ChangeJumpForce());
    }

    private IEnumerator ChangeJumpForce()
    {
        jumpForce = 12;
        Debug.Log($"Jump force changed to {jumpForce} at {Time.time}");
        yield return new WaitForSeconds(5f);
        jumpForce = 6;
        Debug.Log($"Jump force reset to {jumpForce} at {Time.time}");
        jumpForceChange = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("DeadCollider"))
        {
            isDead = true;
            Lives--;
            anim.SetTrigger("Dead");
            StartCoroutine(HandleDeath());
        }
        else if (collision.CompareTag("Squish") && rb.linearVelocity.y < 0)
        {
            collision.GetComponentInParent<Enemy>().TakeDamage(0, DamageType.JumpedOn);
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private IEnumerator HandleDeath()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        col.enabled = false;

        anim.SetTrigger("Dead");

        yield return new WaitForSeconds(1.5f); // Wait for death animation

        sr.enabled = false;

        yield return new WaitForSeconds(0.5f); // Optional delay

        // Respawn
        transform.position = respawnPoint.position;
        rb.bodyType = RigidbodyType2D.Dynamic;
        col.enabled = true;
        sr.enabled = true;

        isDead = false;
    }

}


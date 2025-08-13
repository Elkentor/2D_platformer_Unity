using Unity.VisualScripting;
using UnityEngine;

public class PickUpCoin : MonoBehaviour
{
    // Optional: Add score value if needed
    // public int scoreValue = 10;

    private Animator animator;
    private bool isPickedUp = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isPickedUp) return;

        if (other.CompareTag("Player"))
        {
            isPickedUp = true;

            animator.SetTrigger("PickupGet");

            // Only use the coroutine — don't destroy immediately
            StartCoroutine(DestroyAfterAnimation());
        }
    }


    System.Collections.IEnumerator DestroyAfterAnimation()
    {
        // Wait for animation to finish (adjust timing as needed)
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
}
using UnityEngine;
public class Pickups : MonoBehaviour
{
    public enum PickupType
    {
        Life = 0,
        Score = 1,
        Powerup = 2
    }

    public PickupType pickupType = PickupType.Life; // Type of the pickup

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController pc = collision.GetComponent<PlayerController>();
            Animator animator = collision.GetComponent<Animator>();
            Animator pickupAnimator = GetComponent<Animator>();

            if (pickupAnimator != null)
            {
                pickupAnimator.SetTrigger("PickupGet");
            }

            switch (pickupType)
            {
                case PickupType.Life:
                    pc.lives++;
                    Debug.Log("Life collected! Current lives: " + pc.lives);
                    break;
                case PickupType.Score:
                    pc.score++;
                    //if (animator != null)
                    Debug.Log("Score collected! Current score: " + pc.score);
                    break;
                case PickupType.Powerup:
                    pc.ActivateJumpForceChange();
                    break;
            }
         
            Destroy(gameObject, 1f); // Destroy the pickup after collection
        }
    }
}

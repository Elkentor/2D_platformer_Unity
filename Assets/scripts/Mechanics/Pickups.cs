using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class Pickups : MonoBehaviour
{
    public enum PickupType
    {
        Life = 0,
        Score = 1,
        Powerup = 2
    }

    public int Score = 0;

    public PickupType pickupType = PickupType.Life;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController pc = collision.GetComponent<PlayerController>();
            Animator pickupAnimator = GetComponent<Animator>();

            if (pickupAnimator != null)
            {
                pickupAnimator.SetTrigger("PickupGet");
                //after animation plays stop the animation and destroy the pickup object
                Destroy(gameObject, 1f); // Delay destruction to allow animation to play
            }

            switch (pickupType)
            {
                case PickupType.Life:
                    Debug.Log("Life collected! Adding 1 life.");
                    GameManager.Instance.AddLife(1);            
                    break;
                    

                case PickupType.Score:
                    Debug.Log("Score collected! Adding 100 points.");
                    GameManager.Instance.AddScore(100);                  
                    break;
                    

                case PickupType.Powerup:
                    Debug.Log("Powerup collected! Activating jump force change.");
                    pc.ActivateJumpForceChange();
                    break;
            }

            //Destroy(gameObject, 1f);
        }
    }
}


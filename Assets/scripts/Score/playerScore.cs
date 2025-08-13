using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int currentScore = 0;

    public void AddScore(int amount)
    {
        currentScore += amount;
        Debug.Log("Score: " + currentScore);
    }
}

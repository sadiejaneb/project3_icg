using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int lives = 3;  // Player's total lives
    public delegate void PlayerDied();  // Delegate for player death event
    public static event PlayerDied OnPlayerDied;  // Event triggered when the player dies

    // This method gets called when the player is hit by a bullet
    public void TakeDamage(int damageAmount)
    {
        lives -= damageAmount;

        if (lives <= 0)
        {
            lives = 0;
            Die();
        }
    }

    void Die()
    {
        // Handle player death here
        // For example, you can disable player movement, play a death animation, etc.

        // Trigger the OnPlayerDied event
        OnPlayerDied?.Invoke();

        Debug.Log("Player Died!");
    }
}
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int lives = 3;  // Player's total lives
    public delegate void PlayerDied();  // Delegate for player death event
    public static event PlayerDied OnPlayerDied;  // Event triggered when the player dies

    public AudioClip damageSound;  // Drag the damage sound clip here in the inspector
    private AudioSource audioSource;  // Reference to the AudioSource component

    private void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        // Ensure that the audio source exists
        if (audioSource == null)
        {
            Debug.LogWarning("No AudioSource found on the player. Adding one.");
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // This method gets called when the player is hit by a bullet
    public void TakeDamage(int damageAmount)
    {
        lives -= damageAmount;

        // Play the damage sound
        PlayDamageSound();

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

    void PlayDamageSound()
    {
        if (damageSound && audioSource)
        {
            audioSource.PlayOneShot(damageSound);
        }
    }
}

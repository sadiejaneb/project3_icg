using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int lives = 3;  // Player's total lives
    public delegate void PlayerDied();  // Delegate for player death event
    public static event PlayerDied OnPlayerDied;  // Event triggered when the player dies
    private AudioSource audioSource;  // Reference to the AudioSource component

    public AudioClip damageSound;  // Drag the damage sound clip here in the inspector
    public AudioClip damageYell;

    public delegate void PlayerTookDamage(int currentLives);
    public static event PlayerTookDamage OnPlayerTookDamage;
    public UIManager uiManager;
    private navigation_patrol navPatrolScript;


    private void Start()
    {
        navPatrolScript = FindObjectOfType<navigation_patrol>();
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
    public void TakeDamage()

    {
        lives--;
        OnPlayerTookDamage?.Invoke(lives);

        PlayDamageSound();

        if (lives <= 0)
        {
            Die();
            navPatrolScript.stopShooting();
        }
    }



    void Die()
    {
        Debug.Log("Player is dead");  // This will confirm if Die() is being called
        if (uiManager != null)
        {
            uiManager.GameOver();
        }
        else
        {
            Debug.LogWarning("UIManager reference is not set on PlayerHealth.");
        }
    }


    void PlayDamageSound()
    {
        if (damageSound && audioSource)
        {
            audioSource.PlayOneShot(damageYell);
            audioSource.PlayOneShot(damageSound);
        }
    }

}
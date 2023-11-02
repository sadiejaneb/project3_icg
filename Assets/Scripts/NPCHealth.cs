using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealth : MonoBehaviour
{
    private int health = 3;

    public AudioClip damageSound;  // Drag the sound clip in the inspector
    private AudioSource audioSource;  // Reference to the audio source component

    private void Start()
    {
        // Assuming AudioSource component is attached to the same GameObject as this script
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        // Play the damage sound
        if (audioSource && damageSound)
        {
            audioSource.PlayOneShot(damageSound);
        }

        if (health <= 0)
        {
            GameManager.Instance.NPCDied();
            Die();
        }
    }

    void Die()
    {
        // Logic to handle NPC's death...
        Destroy(this.gameObject);
    }
}

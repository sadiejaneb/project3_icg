using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealth : MonoBehaviour
{
    private int health = 3;
   
    [SerializeField]

    private navigation_patrol navigationPatrol;

    private void Start()
    {
        navigationPatrol = GetComponent<navigation_patrol>();
    }


    public void TakeDamage(int amount)
    {
        if (health >0) {
        health -= amount;
        navigationPatrol.playDamageSound();
        }

        if (health <= 0)
        {
            StartCoroutine(DeathSound());
        }
    }
    IEnumerator DeathSound()
    {
        // Play the death sound
        navigationPatrol.playDamageSound();
        // Wait until the clip has finished playing
        float soundDuration = navigationPatrol.playDeathSound();
        yield return new WaitForSeconds(soundDuration);
        Die();
    }
    private void Die() {
        // Notify the GameManager that the NPC has died
        GameManager.Instance.NPCDied();

        // Logic to handle NPC's death...
        Destroy(gameObject);
    }
}

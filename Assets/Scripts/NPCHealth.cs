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
        
        health -= amount;

        navigationPatrol.playDamageSound();

        if (health <= 0)
        {
            navigationPatrol.playDamageSound();
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

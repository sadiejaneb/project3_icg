using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float explosionRadius = 3f;
    public int explosionDamage = 3;

    private void OnCollisionEnter(Collision collision)
    {
        // Check for direct collision with an NPC and deal damage
        NPCHealth npcHealth = collision.gameObject.GetComponent<NPCHealth>();
        if (npcHealth != null)
        {
            npcHealth.TakeDamage(explosionDamage);
            Debug.Log("Direct hit on: " + collision.gameObject.name);
        }

        // Explode for area of effect damage
        Explode();
    }

    void Explode()
    {
        // Get all colliders within the explosion radius.
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            // Check if the nearby object has the NPCHealth script and if so, apply damage.
            NPCHealth npcHealth = nearbyObject.GetComponent<NPCHealth>();
            if (npcHealth != null)
            {
                npcHealth.TakeDamage(explosionDamage);
            }
        }

        // Here, add any other explosion effects, like instantiating an explosion particle system.

        // Finally, destroy the projectile object.
        Destroy(gameObject);
    }
}

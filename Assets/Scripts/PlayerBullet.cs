using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public AudioClip impactSound; // Drag your impact sound here
    public GameObject impactEffect; // Drag your particle system prefab here
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) // If no audio source on the bullet, add one
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (impactSound) // Play sound if available
        {
            audioSource.PlayOneShot(impactSound);
        }

        if (impactEffect) // Instantiate particle effect if available
        {
            GameObject effect = Instantiate(impactEffect, transform.position, Quaternion.identity);
            Destroy(effect, 5f); // Optional: Destroy the effect after 5 seconds
        }

        Destroy(gameObject); // Destroy the bullet
    }
}
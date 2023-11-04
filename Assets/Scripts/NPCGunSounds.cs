using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGunSounds : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip gunFireSound;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void playGunFireSound()
    {
        audioSource.PlayOneShot(gunFireSound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton reference

    public AudioSource audioSource; 
    public AudioClip backgroundMusic; // the initial background music clip here
    public AudioClip zoneMusic; // the music for when the player enters the zone

    private void Awake()
    {
        // Setup singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        audioSource.clip = backgroundMusic;
        audioSource.Play();
    }

    public void PlayZoneMusic()
    {
        audioSource.clip = zoneMusic;
        audioSource.Play();
    }
}

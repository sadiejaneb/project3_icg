using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup blackScreenCanvasGroup;
    [SerializeField]
    private TextMeshProUGUI gameOverText;
    [SerializeField]
    private TextMeshProUGUI youWinText;
    private Sprite[] livesSprites;
    [SerializeField]
   private Image livesImage;
    private void Awake()
    {
        // Subscribe to the player death event
        PlayerHealth.OnPlayerDied += UpdateLives;
    }

    private void Start()
    {
        if (gameOverText != null){
            gameOverText.enabled = false;
        }
        if (youWinText != null){
            youWinText.enabled = false;
        }
        if (blackScreenCanvasGroup == null)
        {
            blackScreenCanvasGroup = GameObject.FindGameObjectWithTag("BlackScreen").GetComponent<CanvasGroup>();
            blackScreenCanvasGroup.alpha = 0; // Set the initial alpha to 0 (completely transparent)
        }// Initialize the lives display with the maximum lives image (assuming player starts with max lives)
        if (livesSprites.Length > 0)
        {
            livesImage.sprite = livesSprites[0];
        }
    }
    private void UpdateLives()
    {
        // Assuming the player object has a reference to the PlayerHealth script
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();

        if (playerHealth)
        {
            int lives = playerHealth.lives;

            // Update the sprite based on the player's remaining lives
            if (lives >= 0 && lives < livesSprites.Length)
            {
                livesImage.sprite = livesSprites[lives];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        // Unsubscribe from the player death event when this object is destroyed
        PlayerHealth.OnPlayerDied -= UpdateLives;
    }
    private void GameOver()
    {
        blackScreenCanvasGroup.alpha = 1f;
        Invoke("QuitGame", 3f);
    }
    void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup blackScreenCanvasGroup;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI youWinText;
    [SerializeField] private Sprite[] livesSprites; // Array of sprites (3 hearts, 2 hearts, 1 heart, no hearts)
    [SerializeField] private Image livesImage; // Reference to the Image component that displays the lives

    private void Awake()
    {
        PlayerHealth.OnPlayerTookDamage += UpdateLives; // Subscribe to player damage event
    }

    private void Start()
    {
        if (gameOverText != null)
            gameOverText.enabled = false;

        if (youWinText != null)
            youWinText.enabled = false;

        if (blackScreenCanvasGroup == null)
        {
            blackScreenCanvasGroup = GameObject.FindGameObjectWithTag("BlackScreen").GetComponent<CanvasGroup>();
            blackScreenCanvasGroup.alpha = 0; // Set the initial alpha to 0 (completely transparent)
        }

        // Initialize the lives display with the maximum lives image (assuming player starts with max lives)
        if (livesSprites.Length > 0 && livesImage != null)
        {
            livesImage.sprite = livesSprites[3]; // Assuming player starts with 3 lives
        }
    }

    private void UpdateLives(int currentLives)
    {
        if (livesImage != null && livesSprites != null && currentLives >= 0 && currentLives < livesSprites.Length)
        {
            livesImage.sprite = livesSprites[currentLives];
        }
    }

    private void OnDestroy()
    {
        PlayerHealth.OnPlayerTookDamage -= UpdateLives; // Unsubscribe from the event when the object is destroyed
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

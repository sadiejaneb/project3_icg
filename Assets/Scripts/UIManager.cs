using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup blackScreenCanvasGroup;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI youWinText;
    [SerializeField] private Sprite[] livesSprites; // Array of sprites (3 hearts, 2 hearts, 1 heart, no hearts)
    [SerializeField] private Image livesImage; // Reference to the Image component that displays the lives
    public Image rifleUIImage; 
    public Image rocketLauncherUIImage;
    public TextMeshProUGUI rocketLauncherAmmoText;
    public TextMeshProUGUI rifleAmmoText;
    public TextMeshProUGUI reserveRifleAmmoText; 
    public TextMeshProUGUI reserveRocketAmmoText;
  

    private void Awake()
    {
        PlayerHealth.OnPlayerTookDamage += UpdateLives; // Subscribe to player damage event
    }

    private void Start()
    {
        // Deactivate all weapon UI images
        if (rifleUIImage) rifleUIImage.gameObject.SetActive(false);
        if (rocketLauncherUIImage) rocketLauncherUIImage.gameObject.SetActive(false);
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
    // Call this method to update the rocket launcher ammo display
    public void UpdateRocketLauncherAmmo(int currentAmmo)
    {
        if (rocketLauncherAmmoText != null)
        {
            rocketLauncherAmmoText.text = $"{currentAmmo}/1";
        }
    }
    public void UpdateRifleAmmo(int currentAmmo)
    {
        if (rifleAmmoText != null)
        {
            rifleAmmoText.text = $"{currentAmmo}/6";
        }
    }
    public void UpdateReserveRifleAmmo(int reserveAmmo)
    {
        if (reserveRifleAmmoText != null)
        {
            reserveRifleAmmoText.text = $"{reserveAmmo}/20";
        }
    }

    public void UpdateReserveRocketAmmo(int reserveAmmo)
    {
        if (reserveRocketAmmoText != null)
        {
            reserveRocketAmmoText.text = $"{reserveAmmo}/3";
        }
    }

    private void UpdateLives(int currentLives)
    {
        if (livesImage != null && livesSprites != null && currentLives >= 0 && currentLives < livesSprites.Length)
        {
            livesImage.sprite = livesSprites[currentLives];
        }
    }
    public void SetWeaponUIActive(string weaponTag)
    {

        // Activate the UI image for the specified weapon
        switch (weaponTag)
        {
            case "Rifle":
                if (rifleUIImage) rifleUIImage.gameObject.SetActive(true);
                break;
            case "RocketLauncher":
                if (rocketLauncherUIImage) rocketLauncherUIImage.gameObject.SetActive(true);
                break;
        }
    }


    private void OnDestroy()
    {
        PlayerHealth.OnPlayerTookDamage -= UpdateLives; // Unsubscribe from the event when the object is destroyed
    }
    public void YouWin()
    {
        youWinText.enabled = true;
        Invoke("QuitGame", 3f);
    }

    public void GameOver()
    {
        gameOverText.enabled = true;
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

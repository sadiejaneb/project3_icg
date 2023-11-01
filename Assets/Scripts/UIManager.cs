using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup blackScreenCanvasGroup;
    [SerializeField]
    private TextMeshProUGUI gameOverText;
    [SerializeField]
    private TextMeshProUGUI youWinText;
    private Sprite[] _livesSprites;
    [SerializeField]
   // private Image _livesImage;
    
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
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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

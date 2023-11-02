using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int deadNPCs = 0;
    public bool playerInWinZone = false;
    public int totalNPCs = 2;
    public UIManager uiManager;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
        }
    }

    public void NPCDied()
    {
        deadNPCs++;
        CheckWinCondition();
    }

    public void PlayerEnteredWinZone()
    {
        playerInWinZone = true;
        CheckWinCondition();
    }
    public void PlayerExitedWinZone() {
        playerInWinZone = false;
    }

    void CheckWinCondition()
    {
        if (deadNPCs >= totalNPCs && playerInWinZone)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        uiManager.YouWin();
    }
}




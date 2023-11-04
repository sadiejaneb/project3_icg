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
    }

    public void NPCDied()
    {
        deadNPCs++;
        uiManager.UpdateDeadNPCsCounter(deadNPCs);
        CheckWinCondition();
    }


    public void PlayerEnteredWinZone()
    {
        playerInWinZone = true;
        CheckWinCondition();
    }

    public void PlayerExitedWinZone()
    {
        playerInWinZone = false;
    }

    private void CheckWinCondition()
    {
        if (deadNPCs >= totalNPCs && playerInWinZone)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        uiManager.YouWin();
    }

}

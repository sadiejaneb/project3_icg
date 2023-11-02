using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming your player has a "Player" tag
        {
            GameManager.Instance.PlayerEnteredWinZone();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming your player has a "Player" tag
        {
            GameManager.Instance.PlayerExitedWinZone();
        }
    }
}

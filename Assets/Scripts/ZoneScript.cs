using UnityEngine;

public class ZoneScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered zone");
            navigation_patrol.playerInZone = true; //alert enemies that player is near
            AudioManager.Instance.PlayZoneMusic(); // Switch to zone music
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            navigation_patrol.playerInZone = false; //alert enemies that player is no longer near
            AudioManager.Instance.PlayBackgroundMusic(); // Switch to main music
        }
    }
}
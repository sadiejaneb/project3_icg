using UnityEngine;

public class ZoneScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered zone");
            navigation_patrol.playerInZone = true;
           
        }
    }
}






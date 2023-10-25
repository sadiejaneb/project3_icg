using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneScript : MonoBehaviour
{
    public navigation_patrol npcScript;
    // Start is called before the first frame update
 void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        npcScript.StartChasing(other.transform);
    }
}
void OnTriggerExit(Collider other)
{
    if (other.CompareTag("Player"))
    {
        npcScript.StopChasing();
    }
}
}

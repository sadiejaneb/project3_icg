using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bullet hit: " + other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by bullet");
            Destroy(gameObject);
        }
    }
}
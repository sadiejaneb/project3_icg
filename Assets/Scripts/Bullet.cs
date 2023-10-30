using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 20f;
    public int bulletDamage = 10;

    private void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Deal damage to the player here, if you have a health system in place
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Enemy"))
        {
            // Optional: Destroy bullet on hitting other obstacles
            Destroy(gameObject);
        }
    }
}

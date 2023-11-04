using UnityEngine;

public class RifleController : MonoBehaviour
{
    private WeaponController weaponController;
    public GameObject bulletPrefab;
    private bool isReloading = false;
    private bool isShooting = false;
    public int gunDamage = 1;
    public float fireRate = .25f;
    public float weaponRange = 50f;
    public Transform gunEnd; // Where the bullet visual is instantiated from

    // Sound-related variables
    private AudioSource audioSource;
    public AudioClip shootingSound; // Drag and drop shooting sound in the inspector
    public AudioClip reloadingSound; // Drag and drop reloading sound in the inspector

    private float nextFire;
    public Camera fpsCam;
    public GameObject impactEffect;


    private void Start()
    {
        weaponController = GetComponentInParent<WeaponController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isReloading || isShooting)
            return;

        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            if (weaponController.TryUseRifleAmmo()) // Check with WeaponController if there's ammo to use
            {
                FireRifle();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    void FireRifle()
    {
        isShooting = true;

        nextFire = Time.time + fireRate;

        // Play gunshot sound
        if (audioSource && shootingSound)
            audioSource.PlayOneShot(shootingSound);

        // Adjust the bullet's rotation by creating a new Quaternion rotation that adds 90 degrees on the X-axis
        Quaternion bulletRotation = gunEnd.rotation * Quaternion.Euler(90, 0, 0);

        // Instantiate bullet with the adjusted rotation
        GameObject bulletInstance = Instantiate(bulletPrefab, gunEnd.position, bulletRotation);
        Rigidbody bulletRb = bulletInstance.GetComponent<Rigidbody>();
        if (bulletRb)
        {
            float bulletSpeed = 5000f; // Adjust for desired bullet speed
            bulletRb.AddForce(gunEnd.forward * bulletSpeed);
        }

        // Raycast shooting
        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
        {
            // Instantiate the impact effect at the point of collision
            Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Debug.Log("Hit: " + hit.transform.name);
            // Check if the hit object has the NPCHealth script
            NPCHealth npcHealth = hit.transform.GetComponent<NPCHealth>();
            if (npcHealth != null)
            {
                npcHealth.TakeDamage(1); // 1 is the damage amount for a rifle bullet
            }
 
    }
        isShooting = false;
    }

    void Reload()
    {
        if(weaponController.reserveRifleBullets <= 0 || isReloading || isShooting) 
        return; 
        
        isReloading = true;

        // Play reloading sound
        if (audioSource && reloadingSound)
            audioSource.PlayOneShot(reloadingSound);

        weaponController.ReloadRifle(); // Notify the WeaponController to reload
        isReloading = false;
    }
}
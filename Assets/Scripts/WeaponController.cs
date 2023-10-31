using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    List<GameObject> collectedWeapons = new List<GameObject>();
    private Dictionary<string, GameObject> weaponMap = new Dictionary<string, GameObject>();
    public GameObject equippedRocketLauncher;
    public GameObject equippedRifle;
    private GameObject currentWeapon;
    private int currentWeaponIndex = 0;

    private void Start()
    {
        // Deactivate both weapons on player start
        equippedRocketLauncher.SetActive(false);
        equippedRifle.SetActive(false);
        currentWeapon = null; // No weapon equipped at the start

        // Use direct references to populate weaponMap
        weaponMap["Rifle"] = equippedRifle;
        weaponMap["RocketLauncher"] = equippedRocketLauncher;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with: " + other.gameObject.name);

        if (weaponMap.ContainsKey(other.tag))
        {
            Debug.Log("Detected " + other.tag);
            ActivateWeapon(weaponMap[other.tag]);
            collectedWeapons.Add(weaponMap[other.tag]);  // Add to the collected weapons list
            Destroy(other.gameObject);
        }

    }

    private void ActivateWeapon(GameObject weaponToActivate)
    {
        // Deactivate all weapons
        if (equippedRocketLauncher) equippedRocketLauncher.SetActive(false);
        if (equippedRifle) equippedRifle.SetActive(false);

        // Activate the desired weapon
        weaponToActivate.SetActive(true);
        currentWeapon = weaponToActivate; // Set the current weapon
    }

    private void SwitchWeapon()
    {
        if (collectedWeapons.Count == 0) return; // No weapon to switch to

        // Deactivate the current weapon
        collectedWeapons[currentWeaponIndex].SetActive(false);

        // Update index to next weapon in the list
        currentWeaponIndex = (currentWeaponIndex + 1) % collectedWeapons.Count;

        // Activate the next weapon
        collectedWeapons[currentWeaponIndex].SetActive(true);
    }
}

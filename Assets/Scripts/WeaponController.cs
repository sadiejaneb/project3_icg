using System.Collections.Generic;
using UnityEngine;
using BigRookGames.Weapons;

public class WeaponController : MonoBehaviour
{
    List<GameObject> collectedWeapons = new List<GameObject>();
    private Dictionary<string, GameObject> weaponMap = new Dictionary<string, GameObject>();
    public GameObject equippedRocketLauncher;
    public GameObject equippedRifle;
    private GameObject currentWeapon;
    private int currentWeaponIndex = 0;
    public int rocketLauncherAmmo = 0;
    public int maxBulletsPerMagazine = 6; // Max bullets that can be loaded into the rifle
    private int currentRifleBullets = 0; // Bullets currently in the rifle (magazine)
    public int reserveRifleBullets = 0; // Bullets in reserve

    public int maxReserveRifleBullets = 20;
    public int currentAmmo; // Current ammo in the gun

    public GunfireController rocketLauncherScript;

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
            if (!collectedWeapons.Contains(weaponMap[other.tag]))
            {
                collectedWeapons.Add(weaponMap[other.tag]);
            }
            ActivateWeapon(weaponMap[other.tag]);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Grenade"))
        {
            rocketLauncherAmmo++;  // Add ammo as per your requirements
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Ammo"))
        {
            Debug.Log("Ammo collected");
            reserveRifleBullets += 6;
            reserveRifleBullets = Mathf.Min(reserveRifleBullets, maxReserveRifleBullets);  // Ensure we don't exceed the cap // Add ammo as per your requirements
                                                                                           // Check if the current weapon is the rifle and try to reload it if it has 0 ammo
            RifleController rifle = currentWeapon.GetComponent<RifleController>(); // Assuming currentWeapon is your active weapon and you have a reference to it
            if (rifle)
            {
                TryReloadRifle();
            }
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
    public void TryShootRocketLauncher()
    {
        if (rocketLauncherAmmo > 0)
        {
            rocketLauncherScript.FireWeapon();
            rocketLauncherAmmo--;
        }
    }
    public bool CanFire()
    {
        return rocketLauncherAmmo > 0;
    }
    public void UseAmmo()
    {
        if (rocketLauncherAmmo > 0)
        {
            rocketLauncherAmmo--;
        }
    }
    public bool TryUseRifleAmmo()
    {
        if (currentRifleBullets > 0)
        {
            currentRifleBullets--;
            return true;
        }
        return false;
    }
    public void AddRifleAmmo(int count)
    {
        // This function can be called when the player picks up ammo.
        reserveRifleBullets += count;
    }
    public void TryReloadRifle()
    {
        if (currentAmmo == 0 && reserveRifleBullets > 0) // Assuming you have a way to access or check reserveRifleBullets
        {
            ReloadRifle();
        }
    }
    // Called by RifleController to reload
    public void ReloadRifle()
    {
        // Calculate how many bullets are needed to fill the magazine
        int bulletsNeeded = maxBulletsPerMagazine - currentRifleBullets;

        // Check if we have enough bullets in reserve
        if (reserveRifleBullets >= bulletsNeeded)
        {
            currentRifleBullets += bulletsNeeded;
            reserveRifleBullets -= bulletsNeeded;
        }
        else
        {
            // If we don't have enough bullets in reserve, load all remaining bullets
            currentRifleBullets += reserveRifleBullets;
            reserveRifleBullets = 0;
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 250, 20), "Rocket Launcher Ammo: " + rocketLauncherAmmo);
        GUI.Label(new Rect(10, 30, 250, 20), "Rifle Ammo Reserve: " + reserveRifleBullets);
        GUI.Label(new Rect(10, 40, 250, 20), "Rifle Ammo: " + currentRifleBullets);
    }
}

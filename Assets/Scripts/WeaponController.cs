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
    public int rocketLauncherAmmo = 0; // Amount of grenades the player has in rocket launcher
    public int reserveRocketAmmo = 0; // Amount of grenades the player has in reserve
    public const int maxReserveRocketAmmo = 3; // Maximum grenades player can hold in reserve

    public int maxBulletsPerMagazine = 6; // Max bullets that can be loaded into the rifle
    private int currentRifleBullets = 0; // Bullets currently in the rifle (magazine)
    public int reserveRifleBullets = 0; // Bullets in reserve

    public int maxReserveRifleBullets = 20;
    public int currentAmmo; // Current ammo in the gun

    public GunfireController rocketLauncherScript;
    private UIManager uiManager;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        // Deactivate both weapons on player start
        equippedRocketLauncher.SetActive(false);
        equippedRifle.SetActive(false);
        currentWeapon = null; // No weapon equipped at the start

        // Use direct references to populate weaponMap
        weaponMap["Rifle"] = equippedRifle;
        weaponMap["RocketLauncher"] = equippedRocketLauncher;
    }
    // Call this method whenever you need to update the ammo count on the UI
    public void RefreshAmmoUI()
    {
        if (uiManager != null)
        {
            uiManager.UpdateRocketLauncherAmmo(rocketLauncherAmmo);
        }
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
            reserveRocketAmmo += 1;
            reserveRocketAmmo = Mathf.Min(reserveRocketAmmo, maxReserveRocketAmmo);
            if (rocketLauncherAmmo == 0) // Auto-reload if empty
            {
                TryReloadRocketLauncher();
            }
            Destroy(other.gameObject);
            // After updating the ammo count, refresh the UI
            RefreshReserveRocketAmmoUI();
        }
        else if (other.CompareTag("Ammo"))
        {
            reserveRifleBullets += 6;
            reserveRifleBullets = Mathf.Min(reserveRifleBullets, maxReserveRifleBullets);  // Ensure we don't exceed the cap // Add ammo as per your requirements
                                                                                           if (currentRifleBullets == 0) {
            
                TryReloadRifle();
                                                                                           }
            
            Destroy(other.gameObject);
            RefreshReserveRifleAmmoUI();
        }
        if (other.CompareTag("Rifle") || other.CompareTag("RocketLauncher"))
        {
            ActivateWeapon(weaponMap[other.tag]);
            uiManager.SetWeaponUIActive(other.tag); // Make the UI image visible
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
                                          // Call the UIManager to update the weapon UI based on the weapon's tag
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
            RefreshRocketLauncherAmmoUI(); // Update the UI after using ammo

            // Check for reload if empty
            if (rocketLauncherAmmo == 0)
            {
                TryReloadRocketLauncher();
            }
        }
    }
    public void TryReloadRocketLauncher()
    {
        if (rocketLauncherAmmo == 0 && reserveRocketAmmo > 0)
        {
            rocketLauncherAmmo++;
            reserveRocketAmmo--;
            RefreshRocketLauncherAmmoUI(); // Update the UI after using ammo
        }
        // After updating the ammo count, refresh the UI
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
            RefreshRocketLauncherAmmoUI(); // Update the UI after using ammo
        }
    }
    public bool TryUseRifleAmmo()
    {
        if (currentRifleBullets > 0)
        {
            currentRifleBullets--;
            RefreshRifleAmmoUI();
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
        RefreshRifleAmmoUI();
        RefreshReserveRifleAmmoUI();
    }
    // Inside the WeaponController class

    public void RefreshRocketLauncherAmmoUI()
    {
        if (uiManager != null)
        {
            uiManager.UpdateRocketLauncherAmmo(rocketLauncherAmmo);
        }
    }
    // Call this method whenever you need to update the rifle ammo count on the UI
    public void RefreshRifleAmmoUI()
    {
        if (uiManager != null)
        {
            uiManager.UpdateRifleAmmo(currentRifleBullets);
        }
    }
    public void RefreshReserveRifleAmmoUI()
    {
        if (uiManager != null)
        {
            uiManager.UpdateReserveRifleAmmo(reserveRifleBullets);
        }
    }

    // Call this method whenever you need to update the reserve rocket ammo count on the UI
    public void RefreshReserveRocketAmmoUI()
    {
        if (uiManager != null)
        {
            uiManager.UpdateReserveRocketAmmo(reserveRocketAmmo);
        }
    }
}

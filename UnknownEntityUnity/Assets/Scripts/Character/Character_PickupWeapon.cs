using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_PickupWeapon : MonoBehaviour
{
    [Header("Script References")]
    public MouseInputs moIn;
    public HUD_Manager HUDManager;
    public Character_Attack atk;
    public Character_EquippedWeapons charEquippedWeapons;
    [Header("To-set variables")]
    public ContactFilter2D lootLayer;
    public Collider2D charLootCol;
    public Transform playerTrans;
    [Header("Read Only")]
    List<Collider2D> results = new List<Collider2D>();

    public void CanIPickUpWeapon() {
        // Check if there are any weapons on the floor.
        Physics2D.OverlapCollider(charLootCol, lootLayer, results);
        if (results.Count > 0) {
            if (results.Count > 1) {
                float shortestDist = 999f;
                Collider2D closestCol;
                int loopIndexCount = 0;
                // Swap active weapon with closest weapon loot.
                foreach(Collider2D result in results) {
                    float distCheck = (result.transform.position - playerTrans.position).sqrMagnitude;
                    if (distCheck < shortestDist) {
                        shortestDist = distCheck;
                        closestCol = result;
                        if (loopIndexCount == results.Count - 1) {
                            WeaponPickup weaponOnFloor = closestCol.gameObject.GetComponent<WeaponPickup>();
                            EquipWeapon(weaponOnFloor);
                        }
                    }
                    loopIndexCount++;
                }
            }
            else {
                // Swap active weapon with weapon loot.
                WeaponPickup weaponOnFloor = results[0].gameObject.GetComponent<WeaponPickup>();
                EquipWeapon(weaponOnFloor);
            }
            // Assign old active weapon to weapon loot on floor.
        }
        else {
            //Debug.Log("Tried to pick up a weapon, but nothing was found.");
        }
    }
    // Assign old active weapon to weapon loot on floor.
    void EquipWeapon (WeaponPickup weaponOnFloor) {
        SO_Weapon weapToDrop = charEquippedWeapons.activeWeapon;
        float weapOnFloorDurability = weaponOnFloor.durability;
        if (charEquippedWeapons.inactiveWeapon == null) {
            charEquippedWeapons.inactiveWeapon = charEquippedWeapons.activeWeapon;
            charEquippedWeapons.activeWeapon = weaponOnFloor.weaponBase;
            charEquippedWeapons.WeaponSwap(true);
            charEquippedWeapons.SetActiveWeaponDurability(weapOnFloorDurability);
            weaponOnFloor.TurnOff();
        }
        else {
            weaponOnFloor.durability = charEquippedWeapons.GetActiveWeaponDurability();
            charEquippedWeapons.activeWeapon = weaponOnFloor.weaponBase;
            weaponOnFloor.weaponBase = weapToDrop;
            weaponOnFloor.SwapWeaponLoot(weapToDrop);
            charEquippedWeapons.WeaponSwap(false);
            charEquippedWeapons.SetActiveWeaponDurability(weapOnFloorDurability);
        }
        // Change the HUD image of the active weapon.
        HUDManager.playerWeapons.PickUpWeapon(charEquippedWeapons.activeWeapon.weaponSprite, weapOnFloorDurability);
        Debug.Log("Dropped weapon: " + weapToDrop + " and equipped: " + charEquippedWeapons.activeWeapon);
    }
}
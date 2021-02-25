using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_PickupWeapon : MonoBehaviour
{
    [Header("Script References")]
    public MouseInputs moIn;
    public HUD_Manager HUDManager;
    public Character_Attack atk;
    public Character_EquippedWeapons equippedWeaps;
    [Header("To-set variables")]
    public ContactFilter2D lootLayer;
    public Collider2D charLootCol;
    public Transform playerTrans;
    [Header("Read Only")]
    List<Collider2D> results = new List<Collider2D>();

    void Update() {
        // TRANSFER TO AN: OTHER BUTTON ACTIONS SCRIPT IN THE Inputs GameObject
        if (moIn.interactPressed) {
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
                                WeaponPickup weapLoot = closestCol.gameObject.GetComponent<WeaponPickup>();
                                EquipWeapon(weapLoot);
                            }
                        }
                        loopIndexCount++;
                    }
                }
                else {
                    // Swap active weapon with weapon loot.
                    WeaponPickup weapLoot = results[0].gameObject.GetComponent<WeaponPickup>();
                    EquipWeapon(weapLoot);
                }
                // Assign old active weapon to weapon loot on floor.
            }
            else {
                //Debug.Log("Tried to pick up a weapon, but nothing was found.");
            }
        }
    }
    // Assign old active weapon to weapon loot on floor.
    void EquipWeapon (WeaponPickup weapLoot) {
        SO_Weapon weapToDrop = equippedWeaps.firstWeap;
        if (equippedWeaps.secondWeap == null) {
            equippedWeaps.secondWeap = equippedWeaps.firstWeap;
            equippedWeaps.firstWeap = weapLoot.weaponBase;
            equippedWeaps.Changes();
            weapLoot.TurnOff();
        }
        else {
            equippedWeaps.firstWeap = weapLoot.weaponBase;
            weapLoot.weaponBase = weapToDrop;
            weapLoot.SwapWeaponLoot(weapToDrop);
            equippedWeaps.Changes();
        }
        // Change the HUD image of the active weapon.
        HUDManager.playerWeapons.PickUpWeapon(equippedWeaps.firstWeap.weaponSprite);
        Debug.Log("Dropped weapon: " + weapToDrop + " and equipped: " + equippedWeaps.firstWeap);
    }
}
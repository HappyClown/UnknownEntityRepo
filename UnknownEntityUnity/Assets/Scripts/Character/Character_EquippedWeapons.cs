using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_EquippedWeapons : MonoBehaviour
{
    [Header("Script References")]
    public Character_Attack charAtk;
    public MouseInputs moIn;
    public HUD_Manager HUDManager;
    [Header("To-set Variables")]
    public SO_Weapon activeWeapon;
    public SpriteRenderer weaponSpriteR;
    [Header("Read Only")]
    public SO_Weapon inactiveWeapon;
    public float weaponOneDurability = 100;
    public float weaponTwoDurability = 100;
    public bool weaponOneIsActive = false; // Starts on false for now because a weapon Changes() is called on start.
    public bool canSwapWeapon = true;

    void Start() {
        Changes();
    }

    public bool CanISwapWeapon() {
        // Weapon swap if two weapons are equipped.
        if (canSwapWeapon) {
            if (activeWeapon != null && inactiveWeapon != null) {
                SO_Weapon tempWeapon = inactiveWeapon;
                inactiveWeapon = activeWeapon;
                activeWeapon = tempWeapon;
                Changes();
                // HUD active weapon changes.
                HUDManager.playerWeapons.SwapActiveWeapon();
            }
            return true;
        }
        return false;
    }

    public void Changes() {
        // Stop its rotations.
        if (charAtk.curWeaponMotion) {
            charAtk.curWeaponMotion.StopMotions();
        }
        //Assign the weapon motion script.
        for (int i = 0; i < activeWeapon.attackChains.Length; i++) {
            if (charAtk.weaponMotions.Count < i+1) {
                charAtk.weaponMotions.Add(charAtk.weaponMotionController.CheckMotionList(activeWeapon.attackChains[i].sO_Weapon_Motion.weapon_Motion));
                //charAtk.weaponMotions[i] = 
            }
            else {
                charAtk.weaponMotions[i] = charAtk.weaponMotionController.CheckMotionList(activeWeapon.attackChains[i].sO_Weapon_Motion.weapon_Motion);
            }
        }
        charAtk.weaponMotion = charAtk.weaponMotions[0];

        // Assign the Scriptable Object weapon in the Character_Attack script.
        charAtk.weapon = activeWeapon;
        // Change the weapon's appearance.
        weaponSpriteR.sprite = activeWeapon.weaponSprite;
        // Find somewhere to insert the rest of the previous weapon's attack duration. 
        weaponSpriteR.color = Color.white;
        // Go back to the first chain.
        charAtk.atkChain.OnWeaponSwap(activeWeapon, inactiveWeapon);
        // Assign its resting angle.
        charAtk.weaponTrans.localEulerAngles = charAtk.WeapAtkChain.sO_Weapon_Motion.restingRotation;
        // Assign its resting position.
        charAtk.weaponTrans.localPosition = charAtk.WeapAtkChain.sO_Weapon_Motion.restingPosition;
        // Change the active weapon by reversing the bool. Used for durability.
        weaponOneIsActive = !weaponOneIsActive;
    }

    public void DurabilityDamage() {
        // Take durability off of the currently equipped weapon that just landed an attack.
        // Active weapon always starts on weapon one.
        if (weaponOneIsActive) {
            weaponOneDurability -= activeWeapon.durabilityDamage;
            HUDManager.playerWeapons.AdjustDurabilityBar(weaponOneDurability);
        }
        else {
            weaponTwoDurability -= activeWeapon.durabilityDamage;
            HUDManager.playerWeapons.AdjustDurabilityBar(weaponTwoDurability);
        }
        // Check if the weapon is broken.
        if (weaponOneDurability <= 0f || weaponTwoDurability <= 0f) {
            BreakActiveWeapon();
            print("WEAPON JUST BROKE YO!");
        }
    }

    public void BreakActiveWeapon() {
        // Remove from HUD with appropriate effects.

        // Remove from player with appropriate in-game effects and change the active weapon.

    }
}
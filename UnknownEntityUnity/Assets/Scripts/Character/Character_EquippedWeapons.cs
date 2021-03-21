using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_EquippedWeapons : MonoBehaviour
{
    [Header("Script References")]
    public Character_Attack charAtk;
    public MouseInputs moIn;
    public HUD_Manager HUDManager;
    public SpriteBouncePool spriteBouncePool;
    [Header("To-set Variables")]
    public SO_Weapon activeWeapon;
    public SpriteRenderer weaponSpriteR;
    [Header("Read Only")]
    public SO_Weapon inactiveWeapon;
    public float weaponOneDurability = 100;
    public float weaponTwoDurability = 100;
    public bool weaponOneIsActive = true;
    public bool canSwapWeapon = true;

    void Start() {
        WeaponSwap(false);
    }

    public bool CanISwapWeapon() {
        // Weapon swap if two weapons are equipped.
        if (canSwapWeapon) {
            if (activeWeapon != null && inactiveWeapon != null) {
                SO_Weapon tempWeapon = inactiveWeapon;
                inactiveWeapon = activeWeapon;
                activeWeapon = tempWeapon;
                WeaponSwap(true);
                // HUD active weapon changes.
                HUDManager.playerWeapons.SwapActiveWeapon();
            }
            return true;
        }
        return false;
    }

    public void WeaponSwap(bool changeActiveWeapon) {
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
        if (activeWeapon != null && inactiveWeapon != null) {
            charAtk.atkChain.OnWeaponSwap(activeWeapon, inactiveWeapon);
        }
        // Assign its resting angle.
        charAtk.weaponTrans.localEulerAngles = charAtk.WeapAtkChain.sO_Weapon_Motion.restingRotation;
        // Assign its resting position.
        charAtk.weaponTrans.localPosition = charAtk.WeapAtkChain.sO_Weapon_Motion.restingPosition;
        // Change the active weapon by reversing the bool. Used for durability.
        if (changeActiveWeapon) weaponOneIsActive = !weaponOneIsActive;
    }
    // Check if the player has an active weapon to hit with. May become obsolete if player has an unbreakable default weapon.
    public bool WeaponEquippedCheck() {
        if (activeWeapon) {
            return true;
        }
        return false;
    }
    public void DurabilityDamage(bool damageWeaponOne) {
        // Take durability off of the currently equipped weapon that just landed an attack.
        // Active weapon always starts on weapon one.
        if (damageWeaponOne) {
            weaponOneDurability -= activeWeapon.durabilityDamage;
            HUDManager.playerWeapons.AdjustDurabilityBar(damageWeaponOne, weaponOneDurability);
            // Check if the weapon is broken.
            if (weaponOneDurability <= 0f) {
                BreakActiveWeapon();
            }
        }
        else {
            weaponTwoDurability -= activeWeapon.durabilityDamage;
            HUDManager.playerWeapons.AdjustDurabilityBar(damageWeaponOne, weaponTwoDurability);
            // Check if the weapon is broken.
            if (weaponTwoDurability <= 0f) {
                BreakActiveWeapon();
            }
        }
    }

    public void BreakActiveWeapon() {
        // Remove from HUD with appropriate effects.
        HUDManager.playerWeapons.RemoveBrokenWeapon();
        // Remove from player with appropriate in-game effects and change the active weapon.
        StartCoroutine(SpawnWeaponPieces(activeWeapon.SO_brokenPiecesSpawner, Vector2.zero));
        // Change weapon. Anything that requires the active weapon that just broke should be done before this.
        activeWeapon = inactiveWeapon;
        inactiveWeapon = null;
        if (activeWeapon != null) {
            WeaponSwap(true);
            // HUD active weapon changes.
            HUDManager.playerWeapons.SwapActiveWeapon();
        }
        else {
            // No more weapons, what now?
            charAtk.weapon = null; // Shouldn't change this externally probably.
            weaponSpriteR.sprite = null;
        }
        // Allow the player to attack since the last attack of the weapon that broke was maybe not allowed to finish. Could set a delay for this to the time it would of taken to finish the last attack if it wasn't completed when the weapon broke.
        charAtk.ReadyToAttack(true);
    }
    public float GetActiveWeaponDurability() {
        if (weaponOneIsActive) {
            return weaponOneDurability;
        }
        else {
            return weaponTwoDurability;
        }
    }
    public void SetActiveWeaponDurability(float newActiveWeaponDurability) {
        if (weaponOneIsActive) {
            weaponOneDurability = newActiveWeaponDurability;
        }
        else {
            weaponTwoDurability = newActiveWeaponDurability;
        }
    }
    public IEnumerator SpawnWeaponPieces (SO_ObjectDestructionSpawner piecesSpawner, Vector2 brokenPiecesDir) {
        SpriteBounce spriteBounce;
        for (int i = 0; i < piecesSpawner.bouncingSpritesSO.Length; i++) {
            spriteBounce = spriteBouncePool.RequestSpriteBounce();
            spriteBounce.transform.position = (Vector2)charAtk.weaponTrans.position + piecesSpawner.spawnPositions[i];
            spriteBounce.StartBounce(piecesSpawner.bouncingSpritesSO[i], brokenPiecesDir);
            yield return null;
        }
    }
}
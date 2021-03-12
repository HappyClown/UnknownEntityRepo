using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherButtonActions : MonoBehaviour
{
    [Header("Movement Skill")]
    public Character_MovementSkills movementSkill; // change for a generic script that can
    public float moveSkillGraceDuration;
    Coroutine moveGraceCoroutine;
    public bool moveSkillGracePressed;
    [Header("Interact")]
    public Character_PickupWeapon charPickUp;
    public bool interactPressed;
    [Header("Weapon Swap")]
    public Character_EquippedWeapons charEquippedWeapon;
    public float weaponSwapGraceDuration;
    public Coroutine weaponSwapCoroutine;
    public bool weaponSwapGracePressed;
    [Header("Confirm")]
    // Scripts that require confirmation. 
    public bool confirmPressed;

#region Movement Skill button checks and grace period
    public void MoveSkillButtonPressedChecks() {
        if (movementSkill.CanIUseMovementSkill()) {
            // Movement skill is start in its own script if it is true.
            // Cancel grace period for movement skills.
            if (moveGraceCoroutine != null) StopCoroutine(moveGraceCoroutine);
            moveSkillGracePressed = false;
        }
        else {
            // Start a grace period coroutine.
            if (moveGraceCoroutine != null) StopCoroutine(moveGraceCoroutine);
            moveGraceCoroutine = StartCoroutine(MoveSkillInGrace());
            moveSkillGracePressed = true;
        }
    }

    IEnumerator MoveSkillInGrace() {
        float timer = 0f;
        while (timer < moveSkillGraceDuration) {
            timer += Time.deltaTime;
            if (movementSkill.CanIUseMovementSkill()) {
                timer = moveSkillGraceDuration;
            }
            yield return null;
        }
        moveGraceCoroutine = null;
        moveSkillGracePressed = false;
    } 
#endregion

#region Interaction button checks
    public void InteractButtonChecks() {
        // Pick up weapon.
        charPickUp.CanIPickUpWeapon();
    }
#endregion

#region Weapon Swap button checks and grace period  
    public void WeaponSwapButtonChecks() {
        // Try to swap weapon.
        if (charEquippedWeapon.CanISwapWeapon()) {
            if (weaponSwapCoroutine != null) StopCoroutine(weaponSwapCoroutine);
            weaponSwapGracePressed = false;
        }
        else {
            // Start a grace period coroutine.
            if (weaponSwapCoroutine != null) StopCoroutine(weaponSwapCoroutine);
            weaponSwapCoroutine = StartCoroutine(WeaponSwapInGrace());
            weaponSwapGracePressed = true;
        }
    }

    IEnumerator WeaponSwapInGrace() {
        float timer = 0f;
        while (timer < weaponSwapGraceDuration) {
            timer += Time.deltaTime;
            if (charEquippedWeapon.CanISwapWeapon()) {
                timer = weaponSwapGraceDuration;
            }
            yield return null;
        }
        weaponSwapCoroutine = null;
        weaponSwapGracePressed = false;
    }
#endregion

#region Confirm button checks
    public void ConfirmButtonChecks() {
        // 
    }
#endregion
}

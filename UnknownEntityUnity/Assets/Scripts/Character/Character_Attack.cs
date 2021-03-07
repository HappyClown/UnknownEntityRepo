using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Attack : MonoBehaviour
{
    [Header("Script References")]
    public MouseInputs moIn;
    public Character_AttackChain atkChain;
    public Character_AttackDetection atkDetection;
    //public Character_AttackWeaponMotion atkWeaMotion;
    public Character_AttackFXPool atkFXPool;
    public Character_AttackVisual atkVisual;
    public Character_AttackMovement atkMovement;
    public Character_AttackPlayerMovement atkPlyrMove;
    public Character_EquippedWeapons equippedWeapons;
    public Character_Movement charMov;
    public WeaponLookAt weaponLookAt;
    public Weapon_MotionController weaponMotionController;
    [Header("To-set Variables")]
    public Transform weaponTrans;
    public SpriteRenderer weaponSpriteR;
    [Header("Read Only")]
    public bool readyToAtk;
    // The initial weapon is set inside the Character_EquippedWeapon script.
    public SO_Weapon weapon;
    public Weapon_Motion weaponMotion;
    public Weapon_Motion curWeaponMotion;
    public List<Weapon_Motion> weaponMotions = new List<Weapon_Motion>();
    public Character_AttackFX atkFX;
    public bool atkDirectionChanges;
    public bool atkFXFlip;
    public int atkFXFlipScale;
    private List<Character_AttackFX> atkFXsInUse = new List<Character_AttackFX>();

    public void Attack() {
        // If the attack is triggered, make sure that the grace period is set back to false since it is used.
        moIn.attackButtonActions.attackButtonTapInGrace = false;
        // Checks and adjusts the current attack chain. (Chaining attacks)
        atkChain.ChainAttacks();
        // Disallow weapon swapping.
        equippedWeapons.canSwapWeapon = false;
        // Stop the previous motion if needed.
        StopPreviousMotion();
        // Set the weapon back to its resting position and rotation. Usually the first attack chain's resting values.
        ResetWeaponLocalValues();
        // Allow character flip and the weapon to "look at" the mouse.
        ForceCharFlipAndWeaponLookAt();
        // If I want to check the weapon motion every attack. (attack chains can have different motions ex; stab, slash)
        //curWeaponMotion = weaponMotionController.CheckMotionList(weaponMotion);
        curWeaponMotion = weaponMotions[atkChain.curChain];
        // Start the attack.
        curWeaponMotion.WeaponMotionSetup(this, weaponTrans, weaponSpriteR);
        // Handles moving the player, slowing him down, etc., during the attack. (Player motion)
        atkPlyrMove.SetupPlayerAttackMotions(WeapAtkChain.sO_CharAtk_Motion);
        // Clear the character_attackFX list.
        atkFXsInUse.Clear();
        // Activate all this attack's FX's.
        foreach (SO_AttackFX sO_AttackFX in ChainAttackFXs) {
            // Request an attack FX from the attack FX pool, the attack FX contains a Sprite Renderer and a PolygonalCollider2D.
            atkFX = atkFXPool.RequestAttackFX();
            // Flip the attack FX Sprite if needed. (So far, for alternating Slash motions)
            // if (atkDirectionChanges) { atkFX.spriteR.flipX = atkFXFlip; } else { atkFX.spriteR.flipX = false; }
            // Try flipping the attack fx's local x scale instead.
            if (atkDirectionChanges) { 
                atkFXFlipScale = (atkFXFlip) ? -1 : 1;
                atkFX.transform.localScale = new Vector2(atkFXFlipScale, atkFX.transform.localScale.y); 
            }
            else {
                atkFX.transform.localScale = new Vector2(1, atkFX.transform.localScale.y);
            }
            // The gameObject needs to be turned on before starting the coroutine on it.
            atkFX.gameObject.SetActive(true);
            // Moves the attack effect over the course of the attack.
            atkFX.StartCoroutine(atkMovement.AttackMovement(sO_AttackFX, atkFX.transform));
            // Enables and changes the attack effect over the course of the attack and dictates if the atkFX pool object is inUse then not.
            // Start the coroutine ON the pool object script, not the script holding the coroutine logic.
            atkFX.StartCoroutine(atkVisual.AttackAnimation(sO_AttackFX, atkFX));
            // Enables and disables the attack's collider during the "animation" and detects colliders it can hit, once, if it has one assigned.
            if (sO_AttackFX.collider != null) {
                atkDetection.StartCoroutine(atkDetection.AttackCollider(WeapAtkChain, sO_AttackFX, atkFX.col));
            }
            // Turn the atk direction change back to false. This is done at the end because colliders will also be flipped by checking this variable. Now flipping the attack FX pool object's x scale instead of just the sprite.
            atkDirectionChanges = false;
            // Player sripte to idle and stop animations
            // charMov.mySpriteAnim.Stop();
            // charMov.spriteRend.sprite = charMov.idleSprite;
            // Add the character_attackFXs used to a list to better keep track of them.
            atkFXsInUse.Add(atkFX);
        }
    }
    // When you want to stop the current attack.
    public void StopAttack() {
        if (curWeaponMotion) {
            curWeaponMotion.StopMotions();
        }
        // foreach (Character_AttackFX poolAtkFX in atkFXPool.atkFXs) {
        //     if (poolAtkFX.stopOnStun) {
        //         poolAtkFX.StopAllCoroutines();
        //         poolAtkFX.spriteR.sprite = null;
        //         poolAtkFX.col.enabled = false;
        //         poolAtkFX.gameObject.SetActive(false);
        //         //poolAtkFX.transform.position = new Vector3(999, 999, poolAtkFX.transform.position.z);
        //         poolAtkFX.stopOnStun = false;
        //         poolAtkFX.inUse = false;
        //     }
        // }
        foreach (Character_AttackFX curAtkFX in atkFXsInUse) {
            if (curAtkFX.involuntaryCancel) {
                curAtkFX.StopAllCoroutines();
                curAtkFX.spriteR.sprite = null;
                curAtkFX.col.enabled = false;
                curAtkFX.gameObject.SetActive(false);
                curAtkFX.stopOnStun = false;
                curAtkFX.inUse = false;
            }
        }
        if (atkPlyrMove.charAtkMotionOn) {
            atkPlyrMove.StopPlayerMotion();
        }
        
        //atkVisual.StopAllCoroutines();
    }

    public bool CanInterruptAttackCheck() {
        foreach (Character_AttackFX poolAtkFX in atkFXPool.atkFXs) {
            if (!poolAtkFX.canInterrupt) {
                return false;
            }
        }
        return true;
    }


    // If I want to check only when a new weapon is equipped, call this from the Character_EquippedWeapons.Change(). (every attack chain with the same motion)
    public void AdjustMotionList() {
        curWeaponMotion = weaponMotionController.CheckMotionList(weaponMotion);
    }
    // Instantly sets the weapon to its basic resting position. (might not be needed where it is called since the next attack would start from the appropriate resting position)
    public void ResetWeaponLocalValues() {
        weaponTrans.localPosition = weapon.restingPosition;
        weaponTrans.localEulerAngles = weapon.restingRotation;
    }
    // Triggers the generic function shared by all weapon motions to stop the weapon motions.
    public void StopPreviousMotion() {
        if (curWeaponMotion != null) {
            curWeaponMotion.StopMotions();
        }
    }
    // Instantly tests the character's sprite flip and adjusts the weapon's orientation both based on mouse position.
    public void ForceCharFlipAndWeaponLookAt() {
        charMov.charCanFlip = true;
        charMov.FlipSpriteMouseBased();
        weaponLookAt.lookAtEnabled = true;
        weaponLookAt.ForceLookAtUpdate();
    }
    // Shorter reference to the weapon scriptable object's attack chain class.
    public SO_Weapon.AttackChain WeapAtkChain {
        get {
            return weapon.attackChains[atkChain.curChain];
        }
    }
    // Shorter reference to the current attack chain's attack FXs.
    public SO_AttackFX[] ChainAttackFXs {
        get {
            return WeapAtkChain.sO_AttackFXs;
        }
    }
}

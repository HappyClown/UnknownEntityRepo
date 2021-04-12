using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AttackSpecial : MonoBehaviour
{
    [Header("Script References")]
    public MouseInputs moIn;
    public Character_Attack charAtk;
    [Header("Attack FXs")]
    public SO_AttackFX SO_WindupFX, SO_HoldFX, SO_ReleaseFX;
    public Character_AttackFX windupFX, holdFX, releaseFX;
    public Coroutine windupCoroutine, holdCoroutine, releaseCoroutine;
    //[Header("Player Motion")]
    public SO_CharAtk_Motion SO_WindupCharMo;/* , SO_HoldCharMo, SO_ReleaseCharMo; */
    //public Character_AttackFX windupFX, holdFX, releaseFX;
    //public Coroutine windupCoroutine, holdCoroutine, releaseCoroutine;
    [Header("General Variables")]
    public bool specAtkButtonDown;
    public bool inWindup, inHold, inRelease;


    public void SpecialAttack() {
        charAtk.ReadyToAttack(false);
        // Perform the special attack.
        // If the attack is triggered, make sure that the grace period is set back to false since it is used.
        moIn.attackButtonActions.attackButtonTapInGrace = false;
        // Disallow weapon swapping.
        charAtk.equippedWeapons.canSwapWeapon = false;
        // Stop the previous motion if needed.
        charAtk.StopPreviousMotion();
        // Set the weapon back to its resting position and rotation. Usually the first attack chain's resting values.
        charAtk.ResetWeaponLocalValues();
        // Allow character flip and the weapon to "look at" the mouse.
        charAtk.ForceCharFlipAndWeaponLookAt();
        // If I want to check the weapon motion every attack. (attack chains can have different motions ex; stab, slash)
        //Handles moving the player, slowing him down, etc., during the attack. (Player motion)
        //charAtk.atkPlyrMove.SetupPlayerAttackMotions(SO_HoldCharMo);
        //curWeaponMotion = weaponMotions[atkChain.curChain];
        //curWeaponMotion.WeaponMotionSetup(this, weaponTrans, weaponSpriteR);
        // Clear the character_attackFX list.
        //atkFXsInUse.Clear();
        windupFX = null;
        holdFX = null;
        releaseFX = null;
        windupFX = charAtk.atkFXPool.RequestAttackFX();
        windupFX.inUse = true;
        holdFX = charAtk.atkFXPool.RequestAttackFX();
        holdFX.inUse = true;
        releaseFX = charAtk.atkFXPool.RequestAttackFX();
        releaseFX.inUse = true;

        StartCoroutine(InSpecialAttack());
        // Activate all this attack's FX's.
        //foreach (SO_AttackFX sO_AttackFX in ChainAttackFXs) {
            // Request an attack FX from the attack FX pool, the attack FX contains a Sprite Renderer and a PolygonalCollider2D.
            //atkFX = atkFXPool.RequestAttackFX();
            // Flip the attack FX Sprite if needed. (So far, for alternating Slash motions)
            // if (atkDirectionChanges) { atkFX.spriteR.flipX = atkFXFlip; } else { atkFX.spriteR.flipX = false; }
            // Try flipping the attack fx's local x scale instead.
            // if (atkDirectionChanges) { 
            //     atkFXFlipScale = (atkFXFlip) ? -1 : 1;
            //     atkFX.transform.localScale = new Vector2(atkFXFlipScale, atkFX.transform.localScale.y); 
            // }
            // else {
            //     atkFX.transform.localScale = new Vector2(1, atkFX.transform.localScale.y);
            // }

            // Enables and changes the attack effect over the course of the attack and dictates if the atkFX pool object is inUse then not.
            // Start the coroutine ON the pool object script, not the script holding the coroutine logic.
            //atkFX.StartCoroutine(atkVisual.AttackAnimation(sO_AttackFX, atkFX));
            // Enables and disables the attack's collider during the "animation" and detects colliders it can hit, once, if it has one assigned.
            // Requires the sOWeapon because durability damage is general, if it was attack chain specific then just having the weaponAttackChain would be sufficient as it would hold the durability damage info.
            //print("Weapon one is active on attack trigger: "+equippedWeapons.weaponOneIsActive);
            // if (sO_AttackFX.collider != null) {
            //     atkDetection.StartCoroutine(atkDetection.AttackCollider(equippedWeapons.weaponOneIsActive, weapon, WeapAtkChain, sO_AttackFX, atkFX.col));
            // }
            // Turn the atk direction change back to false. This is done at the end because colliders will also be flipped by checking this variable. Now flipping the attack FX pool object's x scale instead of just the sprite.
            //atkDirectionChanges = false;
            // Player sripte to idle and stop animations
            // charMov.mySpriteAnim.Stop();
            // charMov.spriteRend.sprite = charMov.idleSprite;
            // Add the character_attackFXs used to a list to better keep track of them.
            //atkFXsInUse.Add(atkFX);
        //}
    }

    public void SpecialAttackButtonReleased(bool releasedByPlayer) {
        specAtkButtonDown = false;
        if (inWindup) { 
            //CANCELATTACK;
        }
        if (inHold) { 
            if (releasedByPlayer) {
                holdFX.exitAttackAnimationFX = true;
                charAtk.atkPlyrMove.exitPlayerMotion = true;
            } 
            else {
                //CANCELATTACK;
            }
        }
    }

    IEnumerator InSpecialAttack() {
        // General Variables 
        float timer = 0f;
        // Windup
        inWindup = true;
        windupFX.gameObject.SetActive(true);
        windupFX.StartCoroutine(charAtk.atkVisual.AttackAnimation(SO_WindupFX, windupFX));
        windupFX.StartCoroutine(charAtk.atkMovement.AttackMovement(SO_WindupFX, windupFX.transform));
        charAtk.atkPlyrMove.SetupPlayerAttackMotions(SO_WindupCharMo);
        while (timer < SO_WindupFX.totalDuration) {
            timer += Time.deltaTime;
            yield return null;
        }
        inWindup = false;
        timer = 0f;
        // Hold
        inHold = true;
        holdFX.gameObject.SetActive(true);
        //holdFX.loopAnimation = true;
        holdFX.StartCoroutine(charAtk.atkVisual.AttackAnimation(SO_HoldFX, holdFX));
        holdFX.StartCoroutine(charAtk.atkMovement.AttackMovement(SO_HoldFX, holdFX.transform));
        //charAtk.atkPlyrMove.SetupPlayerAttackMotions(SO_HoldCharMo);
        while (specAtkButtonDown) {
            yield return null;
        }
        holdFX.exitAttackAnimationFX = true;
        inHold = false;
        // Release
        inRelease = true;
        releaseFX.gameObject.SetActive(true);
        releaseFX.StartCoroutine(charAtk.atkVisual.AttackAnimation(SO_ReleaseFX, releaseFX));
        releaseFX.StartCoroutine(charAtk.atkMovement.AttackMovement(SO_ReleaseFX, releaseFX.transform));
        //charAtk.atkPlyrMove.SetupPlayerAttackMotions(SO_ReleaseCharMo);
        while (timer < SO_ReleaseFX.totalDuration) {
            timer += Time.deltaTime;
            yield return null;
        }
        inRelease = false;
        timer = 0f;
        charAtk.ReadyToAttack(true);
    }

    // Hold rightclick > weaponmotion+atkFX+playermotion+etc.(windup) > btn held state where effects stay active and loop (hold) > weapmotion+atkFX+etc.(release).

    // need to detect at all times the state of the special attack button (right click)

    // cancel point for the attack if it is released before the windup ends

    // early releases with different and incresed effect upon release


}

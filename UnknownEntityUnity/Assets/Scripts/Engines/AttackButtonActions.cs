using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButtonActions : MonoBehaviour
{
    public Character_Attack charAtk;
    public Character_AttackSpecial charAtkSpecial;
    public OtherButtonActions otherBtnActs;
    private bool attackButtonHeld;
    [Header("Grace Period")]
    public bool graceUsesFrames;
    public float attackGraceDuration;
    public int attackGraceFrames;
    private float attackGraceTimer;
    private int attackGraceFrameCount;
    public bool attackButtonTapInGrace;
    [Header("ChargedAttack")]
    public float heldTimer;
    // Coroutines
    private Coroutine attackGraceCoroutine = null, attackButtonCurrentlyHeldCoroutine = null;
    // to sort
    public bool pressed, chargeAttackReady;
    [Header("Special Attack")]
    public bool specialAttackHeld;
    private Coroutine specAtkCoroutine;

    #region Regular Attack button pressed
    // Attack button tapped. (regular attacks)
    public void AttackButtonPressedChecks() {
        // Check if the attack is ready > check if the attack is chargeable > no, do regular attack > yes, start hold counter
        if (charAtk.ReadyToAttackCheck()) {
            if (charAtk.WeapAtkChain.chargeable) {
                print("attack is chargeable starting timer.");
                pressed = true;
                heldTimer = 0f;
                if (attackButtonCurrentlyHeldCoroutine != null) {
                    StopCoroutine(attackButtonCurrentlyHeldCoroutine);
                }
                attackButtonCurrentlyHeldCoroutine = StartCoroutine(AttackButtonCurrentlyHeld());
            }
            else {
                charAtk.Attack();
                attackButtonTapInGrace = false;
                if (attackGraceCoroutine != null) {StopCoroutine(attackGraceCoroutine);}
            }
        }
        else {
            attackButtonTapInGrace = true;
            if (attackGraceCoroutine != null) {StopCoroutine(attackGraceCoroutine);}
            attackGraceCoroutine = StartCoroutine(AttackButtonTappedGrace());
        }
    }
    // While button is held increase timer.
    IEnumerator AttackButtonCurrentlyHeld() {
        float reqTime = charAtk.WeapAtkChain.sO_ChargeAttack.chargingTimeReq;
        while(pressed) {
            heldTimer += Time.deltaTime;
            if (heldTimer > reqTime) {
                //hold state
                chargeAttackReady = true;
                print("charge atk ready");
                yield break;
            }
            print("preparing charge atk");
            yield return null;
        }
    }
    // Attack button is released, no matter how long it was held.
    public void AttackButtonReleasedChecks() {
        pressed = false;
        if (chargeAttackReady) {
            // release state
            chargeAttackReady = false;
            charAtk.Attack();
            print("releasing charged attack");
        }
        if (attackButtonCurrentlyHeldCoroutine != null) {StopCoroutine(attackButtonCurrentlyHeldCoroutine);}
    }
    // While in grace period keep trying to perform an attack. 
    IEnumerator AttackButtonTappedGrace() {
            attackGraceTimer = 0f;
            while(attackGraceTimer < attackGraceDuration) {
                attackGraceTimer += Time.deltaTime;
                // Before checking if I can attack while in grace, always check if the swap weapon button was pressed and if a weapon swap is possible, if so dont attack and instead swap weapon. This is to prioritize changing weapon over attacking.
                if (otherBtnActs.weaponSwapGracePressed) {
                    otherBtnActs.WeaponSwapButtonChecks();
                    break;
                }
                if (charAtk.ReadyToAttackCheck()) {
                    charAtk.Attack();
                    attackButtonTapInGrace = false;
                    if (attackGraceCoroutine != null) {StopCoroutine(attackGraceCoroutine);}
                }
                yield return null;
            }
            attackButtonTapInGrace = false;
            attackGraceCoroutine = null;
            yield break;
    }
    #endregion
    #region Special Sttack button pressed
    // When held, set bool to true, special attack script is poked to active the attack, a bool in that script is turn to true when performed, the bool is set to false when canceled. Thats it. Peppermint candy being chewed.
    // Check if an attack can be made (shared with regular attacks).
    public void SpecialAttackButtonPerformed() {
        if (charAtk.ReadyToAttackCheck()) {
            charAtkSpecial.SpecialAttack();
            specialAttackHeld = true;
        }
        else {
            // Set a bool in this script to true to simulate the button being held, and to keep checking if the attack can be performed.
            //print("Right click has been performed!! BEHOLD, NOTOOTTINHG.");
            specialAttackHeld = true;
            specAtkCoroutine = StartCoroutine(SpecialAttackIsHeld());
        }
        charAtkSpecial.specAtkButtonDown = true;
    }
    public void SpecialAttackButtonCanceled() {
        specialAttackHeld = false;
        charAtkSpecial.specAtkButtonDown = false;
        charAtkSpecial.SpecialAttackButtonReleased(true);
        specAtkCoroutine = null;
    }
    IEnumerator SpecialAttackIsHeld() {
        while (specialAttackHeld) {
            //print("shit");
            if (charAtk.ReadyToAttackCheck()) {
                charAtkSpecial.SpecialAttack();
                specialAttackHeld = false;
            }
            yield return null;
        }
    }
    #endregion
}

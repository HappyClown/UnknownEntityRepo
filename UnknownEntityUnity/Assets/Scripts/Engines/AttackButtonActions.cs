using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButtonActions : MonoBehaviour
{
    public Character_Attack charAttack;
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

    // Attack button tapped. (regular attacks)
    public void AttackButtonPressedChecks() {
        // Check if the attack is ready > check if the attack is chargeable > no, do regular attack > yes, start hold counter
        //print("pressed");
        if (charAttack.readyToAtk) {
            if (charAttack.WeapAtkChain.chargeable) {
                print("attack is chargeable starting timer.");
                pressed = true;
                heldTimer = 0f;
                if (attackButtonCurrentlyHeldCoroutine != null) {
                    StopCoroutine(attackButtonCurrentlyHeldCoroutine);
                }
                attackButtonCurrentlyHeldCoroutine = StartCoroutine(AttackButtonCurrentlyHeld());
            }
            else {
                charAttack.Attack();
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
        float reqTime = charAttack.WeapAtkChain.sO_ChargeAttack.chargingTimeReq;
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
            charAttack.Attack();
            print("releasing charged attack");
        }
        //print("released");
        if (attackButtonCurrentlyHeldCoroutine != null) {StopCoroutine(attackButtonCurrentlyHeldCoroutine);}
    }


    IEnumerator AttackButtonTappedGrace() {
        // Grace period counted in Update() frames.
        // if (graceUsesFrames) {
        //     attackGraceFrameCount = 0;
        //     while(attackGraceFrameCount < attackGraceFrames) {
        //         attackGraceFrameCount++;
        //         if (charAttack.readyToAtk) {
        //             charAttack.Attack();
        //             attackButtonTapInGrace = false;
        //             if (attackGraceCoroutine != null) {StopCoroutine(attackGraceCoroutine);}
        //         }
        //         yield return null;
        //     }
        //     attackButtonTapInGrace = false;
        //     attackGraceCoroutine = null;
        //     yield break;
        // }
        // Grace period counted in deltaTime. (seconds)
        //else {
            attackGraceTimer = 0f;
            while(attackGraceTimer < attackGraceDuration) {
                attackGraceTimer += Time.deltaTime;
                if (charAttack.readyToAtk) {
                    charAttack.Attack();
                    attackButtonTapInGrace = false;
                    if (attackGraceCoroutine != null) {StopCoroutine(attackGraceCoroutine);}
                }
                yield return null;
            }
            attackButtonTapInGrace = false;
            attackGraceCoroutine = null;
            yield break;
        //}
    }
}
